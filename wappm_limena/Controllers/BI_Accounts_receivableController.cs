using CrystalDecisions.CrystalReports.Engine;
using Postal;
using Spire.Pdf;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using wappm_limena.Helper;
using wappm_limena.Models;

namespace wappm_limena.Controllers
{
    public class BI_Accounts_receivableController : Controller
    {
        private DLI_PROEntities db = new DLI_PROEntities();

        // GET: BI_Accounts_receivable
        public ActionResult IndexAccountsReceivable()
        {
            XMLReader readXML = new XMLReader();
            var Sellersdata = readXML.ReturnListOfSellersSR();
            var Ccdata = readXML.ReturnListOfCcSR();
            var Config = readXML.ReturnEmailConfig().FirstOrDefault();

            ViewBag.SellersData = Sellersdata.ToList();
            ViewBag.CcData = Ccdata.ToList();

            ViewBag.Menu = "Applications";
            ViewBag.SubMenu = "Accounts Receivable Report";
            ViewBag.Description = "Send emails to each Seller, with Accounts Receivable information";

            //Datos de configuracion
            ViewBag.confEmail = Config.Email;
            ViewBag.confPassword = Config.Password;

            return View();
        }

        public ActionResult sendMessage()
        {
            var path = "";
            //var pathimage = "";
            XMLReader readXML = new XMLReader();
            var Sellersdata = readXML.ReturnListOfSellersAR();
            var CcData = readXML.ReturnListOfCcAR();
            var destinatariosCC = "";
            int count = 1;


            //Llamamos a los datos de configuracion
            var Config = readXML.ReturnEmailConfig().FirstOrDefault();


            //Llamamos los datos de Cc
            foreach (var cc in CcData)
            {

                if (CcData.Count == 1)
                {
                    destinatariosCC = cc.Email;
                }
                else if (count == CcData.Count())
                {
                    destinatariosCC = destinatariosCC + cc.Email;

                }
                else
                {
                    destinatariosCC += cc.Email + ",";
                }

                count += 1;
            }


            if (Sellersdata.Count > 0)
            {
                foreach (var seller in Sellersdata)
                {
                    var accounts_receivable = (from c in db.BI_Accounts_receivable where (c.SalesRepCode == seller.Id) select c).OrderByDescending(x => x.idAging).ThenByDescending(x => x.Amount).ToList();
                    if (accounts_receivable.Count > 0)
                    {
                        //Existen datos

                        ReportDocument rd = new ReportDocument();
                        rd.Load(Path.Combine(Server.MapPath("~/Reports"), "rptAccountsReceivableBySeller.rpt"));

                        rd.SetDataSource(accounts_receivable);
                        string fecha = DateTime.Now.ToLongDateString();
                        rd.SetParameterValue("fecha_actual", fecha);

                        rd.SetParameterValue("nombre", seller.Names.ToUpper());
                        rd.SetParameterValue("apellido", seller.LastNames.ToUpper());

                        var totalAR = accounts_receivable.AsEnumerable().Sum(x => x.Amount);
                        rd.SetParameterValue("totalAR", totalAR);



                        var filePathOriginal = Server.MapPath("/Reports/pdfReports");

                        try
                        {

                            //limpiamos el directorio
                            System.IO.DirectoryInfo di = new DirectoryInfo(filePathOriginal);

                            foreach (FileInfo file in di.GetFiles())
                            {
                                file.Delete();
                            }
                            foreach (DirectoryInfo dir in di.GetDirectories())
                            {
                                dir.Delete(true);
                            }

                        }
                        catch (Exception e)
                        {
                            var mensaje = e.ToString();
                        }



                        var filename = "Accounts Receivable Report " + seller.SalesRepresentative + ".pdf";
                        path = Path.Combine(filePathOriginal, filename);
                        rd.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, path);

                        //PdfDocument doc = new PdfDocument();
                        //doc.LoadFromFile(path);
                        ////Contamos numero total de paginas
                        //int indexpages = doc.Pages.Count;

                        //Image img = doc.SaveAsImage(0);
                        //var imagename = "Accounts Receivable Report " + seller.SalesRepresentative + ".jpg";
                        //pathimage = Path.Combine(filePathOriginal, imagename);
                        //img.Save(pathimage);
                        //doc.Close();


                        //Para enviar correos
                        try
                        {
                            dynamic email = new Email("EmailAccountsReceivableReport");
                            email.To = seller.Email.ToString();
                            email.From = Config.Email;
                            email.Subject = "Accounts Receivable Report | " + seller.SalesRepresentative;
                            email.Cc = destinatariosCC.ToString();
                            email.CcSupervisor = seller.Supervisor.ToString();
                            email.Attach(new Attachment(path));
                            //email.Body = imagename;
                            //return new EmailViewResult(email);





                            email.Send();
                        }

                        catch (Exception e)
                        {
                            Console.WriteLine("{0} Exception caught.", e);
                        }


                    }
                }


                //Retornamos la misma pagina pero con alertas de exito
                //Le mandamos un tempdata para mostrar los mensajes
                TempData["success"] = "All emails were successfully sent.";
                return RedirectToAction("IndexAccountsReceivable", "BI_Accounts_receivable");
            }
            else
            {
                //No habia datos que mostrars
                TempData["warning"] = "No data was found.";
                return RedirectToAction("IndexAccountsReceivable", "BI_Accounts_receivable");
            }

        }

        public ActionResult previewMessage(int? id)
        {
            //var path = "";
            //var pathimage = "";
            XMLReader readXML = new XMLReader();
            var Sellersdata = readXML.ReturnListOfSellersAR();
            Sellersdata = (from c in Sellersdata where (c.Id == id) select c).ToList();
            var CcData = readXML.ReturnListOfCcAR();
            var destinatariosCC = "";
            int count = 1;


            //Llamamos a los datos de configuracion
            var Config = readXML.ReturnEmailConfig().FirstOrDefault();


            //Llamamos los datos de Cc
            foreach (var cc in CcData)
            {

                if (CcData.Count == 1)
                {
                    destinatariosCC = cc.Email;
                }
                else if (count == CcData.Count())
                {
                    destinatariosCC = destinatariosCC + cc.Email;

                }
                else
                {
                    destinatariosCC += cc.Email + ",";
                }

                count += 1;
            }

            if (Sellersdata.Count() > 0)
            {
                foreach (var seller in Sellersdata)
                {
                    var accounts_receivable = (from c in db.BI_Accounts_receivable where (c.SalesRepCode == seller.Id) select c).OrderByDescending(x => x.idAging).ThenByDescending(x => x.Amount).ToList();
                    if (accounts_receivable.Count > 0)
                    {
                        //Existen datos

                        ReportDocument rd = new ReportDocument();
                        rd.Load(Path.Combine(Server.MapPath("~/Reports"), "rptAccountsReceivableBySeller.rpt"));

                        rd.SetDataSource(accounts_receivable);
                        string fecha = DateTime.Now.ToLongDateString();
                        rd.SetParameterValue("fecha_actual", fecha);

                        rd.SetParameterValue("nombre", seller.Names.ToUpper());
                        rd.SetParameterValue("apellido", seller.LastNames.ToUpper());

                        var totalAR = accounts_receivable.AsEnumerable().Sum(x => x.Amount);
                        rd.SetParameterValue("totalAR", totalAR);


                        Response.Buffer = false;
                        Response.ClearContent();
                        Response.ClearHeaders();

                        Response.AppendHeader("Content-Disposition", "inline; filename=" + "Accounts Receivable Report " + seller.SalesRepresentative + ".pdf; ");

                        Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                        stream.Seek(0, SeekOrigin.Begin);

                        return File(stream, System.Net.Mime.MediaTypeNames.Application.Pdf);


                    }
                    else
                    {
                        //No habia datos que mostrars
                        TempData["warning"] = "No data was found.";
                        return RedirectToAction("IndexAccountsReceivable", "BI_Accounts_receivable");
                    }
                }


            }
            else
            {
                //No habia datos que mostrars
                TempData["warning"] = "No data was found.";
                return RedirectToAction("IndexAccountsReceivable", "BI_Accounts_receivable");
            }
            return RedirectToAction("IndexAccountsReceivable", "BI_Accounts_receivable");
        }



        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

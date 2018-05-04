using CrystalDecisions.CrystalReports.Engine;
using Postal;
using Spire.Pdf;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using wappm_limena.Helper;
using wappm_limena.Helper.External.Models;
using wappm_limena.Models;
using CrystalDecisions.Shared;
namespace wappm_limena.Controllers
{
    public class SalesReportController : Controller
    {
        // GET: SalesReport
        private DLI_PROEntities db = new DLI_PROEntities();
        // GET: SendMWRCI
        public ActionResult IndexSalesReport()
        {
            XMLReader readXML = new XMLReader();
            var Sellersdata = readXML.ReturnListOfSellersSR();
            var Ccdata = readXML.ReturnListOfCcSR();
            var Config = readXML.ReturnEmailConfig().FirstOrDefault();

            ViewBag.SellersData = Sellersdata.ToList();
            ViewBag.CcData = Ccdata.ToList();

            ViewBag.Menu = "Applications";
            ViewBag.SubMenu = "Sales Report";
            ViewBag.Description = "Send emails to each Seller, with sales orders information";

            //Datos de configuracion
            ViewBag.confEmail = Config.Email;
            ViewBag.confPassword = Config.Password;


            return View();
        }

        public ActionResult sendMessage()
        {
            var path = "";
            var pathimage = "";
            XMLReader readXML = new XMLReader();
            var Sellersdata = readXML.ReturnListOfSellersSR();
            var CcData = readXML.ReturnListOfCcSR();
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
                    var salesorders = (from c in db.BI_sales_report where (c.SlpCode == seller.Id) select c).OrderBy(x => x.Time).ToList();
                    if (salesorders.Count > 0)
                    {
                        //Existen datos

                        ReportDocument rd = new ReportDocument();
                        rd.Load(Path.Combine(Server.MapPath("~/Reports"), "rptSalesReportBySeller.rpt"));

                        rd.SetDataSource(salesorders);
                        string fecha = DateTime.Now.ToLongDateString();
                        rd.SetParameterValue("fecha_actual", fecha);

                        rd.SetParameterValue("nombre", seller.Names.ToUpper());
                        rd.SetParameterValue("apellido", seller.LastNames.ToUpper());
                        rd.SetParameterValue("weektype", "Week" + " " + salesorders.Where(x => x.WeekType != null).FirstOrDefault().WeekType.ToString());

                        var totalBudget = salesorders.AsEnumerable().Sum(x => x.Budget);
                        rd.SetParameterValue("budget", totalBudget);
                        var totalOrderSales = salesorders.Where(x => x.Budget > 0).AsEnumerable().Sum(x => x.Total);
                        rd.SetParameterValue("salesorder", totalOrderSales);
                        var totalOtherSales = salesorders.Where(x => x.Budget == 0).AsEnumerable().Sum(x => x.Total);
                        rd.SetParameterValue("othersales", totalOtherSales);
                        var totalSales = totalOrderSales + totalOtherSales;
                        rd.SetParameterValue("Totalsales", totalSales);

                        if (totalBudget > 0)
                        {
                            var totalAchievements = (totalSales / totalBudget) * 100;
                            rd.SetParameterValue("total_achievements", totalAchievements);
                        }
                        else
                        {
                            var totalAchievements = 100.00;
                            rd.SetParameterValue("total_achievements", totalAchievements);

                        }




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



                        var filename = "Sales Report " + seller.SalesRepresentative + " " + DateTime.Now.ToString("dddd").ToUpper() + ".pdf";
                        path = Path.Combine(filePathOriginal, filename);
                        rd.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, path);

                        PdfDocument doc = new PdfDocument();
                        doc.LoadFromFile(path);
                        Image img = doc.SaveAsImage(0);
                        var imagename = "Sales Report " + seller.SalesRepresentative + " " + DateTime.Now.ToString("dddd").ToUpper() + ".jpg";
                        pathimage = Path.Combine(filePathOriginal, imagename);
                        img.Save(pathimage);
                        doc.Close();

                        
                        //Para enviar correos
                        try
                        {
                            dynamic email = new Email("EmailSalesReport");
                            email.To = seller.Email.ToString();
                            email.From = Config.Email;
                            email.Subject = "Sales Report | " + seller.SalesRepresentative + " " + DateTime.Now.ToString("dddd").ToUpper();
                            email.Cc = destinatariosCC.ToString();
                            email.CcSupervisor = seller.Supervisor.ToString();
                            email.Body = imagename;
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
                return RedirectToAction("IndexSalesReport", "SalesReport");
            }
            else
            {
                //No habia datos que mostrars
                TempData["warning"] = "No data was found.";
                return RedirectToAction("IndexSalesReport", "SalesReport");
            }

        }
        public ActionResult previewMessage(int? id)
        {
            //var path = "";
            //var pathimage = "";
            XMLReader readXML = new XMLReader();
            var Sellersdata = readXML.ReturnListOfSellersSR();
            Sellersdata = (from c in Sellersdata where (c.Id == id) select c).ToList();
            var CcData = readXML.ReturnListOfCcSR();
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
                    var salesorders = (from c in db.BI_sales_report where (c.SlpCode == seller.Id) select c).OrderBy(x => x.Time).ToList();
                    if (salesorders.Count > 0)
                    {
                        //Existen datos
                        foreach (var item in salesorders)
                        {
                            item.Logro = Math.Round((item.Logro * 100), 2);
                          
                        }

                        ReportDocument rd = new ReportDocument();

                        rd.Load(Path.Combine(Server.MapPath("/Reports"), "rptSalesReportBySeller.rpt"));

                        rd.SetDataSource(salesorders);
                        string fecha = DateTime.Now.ToLongDateString();
                        rd.SetParameterValue("fecha_actual", fecha);

                        rd.SetParameterValue("nombre", seller.Names.ToUpper());
                        rd.SetParameterValue("apellido", seller.LastNames.ToUpper());
                        rd.SetParameterValue("weektype", "Week" + " " + salesorders.Where(x => x.WeekType != null).FirstOrDefault().WeekType.ToString());

                        var totalBudget = salesorders.AsEnumerable().Sum(x => x.Budget);
                        rd.SetParameterValue("budget", totalBudget);
                        var totalOrderSales = salesorders.Where(x => x.Budget > 0).AsEnumerable().Sum(x => x.Total);
                        rd.SetParameterValue("salesorder", totalOrderSales);
                        var totalOtherSales = salesorders.Where(x => x.Budget == 0).AsEnumerable().Sum(x => x.Total);
                        rd.SetParameterValue("othersales", totalOtherSales);
                        var totalSales = totalOrderSales + totalOtherSales;
                        rd.SetParameterValue("Totalsales", totalSales);
                        if (totalBudget > 0)
                        {
                            var totalAchievements = (totalSales / totalBudget) * 100;
                            rd.SetParameterValue("total_achievements", totalAchievements);
                        }
                        else
                        {
                            var totalAchievements = 100.00;
                            rd.SetParameterValue("total_achievements", totalAchievements);

                        }

                        Response.Buffer = false;
                        Response.ClearContent();
                        Response.ClearHeaders();

                        Response.AppendHeader("Content-Disposition", "inline; filename=" + "Sales Report " + seller.SalesRepresentative + "; ");

                        Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                        stream.Seek(0, SeekOrigin.Begin);

                        return File(stream, System.Net.Mime.MediaTypeNames.Application.Pdf);


                    }
                    else {
                        //No habia datos que mostrars
                        TempData["warning"] = "No data was found.";
                        return RedirectToAction("IndexSalesReport", "SalesReport");
                    }
                }


            }
            else
            {
                //No habia datos que mostrars
                TempData["warning"] = "No data was found.";
                return RedirectToAction("IndexSalesReport", "SalesReport");
            }
            return RedirectToAction("IndexSalesReport", "SalesReport");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditConfiguration([Bind(Include = "Id,Email,Password,AllDays,Hour")] Config configEmail)
        {
            try
            {
                //Guardamos
                XmlDocument doc = new XmlDocument();
                string path = Path.Combine(Server.MapPath("~/App_Data/SendMWRCI"), "Config.xml");
                doc.Load(path);
                //IEnumerator ie = doc.SelectNodes("ConfigEmail/Config/").GetEnumerator();


                doc.SelectSingleNode("ConfigEmail/Config/Email").InnerText = configEmail.Email.ToString();
                doc.SelectSingleNode("ConfigEmail/Config/Password").InnerText = configEmail.Password.ToString();

                doc.Save(path);

                //Luego modificamos en el web.config
                XmlDocument webconfig = new XmlDocument();
                string pathwc = Path.Combine(Server.MapPath("~"), "Web.config");
                webconfig.Load(pathwc);

                webconfig.SelectSingleNode("configuration/system.net/mailSettings/smtp").Attributes["from"].Value = configEmail.Email.ToString();
                webconfig.SelectSingleNode("configuration/system.net/mailSettings/smtp/network").Attributes["userName"].Value = configEmail.Email.ToString();
                webconfig.SelectSingleNode("configuration/system.net/mailSettings/smtp/network").Attributes["password"].Value = configEmail.Password.ToString();
                webconfig.Save(pathwc);



                TempData["success"] = "Email Settings Updated.";
                return RedirectToAction("IndexSalesReport", "SalesReport");
            }
            catch (Exception exception)
            {
                TempData["error"] = "Error:  " + exception;
                return RedirectToAction("IndexSalesReport", "SalesReport");
            }



        }
    }
}
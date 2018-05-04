using CrystalDecisions.CrystalReports.Engine;
using Postal;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using wappm_limena.Helper;
using wappm_limena.Models;
using Spire.Pdf;
using System.Drawing;
using wappm_limena.Helper.External.Models;
using System.Xml;
using System.Collections;
using System.Data;
using System.ComponentModel;

namespace wappm_limena.Controllers
{
    public class SendMWRCIController : Controller
    {
        private DLI_PROEntities db = new DLI_PROEntities();
        // GET: SendMWRCI
        public ActionResult IndexMWRCI()
        {
            XMLReader readXML = new XMLReader();
            var Sellersdata = readXML.ReturnListOfSellers();
            var Ccdata = readXML.ReturnListOfCc();
            var Config = readXML.ReturnEmailConfig().FirstOrDefault();

            ViewBag.SellersData = Sellersdata.ToList();
            ViewBag.CcData = Ccdata.ToList();

            ViewBag.Menu = "Applications";
            ViewBag.SubMenu = "Returns and allowances";
            ViewBag.Description = "Send emails to each Seller, with returns and allowances information";

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
            var Sellersdata = readXML.ReturnListOfSellers();
            var CcData = readXML.ReturnListOfCc();
            var destinatariosCC = "";
            int count = 1;


            //Llamamos a los datos de configuracion
            var Config = readXML.ReturnEmailConfig().FirstOrDefault();
            

            //Llamamos los datos de Cc
            foreach (var cc in CcData) {

                if (CcData.Count == 1) {
                    destinatariosCC = cc.Email;
                }
                else if (count == CcData.Count())
                {
                    destinatariosCC = destinatariosCC + cc.Email;

                }
                else {
                    destinatariosCC += cc.Email + ",";
                }

                count += 1;
            }


            if (Sellersdata.Count > 0) {
                foreach (var seller in Sellersdata)
                {
                    var returns = (from c in db.BI_Email_RA where (c.id_SalesRep == seller.Id) select c).ToList();
                
                    if (returns.Count > 0)
                    {
                        //Existen datos
                        //Buscamos para tabla reason


                        var returns_header = (from b in db.BI_Email_RA_Head where (b.SlpCode == seller.Id) select b).OrderByDescending(b => b.Descr == "EXPIRED W99").ThenByDescending(b => b.Returns).ToList();

                        ReportDocument rd = new ReportDocument();
                        rd.Load(Path.Combine(Server.MapPath("~/Reports"), "rptReturnsAndAllowancesBySeller.rpt"));

                        rd.SetDataSource(returns);
                        rd.Subreports[0].SetDataSource(returns_header);




                        string fecha = DateTime.Now.ToLongDateString();
                        rd.SetParameterValue("fecha_actual", fecha);

                        rd.SetParameterValue("nombre", seller.Names.ToUpper());
                        rd.SetParameterValue("apellido", seller.LastNames.ToUpper());

                        var totalBudget = returns_header.AsEnumerable().Sum(x => x.Budget);
                        rd.SetParameterValue("budget", totalBudget);
                        var totalReturns = returns.Where(x => x.Budget > 0).AsEnumerable().Sum(x => x.Returns);
                        rd.SetParameterValue("returns", totalReturns);


                        if (totalBudget > 0)
                        {
                            var totalAchievements = (totalReturns / totalBudget) * 100;
                            rd.SetParameterValue("total_achievements", totalAchievements);
                        }
                        else
                        {
                            var totalAchievements = 0.00;
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



                        var filename = "Returns and Allowances Report " + seller.SalesRepresentative + " " + DateTime.Now.ToString("dddd").ToUpper() + ".pdf";
                        path = Path.Combine(filePathOriginal, filename);
                        rd.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, path);

                        PdfDocument doc = new PdfDocument();
                        doc.LoadFromFile(path);
                        Image img = doc.SaveAsImage(0);
                        var imagename = "Returns and Allowances Report " + seller.SalesRepresentative + " " + DateTime.Now.ToString("dddd").ToUpper() + ".jpg";
                        pathimage = Path.Combine(filePathOriginal, imagename);
                        img.Save(pathimage);                        
                        doc.Close();


                        //Para enviar correos
                        try
                        {
                            dynamic email = new Email("EmailMWRCI");
                            email.To = seller.Email.ToString();
                            email.From = Config.Email;
                            email.Subject = "Returns and Allowances Report | " + seller.SalesRepresentative + " " + DateTime.Now.ToString("dddd").ToUpper();
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
                return RedirectToAction("IndexMWRCI", "SendMWRCI");
            }
            else {
                //No habia datos que mostrars
                TempData["success"] = "No data was found.";
                return RedirectToAction("IndexMWRCI", "SendMWRCI");
            }
            
        }
        public ActionResult previewMessage(int? id)
        {
            //var path = "";
            //var pathimage = "";
            XMLReader readXML = new XMLReader();
            var Sellersdata = readXML.ReturnListOfSellers();
            Sellersdata = (from c in Sellersdata where (c.Id == id) select c).ToList();
            var CcData = readXML.ReturnListOfCc();
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
                    var returns = (from c in db.BI_Email_RA where (c.id_SalesRep == seller.Id) select c).ToList();
                    if (returns.Count > 0)
                    {
                        //Existen datos
                        //Buscamos para tabla reason
              
                        var returns_header = (from b in db.BI_Email_RA_Head where (b.SlpCode == seller.Id) select b).OrderByDescending(b => b.Descr == "EXPIRED W99" ).ThenByDescending(b => b.Returns).ToList();

                        ReportDocument rd = new ReportDocument();
                        rd.Load(Path.Combine(Server.MapPath("~/Reports"), "rptReturnsAndAllowancesBySeller.rpt"));

                        rd.SetDataSource(returns);
                        rd.Subreports[0].SetDataSource(returns_header);



                        string fecha = DateTime.Now.ToLongDateString();
                        rd.SetParameterValue("fecha_actual", fecha);

                        rd.SetParameterValue("nombre", seller.Names.ToUpper());
                        rd.SetParameterValue("apellido", seller.LastNames.ToUpper());


                        var totalBudget = returns_header.AsEnumerable().Sum(x => x.Budget);
                        rd.SetParameterValue("budget", totalBudget);
                        var totalReturns = returns.Where(x => x.Budget > 0).AsEnumerable().Sum(x => x.Returns);
                        rd.SetParameterValue("returns", totalReturns);
                        

                        if (totalBudget > 0)
                        {
                            var totalAchievements = (totalReturns / totalBudget) * 100;
                            rd.SetParameterValue("total_achievements", totalAchievements);
                        }
                        else
                        {
                            var totalAchievements = 0.00;
                            rd.SetParameterValue("total_achievements", totalAchievements);

                        }



                        var filePathOriginal = Server.MapPath("/Reports/pdfReports");



                        Response.Buffer = false;
                        Response.ClearContent();
                        Response.ClearHeaders();

                        Response.AppendHeader("Content-Disposition", "inline; filename=" + "Returns and Allowances Report " + seller.SalesRepresentative + "; ");

                        Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                        stream.Seek(0, SeekOrigin.Begin);

                        return File(stream, System.Net.Mime.MediaTypeNames.Application.Pdf);

                    }
                    else {
                        //No habia datos que mostrars
                        TempData["warning"] = "No data was found.";
                        return RedirectToAction("IndexMWRCI", "SendMWRCI");
                    }
                }


                //Retornamos la misma pagina pero con alertas de exito
                //Le mandamos un tempdata para mostrar los mensajes
                TempData["success"] = "All emails were successfully sent.";
                return RedirectToAction("IndexMWRCI", "SendMWRCI");
            }
            else
            {
                //No habia datos que mostrars
                TempData["warning"] = "No data was found.";
                return RedirectToAction("IndexMWRCI", "SendMWRCI");
            }

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
                    return RedirectToAction("IndexMWRCI", "SendMWRCI");
                }
                catch (Exception exception)
                {
                    TempData["error"] = "Error:  " + exception;
                    return RedirectToAction("IndexMWRCI", "SendMWRCI");
                }



        }

        public DataTable ConvertToDataTable<T>(IList<T> data)
        {
            PropertyDescriptorCollection properties =
               TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            foreach (PropertyDescriptor prop in properties)
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            foreach (T item in data)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                table.Rows.Add(row);
            }
            return table;

        }



    }
}
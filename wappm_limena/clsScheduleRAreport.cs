using CrystalDecisions.CrystalReports.Engine;
using Postal;
using Spire.Pdf;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Web;
using System.Web.Mvc;
using wappm_limena.Helper;
using wappm_limena.Models;

namespace wappm_limena
{
    public class clsScheduleRareport
    {
        private DLI_PROEntities db = new DLI_PROEntities();
        public void sendMessage_console()
        {

            var date = DateTime.Now.ToString("yyyyMMdd");

            FileStream ostrm;
            StreamWriter writer;
            TextWriter oldOut = Console.Out;

            ostrm = new FileStream("./logs/ralog_" + date + ".txt", FileMode.OpenOrCreate, FileAccess.Write);
            writer = new StreamWriter(ostrm);

            Console.SetOut(writer);

            var path = "";
            var pathimage = "";
            XMLReader readXML = new XMLReader();
            Console.WriteLine("Auto Mail Sender for Return and Allowances");
            Console.WriteLine("Returning Sellers list...");
            var Sellersdata = readXML.ReturnListOfSellers_console();
            Console.WriteLine("Returning CC list...");
            var CcData = readXML.ReturnListOfCc_console();
            //var destinatariosCC = "";
            //int count = 1;

            Console.WriteLine("Getting email configuration...");
            //Llamamos a los datos de configuracion
            var Config = readXML.ReturnEmailConfigRA_console().FirstOrDefault();


            if (Sellersdata.Count > 0)
            {
                Console.WriteLine("Sending emails...");
                foreach (var seller in Sellersdata)
                {
                    var returns = (from c in db.BI_Email_RA where (c.id_SalesRep == seller.Id) select c).ToList();
                    if (returns.Count() > 0)
                    {
                        //Existen datos
                        //Buscamos para tabla reason
                        var returns_header = (from b in db.BI_Email_RA_Head where (b.SlpCode == seller.Id) select b).OrderByDescending(b => b.Descr == "EXPIRED W99").ThenByDescending(b => b.Returns).ToList();

                        ReportDocument rd = new ReportDocument();




                        string reportpath = new Uri(
System.IO.Path.GetDirectoryName(
System.Reflection.Assembly.GetExecutingAssembly().CodeBase)
).LocalPath;//Path of the xml script  

                        reportpath = reportpath + "\\Reports\\rptReturnsAndAllowancesBySeller.rpt";


                        rd.Load(reportpath);


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


                        var filePathOriginal = new Uri(
System.IO.Path.GetDirectoryName(
System.Reflection.Assembly.GetExecutingAssembly().CodeBase)
).LocalPath;//Path of the xml script  

                        filePathOriginal = filePathOriginal + "\\Reports\\pdfReports";

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



                        var filename = "Return and Allowances " + seller.SalesRepresentative + ".pdf";
                        path = Path.Combine(filePathOriginal, filename);
                        rd.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, path);

                        PdfDocument doc = new PdfDocument();
                        doc.LoadFromFile(path);
                        Image img = doc.SaveAsImage(0);
                        var imagename = "Return and Allowances " + seller.SalesRepresentative + ".jpg";
                        pathimage = Path.Combine(filePathOriginal, imagename);
                        img.Save(pathimage);
                        doc.Close();


                        //Para enviar correos
                        try
                        {

                            MailMessage objeto_mail = new MailMessage();
                            SmtpClient client = new SmtpClient();
                            client.Port = 587;
                            client.Host = "smtp-mail.outlook.com";
                            client.Timeout = 100000;
                            client.DeliveryMethod = SmtpDeliveryMethod.Network;
                            client.UseDefaultCredentials = false;
                            client.Credentials = new System.Net.NetworkCredential(Config.Email.ToString(), Config.Password.ToString());

                            objeto_mail.IsBodyHtml = true;
                            objeto_mail.AlternateViews.Add(getEmbeddedImage(pathimage));


                            objeto_mail.From = new MailAddress(Config.Email);
                            objeto_mail.To.Add(new MailAddress(seller.Email.ToString()));
                            objeto_mail.Subject = "Returns and Allowances Report | " + seller.SalesRepresentative + " " + DateTime.Now.ToString("dddd").ToUpper();
                            foreach (var bcc in CcData)
                            {
                                MailAddress bccEmail = new MailAddress(bcc.Email.ToString());
                                objeto_mail.CC.Add(bccEmail);
                            }
                            MailAddress cc = new MailAddress(seller.Supervisor.ToString());
                            objeto_mail.CC.Add(cc);

                            //Enviamos el mensaje
                            client.Send(objeto_mail);


                            Console.WriteLine("Email sent successfully to : " + seller.SalesRepresentative.ToString() + " - " + seller.Email.ToString());
                        }

                        catch (Exception e)
                        {
                            Console.WriteLine("Email error, can't sent to : " + seller.SalesRepresentative.ToString() + " - " + seller.Email.ToString() + ". Error: " + e);
                        }


                    }
                    else
                    {
                        Console.WriteLine("No Data was found for " + seller.SalesRepresentative.ToString());
                    }
                }

            }
            else
            {
                Console.WriteLine("No Data was found...");
                Console.WriteLine("Exit program...");
            }

            Console.SetOut(oldOut);
            writer.Close();
            ostrm.Close();
        }
        private AlternateView getEmbeddedImage(String filePath)
        {
            LinkedResource res = new LinkedResource(filePath, MediaTypeNames.Image.Jpeg);
            res.ContentId = Guid.NewGuid().ToString();
            
            string htmlBody = @"<img src='cid:" + res.ContentId + @"'/>";
            AlternateView alternateView = AlternateView.CreateAlternateViewFromString(htmlBody,null, MediaTypeNames.Text.Html);

            alternateView.LinkedResources.Add(res);
            return alternateView;
        }
    }
}
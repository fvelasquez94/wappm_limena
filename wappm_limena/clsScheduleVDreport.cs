using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Web;
using wappm_limena.Helper;

namespace wappm_limena
{
    public class clsScheduleVDreport
    {
        public void sendMessage_console()
        {

            var date = DateTime.Now.ToString("yyyyMMdd");

            FileStream ostrm;
            StreamWriter writer;
            TextWriter oldOut = Console.Out;

            ostrm = new FileStream("./logs/vdlog_" + date + ".txt", FileMode.OpenOrCreate, FileAccess.Write);
            writer = new StreamWriter(ostrm);

            Console.SetOut(writer);

            XMLReader readXML = new XMLReader();
            Console.WriteLine("Auto Mail Sender for Vendors Data");
            Console.WriteLine("Returning Vendors list...");
            var Vendordata = readXML.ReturnListOfVendorsVD_console();
            Console.WriteLine("Returning CC list...");
            var CcData = readXML.ReturnListOfCcVD_console();


            Console.WriteLine("Getting email configuration...");
            var Config = readXML.ReturnEmailConfigVD_console().FirstOrDefault();

            if (Vendordata.Count > 0)
            {
                Console.WriteLine("Sending emails...");
                foreach (var seller in Vendordata)
                {
                    //var returns = (from c in db.BI_Email_RA where (c.id_SalesRep == seller.Id) select c).ToList();
                    //if (returns.Count() > 0)
                    //{
                    //    //Existen datos
                    //    //Buscamos para tabla reason
                    //    var returns_header = (from b in db.BI_Email_RA_Head where (b.SlpCode == seller.Id) select b).OrderByDescending(b => b.Descr == "EXPIRED W99").ThenByDescending(b => b.Returns).ToList();

                    //Para enviar correos
                    try
                    {

                        //MailMessage objeto_mail = new MailMessage();
                        //SmtpClient client = new SmtpClient();
                        //client.Port = 587;
                        //client.Host = "smtp-mail.outlook.com";
                        //client.Timeout = 100000;
                        //client.DeliveryMethod = SmtpDeliveryMethod.Network;
                        //client.UseDefaultCredentials = false;
                        //client.Credentials = new System.Net.NetworkCredential(Config.Email.ToString(), Config.Password.ToString());


                        //objeto_mail.From = new MailAddress(Config.Email);
                        //objeto_mail.To.Add(new MailAddress(seller.Email.ToString()));
                        //objeto_mail.Subject = "Vendor Data Report | " + seller.SalesRepresentative + " " + DateTime.Now.ToString("dddd").ToUpper();
                        //foreach (var bcc in CcData)
                        //{
                        //    MailAddress bccEmail = new MailAddress(bcc.Email.ToString());
                        //    objeto_mail.CC.Add(bccEmail);
                        //}
                        //MailAddress cc = new MailAddress(seller.Supervisor.ToString());
                        //objeto_mail.CC.Add(cc);

                        ////Enviamos el mensaje
                        //client.Send(objeto_mail);


                        Console.WriteLine("Email sent successfully to : " + seller.SalesRepresentative.ToString() + " - " + seller.Email.ToString());
                    }

                    catch (Exception e)
                    {
                        Console.WriteLine("Email error, can't sent to : " + seller.SalesRepresentative.ToString() + " - " + seller.Email.ToString() + ". Error: " + e);
                    }
                }
            }

            //    //}
            //    else
            //    {
            //        Console.WriteLine("No Data was found for " + seller.SalesRepresentative.ToString());
            //    }


            //}
            else
            {
                Console.WriteLine("No Data was found...");
                Console.WriteLine("Exit program...");
            }

            Console.SetOut(oldOut);
            writer.Close();
            ostrm.Close();
            Console.ReadKey();
        }

    }
}
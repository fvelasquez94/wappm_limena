using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Web;
using wappm_limena.Helper;
using wappm_limena.Models;
using LINQtoCSV;
using System.Data;
using System.ComponentModel;
using static System.Net.Mime.MediaTypeNames;

namespace wappm_limena
{
    public class clsScheduleVDreport
    {
        private DLI_PROEntities db = new DLI_PROEntities();
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
                Console.WriteLine("Returning Vendor List Data...");
                foreach (var vendor in Vendordata)
                {
                    List<view_VendorsData> lista_datos = new List<view_VendorsData>();

                   
                        lista_datos = (from b in db.view_VendorsData where (b.CODIGO_CLIENTE == vendor.Vendor_id) select b).ToList();
                    
                   
                    //filtramos

                    if (lista_datos.Count >= 0)
                    {
                        //Generamos el archivo csv
                        //EN ESTE CASO SERIA UN ARCHIVO DE EXCEL .XLSX
                        try
                        {
                            ////Propiedades
                            //CsvFileDescription outputFileDescription = new CsvFileDescription
                            //{
                            //    SeparatorChar = ',', // delimitamos por comas
                            //    FirstLineHasColumnNames = true // aquí se asigna si llevará header o no
                            //};
                            ////objeto
                            //Console.Write("\n Creating .csv file... \n");
                            //CsvContext cc = new CsvContext();
                            ////Salida

                            //cc.Write(lista_datos, "Vendor_data" + DateTime.Now.AddDays(-6).ToString("yyyyMMdd") + "_" + DateTime.Now.ToString("yyyyMMdd") + ".csv", outputFileDescription);


                            Selles.Helper.External.Models.ExcelUtlity obj = new Selles.Helper.External.Models.ExcelUtlity();


                            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(view_VendorsData));
                            DataTable table = new DataTable();
                            for (int i = 0; i < props.Count -3; i++)
                            {
                                PropertyDescriptor prop = props[i];
                                table.Columns.Add(prop.Name, prop.PropertyType);
                            }
                            object[] values = new object[props.Count -3];
                            foreach (var item in lista_datos)
                            {
                                for (int i = 0; i < values.Length; i++)
                                {
                                    values[i] = props[i].GetValue(item);
                                }
                                table.Rows.Add(values);
                            }

                            DataTable dt = table;

                            decimal n = DateTime.Now.DayOfYear;
                            decimal f = Math.Ceiling(n/7);
                            int weekNum = Convert.ToInt32(f) -1;

                            int LimenaWeek = weekNum + 9;

                            var filePathOriginal = new Uri(
System.IO.Path.GetDirectoryName(
System.Reflection.Assembly.GetExecutingAssembly().CodeBase)
).LocalPath;//Path of the xml script  

                            var path = filePathOriginal + "\\Reports\\excel";

                            var name = vendor.Vendor_name + "_DATA_WEEK_" + LimenaWeek + ".xlsx" ;
                            var pathforemail = Path.Combine(path, name);

                            obj.WriteDataTableToExcel(dt, "Details", pathforemail, "Details");


                            Console.WriteLine(pathforemail);

                            Console.WriteLine("File created successfully for " + vendor.Vendor_name);


                           


                            Console.WriteLine("Sending notifications for " + vendor.Vendor_name);

                            try
                            {

                                var todayLastWeek = DateTime.Today.AddDays(-7);


                                var sunday = todayLastWeek.AddDays(-(int)DateTime.Today.DayOfWeek);
                                var saturday = sunday.AddDays(6);
                                //Para enviar correos


                                MailMessage objeto_mail = new MailMessage();
                                SmtpClient client = new SmtpClient();
                                client.Port = 587;

                                client.Host = "smtp-mail.outlook.com";
                                client.Timeout = 100000;
                                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                                client.UseDefaultCredentials = false;
                                client.Credentials = new System.Net.NetworkCredential(Config.Email.ToString(), Config.Password.ToString());


                                objeto_mail.From = new MailAddress(Config.Email);
                                objeto_mail.To.Add(new MailAddress(vendor.Email.ToString()));
                                objeto_mail.Subject = vendor.Vendor_name + " DATA REPORT | " + sunday.ToShortDateString() + " - " + saturday.ToShortDateString();
                                objeto_mail.Attachments.Add(new Attachment(pathforemail));
                                foreach (var bcc in CcData)
                                {
                                    MailAddress bccEmail = new MailAddress(bcc.Email.ToString());
                                    objeto_mail.CC.Add(bccEmail);
                                }


                                //Enviamos el mensaje
                                client.Send(objeto_mail);

                                Console.WriteLine("Email successfully sent to: " + vendor.Vendor_name.ToString());


                            }
                            catch (Exception e)
                            {
                               Console.WriteLine("Email error, can't sent to : " + vendor.Vendor_name.ToString() + " - " + ". Error: " + e);
                            }

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("\n An error was handle for:  " + vendor.Vendor_name + ". " + ex.Message);

                            //// Enviamos el correo de notificación
                            //var viewsPath = Path.GetFullPath(@"Views\Emails");

                            //var engines = new ViewEngineCollection();
                            //engines.Add(new FileSystemRazorViewEngine(viewsPath));

                            //var service = new EmailService(engines);

                            //dynamic email = new Email("ftp_notification_error");

                            //service.Send(email);
                        }

                    }
                    else
                    {
                        //Realizamos solicitud de datos
                        Console.WriteLine("No data was found for " + vendor.Vendor_name);
                    }

                    //if (returns.Count() > 0)
                    //{
                    //    //Existen datos
                    //    //Buscamos para tabla reason
                    //    var returns_header = (from b in db.BI_Email_RA_Head where (b.SlpCode == seller.Id) select b).OrderByDescending(b => b.Descr == "EXPIRED W99").ThenByDescending(b => b.Returns).ToList();

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
                Console.WriteLine("No Vendor List Data was found...");
                Console.WriteLine("Exit program...");
            }

            Console.SetOut(oldOut);
            writer.Close();
            ostrm.Close();
         
        }


    }


}
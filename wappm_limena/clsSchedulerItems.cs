using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Web;
using wappm_limena.Helper;
using wappm_limena.Models;

namespace wappm_limena
{
    public class clsSchedulerItems
    {
        private DLI_PROEntities db = new DLI_PROEntities();
        public void sendMessage_console()
        {
            var date = DateTime.Now.ToString("yyyyMMdd");

            FileStream ostrm;
            StreamWriter writer;
            TextWriter oldOut = Console.Out;

            ostrm = new FileStream("./logs/items_" + date + ".txt", FileMode.OpenOrCreate, FileAccess.Write);
            writer = new StreamWriter(ostrm);

            Console.SetOut(writer);

            XMLReader readXML = new XMLReader();
            Console.WriteLine("Auto Mail Sender for Items Data");
            Console.WriteLine("Returning Limena Users list...");
            var userlist = readXML.ReturnListOfUsersItems_console();
            Console.WriteLine("Returning CC list...");
            var CcData = readXML.ReturnListOfCcItems_console();


            Console.WriteLine("Getting email configuration...");
            var Config = readXML.ReturnEmailConfigVD_console().FirstOrDefault();
            var allok = true;
            var pathforemail = "";
            var pathforemail2 = "";
            if (userlist.Count > 0)
            {


                List<V_ItemList> lista_datos = new List<V_ItemList>();

                Console.WriteLine("Getting data for ITEM LIST BY BRAND (V_ItemList)...");
                lista_datos = (from b in db.V_ItemList select b).ToList();


                    //filtramos

                    if (lista_datos.Count >= 0)
                    {
                        //Generamos el archivo csv
                        //EN ESTE CASO SERIA UN ARCHIVO DE EXCEL .XLSX
                        try
                        {
                            Selles.Helper.External.Models.ExcelUtlity obj2 = new Selles.Helper.External.Models.ExcelUtlity();


                            PropertyDescriptorCollection props2 = TypeDescriptor.GetProperties(typeof(V_ItemList));
                            DataTable table2 = new DataTable();
                            for (int i = 0; i < props2.Count - 3; i++)
                            {
                                PropertyDescriptor prop2 = props2[i];
                                table2.Columns.Add(prop2.Name, prop2.PropertyType);
                            }
                            object[] values2 = new object[props2.Count - 3];
                            foreach (var item in lista_datos)
                            {
                                for (int i = 0; i < values2.Length; i++)
                                {
                                    values2[i] = props2[i].GetValue(item);
                                }
                                table2.Rows.Add(values2);
                            }

                            DataTable dt2 = table2;

                            decimal n = DateTime.Now.DayOfYear;
                            decimal f = Math.Ceiling(n / 7);
                            int weekNum = Convert.ToInt32(f) - 1;

                            int LimenaWeek = weekNum + 9;

                            var filePathOriginal = new Uri(
System.IO.Path.GetDirectoryName(
System.Reflection.Assembly.GetExecutingAssembly().CodeBase)
).LocalPath;//Path of the xml script  

                            var path2 = filePathOriginal + "\\Reports\\excel";

                            var name2 = "ITEMLISTBYBRAND" + ".xlsx";
                            pathforemail = Path.Combine(path2, name2);

                            obj2.WriteDataTableToExcel(dt2, "Details", pathforemail, "Details");


                            Console.WriteLine(pathforemail);

                            Console.WriteLine("File created successfully");

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("\n An error was handle for ITEM LIST BY BRAND (V_ItemList)" + ex.Message);
                        allok = false;
                        }

                    }
                    else
                    {
                        //Realizamos solicitud de datos
                        Console.WriteLine("No data was found");
                    allok = false;
                }



                //Seleccionamos factores
                if (userlist.Count > 0)
                {


                    List<V_FACTORS> lista_datos2 = new List<V_FACTORS>();

                    Console.WriteLine("Getting data for ITEMS PRICE AND FACTORS (V_Factors)...");
                    lista_datos2 = (from b in db.V_FACTORS select b).ToList();


                    //filtramos

                    if (lista_datos2.Count >= 0)
                    {
                        //Generamos el archivo csv
                        //EN ESTE CASO SERIA UN ARCHIVO DE EXCEL .XLSX
                        try
                        {
                            Selles.Helper.External.Models.ExcelUtlity obj = new Selles.Helper.External.Models.ExcelUtlity();


                            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(V_FACTORS));
                            DataTable table = new DataTable();
                            for (int i = 0; i < props.Count - 3; i++)
                            {
                                PropertyDescriptor prop = props[i];
                                table.Columns.Add(prop.Name, prop.PropertyType);
                            }
                            object[] values = new object[props.Count - 3];
                            foreach (var item in lista_datos2)
                            {
                                for (int i = 0; i < values.Length; i++)
                                {
                                    values[i] = props[i].GetValue(item);
                                }
                                table.Rows.Add(values);
                            }

                            DataTable dt = table;

                            decimal n = DateTime.Now.DayOfYear;
                            decimal f = Math.Ceiling(n / 7);
                            int weekNum = Convert.ToInt32(f) - 1;

                            int LimenaWeek = weekNum + 9;

                            var filePathOriginal = new Uri(
System.IO.Path.GetDirectoryName(
System.Reflection.Assembly.GetExecutingAssembly().CodeBase)
).LocalPath;//Path of the xml script  

                            var path = filePathOriginal + "\\Reports\\excel";

                            var name = "ITEMSPRICEANDFACTORS" + ".xlsx";
                            pathforemail2 = Path.Combine(path, name);

                            obj.WriteDataTableToExcel(dt, "Details", pathforemail2, "Details");


                            Console.WriteLine(pathforemail2);

                            Console.WriteLine("File created successfully");

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("\n An error was handle for ITEMS PRICE AND FACTORS (V_Factors)" + ex.Message);
                            allok = false;
                        }

                    }
                    else
                    {
                        //Realizamos solicitud de datos
                        Console.WriteLine("No data was found");
                        allok = false;
                    }

                    if (allok == true) {

                        foreach (var vendor in userlist) {

                            Console.WriteLine("Sending notifications for " + vendor.Vendor_name);

                            try
                            {
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
                                objeto_mail.Subject = "Items and Prices" + " - " + DateTime.Today.ToShortDateString();
                                objeto_mail.Attachments.Add(new Attachment(pathforemail));
                                objeto_mail.Attachments.Add(new Attachment(pathforemail2));
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
                                Console.WriteLine("Email error, can't sent to: " + vendor.Vendor_name.ToString()  + " : " + e.Message);
                            }
                        }

                       

                    }


                }
                else
            {
                Console.WriteLine("No User List was found...");
                Console.WriteLine("Exit program...");
            }

            Console.SetOut(oldOut);
            writer.Close();
            ostrm.Close();

        }
    }
    }
    }

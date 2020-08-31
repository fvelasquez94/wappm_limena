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

            ostrm = new FileStream("./logs/vdlog_" + date + ".txt", FileMode.OpenOrCreate, FileAccess.Write);
            writer = new StreamWriter(ostrm);

            Console.SetOut(writer);

            XMLReader readXML = new XMLReader();
            Console.WriteLine("Auto Mail Sender for Items Data");
            Console.WriteLine("Returning Limena Users list...");
            var userlist = readXML.ReturnListOfVendorsVD_console();
            Console.WriteLine("Returning CC list...");
            var CcData = readXML.ReturnListOfCcVD_console();


            Console.WriteLine("Getting email configuration...");
            var Config = readXML.ReturnEmailConfigVD_console().FirstOrDefault();

            if (userlist.Count > 0)
            {
               
                    List<view_VendorsData> lista_datos = new List<view_VendorsData>();

                Console.WriteLine("Getting data for ITEM LIST BY BRAND (V_ItemList)...");
                //lista_datos = (from b in db.view_VendorsData where (b.CODIGO_CLIENTE == vendor.Vendor_id) select b).ToList();


                    //filtramos

                    if (lista_datos.Count >= 0)
                    {
                        //Generamos el archivo csv
                        //EN ESTE CASO SERIA UN ARCHIVO DE EXCEL .XLSX
                        try
                        {
                            Selles.Helper.External.Models.ExcelUtlity obj = new Selles.Helper.External.Models.ExcelUtlity();


                            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(view_VendorsData));
                            DataTable table = new DataTable();
                            for (int i = 0; i < props.Count - 3; i++)
                            {
                                PropertyDescriptor prop = props[i];
                                table.Columns.Add(prop.Name, prop.PropertyType);
                            }
                            object[] values = new object[props.Count - 3];
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
                            decimal f = Math.Ceiling(n / 7);
                            int weekNum = Convert.ToInt32(f) - 1;

                            int LimenaWeek = weekNum + 9;

                            var filePathOriginal = new Uri(
System.IO.Path.GetDirectoryName(
System.Reflection.Assembly.GetExecutingAssembly().CodeBase)
).LocalPath;//Path of the xml script  

                            var path = filePathOriginal + "\\Reports\\excel";

                            var name = "DAILYREPORT_" + "ITEMLISTBYBRAND" + ".xlsx";
                            var pathforemail = Path.Combine(path, name);

                            obj.WriteDataTableToExcel(dt, "Details", pathforemail, "Details");


                            Console.WriteLine(pathforemail);

                            Console.WriteLine("File created successfully");

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("\n An error was handle for ITEM LIST BY BRAND (V_ItemList)" + ex.Message);

                        }

                    }
                    else
                    {
                        //Realizamos solicitud de datos
                        Console.WriteLine("No data was found");
                    }



                    //Seleccionamos factores

                
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
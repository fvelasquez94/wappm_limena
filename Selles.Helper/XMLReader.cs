using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;//Add References -> Choose assemblies
using System.Data;
using wappm_limena.Helper.External.Models; // This is the External Model that I created.
using Selles.Helper.External.Models;

namespace wappm_limena.Helper
{
    public class XMLReader
    {
        /// <summary>  
        /// Return list of products from XML.  
        /// </summary>  
        /// <returns>List of products</returns>  
        /// 
        // SECCION PARA RETURNS AND ALLOWANCES
        public List<Sellers> ReturnListOfSellers()
        {
            string xmlData = HttpContext.Current.Server.MapPath("~/App_Data/SendMWRCI/SellersData.xml");//Path of the xml script  
            DataSet ds = new DataSet();//Using dataset to read xml file  
            ds.ReadXml(xmlData);
            var sellers = new List<Sellers>();
           sellers = (from rows in ds.Tables[0].AsEnumerable()
                        select new Sellers
                        {
                            Id = Convert.ToInt32(rows[0]),// Convert.ToInt32(rows[0].ToString()), //Convert row to int  
                            SalesRepresentative = rows[1].ToString(),
                            Email = rows[2].ToString(),
                            Names = rows[3].ToString(),
                            LastNames = rows[4].ToString(),
                            Supervisor = rows[5].ToString(),
                        }).ToList();
            return sellers;
        }




        public List<Cc> ReturnListOfCc()
        {
            string xmlData = HttpContext.Current.Server.MapPath("~/App_Data/SendMWRCI/CcData.xml");//Path of the xml script  
            DataSet ds = new DataSet();//Using dataset to read xml file  
            ds.ReadXml(xmlData);
            var Cc = new List<Cc>();
            Cc = (from rows in ds.Tables[0].AsEnumerable()
                       select new Cc
                       {
                           Id = rows[0].ToString(),// Convert.ToInt32(rows[0].ToString()), //Convert row to int  
                           Name = rows[1].ToString(),
                           Email = rows[2].ToString(),
                           Position = rows[3].ToString(),
                       }).ToList();
            return Cc;
        }

        public List<Config> ReturnEmailConfig()
        {
            string xmlData = HttpContext.Current.Server.MapPath("~/App_Data/SendMWRCI/Config.xml");//Path of the xml script  
            DataSet ds = new DataSet();//Using dataset to read xml file  
            ds.ReadXml(xmlData);
            var Config = new List<Config>();
            Config = (from rows in ds.Tables[0].AsEnumerable()
                  select new Config
                  {
                      Id = rows[0].ToString(),// Convert.ToInt32(rows[0].ToString()), //Convert row to int  
                      Email = rows[1].ToString(),
                      Password = rows[2].ToString(),
                      AllDays = Convert.ToInt32(rows[3]),
                      Hour = rows[4].ToString(),
                  }).ToList();
            return Config;
        }
        // FIN SECCION PARA RETURNS AND ALLOWANCES

        // SECCION SALES REPORT
        public List<Sellers> ReturnListOfSellersSR()
        {
            string xmlData = HttpContext.Current.Server.MapPath("~/App_Data/SalesReport/SellersData.xml");//Path of the xml script  
            DataSet ds = new DataSet();//Using dataset to read xml file  
            ds.ReadXml(xmlData);
            var sellers = new List<Sellers>();
            sellers = (from rows in ds.Tables[0].AsEnumerable()
                       select new Sellers
                       {
                           Id = Convert.ToInt32(rows[0]),// Convert.ToInt32(rows[0].ToString()), //Convert row to int  
                           SalesRepresentative = rows[1].ToString(),
                           Email = rows[2].ToString(),
                           Names = rows[3].ToString(),
                           LastNames = rows[4].ToString(),
                           Supervisor = rows[5].ToString(),
                       }).ToList();
            return sellers;
        }




        public List<Cc> ReturnListOfCcSR()
        {
            string xmlData = HttpContext.Current.Server.MapPath("~/App_Data/SalesReport/CcData.xml");//Path of the xml script  
            DataSet ds = new DataSet();//Using dataset to read xml file  
            ds.ReadXml(xmlData);
            var Cc = new List<Cc>();
            Cc = (from rows in ds.Tables[0].AsEnumerable()
                  select new Cc
                  {
                      Id = rows[0].ToString(),// Convert.ToInt32(rows[0].ToString()), //Convert row to int  
                      Name = rows[1].ToString(),
                      Email = rows[2].ToString(),
                      Position = rows[3].ToString(),
                  }).ToList();
            return Cc;
        }

        //EL UNICO QUE NO SE REPITE ES LA CONFIGURACION DEL EMAIL YA QUE ESTE NO PUEDE TENER MULTIPLES CONFIGURACIONES
        // FIN SECCION SALES REPORT

            //SE REPITE LA CONFIGURACION DE EMAIL EN CONSOLA PORQUE SE INSTALA EN DIFERENTE PATH


        // SECCION SALES REPORT para consola
        public List<Sellers> ReturnListOfSellersSR_console()
        {
            string xmlData = new Uri(
    System.IO.Path.GetDirectoryName(
        System.Reflection.Assembly.GetExecutingAssembly().CodeBase)
    ).LocalPath;//Path of the xml script  

            xmlData = xmlData + "\\SalesReport\\SellersData.xml";
            DataSet ds = new DataSet();//Using dataset to read xml file  
            ds.ReadXml(xmlData);
            var sellers = new List<Sellers>();
            sellers = (from rows in ds.Tables[0].AsEnumerable()
                       select new Sellers
                       {
                           Id = Convert.ToInt32(rows[0]),// Convert.ToInt32(rows[0].ToString()), //Convert row to int  
                           SalesRepresentative = rows[1].ToString(),
                           Email = rows[2].ToString(),
                           Names = rows[3].ToString(),
                           LastNames = rows[4].ToString(),
                           Supervisor = rows[5].ToString(),
                       }).ToList();
            return sellers;
        }

        public List<Cc> ReturnListOfCcSR_console()
        {
            string xmlData = new Uri(
System.IO.Path.GetDirectoryName(
System.Reflection.Assembly.GetExecutingAssembly().CodeBase)
).LocalPath;//Path of the xml script  

            xmlData = xmlData + "\\SalesReport\\CcData.xml";
            DataSet ds = new DataSet();//Using dataset to read xml file  
            ds.ReadXml(xmlData);
            var Cc = new List<Cc>();
            Cc = (from rows in ds.Tables[0].AsEnumerable()
                  select new Cc
                  {
                      Id = rows[0].ToString(),// Convert.ToInt32(rows[0].ToString()), //Convert row to int  
                      Name = rows[1].ToString(),
                      Email = rows[2].ToString(),
                      Position = rows[3].ToString(),
                  }).ToList();
            return Cc;
        }

        public List<Config> ReturnEmailConfig_console()
        {
            string xmlData = new Uri(
System.IO.Path.GetDirectoryName(
System.Reflection.Assembly.GetExecutingAssembly().CodeBase)
).LocalPath;//Path of the xml script  

            xmlData = xmlData + "\\SalesReport\\Config.xml";
            DataSet ds = new DataSet();//Using dataset to read xml file  
            ds.ReadXml(xmlData);
            var Config = new List<Config>();
            Config = (from rows in ds.Tables[0].AsEnumerable()
                      select new Config
                      {
                          Id = rows[0].ToString(),// Convert.ToInt32(rows[0].ToString()), //Convert row to int  
                          Email = rows[1].ToString(),
                          Password = rows[2].ToString(),
                          AllDays = Convert.ToInt32(rows[3]),
                          Hour = rows[4].ToString(),
                      }).ToList();
            return Config;
        }

        // SECCION RETURNS AND ALLOWANCES para consola
        public List<Sellers> ReturnListOfSellers_console()
        {
            string xmlData = new Uri(
    System.IO.Path.GetDirectoryName(
        System.Reflection.Assembly.GetExecutingAssembly().CodeBase)
    ).LocalPath;//Path of the xml script  

            xmlData = xmlData + "\\SendMWRCI\\SellersData.xml";
            DataSet ds = new DataSet();//Using dataset to read xml file  
            ds.ReadXml(xmlData);
            var sellers = new List<Sellers>();
            sellers = (from rows in ds.Tables[0].AsEnumerable()
                       select new Sellers
                       {
                           Id = Convert.ToInt32(rows[0]),// Convert.ToInt32(rows[0].ToString()), //Convert row to int  
                           SalesRepresentative = rows[1].ToString(),
                           Email = rows[2].ToString(),
                           Names = rows[3].ToString(),
                           LastNames = rows[4].ToString(),
                           Supervisor = rows[5].ToString(),
                       }).ToList();
            return sellers;
        }

        public List<Cc> ReturnListOfCc_console()
        {
            string xmlData = new Uri(
System.IO.Path.GetDirectoryName(
System.Reflection.Assembly.GetExecutingAssembly().CodeBase)
).LocalPath;//Path of the xml script  

            xmlData = xmlData + "\\SendMWRCI\\CcData.xml";
            DataSet ds = new DataSet();//Using dataset to read xml file  
            ds.ReadXml(xmlData);
            var Cc = new List<Cc>();
            Cc = (from rows in ds.Tables[0].AsEnumerable()
                  select new Cc
                  {
                      Id = rows[0].ToString(),// Convert.ToInt32(rows[0].ToString()), //Convert row to int  
                      Name = rows[1].ToString(),
                      Email = rows[2].ToString(),
                      Position = rows[3].ToString(),
                  }).ToList();
            return Cc;
        }
        /// <summary>
        /// RAAAAA
        /// </summary>
        /// <returns></returns>
        public List<Config> ReturnEmailConfigRA_console()
        {
            string xmlData = new Uri(
System.IO.Path.GetDirectoryName(
System.Reflection.Assembly.GetExecutingAssembly().CodeBase)
).LocalPath;//Path of the xml script  

            xmlData = xmlData + "\\SendMWRCI\\Config.xml";
            DataSet ds = new DataSet();//Using dataset to read xml file  
            ds.ReadXml(xmlData);
            var Config = new List<Config>();
            Config = (from rows in ds.Tables[0].AsEnumerable()
                      select new Config
                      {
                          Id = rows[0].ToString(),// Convert.ToInt32(rows[0].ToString()), //Convert row to int  
                          Email = rows[1].ToString(),
                          Password = rows[2].ToString(),
                          AllDays = Convert.ToInt32(rows[3]),
                          Hour = rows[4].ToString(),
                      }).ToList();
            return Config;
        }

        // SECCION PARA Accounts Receivable
        public List<Sellers> ReturnListOfSellersAR()
        {
            string xmlData = HttpContext.Current.Server.MapPath("~/App_Data/BI_Accounts_receivable/SellersData.xml");//Path of the xml script  
            DataSet ds = new DataSet();//Using dataset to read xml file  
            ds.ReadXml(xmlData);
            var sellers = new List<Sellers>();
            sellers = (from rows in ds.Tables[0].AsEnumerable()
                       select new Sellers
                       {
                           Id = Convert.ToInt32(rows[0]),// Convert.ToInt32(rows[0].ToString()), //Convert row to int  
                           SalesRepresentative = rows[1].ToString(),
                           Email = rows[2].ToString(),
                           Names = rows[3].ToString(),
                           LastNames = rows[4].ToString(),
                           Supervisor = rows[5].ToString(),
                       }).ToList();
            return sellers;
        }




        public List<Cc> ReturnListOfCcAR()
        {
            string xmlData = HttpContext.Current.Server.MapPath("~/App_Data/BI_Accounts_receivable/CcData.xml");//Path of the xml script  
            DataSet ds = new DataSet();//Using dataset to read xml file  
            ds.ReadXml(xmlData);
            var Cc = new List<Cc>();
            Cc = (from rows in ds.Tables[0].AsEnumerable()
                  select new Cc
                  {
                      Id = rows[0].ToString(),// Convert.ToInt32(rows[0].ToString()), //Convert row to int  
                      Name = rows[1].ToString(),
                      Email = rows[2].ToString(),
                      Position = rows[3].ToString(),
                  }).ToList();
            return Cc;
        }

        // FIN SECCION PARA Accounts Receivable
        // SECCION Accounts Receivable para consola
        public List<Sellers> ReturnListOfSellersAR_console()
        {
            string xmlData = new Uri(
    System.IO.Path.GetDirectoryName(
        System.Reflection.Assembly.GetExecutingAssembly().CodeBase)
    ).LocalPath;//Path of the xml script  

            xmlData = xmlData + "\\BI_Accounts_receivable\\SellersData.xml";
            DataSet ds = new DataSet();//Using dataset to read xml file  
            ds.ReadXml(xmlData);
            var sellers = new List<Sellers>();
            sellers = (from rows in ds.Tables[0].AsEnumerable()
                       select new Sellers
                       {
                           Id = Convert.ToInt32(rows[0]),// Convert.ToInt32(rows[0].ToString()), //Convert row to int  
                           SalesRepresentative = rows[1].ToString(),
                           Email = rows[2].ToString(),
                           Names = rows[3].ToString(),
                           LastNames = rows[4].ToString(),
                           Supervisor = rows[5].ToString(),
                       }).ToList();
            return sellers;
        }

        public List<Cc> ReturnListOfCcAR_console()
        {
            string xmlData = new Uri(
System.IO.Path.GetDirectoryName(
System.Reflection.Assembly.GetExecutingAssembly().CodeBase)
).LocalPath;//Path of the xml script  

            xmlData = xmlData + "\\BI_Accounts_receivable\\CcData.xml";
            DataSet ds = new DataSet();//Using dataset to read xml file  
            ds.ReadXml(xmlData);
            var Cc = new List<Cc>();
            Cc = (from rows in ds.Tables[0].AsEnumerable()
                  select new Cc
                  {
                      Id = rows[0].ToString(),// Convert.ToInt32(rows[0].ToString()), //Convert row to int  
                      Name = rows[1].ToString(),
                      Email = rows[2].ToString(),
                      Position = rows[3].ToString(),
                  }).ToList();
            return Cc;
        }

        public List<Config> ReturnEmailConfigAR_console()
        {
            string xmlData = new Uri(
System.IO.Path.GetDirectoryName(
System.Reflection.Assembly.GetExecutingAssembly().CodeBase)
).LocalPath;//Path of the xml script  

            xmlData = xmlData + "\\BI_Accounts_receivable\\Config.xml";
            DataSet ds = new DataSet();//Using dataset to read xml file  
            ds.ReadXml(xmlData);
            var Config = new List<Config>();
            Config = (from rows in ds.Tables[0].AsEnumerable()
                      select new Config
                      {
                          Id = rows[0].ToString(),// Convert.ToInt32(rows[0].ToString()), //Convert row to int  
                          Email = rows[1].ToString(),
                          Password = rows[2].ToString(),
                          AllDays = Convert.ToInt32(rows[3]),
                          Hour = rows[4].ToString(),
                      }).ToList();
            return Config;
        }

        //FIN DE SECCION Accounts Receivable


        /// <summary>
        /// VENDORS LIST VD
        /// </summary>
        /// <returns></returns>
        public List<Config> ReturnEmailConfigVD()
        {
            string xmlData = new Uri(
System.IO.Path.GetDirectoryName(
System.Reflection.Assembly.GetExecutingAssembly().CodeBase)
).LocalPath;//Path of the xml script  

            xmlData = xmlData + "\\SendMWRCI\\Config.xml";
            DataSet ds = new DataSet();//Using dataset to read xml file  
            ds.ReadXml(xmlData);
            var Config = new List<Config>();
            Config = (from rows in ds.Tables[0].AsEnumerable()
                      select new Config
                      {
                          Id = rows[0].ToString(),// Convert.ToInt32(rows[0].ToString()), //Convert row to int  
                          Email = rows[1].ToString(),
                          Password = rows[2].ToString(),
                          AllDays = Convert.ToInt32(rows[3]),
                          Hour = rows[4].ToString(),
                      }).ToList();
            return Config;
        }

        // SECCION PARA VENDORS LIST
        public List<Vendors> ReturnListOfVendorsVD()
        {
            string xmlData = HttpContext.Current.Server.MapPath("~/App_Data/Vendors_data/VendorsData.xml");//Path of the xml script  
            DataSet ds = new DataSet();//Using dataset to read xml file  
            ds.ReadXml(xmlData);
            var sellers = new List<Vendors>();
            sellers = (from rows in ds.Tables[0].AsEnumerable()
                       select new Vendors
                       {
                           Vendor_id = rows[0].ToString(),// Convert.ToInt32(rows[0].ToString()), //Convert row to int  
                           Vendor_name = rows[1].ToString(),
                           Email = rows[2].ToString(),
                           Names = rows[3].ToString(),
                           LastNames = rows[4].ToString(),
                           Position = rows[5].ToString(),
                       }).ToList();
            return sellers;
        }




        public List<Cc_vendorsdata> ReturnListOfCcVD()
        {
            string xmlData = HttpContext.Current.Server.MapPath("~/App_Data/Vendors_data/CcData.xml");//Path of the xml script  
            DataSet ds = new DataSet();//Using dataset to read xml file  
            ds.ReadXml(xmlData);
            var Cc = new List<Cc_vendorsdata>();
            Cc = (from rows in ds.Tables[0].AsEnumerable()
                  select new Cc_vendorsdata
                  {
                      Id = rows[0].ToString(),// Convert.ToInt32(rows[0].ToString()), //Convert row to int  
                      Name = rows[1].ToString(),
                      Email = rows[2].ToString(),
                      Position = rows[3].ToString(),
                      Vendor_id = rows[4].ToString(),
                  }).ToList();
            return Cc;
        }

        // FIN SECCION PARA Accounts Receivable
        // SECCION Accounts Receivable para consola
        public List<Vendors> ReturnListOfVendorsVD_console()
        {
            string xmlData = new Uri(
    System.IO.Path.GetDirectoryName(
        System.Reflection.Assembly.GetExecutingAssembly().CodeBase)
    ).LocalPath;//Path of the xml script  

            xmlData = xmlData + "\\Vendors_data\\VendorsData.xml";
            DataSet ds = new DataSet();//Using dataset to read xml file  
            ds.ReadXml(xmlData);
            var vendors = new List<Vendors>();
            vendors = (from rows in ds.Tables[0].AsEnumerable()
                       select new Vendors
                       {
                           Vendor_id = rows[0].ToString(),// Convert.ToInt32(rows[0].ToString()), //Convert row to int  
                           Vendor_name = rows[1].ToString(),
                           Email = rows[2].ToString(),
                           Names = rows[3].ToString(),
                           LastNames = rows[4].ToString(),
                           Position = rows[5].ToString(),
                       }).ToList();
            return vendors;
        }

        public List<Cc_vendorsdata> ReturnListOfCcVD_console()
        {
            string xmlData = new Uri(
System.IO.Path.GetDirectoryName(
System.Reflection.Assembly.GetExecutingAssembly().CodeBase)
).LocalPath;//Path of the xml script  

            xmlData = xmlData + "\\Vendors_data\\CcData.xml";
            DataSet ds = new DataSet();//Using dataset to read xml file  
            ds.ReadXml(xmlData);
            var Cc = new List<Cc_vendorsdata>();
            Cc = (from rows in ds.Tables[0].AsEnumerable()
                  select new Cc_vendorsdata
                  {
                      Id = rows[0].ToString(),// Convert.ToInt32(rows[0].ToString()), //Convert row to int  
                      Name = rows[1].ToString(),
                      Email = rows[2].ToString(),
                      Position = rows[3].ToString(),
                      Vendor_id = rows[4].ToString(),
                  }).ToList();
            return Cc;
        }


        // SECCION Accounts Receivable para consola
        public List<Vendors> ReturnListOfUsersItems_console()
        {
            string xmlData = new Uri(
    System.IO.Path.GetDirectoryName(
        System.Reflection.Assembly.GetExecutingAssembly().CodeBase)
    ).LocalPath;//Path of the xml script  

            xmlData = xmlData + "\\Vendors_data\\VendorsData.xml";
            DataSet ds = new DataSet();//Using dataset to read xml file  
            ds.ReadXml(xmlData);
            var vendors = new List<Vendors>();
            vendors = (from rows in ds.Tables[0].AsEnumerable()
                       select new Vendors
                       {
                           Vendor_id = rows[0].ToString(),// Convert.ToInt32(rows[0].ToString()), //Convert row to int  
                           Vendor_name = rows[1].ToString(),
                           Email = rows[2].ToString(),
                           Names = rows[3].ToString(),
                           LastNames = rows[4].ToString(),
                           Position = rows[5].ToString(),
                       }).ToList();
            return vendors;
        }

        public List<Cc_vendorsdata> ReturnListOfCcItems_console()
        {
            string xmlData = new Uri(
System.IO.Path.GetDirectoryName(
System.Reflection.Assembly.GetExecutingAssembly().CodeBase)
).LocalPath;//Path of the xml script  

            xmlData = xmlData + "\\Vendors_data\\CcData.xml";
            DataSet ds = new DataSet();//Using dataset to read xml file  
            ds.ReadXml(xmlData);
            var Cc = new List<Cc_vendorsdata>();
            Cc = (from rows in ds.Tables[0].AsEnumerable()
                  select new Cc_vendorsdata
                  {
                      Id = rows[0].ToString(),// Convert.ToInt32(rows[0].ToString()), //Convert row to int  
                      Name = rows[1].ToString(),
                      Email = rows[2].ToString(),
                      Position = rows[3].ToString(),
                      Vendor_id = rows[4].ToString(),
                  }).ToList();
            return Cc;
        }


        public List<Config> ReturnEmailConfigItems_console()
        {
            string xmlData = new Uri(
System.IO.Path.GetDirectoryName(
System.Reflection.Assembly.GetExecutingAssembly().CodeBase)
).LocalPath;//Path of the xml script  

            xmlData = xmlData + "\\Vendors_data\\Config.xml";
            DataSet ds = new DataSet();//Using dataset to read xml file  
            ds.ReadXml(xmlData);
            var Config = new List<Config>();
            Config = (from rows in ds.Tables[0].AsEnumerable()
                      select new Config
                      {
                          Id = rows[0].ToString(),// Convert.ToInt32(rows[0].ToString()), //Convert row to int  
                          Email = rows[1].ToString(),
                          Password = rows[2].ToString(),
                          AllDays = Convert.ToInt32(rows[3]),
                          Hour = rows[4].ToString(),
                      }).ToList();
            return Config;
        }

        public List<Config> ReturnEmailConfigVD_console()
        {
            string xmlData = new Uri(
System.IO.Path.GetDirectoryName(
System.Reflection.Assembly.GetExecutingAssembly().CodeBase)
).LocalPath;//Path of the xml script  

            xmlData = xmlData + "\\Vendors_data\\Config.xml";
            DataSet ds = new DataSet();//Using dataset to read xml file  
            ds.ReadXml(xmlData);
            var Config = new List<Config>();
            Config = (from rows in ds.Tables[0].AsEnumerable()
                      select new Config
                      {
                          Id = rows[0].ToString(),// Convert.ToInt32(rows[0].ToString()), //Convert row to int  
                          Email = rows[1].ToString(),
                          Password = rows[2].ToString(),
                          AllDays = Convert.ToInt32(rows[3]),
                          Hour = rows[4].ToString(),
                      }).ToList();
            return Config;
        }

        //FIN DE SECCION VENDORS DATA

    }


}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;//Add References -> Choose assemblies
using System.Data;
using wappm_limena.Helper.External.Models; // This is the External Model that I created.


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

    }


}

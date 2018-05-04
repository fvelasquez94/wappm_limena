using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wappm_limena;
using wappm_limena.Helper;
using wappm_limena.Helper.External.Models;
using wappm_limena.Models;

namespace schedulerControlforSalesReport
{
    class Program
    {
        
        static void Main(string[] args)
        {
            clsScheduleSalesReport cls = new clsScheduleSalesReport();
            cls.sendMessage_console();


        }
    }
}

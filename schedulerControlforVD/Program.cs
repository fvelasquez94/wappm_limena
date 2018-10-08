using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wappm_limena;
namespace schedulerControlforVD
{
    class Program
    {
        static void Main(string[] args)
        {
            clsScheduleVDreport cls = new clsScheduleVDreport();
            cls.sendMessage_console();
        }
    }
}

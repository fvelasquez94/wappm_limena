using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Selles.Helper.External.Models
{
    [Serializable]
    [XmlRoot("Vendors"), XmlType("Vendors")]
    public class Vendors
    {
        public string Vendor_id { get; set; }
        public string Vendor_name { get; set; }
        public string Email { get; set; }
        public string Names { get; set; }
        public string LastNames { get; set; }
        public string Position { get; set; }
    }
}

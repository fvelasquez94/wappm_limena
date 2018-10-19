using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization; //Add  

namespace wappm_limena.Helper.External.Models
{
        /// <summary>  
        /// This class is being serialized to XML.  
        /// </summary>  
        [Serializable]
        [XmlRoot("Sellers"), XmlType("Sellers")]
        public class Sellers
        {
            public int Id { get; set; }
            public string SalesRepresentative { get; set; }
            public string Email { get; set; }
            public string Names { get; set; }
            public string LastNames{ get; set; }
            public string Supervisor { get; set; }
        
    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace wappm_limena.Helper.External.Models
{
    /// <summary>  
    /// This class is being serialized to XML.  
    /// </summary>  
    [Serializable]
    [XmlRoot("Config"), XmlType("Config")]
    public class Config
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public int AllDays { get; set; }

        public string Hour { get; set; }
    }
}

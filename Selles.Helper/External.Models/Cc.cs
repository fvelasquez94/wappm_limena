﻿using System;
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
    [XmlRoot("Cc"), XmlType("Cc")]
    public class Cc
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Position { get; set; }
    }
}
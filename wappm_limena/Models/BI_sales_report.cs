//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace wappm_limena.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class BI_Sales_Report
    {
        public long ID_generico { get; set; }
        public string CardCode { get; set; }
        public string CardName { get; set; }
        public string Route { get; set; }
        public int SlpCode { get; set; }
        public string SlpName { get; set; }
        public decimal Budget { get; set; }
        public decimal Total { get; set; }
        public decimal Logro { get; set; }
        public decimal Diferencia { get; set; }
        public string WeekType { get; set; }
        public string Dia { get; set; }
        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{0:hh.mm}")]
        public System.TimeSpan Time { get; set; }
    }
}

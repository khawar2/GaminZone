//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace GamingZone.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Product
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public double UnitPrice { get; set; }
        public string Seller { get; set; }
        public string Color { get; set; }
        public string Date { get; set; }
        public int SubcategoryID { get; set; }
    
        public virtual SubCategory SubCategory { get; set; }
    }
}
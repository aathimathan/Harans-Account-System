using System;
using System.Xml.Serialization;

namespace HaranInvoiceSoftware.Models
{
    [Serializable]
    public class FoodItem
    {
        [XmlElement]
        public string Description { get; set; } = "";
        
        [XmlElement]
        public string Note { get; set; } = "";
        
        [XmlElement]
        public decimal Price { get; set; } = 0m;
    }
}
using System;
using System.Xml.Serialization;

namespace HaranInvoiceSoftware.Models
{
    [Serializable]
    public class Customer
    {
        [XmlElement]
        public string Name { get; set; } = "";
        
        [XmlElement]
        public string CompanyName { get; set; } = "";
        
        [XmlElement]
        public string Address { get; set; } = "";
        
        [XmlElement]
        public string Phone { get; set; } = "";
        
        [XmlElement]
        public string Email { get; set; } = "";
    }
}
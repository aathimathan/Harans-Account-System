using System;
using System.Xml.Serialization;

namespace HaranInvoiceSoftware.Models
{
    [Serializable]
    public class Company
    {
        [XmlElement]
        public string Name { get; set; } = "NALLUR RESIDENCE";
        
        [XmlElement]
        public string Address { get; set; } = "956 Point Pedro Rd, Nallur Jaffna";
        
        [XmlElement]
        public string Phone { get; set; } = "077-244-6241";
        
        [XmlElement]
        public string Email { get; set; } = "harangovindaraj@gmail.com";
        
        [XmlElement]
        public string Logo { get; set; } = ""; // Path to logo image
    }
}
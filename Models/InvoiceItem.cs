using System;
using System.Xml.Serialization;

namespace HaranInvoiceSoftware.Models
{
    [Serializable]
    public class InvoiceItem
    {
        [XmlElement]
        public string Description { get; set; } = "";
        
        [XmlElement]
        public DateTime CheckIn { get; set; } = DateTime.Now;
        
        [XmlElement]
        public DateTime CheckOut { get; set; } = DateTime.Now.AddDays(1);
        
        [XmlElement]
        public int NumberOfNights { get; set; } = 1;
        
        [XmlElement]
        public decimal PricePerNight { get; set; } = 0m;
        
        [XmlElement]
        public decimal Total { get; set; } = 0m;

        public void CalculateTotal()
        {
            Total = NumberOfNights * PricePerNight;
        }
    }
}
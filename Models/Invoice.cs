using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace HaranInvoiceSoftware.Models
{
    [Serializable]
    [XmlRoot("Invoice")]
    public class Invoice
    {
        [XmlElement]
        public string InvoiceNumber { get; set; } = "";
        
        [XmlElement]
        public DateTime InvoiceDate { get; set; } = DateTime.Now;
        
        [XmlElement]
        public Company Company { get; set; } = new Company();
        
        [XmlElement]
        public Customer Customer { get; set; } = new Customer();
        
        [XmlArray("Items")]
        [XmlArrayItem("Item")]
        public List<InvoiceItem> Items { get; set; } = new List<InvoiceItem>();
        
        [XmlArray("FoodItems")]
        [XmlArrayItem("FoodItem")]
        public List<FoodItem> FoodItems { get; set; } = new List<FoodItem>();
        
        [XmlElement]
        public decimal Subtotal { get; set; } = 0m;
        
        [XmlElement]
        public decimal TaxRate { get; set; } = 0.08m; // 8% tax
        
        [XmlElement]
        public decimal TaxAmount { get; set; } = 0m;
        
        [XmlElement]
        public decimal Other { get; set; } = 0m;
        
        [XmlElement]
        public decimal Total { get; set; } = 0m;
        
        [XmlElement]
        public decimal Paid { get; set; } = 0m;
        
        [XmlElement]
        public decimal TotalDue { get; set; } = 0m;
        
        [XmlElement]
        public decimal Advance { get; set; } = 0m;
        
        [XmlElement]
        public string Notes { get; set; } = "";
        
        [XmlElement]
        public string FileName { get; set; } = "";

    // Currency code for this invoice (e.g. LKR, USD). Defaults to LKR (Sri Lankan Rupees)
    [XmlElement]
    public string CurrencyCode { get; set; } = "LKR";

        public void CalculateTotals()
        {
            Subtotal = Items.Sum(item => item.Total) + FoodItems.Sum(food => food.Price);
            TaxAmount = Subtotal * TaxRate;
            Total = Subtotal + TaxAmount + Other;
            TotalDue = Total - Paid - Advance;

            // Ensure currency code always has a sensible default when loading older XML without the field
            if (string.IsNullOrWhiteSpace(CurrencyCode))
            {
                CurrencyCode = "LKR"; // backward compatibility
            }
        }
    }
}
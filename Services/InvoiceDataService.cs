using System;
using System.IO;
using System.Xml.Serialization;
using System.Xml;
using HaranInvoiceSoftware.Models;

namespace HaranInvoiceSoftware.Services
{
    public class InvoiceDataService
    {
        private readonly string _invoicesDirectory;
        private readonly string _lastInvoiceFile;

        public InvoiceDataService()
        {
            _invoicesDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Invoices");
            _lastInvoiceFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "last_invoice.txt");
            
            // Ensure invoices directory exists
            if (!Directory.Exists(_invoicesDirectory))
            {
                Directory.CreateDirectory(_invoicesDirectory);
            }
        }

        public void SaveInvoice(Invoice invoice)
        {
            try
            {
                if (string.IsNullOrEmpty(invoice.InvoiceNumber))
                {
                    invoice.InvoiceNumber = GenerateInvoiceNumber();
                }

                string filePath;
                
                // Use stored filename if available, otherwise generate one
                if (!string.IsNullOrEmpty(invoice.FileName))
                {
                    filePath = invoice.FileName;
                }
                else
                {
                    string fileName = $"Invoice_{invoice.InvoiceNumber}_{invoice.InvoiceDate:yyyyMMdd}.xml";
                    filePath = Path.Combine(_invoicesDirectory, fileName);
                    invoice.FileName = filePath; // Store the generated filename
                }

                XmlSerializer serializer = new XmlSerializer(typeof(Invoice));
                using (FileStream stream = new FileStream(filePath, FileMode.Create))
                {
                    serializer.Serialize(stream, invoice);
                }

                // Save as last worked invoice
                File.WriteAllText(_lastInvoiceFile, filePath);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error saving invoice: {ex.Message}");
            }
        }

        public Invoice LoadInvoice(string filePath)
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    return new Invoice();
                }

                XmlSerializer serializer = new XmlSerializer(typeof(Invoice));
                using (FileStream stream = new FileStream(filePath, FileMode.Open))
                {
                    var invoice = (Invoice)serializer.Deserialize(stream);
                    invoice.FileName = filePath; // Store the loaded file path
                    return invoice;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error loading invoice: {ex.Message}");
            }
        }

        public Invoice LoadLastInvoice()
        {
            try
            {
                if (File.Exists(_lastInvoiceFile))
                {
                    string lastInvoicePath = File.ReadAllText(_lastInvoiceFile);
                    if (File.Exists(lastInvoicePath))
                    {
                        return LoadInvoice(lastInvoicePath);
                    }
                }
                return new Invoice();
            }
            catch
            {
                return new Invoice();
            }
        }

        public string[] GetInvoiceFiles()
        {
            try
            {
                return Directory.GetFiles(_invoicesDirectory, "*.xml");
            }
            catch
            {
                return new string[0];
            }
        }

        private string GenerateInvoiceNumber()
        {
            return DateTime.Now.ToString("yyyyMMddHHmmss");
        }

        public void SaveCurrentInvoicePath(string filePath)
        {
            try
            {
                File.WriteAllText(_lastInvoiceFile, filePath);
            }
            catch
            {
                // Ignore errors when saving last invoice path
            }
        }
    }
}
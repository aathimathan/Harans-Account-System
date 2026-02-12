using System;
using System.IO;
using System.Linq;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Kernel.Colors;
using iText.Layout.Borders;
using iText.Kernel.Font;
using iText.IO.Font.Constants;
using iText.IO.Image;
using iText.Kernel.Geom;
using HaranInvoiceSoftware.Models;
using HaranInvoiceSoftware.Utils;

namespace HaranInvoiceSoftware.Services
{
    public class PdfExportService
    {
        private static string NormalizeLineBreaks(string text)
        {
            if (string.IsNullOrEmpty(text)) return string.Empty;

            // Handle cases where the text contains the *literal* characters "\r\n" (backslash-r-backslash-n)
            // instead of actual CRLF characters.
            text = text.Replace("\\r\\n", "\n")
                       .Replace("\\n", "\n")
                       .Replace("\\r", "\n");

            // Normalize actual CRLF/CR to LF for consistent PDF rendering.
            text = text.Replace("\r\n", "\n").Replace("\r", "\n");

            return text;
        }

        public void ExportToPdf(Invoice invoice, string filePath)
        {
            try
            {
                using (var writer = new PdfWriter(filePath))
                {
                    using (var pdf = new PdfDocument(writer))
                    {
                        // Set A4 page size explicitly
                        pdf.SetDefaultPageSize(iText.Kernel.Geom.PageSize.A4);

                        var document = new Document(pdf);

                        // Reduced margins for more content space
                        document.SetMargins(20, 20, 20, 20);

                        // Add header with company information
                        AddHeader(document, invoice.Company);

                        // Add invoice title and details
                        AddInvoiceHeader(document, invoice);

                        // Add customer information
                        AddCustomerInfo(document, invoice.Customer);

                        // Add invoice items table only if there are room records
                        if (invoice.Items.Any(i => !string.IsNullOrEmpty(i.Description)))
                        {
                            AddInvoiceItemsTable(document, invoice);
                        }

                        // Add food items table if items exist
                        if (invoice.FoodItems.Any(f => !string.IsNullOrEmpty(f.Description)))
                        {
                            AddFoodItemsTable(document, invoice);
                        }

                        // Add totals section
                        AddTotalsSection(document, invoice);

                        document.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error exporting to PDF: {ex.Message}");
            }
        }

        private void AddHeader(Document document, Company company)
        {
            // Create compact header table with logo and company info - adjusted proportions
            var headerTable = new Table(new float[] { 1f, 3f }).UseAllAvailableWidth();
            headerTable.SetMarginBottom(15)
                       .SetPadding(0)
                       .SetBorderCollapse(BorderCollapsePropertyValue.SEPARATE)
                       .SetHorizontalBorderSpacing(0)
                       .SetVerticalBorderSpacing(0);

            // Logo cell - smaller for A4 compactness
            var logoCell = new Cell();
            var logoPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets", "nallur-residence-logo.png");

            if (File.Exists(logoPath))
            {
                try
                {
                    var logoData = ImageDataFactory.Create(logoPath);
                    var logo = new Image(logoData);
                    logo.SetWidth(70);  // Further reduced size
                    logo.SetHeight(85);
                    logoCell.Add(logo);
                }
                catch
                {
                    // Fallback to text if image fails to load
                    logoCell.Add(new Paragraph("NALLUR\nRESIDENCE\n(PVT) LTD")
                        .SetFontSize(10)
                        .SetBold()
                        .SetTextAlignment(TextAlignment.CENTER));
                }
            }
            else
            {
                // Fallback when logo file doesn't exist
                logoCell.Add(new Paragraph("NALLUR\nRESIDENCE\n(PVT) LTD")
                    .SetFontSize(10)
                    .SetBold()
                    .SetTextAlignment(TextAlignment.CENTER));
            }

            logoCell.SetBorder(Border.NO_BORDER)
                   .SetVerticalAlignment(VerticalAlignment.TOP)
                   .SetTextAlignment(TextAlignment.CENTER)
                   .SetPadding(0);

            // Company details cell - compact layout with minimal spacing
            var companyInfoCell = new Cell()
                .Add(new Paragraph(company.Name)
                    .SetFontSize(18)
                    .SetBold()
                    .SetMarginTop(0)
                    .SetMarginBottom(3)
                    .SetPaddingTop(0))
                .Add(new Paragraph($"Company Reg No: {company.RegistrationNumber}")
                    .SetFontSize(8)
                    .SetMarginBottom(2)
                    .SetItalic())
                .Add(new Paragraph(company.Address)
                    .SetFontSize(9)
                    .SetMarginBottom(2))
                .Add(new Paragraph($"Tel: {company.Phone}")
                    .SetFontSize(9)
                    .SetMarginBottom(2))
                .Add(new Paragraph($"Email: {company.Email}")
                    .SetFontSize(9))
                .SetBorder(Border.NO_BORDER)
                .SetVerticalAlignment(VerticalAlignment.TOP)
                .SetPadding(0)
                .SetPaddingLeft(5);

            headerTable.AddCell(logoCell);
            headerTable.AddCell(companyInfoCell);

            document.Add(headerTable);

            // Add a subtle separator line
            document.Add(new Paragraph()
                .SetBorderBottom(new SolidBorder(ColorConstants.LIGHT_GRAY, 1))
                .SetMarginBottom(15));
        }

        private void AddInvoiceHeader(Document document, Invoice invoice)
        {
            var headerTable = new Table(2).UseAllAvailableWidth();
            headerTable.SetMarginBottom(15);

            // INVOICE title - compact and professional
            var invoiceTitle = new Cell()
                .Add(new Paragraph("INVOICE")
                    .SetFontSize(24)
                    .SetBold()
                    .SetTextAlignment(TextAlignment.LEFT))
                .SetBorder(Border.NO_BORDER)
                .SetVerticalAlignment(VerticalAlignment.BOTTOM);

            // Invoice details - aligned right, compact
            var invoiceDetails = new Cell()
                .Add(new Paragraph($"DATE: {invoice.InvoiceDate:dd-MMM-yyyy}")
                    .SetFontSize(10)
                    .SetTextAlignment(TextAlignment.RIGHT)
                    .SetMarginBottom(3))
                .Add(new Paragraph($"INVOICE #: {invoice.InvoiceNumber}")
                    .SetFontSize(10)
                    .SetBold()
                    .SetTextAlignment(TextAlignment.RIGHT))
                .SetBorder(Border.NO_BORDER)
                .SetVerticalAlignment(VerticalAlignment.BOTTOM);

            headerTable.AddCell(invoiceTitle);
            headerTable.AddCell(invoiceDetails);

            document.Add(headerTable);
        }

        private void AddCustomerInfo(Document document, Customer customer)
        {
            // Compact "BILL TO" header
            var billToHeader = new Paragraph("BILL TO")
                .SetFontSize(10)
                .SetBold()
                .SetMarginBottom(5);

            document.Add(billToHeader);

            // Customer details with thin border
            var customerTable = new Table(1).UseAllAvailableWidth();
            customerTable.SetBorder(new SolidBorder(ColorConstants.DARK_GRAY, 0.5f));

            var customerContent = new Cell()
                .Add(new Paragraph($"{customer.Name}")
                    .SetFontSize(10)
                    .SetBold()
                    .SetMarginBottom(3))
                .Add(new Paragraph($"{customer.CompanyName}")
                    .SetFontSize(9)
                    .SetMarginBottom(2))
                .Add(new Paragraph($"{customer.Address}")
                    .SetFontSize(9)
                    .SetMarginBottom(2))
                .Add(new Paragraph($"Tel: {customer.Phone}")
                    .SetFontSize(9)
                    .SetMarginBottom(2))
                .Add(new Paragraph($"Email: {customer.Email}")
                    .SetFontSize(9))
                .SetPadding(10)
                .SetBorder(Border.NO_BORDER);

            customerTable.AddCell(customerContent);
            document.Add(customerTable);
            document.Add(new Paragraph().SetMarginBottom(15));
        }

        private void AddInvoiceItemsTable(Document document, Invoice invoice)
        {
            var table = new Table(new float[] { 2.5f, 1.3f, 1.3f, 0.8f, 1.5f, 1.5f }).UseAllAvailableWidth();
            table.SetMarginBottom(15);

            // Compact header row with light gray background and black text
            table.AddHeaderCell(new Cell().Add(new Paragraph("Description/Number of Rooms")
                .SetFontSize(9).SetBold().SetTextAlignment(TextAlignment.CENTER))
                .SetBackgroundColor(ColorConstants.LIGHT_GRAY).SetPadding(6)
                .SetBorder(new SolidBorder(ColorConstants.DARK_GRAY, 0.5f)));

            table.AddHeaderCell(new Cell().Add(new Paragraph("Check In")
                .SetFontSize(9).SetBold().SetTextAlignment(TextAlignment.CENTER))
                .SetBackgroundColor(ColorConstants.LIGHT_GRAY).SetPadding(6)
                .SetBorder(new SolidBorder(ColorConstants.DARK_GRAY, 0.5f)));

            table.AddHeaderCell(new Cell().Add(new Paragraph("Check Out")
                .SetFontSize(9).SetBold().SetTextAlignment(TextAlignment.CENTER))
                .SetBackgroundColor(ColorConstants.LIGHT_GRAY).SetPadding(6)
                .SetBorder(new SolidBorder(ColorConstants.DARK_GRAY, 0.5f)));

            table.AddHeaderCell(new Cell().Add(new Paragraph("Nights")
                .SetFontSize(9).SetBold().SetTextAlignment(TextAlignment.CENTER))
                .SetBackgroundColor(ColorConstants.LIGHT_GRAY).SetPadding(6)
                .SetBorder(new SolidBorder(ColorConstants.DARK_GRAY, 0.5f)));

            table.AddHeaderCell(new Cell().Add(new Paragraph("Price/Night")
                .SetFontSize(9).SetBold().SetTextAlignment(TextAlignment.CENTER))
                .SetBackgroundColor(ColorConstants.LIGHT_GRAY).SetPadding(6)
                .SetBorder(new SolidBorder(ColorConstants.DARK_GRAY, 0.5f)));

            table.AddHeaderCell(new Cell().Add(new Paragraph("Total")
                .SetFontSize(9).SetBold().SetTextAlignment(TextAlignment.CENTER))
                .SetBackgroundColor(ColorConstants.LIGHT_GRAY).SetPadding(6)
                .SetBorder(new SolidBorder(ColorConstants.DARK_GRAY, 0.5f)));

            // Data rows with compact spacing
            bool isEvenRow = false;
            foreach (var item in invoice.Items)
            {
                var rowColor = isEvenRow ? new DeviceRgb(248, 248, 248) : ColorConstants.WHITE;

                table.AddCell(new Cell().Add(new Paragraph(item.Description).SetFontSize(8).SetTextAlignment(TextAlignment.LEFT))
                    .SetBackgroundColor(rowColor).SetPadding(5).SetBorder(new SolidBorder(ColorConstants.DARK_GRAY, 0.5f)));

                table.AddCell(new Cell().Add(new Paragraph(item.CheckIn.ToString("dd-MMM-yy")).SetFontSize(8).SetTextAlignment(TextAlignment.CENTER))
                    .SetBackgroundColor(rowColor).SetPadding(5).SetBorder(new SolidBorder(ColorConstants.DARK_GRAY, 0.5f)));

                table.AddCell(new Cell().Add(new Paragraph(item.CheckOut.ToString("dd-MMM-yy")).SetFontSize(8).SetTextAlignment(TextAlignment.CENTER))
                    .SetBackgroundColor(rowColor).SetPadding(5).SetBorder(new SolidBorder(ColorConstants.DARK_GRAY, 0.5f)));

                table.AddCell(new Cell().Add(new Paragraph(item.NumberOfNights.ToString()).SetFontSize(8).SetTextAlignment(TextAlignment.CENTER))
                    .SetBackgroundColor(rowColor).SetPadding(5).SetBorder(new SolidBorder(ColorConstants.DARK_GRAY, 0.5f)));

                table.AddCell(new Cell().Add(new Paragraph(CurrencyHelper.Format(item.PricePerNight, invoice.CurrencyCode ?? "LKR")).SetFontSize(8).SetTextAlignment(TextAlignment.RIGHT))
                    .SetBackgroundColor(rowColor).SetPadding(5).SetBorder(new SolidBorder(ColorConstants.DARK_GRAY, 0.5f)));

                table.AddCell(new Cell().Add(new Paragraph(CurrencyHelper.Format(item.Total, invoice.CurrencyCode ?? "LKR")).SetFontSize(8).SetBold().SetTextAlignment(TextAlignment.RIGHT))
                    .SetBackgroundColor(rowColor).SetPadding(5).SetBorder(new SolidBorder(ColorConstants.DARK_GRAY, 0.5f)));

                isEvenRow = !isEvenRow;
            }

            document.Add(table);
        }

        private void AddFoodItemsTable(Document document, Invoice invoice)
        {
            // Add food section header
            var foodHeader = new Paragraph("FOOD")
                .SetFontSize(12)
                .SetBold()
                .SetMarginTop(10)
                .SetMarginBottom(5);

            document.Add(foodHeader);

            var table = new Table(new float[] { 1.2f, 2.8f, 2.0f, 1.4f }).UseAllAvailableWidth();
            table.SetMarginBottom(15);

            // Header row
            table.AddHeaderCell(new Cell().Add(new Paragraph("Date")
                .SetFontSize(9).SetBold().SetTextAlignment(TextAlignment.CENTER))
                .SetBackgroundColor(ColorConstants.LIGHT_GRAY).SetPadding(6)
                .SetBorder(new SolidBorder(ColorConstants.DARK_GRAY, 0.5f)));

            table.AddHeaderCell(new Cell().Add(new Paragraph("Description")
                .SetFontSize(9).SetBold().SetTextAlignment(TextAlignment.CENTER))
                .SetBackgroundColor(ColorConstants.LIGHT_GRAY).SetPadding(6)
                .SetBorder(new SolidBorder(ColorConstants.DARK_GRAY, 0.5f)));

            table.AddHeaderCell(new Cell().Add(new Paragraph("Note")
                .SetFontSize(9).SetBold().SetTextAlignment(TextAlignment.CENTER))
                .SetBackgroundColor(ColorConstants.LIGHT_GRAY).SetPadding(6)
                .SetBorder(new SolidBorder(ColorConstants.DARK_GRAY, 0.5f)));

            table.AddHeaderCell(new Cell().Add(new Paragraph("Price")
                .SetFontSize(9).SetBold().SetTextAlignment(TextAlignment.CENTER))
                .SetBackgroundColor(ColorConstants.LIGHT_GRAY).SetPadding(6)
                .SetBorder(new SolidBorder(ColorConstants.DARK_GRAY, 0.5f)));

            // Data rows
            bool isEvenRow = false;
            foreach (var item in invoice.FoodItems)
            {
                if (string.IsNullOrEmpty(item.Description)) continue;

                var rowColor = isEvenRow ? new DeviceRgb(248, 248, 248) : ColorConstants.WHITE;

                string dateText = "";
                if (item.Date != default(DateTime) && item.Date != DateTime.MinValue)
                {
                    dateText = item.Date.ToString("dd-MMM-yy");
                }

                table.AddCell(new Cell().Add(new Paragraph(dateText).SetFontSize(8).SetTextAlignment(TextAlignment.CENTER))
                    .SetBackgroundColor(rowColor).SetPadding(5).SetBorder(new SolidBorder(ColorConstants.DARK_GRAY, 0.5f)));

                table.AddCell(new Cell().Add(new Paragraph(item.Description).SetFontSize(8).SetTextAlignment(TextAlignment.LEFT))
                    .SetBackgroundColor(rowColor).SetPadding(5).SetBorder(new SolidBorder(ColorConstants.DARK_GRAY, 0.5f)));

                var normalizedNote = NormalizeLineBreaks(item.Note ?? "");
                table.AddCell(new Cell().Add(new Paragraph(normalizedNote)
                        .SetFontSize(8)
                        .SetTextAlignment(TextAlignment.LEFT)
                        .SetMultipliedLeading(1.0f))
                    .SetBackgroundColor(rowColor).SetPadding(5).SetBorder(new SolidBorder(ColorConstants.DARK_GRAY, 0.5f)));

                table.AddCell(new Cell().Add(new Paragraph(CurrencyHelper.Format(item.Price, invoice.CurrencyCode ?? "LKR")).SetFontSize(8).SetBold().SetTextAlignment(TextAlignment.RIGHT))
                    .SetBackgroundColor(rowColor).SetPadding(5).SetBorder(new SolidBorder(ColorConstants.DARK_GRAY, 0.5f)));

                isEvenRow = !isEvenRow;
            }

            document.Add(table);
        }

        private void AddTotalsSection(Document document, Invoice invoice)
        {
            document.Add(new Paragraph().SetMarginBottom(10));

            var totalsTable = new Table(2).UseAllAvailableWidth();

            // Left side - Compact notes
            var notesCell = new Cell()
                .Add(new Paragraph("NOTES")
                    .SetFontSize(10)
                    .SetBold()
                    .SetMarginBottom(5))
                .Add(new Paragraph(NormalizeLineBreaks(invoice.Notes ?? "No additional notes"))
                    .SetFontSize(8)
                    // Ensure single line spacing; preserve explicit blank lines from user input
                    .SetMultipliedLeading(1.0f)
                    .SetMarginTop(0)
                    .SetMarginBottom(0))
                .SetBorder(new SolidBorder(ColorConstants.DARK_GRAY, 0.5f))
                .SetPadding(8)
                .SetWidth(UnitValue.CreatePercentValue(55))
                .SetVerticalAlignment(VerticalAlignment.TOP);

            // Right side - Compact totals
            var totalsCell = new Cell()
                .SetBorder(Border.NO_BORDER)
                .SetWidth(UnitValue.CreatePercentValue(45))
                .SetVerticalAlignment(VerticalAlignment.TOP);

            var totalsInnerTable = new Table(new float[] { 3, 2 }).UseAllAvailableWidth();

            // Compact totals rows
            totalsInnerTable.AddCell(new Cell().Add(new Paragraph("SUBTOTAL").SetFontSize(9))
                .SetBorder(new SolidBorder(ColorConstants.DARK_GRAY, 0.5f)).SetTextAlignment(TextAlignment.RIGHT)
                .SetPadding(5));
            totalsInnerTable.AddCell(new Cell().Add(new Paragraph(CurrencyHelper.Format(invoice.Subtotal, invoice.CurrencyCode ?? "LKR")).SetFontSize(9))
                .SetBorder(new SolidBorder(ColorConstants.DARK_GRAY, 0.5f)).SetTextAlignment(TextAlignment.RIGHT)
                .SetPadding(5));

            totalsInnerTable.AddCell(new Cell().Add(new Paragraph("Taxes").SetFontSize(9))
                .SetBorder(new SolidBorder(ColorConstants.DARK_GRAY, 0.5f)).SetTextAlignment(TextAlignment.RIGHT)
                .SetPadding(5));
            totalsInnerTable.AddCell(new Cell().Add(new Paragraph(CurrencyHelper.Format(invoice.TaxAmount, invoice.CurrencyCode ?? "LKR")).SetFontSize(9))
                .SetBorder(new SolidBorder(ColorConstants.DARK_GRAY, 0.5f)).SetTextAlignment(TextAlignment.RIGHT)
                .SetPadding(5));

            totalsInnerTable.AddCell(new Cell().Add(new Paragraph("Other").SetFontSize(9))
                .SetBorder(new SolidBorder(ColorConstants.DARK_GRAY, 0.5f)).SetTextAlignment(TextAlignment.RIGHT)
                .SetPadding(5));
            totalsInnerTable.AddCell(new Cell().Add(new Paragraph(CurrencyHelper.Format(invoice.Other, invoice.CurrencyCode ?? "LKR")).SetFontSize(9))
                .SetBorder(new SolidBorder(ColorConstants.DARK_GRAY, 0.5f)).SetTextAlignment(TextAlignment.RIGHT)
                .SetPadding(5));

            // Total row - slightly bolder but still thin
            totalsInnerTable.AddCell(new Cell().Add(new Paragraph("TOTAL").SetFontSize(10).SetBold())
                .SetBorder(new SolidBorder(ColorConstants.DARK_GRAY, 0.5f)).SetTextAlignment(TextAlignment.RIGHT)
                .SetPadding(6));
            totalsInnerTable.AddCell(new Cell().Add(new Paragraph(CurrencyHelper.Format(invoice.Total, invoice.CurrencyCode ?? "LKR")).SetFontSize(10).SetBold())
                .SetBorder(new SolidBorder(ColorConstants.DARK_GRAY, 0.5f)).SetTextAlignment(TextAlignment.RIGHT)
                .SetPadding(6));

            totalsInnerTable.AddCell(new Cell().Add(new Paragraph("PAID").SetFontSize(9))
                .SetBorder(new SolidBorder(ColorConstants.DARK_GRAY, 0.5f)).SetTextAlignment(TextAlignment.RIGHT)
                .SetPadding(5));
            totalsInnerTable.AddCell(new Cell().Add(new Paragraph(CurrencyHelper.Format(invoice.Paid, invoice.CurrencyCode ?? "LKR")).SetFontSize(9))
                .SetBorder(new SolidBorder(ColorConstants.DARK_GRAY, 0.5f)).SetTextAlignment(TextAlignment.RIGHT)
                .SetPadding(5));

            // Total Due row - slightly bolder but still thin
            totalsInnerTable.AddCell(new Cell().Add(new Paragraph("TOTAL DUE").SetFontSize(10).SetBold())
                .SetBorder(new SolidBorder(ColorConstants.DARK_GRAY, 0.5f)).SetTextAlignment(TextAlignment.RIGHT)
                .SetPadding(6));
            totalsInnerTable.AddCell(new Cell().Add(new Paragraph(CurrencyHelper.Format(invoice.TotalDue, invoice.CurrencyCode ?? "LKR")).SetFontSize(10).SetBold())
                .SetBorder(new SolidBorder(ColorConstants.DARK_GRAY, 0.5f)).SetTextAlignment(TextAlignment.RIGHT)
                .SetPadding(6));

            totalsInnerTable.AddCell(new Cell().Add(new Paragraph("Advance Bal").SetFontSize(9))
                .SetBorder(new SolidBorder(ColorConstants.DARK_GRAY, 0.5f)).SetTextAlignment(TextAlignment.RIGHT)
                .SetPadding(5));
            totalsInnerTable.AddCell(new Cell().Add(new Paragraph(CurrencyHelper.Format(invoice.Advance, invoice.CurrencyCode ?? "LKR")).SetFontSize(9))
                .SetBorder(new SolidBorder(ColorConstants.DARK_GRAY, 0.5f)).SetTextAlignment(TextAlignment.RIGHT)
                .SetPadding(5));

            totalsCell.Add(totalsInnerTable);

            totalsTable.AddCell(notesCell);
            totalsTable.AddCell(totalsCell);

            document.Add(totalsTable);
        }

        public void ExportReceiptToPdf(Invoice invoice, string filePath, decimal currentPaymentAmount)
        {
            if (invoice.Paid <= 0)
            {
                throw new Exception("Cannot generate receipt: no payment amount recorded.");
            }

            try
            {
                using (var writer = new PdfWriter(filePath))
                {
                    using (var pdf = new PdfDocument(writer))
                    {
                        pdf.SetDefaultPageSize(iText.Kernel.Geom.PageSize.A4);
                        var document = new Document(pdf);
                        document.SetMargins(20, 20, 20, 20);

                        // Add header with company information
                        AddHeader(document, invoice.Company);

                        // Add receipt title and details
                        AddReceiptHeader(document, invoice);

                        // Add customer information
                        AddCustomerInfo(document, invoice.Customer);

                        // Invoice reference section
                        AddPaymentDetails(document, invoice);

                        // Add totals section showing payment breakup
                        AddReceiptTotalsSection(document, invoice, currentPaymentAmount);

                        // Add payment confirmation message
                        AddPaymentConfirmation(document, invoice, currentPaymentAmount);

                        document.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error exporting receipt to PDF: {ex.Message}");
            }
        }

        private void AddReceiptHeader(Document document, Invoice invoice)
        {
            var headerTable = new Table(2).UseAllAvailableWidth();
            headerTable.SetMarginBottom(15);

            // RECEIPT title - prominent
            var receiptTitle = new Cell()
                .Add(new Paragraph("RECEIPT")
                    .SetFontSize(24)
                    .SetBold()
                    .SetTextAlignment(TextAlignment.LEFT)
                    .SetFontColor(ColorConstants.BLACK))
                .SetBorder(Border.NO_BORDER)
                .SetVerticalAlignment(VerticalAlignment.BOTTOM);

            // Receipt details - aligned right
            var receiptDetails = new Cell()
                .Add(new Paragraph($"RECEIPT DATE: {(invoice.PaymentDate ?? DateTime.Now):dd-MMM-yyyy}")
                    .SetFontSize(10)
                    .SetTextAlignment(TextAlignment.RIGHT)
                    .SetMarginBottom(3))
                .Add(new Paragraph($"INVOICE #: {invoice.InvoiceNumber}")
                    .SetFontSize(10)
                    .SetBold()
                    .SetTextAlignment(TextAlignment.RIGHT)
                    .SetMarginBottom(3))
                .Add(new Paragraph($"INVOICE DATE: {invoice.InvoiceDate:dd-MMM-yyyy}")
                    .SetFontSize(9)
                    .SetTextAlignment(TextAlignment.RIGHT))
                .SetBorder(Border.NO_BORDER)
                .SetVerticalAlignment(VerticalAlignment.BOTTOM);

            headerTable.AddCell(receiptTitle);
            headerTable.AddCell(receiptDetails);

            document.Add(headerTable);
        }

        private void AddPaymentDetails(Document document, Invoice invoice)
        {
            var paymentSection = new Paragraph("PAYMENT REFERENCE")
                .SetFontSize(11)
                .SetBold()
                .SetMarginTop(20)
                .SetMarginBottom(10);

            document.Add(paymentSection);

            var detailsTable = new Table(2).UseAllAvailableWidth();
            detailsTable.SetMarginBottom(15);
            detailsTable.SetBorder(new SolidBorder(ColorConstants.BLACK, 0.5f));

            AddTableRow(detailsTable, "Invoice Number:", invoice.InvoiceNumber);
            AddTableRow(detailsTable, "Invoice Date:", invoice.InvoiceDate.ToString("dd-MMM-yyyy"));
            AddTableRow(detailsTable, "Payment Date:", (invoice.PaymentDate ?? DateTime.Now).ToString("dd-MMM-yyyy"));
            AddTableRow(detailsTable, "Bill To:", invoice.Customer.Name);

            document.Add(detailsTable);
        }

        private void AddTableRow(Table table, string label, string value)
        {
            var labelCell = new Cell()
                .Add(new Paragraph(label).SetFontSize(9).SetBold())
                .SetBackgroundColor(ColorConstants.LIGHT_GRAY)
                .SetPadding(6)
                .SetBorder(new SolidBorder(ColorConstants.BLACK, 0.5f));

            var valueCell = new Cell()
                .Add(new Paragraph(value).SetFontSize(9))
                .SetPadding(6)
                .SetBorder(new SolidBorder(ColorConstants.BLACK, 0.5f));

            table.AddCell(labelCell);
            table.AddCell(valueCell);
        }

        private void AddReceiptTotalsSection(Document document, Invoice invoice, decimal currentPaymentAmount)
        {
            var totalsSection = new Paragraph("PAYMENT SUMMARY")
                .SetFontSize(11)
                .SetBold()
                .SetMarginTop(20)
                .SetMarginBottom(10);

            document.Add(totalsSection);

            var totalsTable = new Table(new float[] { 3, 2 }).UseAllAvailableWidth();
            totalsTable.SetMarginBottom(15);

            // Original invoice total
            totalsTable.AddCell(new Cell().Add(new Paragraph("Invoice Total").SetFontSize(10))
                .SetBorder(new SolidBorder(ColorConstants.BLACK, 0.5f)).SetTextAlignment(TextAlignment.RIGHT)
                .SetPadding(6).SetBackgroundColor(ColorConstants.LIGHT_GRAY));
            totalsTable.AddCell(new Cell().Add(new Paragraph(CurrencyHelper.Format(invoice.Total, invoice.CurrencyCode ?? "LKR")).SetFontSize(10))
                .SetBorder(new SolidBorder(ColorConstants.BLACK, 0.5f)).SetTextAlignment(TextAlignment.RIGHT)
                .SetPadding(6).SetBackgroundColor(ColorConstants.LIGHT_GRAY));

            // Previous payments
            decimal previousPayments = Math.Max(0m, invoice.Paid - currentPaymentAmount);
            if (previousPayments > 0)
            {
                totalsTable.AddCell(new Cell().Add(new Paragraph("Previous Payments").SetFontSize(9))
                    .SetBorder(new SolidBorder(ColorConstants.BLACK, 0.5f)).SetTextAlignment(TextAlignment.RIGHT)
                    .SetPadding(5).SetBackgroundColor(ColorConstants.LIGHT_GRAY));
                totalsTable.AddCell(new Cell().Add(new Paragraph(CurrencyHelper.Format(previousPayments, invoice.CurrencyCode ?? "LKR")).SetFontSize(9))
                    .SetBorder(new SolidBorder(ColorConstants.BLACK, 0.5f)).SetTextAlignment(TextAlignment.RIGHT)
                    .SetPadding(5).SetBackgroundColor(ColorConstants.LIGHT_GRAY));
            }

            // Current payment
            totalsTable.AddCell(new Cell().Add(new Paragraph("PAYMENT RECEIVED").SetFontSize(10).SetBold())
                .SetBorder(new SolidBorder(ColorConstants.BLACK, 0.5f)).SetTextAlignment(TextAlignment.RIGHT)
                .SetPadding(6).SetBackgroundColor(ColorConstants.LIGHT_GRAY));
            totalsTable.AddCell(new Cell().Add(new Paragraph(CurrencyHelper.Format(currentPaymentAmount, invoice.CurrencyCode ?? "LKR")).SetFontSize(10).SetBold())
                .SetBorder(new SolidBorder(ColorConstants.BLACK, 0.5f)).SetTextAlignment(TextAlignment.RIGHT)
                .SetPadding(6).SetBackgroundColor(ColorConstants.LIGHT_GRAY));

            document.Add(totalsTable);
        }

        private void AddPaymentConfirmation(Document document, Invoice invoice, decimal currentPaymentAmount)
        {
            document.Add(new Paragraph().SetMarginBottom(15));

            var confirmationBox = new Cell()
                .Add(new Paragraph("PAYMENT CONFIRMATION").SetFontSize(11).SetBold().SetMarginBottom(5))
                .Add(new Paragraph($"A payment of {CurrencyHelper.Format(currentPaymentAmount, invoice.CurrencyCode ?? "LKR")} has been received on {(invoice.PaymentDate ?? DateTime.Now):dd-MMM-yyyy}.").SetFontSize(9).SetMarginBottom(5))
                .Add(new Paragraph($"Thank you for your payment.").SetFontSize(9))
                .SetBorder(new SolidBorder(ColorConstants.BLACK, 0.5f))
                .SetPadding(12)
                .SetBackgroundColor(ColorConstants.LIGHT_GRAY);

            var confirmationTable = new Table(1).UseAllAvailableWidth();
            confirmationTable.AddCell(confirmationBox);
            confirmationTable.SetMarginTop(20);

            document.Add(confirmationTable);
        }
    }
}

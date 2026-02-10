using System;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace HaranInvoiceSoftware.Forms
{
    public class AiXmlGuideForm : Form
    {
        private readonly TextBox _txtGuide;
        private readonly Button _btnCopy;
        private readonly Button _btnClose;

        public AiXmlGuideForm()
        {
            Text = "AI XML Guide";
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            ShowInTaskbar = false;
            Font = new Font("Segoe UI", 11F);
            BackColor = Color.White;
            ClientSize = new Size(980, 640);

            var lbl = new Label
            {
                AutoSize = false,
                Text = "Copy this instruction + example into ChatGPT. Then provide your PDF/text data and ask it to return ONLY XML.",
                Location = new Point(16, 14),
                Size = new Size(ClientSize.Width - 32, 44),
                ForeColor = Color.FromArgb(37, 42, 64)
            };

            _txtGuide = new TextBox
            {
                Multiline = true,
                ReadOnly = true,
                ScrollBars = ScrollBars.Both,
                WordWrap = false,
                BorderStyle = BorderStyle.FixedSingle,
                Location = new Point(16, 64),
                Size = new Size(ClientSize.Width - 32, ClientSize.Height - 64 - 72),
                Font = new Font("Segoe UI", 10F)
            };

            _btnCopy = new Button
            {
                Text = "Copy",
                BackColor = Color.FromArgb(33, 150, 243),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Location = new Point(16, ClientSize.Height - 54),
                Size = new Size(140, 40)
            };
            _btnCopy.FlatAppearance.BorderSize = 0;
            _btnCopy.Click += (s, e) =>
            {
                try
                {
                    Clipboard.SetText(_txtGuide.Text);
                    MessageBox.Show(this, "Copied to clipboard.", "Copied", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, ex.Message, "Copy Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };

            _btnClose = new Button
            {
                Text = "Close",
                BackColor = Color.FromArgb(244, 67, 54),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Location = new Point(ClientSize.Width - 16 - 140, ClientSize.Height - 54),
                Size = new Size(140, 40)
            };
            _btnClose.FlatAppearance.BorderSize = 0;
            _btnClose.Click += (s, e) => Close();

            Controls.Add(lbl);
            Controls.Add(_txtGuide);
            Controls.Add(_btnCopy);
            Controls.Add(_btnClose);

            _txtGuide.Text = BuildGuideText();
        }

        private static string BuildGuideText()
        {
            var sb = new StringBuilder();

            sb.AppendLine("INSTRUCTION FOR CHATGPT");
            sb.AppendLine("You are helping format an invoice XML for a Windows app that uses .NET XmlSerializer.");
            sb.AppendLine("Return ONLY valid XML (no markdown, no explanations). The XML MUST be UTF-8 compatible.");
            sb.AppendLine();
            sb.AppendLine("Rules:");
            sb.AppendLine("1) Root element must be <Invoice>.");
            sb.AppendLine("2) Dates must be ISO format like 2026-02-09T00:00:00.");
            sb.AppendLine("3) Decimal numbers must use a dot (.) and no currency symbols.");
            sb.AppendLine("4) Items must use these fields exactly: Description, CheckIn, CheckOut, NumberOfNights, PricePerNight, Total.");
            sb.AppendLine("5) Optional FoodItems list uses: Date, Description, Note, Price.");
            sb.AppendLine("6) Include CurrencyCode (e.g., LKR or USD). Output should still load if FoodItems is empty.");
            sb.AppendLine();
            sb.AppendLine("I will provide the invoice data (possibly from a PDF). Extract the data and fill this XML schema.");
            sb.AppendLine();
            sb.AppendLine("EXAMPLE XML (copy and replace values)");
            sb.AppendLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
            sb.AppendLine("<Invoice xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">");
            sb.AppendLine("  <InvoiceNumber>INV-20260209-000001</InvoiceNumber>");
            sb.AppendLine("  <InvoiceDate>2026-02-09T00:00:00</InvoiceDate>");
            sb.AppendLine();
            sb.AppendLine("  <Company>");
            sb.AppendLine("    <Name>NALLUR RESIDENCE (PVT) LTD</Name>");
            sb.AppendLine("    <RegistrationNumber>PV 00301818</RegistrationNumber>");
            sb.AppendLine("    <Address>656, Point Pedro Road, Nallur, Jaffna 40000</Address>");
            sb.AppendLine("    <Phone>077-244-6241</Phone>");
            sb.AppendLine("    <Email>harangovindaraj@gmail.com</Email>");
            sb.AppendLine("    <Logo></Logo>");
            sb.AppendLine("  </Company>");
            sb.AppendLine();
            sb.AppendLine("  <Customer>");
            sb.AppendLine("    <Name>Customer Name</Name>");
            sb.AppendLine("    <CompanyName>Customer Company</CompanyName>");
            sb.AppendLine("    <Address>Customer Address</Address>");
            sb.AppendLine("    <Phone></Phone>");
            sb.AppendLine("    <Email>customer@example.com</Email>");
            sb.AppendLine("  </Customer>");
            sb.AppendLine();
            sb.AppendLine("  <Items>");
            sb.AppendLine("    <Item>");
            sb.AppendLine("      <Description>Accommodation - Deluxe Room</Description>");
            sb.AppendLine("      <CheckIn>2026-02-07T00:00:00</CheckIn>");
            sb.AppendLine("      <CheckOut>2026-02-09T00:00:00</CheckOut>");
            sb.AppendLine("      <NumberOfNights>2</NumberOfNights>");
            sb.AppendLine("      <PricePerNight>50.00</PricePerNight>");
            sb.AppendLine("      <Total>100.00</Total>");
            sb.AppendLine("    </Item>");
            sb.AppendLine("  </Items>");
            sb.AppendLine();
            sb.AppendLine("  <FoodItems>");
            sb.AppendLine("    <FoodItem>");
            sb.AppendLine("      <Date>2026-02-08T00:00:00</Date>");
            sb.AppendLine("      <Description>Dinner</Description>");
            sb.AppendLine("      <Note></Note>");
            sb.AppendLine("      <Price>30.00</Price>");
            sb.AppendLine("    </FoodItem>");
            sb.AppendLine("  </FoodItems>");
            sb.AppendLine();
            sb.AppendLine("  <Subtotal>130.00</Subtotal>");
            sb.AppendLine("  <TaxRate>0.00</TaxRate>");
            sb.AppendLine("  <TaxAmount>0.00</TaxAmount>");
            sb.AppendLine("  <Other>0.00</Other>");
            sb.AppendLine("  <Total>130.00</Total>");
            sb.AppendLine("  <Paid>0.00</Paid>");
            sb.AppendLine("  <Advance>0.00</Advance>");
            sb.AppendLine("  <TotalDue>130.00</TotalDue>");
            sb.AppendLine("  <CurrencyCode>USD</CurrencyCode>");
            sb.AppendLine("  <Notes>Any notes here.</Notes>");
            sb.AppendLine("</Invoice>");

            return sb.ToString();
        }
    }
}

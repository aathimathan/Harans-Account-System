using System;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;
using HaranInvoiceSoftware.Models;
using HaranInvoiceSoftware.Services;

namespace HaranInvoiceSoftware.Forms
{
    public class PasteXmlForm : Form
    {
        private readonly TextBox _txtXml;
        private readonly Button _btnLoad;
        private readonly Button _btnCancel;
        private readonly InvoiceDataService _dataService;

        public Invoice LoadedInvoice { get; private set; }

        public PasteXmlForm(InvoiceDataService dataService)
        {
            _dataService = dataService ?? throw new ArgumentNullException(nameof(dataService));

            Text = "Paste XML";
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
                Text = _dataService.IsUsingMySql
                    ? "Paste invoice XML below. The app will import it and save it into MySQL."
                    : "Paste invoice XML below. The app will try to load it, then save a new XML file into the Invoices folder.",
                Location = new Point(16, 14),
                Size = new Size(ClientSize.Width - 32, 44),
                ForeColor = Color.FromArgb(37, 42, 64)
            };

            _txtXml = new TextBox
            {
                Multiline = true,
                ScrollBars = ScrollBars.Both,
                WordWrap = false,
                BorderStyle = BorderStyle.FixedSingle,
                Location = new Point(16, 64),
                Size = new Size(ClientSize.Width - 32, ClientSize.Height - 64 - 72),
                Font = new Font("Segoe UI", 10F)
            };

            _btnLoad = new Button
            {
                Text = "Load",
                BackColor = Color.FromArgb(76, 175, 80),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Location = new Point(16, ClientSize.Height - 54),
                Size = new Size(140, 40)
            };
            _btnLoad.FlatAppearance.BorderSize = 0;
            _btnLoad.Click += BtnLoad_Click;

            _btnCancel = new Button
            {
                Text = "Cancel",
                BackColor = Color.FromArgb(244, 67, 54),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Location = new Point(ClientSize.Width - 16 - 140, ClientSize.Height - 54),
                Size = new Size(140, 40)
            };
            _btnCancel.FlatAppearance.BorderSize = 0;
            _btnCancel.Click += (s, e) => { DialogResult = DialogResult.Cancel; Close(); };

            Controls.Add(lbl);
            Controls.Add(_txtXml);
            Controls.Add(_btnLoad);
            Controls.Add(_btnCancel);

            _txtXml.Text = "";
        }

        private void BtnLoad_Click(object sender, EventArgs e)
        {
            string xml = _txtXml.Text;
            if (string.IsNullOrWhiteSpace(xml))
            {
                MessageBox.Show(this, "Please paste XML first.", "Missing XML", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var serializer = new XmlSerializer(typeof(Invoice));
                Invoice invoice;
                using (var reader = new StringReader(xml))
                {
                    invoice = (Invoice)serializer.Deserialize(reader);
                }

                if (invoice == null)
                {
                    MessageBox.Show(this, "Could not deserialize invoice.", "Invalid XML", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Ensure required objects/collections are initialized (same as LoadInvoice)
                if (invoice.Customer == null) invoice.Customer = new Customer();
                if (invoice.Company == null) invoice.Company = new Company();
                if (invoice.Items == null) invoice.Items = new System.Collections.Generic.List<InvoiceItem>();
                if (invoice.FoodItems == null) invoice.FoodItems = new System.Collections.Generic.List<FoodItem>();
                if (string.IsNullOrWhiteSpace(invoice.CurrencyCode)) invoice.CurrencyCode = "LKR";

                if (!_dataService.IsUsingMySql)
                {
                    // Save into the app's own XML format with a safe filename.
                    string invoicesDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Invoices");
                    Directory.CreateDirectory(invoicesDir);
                    string safeFileName = $"Invoice_Pasted_{DateTime.Now:yyyyMMdd_HHmmss}.xml";
                    invoice.FileName = Path.Combine(invoicesDir, safeFileName);
                }
                else
                {
                    invoice.FileName = string.Empty;
                }

                // Recalculate totals in case values were missing
                invoice.CalculateTotals();

                _dataService.SaveInvoice(invoice);

                LoadedInvoice = invoice;
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (InvalidOperationException ex)
            {
                // XmlSerializer errors typically come in InvalidOperationException with InnerException
                var details = new StringBuilder();
                details.AppendLine(ex.Message);
                if (ex.InnerException != null)
                {
                    details.AppendLine();
                    details.AppendLine(ex.InnerException.Message);
                }
                MessageBox.Show(this, details.ToString(), "XML Load Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "XML Load Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}

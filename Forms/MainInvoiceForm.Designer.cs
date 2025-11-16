namespace HaranInvoiceSoftware.Forms
{
    partial class MainInvoiceForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.panelHeader = new System.Windows.Forms.Panel();
            this.pictureBoxLogo = new System.Windows.Forms.PictureBox();
            this.lblCompanyName = new System.Windows.Forms.Label();
            this.lblCompanyAddress = new System.Windows.Forms.Label();
            this.lblCompanyPhone = new System.Windows.Forms.Label();
            this.lblCompanyEmail = new System.Windows.Forms.Label();
            this.lblInvoice = new System.Windows.Forms.Label();
            this.lblDate = new System.Windows.Forms.Label();
            this.lblInvoiceNumber = new System.Windows.Forms.Label();
            this.dtpInvoiceDate = new System.Windows.Forms.DateTimePicker();
            this.txtInvoiceNumber = new System.Windows.Forms.TextBox();
            this.lblCurrency = new System.Windows.Forms.Label();
            this.cboCurrency = new System.Windows.Forms.ComboBox();
            this.panelCustomer = new System.Windows.Forms.Panel();
            this.lblBillTo = new System.Windows.Forms.Label();
            this.lblCustomerName = new System.Windows.Forms.Label();
            this.txtCustomerName = new System.Windows.Forms.TextBox();
            this.lblCompanyNameCustomer = new System.Windows.Forms.Label();
            this.txtCompanyName = new System.Windows.Forms.TextBox();
            this.lblAddress = new System.Windows.Forms.Label();
            this.txtAddress = new System.Windows.Forms.TextBox();
            this.lblTel = new System.Windows.Forms.Label();
            this.txtTel = new System.Windows.Forms.TextBox();
            this.lblEmail = new System.Windows.Forms.Label();
            this.txtEmail = new System.Windows.Forms.TextBox();
            this.dgvItems = new System.Windows.Forms.DataGridView();
            this.lblFoodItems = new System.Windows.Forms.Label();
            this.dgvFoodItems = new System.Windows.Forms.DataGridView();
            this.panelTotals = new System.Windows.Forms.Panel();
            this.lblSubtotal = new System.Windows.Forms.Label();
            this.txtSubtotal = new System.Windows.Forms.TextBox();
            this.lblTaxRate = new System.Windows.Forms.Label();
            this.txtTaxRate = new System.Windows.Forms.TextBox();
            this.lblPercent = new System.Windows.Forms.Label();
            this.lblTaxes = new System.Windows.Forms.Label();
            this.txtTaxes = new System.Windows.Forms.TextBox();
            this.lblOther = new System.Windows.Forms.Label();
            this.txtOther = new System.Windows.Forms.TextBox();
            this.lblTotal = new System.Windows.Forms.Label();
            this.txtTotal = new System.Windows.Forms.TextBox();
            this.lblPaid = new System.Windows.Forms.Label();
            this.txtPaid = new System.Windows.Forms.TextBox();
            this.lblTotalDue = new System.Windows.Forms.Label();
            this.txtTotalDue = new System.Windows.Forms.TextBox();
            this.lblAdvance = new System.Windows.Forms.Label();
            this.txtAdvance = new System.Windows.Forms.TextBox();
            this.panelNotes = new System.Windows.Forms.Panel();
            this.lblNotes = new System.Windows.Forms.Label();
            this.txtNotes = new System.Windows.Forms.RichTextBox();
            this.panelButtons = new System.Windows.Forms.Panel();
            this.btnNew = new System.Windows.Forms.Button();
            this.btnLoad = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnExportPdf = new System.Windows.Forms.Button();
            this.panelHeader.SuspendLayout();
            this.panelCustomer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvItems)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvFoodItems)).BeginInit();
            this.panelTotals.SuspendLayout();
            this.panelNotes.SuspendLayout();
            this.panelButtons.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLogo)).BeginInit();
            this.SuspendLayout();
            // 
            // panelHeader
            // 
            this.panelHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(40)))), ((int)(((byte)(62)))));
            this.panelHeader.Controls.Add(this.cboCurrency);
            this.panelHeader.Controls.Add(this.lblCurrency);
            this.panelHeader.Controls.Add(this.pictureBoxLogo);
            this.panelHeader.Controls.Add(this.lblCompanyName);
            this.panelHeader.Controls.Add(this.lblCompanyAddress);
            this.panelHeader.Controls.Add(this.lblCompanyPhone);
            this.panelHeader.Controls.Add(this.lblCompanyEmail);
            this.panelHeader.Controls.Add(this.lblInvoice);
            this.panelHeader.Controls.Add(this.lblDate);
            this.panelHeader.Controls.Add(this.lblInvoiceNumber);
            this.panelHeader.Controls.Add(this.dtpInvoiceDate);
            this.panelHeader.Controls.Add(this.txtInvoiceNumber);
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHeader.Location = new System.Drawing.Point(0, 0);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Size = new System.Drawing.Size(1400, 140);
            this.panelHeader.TabIndex = 0;
            // 
            // lblCompanyName
            // 
            this.lblCompanyName.AutoSize = true;
            this.lblCompanyName.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Bold);
            this.lblCompanyName.ForeColor = System.Drawing.Color.White;
            this.lblCompanyName.Location = new System.Drawing.Point(140, 15);
            this.lblCompanyName.Name = "lblCompanyName";
            this.lblCompanyName.Size = new System.Drawing.Size(261, 37);
            this.lblCompanyName.TabIndex = 0;
            this.lblCompanyName.Text = "NALLUR RESIDENCE (PVT) LTD";
            // 
            // lblCompanyAddress
            // 
            this.lblCompanyAddress.AutoSize = true;
            this.lblCompanyAddress.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.lblCompanyAddress.ForeColor = System.Drawing.Color.LightGray;
            this.lblCompanyAddress.Location = new System.Drawing.Point(140, 54);
            this.lblCompanyAddress.Name = "lblCompanyAddress";
            this.lblCompanyAddress.Size = new System.Drawing.Size(241, 21);
            this.lblCompanyAddress.TabIndex = 1;
            this.lblCompanyAddress.Text = "956 Point Pedro Rd, Nallur Jaffna";
            // 
            // lblCompanyPhone
            // 
            this.lblCompanyPhone.AutoSize = true;
            this.lblCompanyPhone.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.lblCompanyPhone.ForeColor = System.Drawing.Color.LightGray;
            this.lblCompanyPhone.Location = new System.Drawing.Point(140, 78);
            this.lblCompanyPhone.Name = "lblCompanyPhone";
            this.lblCompanyPhone.Size = new System.Drawing.Size(125, 21);
            this.lblCompanyPhone.TabIndex = 2;
            this.lblCompanyPhone.Text = "Tel: 077-244-6241";
            // 
            // lblCompanyEmail
            // 
            this.lblCompanyEmail.AutoSize = true;
            this.lblCompanyEmail.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.lblCompanyEmail.ForeColor = System.Drawing.Color.LightGray;
            this.lblCompanyEmail.Location = new System.Drawing.Point(140, 102);
            this.lblCompanyEmail.Name = "lblCompanyEmail";
            this.lblCompanyEmail.Size = new System.Drawing.Size(219, 21);
            this.lblCompanyEmail.TabIndex = 3;
            this.lblCompanyEmail.Text = "Email: harangovindaraj@gmail.com";
            // 
            // pictureBoxLogo
            // 
            this.pictureBoxLogo.Location = new System.Drawing.Point(15, 15);
            this.pictureBoxLogo.Name = "pictureBoxLogo";
            this.pictureBoxLogo.Size = new System.Drawing.Size(120, 110);
            this.pictureBoxLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxLogo.TabIndex = 10;
            this.pictureBoxLogo.TabStop = false;
            // 
            // lblInvoice
            // 
            this.lblInvoice.AutoSize = true;
            this.lblInvoice.Font = new System.Drawing.Font("Segoe UI", 26F, System.Drawing.FontStyle.Bold);
            this.lblInvoice.ForeColor = System.Drawing.Color.White;
            this.lblInvoice.Location = new System.Drawing.Point(900, 40);
            this.lblInvoice.Name = "lblInvoice";
            this.lblInvoice.Size = new System.Drawing.Size(164, 47);
            this.lblInvoice.TabIndex = 4;
            this.lblInvoice.Text = "INVOICE";
            // 
            // lblDate
            // 
            this.lblDate.AutoSize = true;
            this.lblDate.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.lblDate.ForeColor = System.Drawing.Color.White;
            this.lblDate.Location = new System.Drawing.Point(1100, 25);
            this.lblDate.Name = "lblDate";
            this.lblDate.Size = new System.Drawing.Size(51, 21);
            this.lblDate.TabIndex = 5;
            this.lblDate.Text = "DATE:";
            // 
            // lblInvoiceNumber
            // 
            this.lblInvoiceNumber.AutoSize = true;
            this.lblInvoiceNumber.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.lblInvoiceNumber.ForeColor = System.Drawing.Color.White;
            this.lblInvoiceNumber.Location = new System.Drawing.Point(1100, 65);
            this.lblInvoiceNumber.Name = "lblInvoiceNumber";
            this.lblInvoiceNumber.Size = new System.Drawing.Size(96, 21);
            this.lblInvoiceNumber.TabIndex = 6;
            this.lblInvoiceNumber.Text = "INVOICE #:";
            // 
            // dtpInvoiceDate
            // 
            this.dtpInvoiceDate.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.dtpInvoiceDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpInvoiceDate.Location = new System.Drawing.Point(1160, 22);
            this.dtpInvoiceDate.Name = "dtpInvoiceDate";
            this.dtpInvoiceDate.Size = new System.Drawing.Size(210, 29);
            this.dtpInvoiceDate.TabIndex = 1;
            // 
            // txtInvoiceNumber
            // 
            this.txtInvoiceNumber.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.txtInvoiceNumber.Location = new System.Drawing.Point(1200, 62);
            this.txtInvoiceNumber.Name = "txtInvoiceNumber";
            this.txtInvoiceNumber.Size = new System.Drawing.Size(170, 29);
            this.txtInvoiceNumber.TabIndex = 2;
            this.txtInvoiceNumber.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            // 
            // lblCurrency
            // 
            this.lblCurrency.AutoSize = true;
            this.lblCurrency.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.lblCurrency.ForeColor = System.Drawing.Color.White;
            this.lblCurrency.Location = new System.Drawing.Point(1100, 105);
            this.lblCurrency.Name = "lblCurrency";
            this.lblCurrency.Size = new System.Drawing.Size(81, 21);
            this.lblCurrency.TabIndex = 11;
            this.lblCurrency.Text = "Currency:";
            // 
            // cboCurrency
            // 
            this.cboCurrency.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboCurrency.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.cboCurrency.FormattingEnabled = true;
            this.cboCurrency.Items.AddRange(new object[] {
            "LKR",
            "USD"});
            this.cboCurrency.Location = new System.Drawing.Point(1185, 102);
            this.cboCurrency.Name = "cboCurrency";
            this.cboCurrency.Size = new System.Drawing.Size(185, 29);
            this.cboCurrency.TabIndex = 3;
            // 
            // panelCustomer
            // 
            this.panelCustomer.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(250)))), ((int)(((byte)(252)))));
            this.panelCustomer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelCustomer.Controls.Add(this.lblBillTo);
            this.panelCustomer.Controls.Add(this.lblCustomerName);
            this.panelCustomer.Controls.Add(this.txtCustomerName);
            this.panelCustomer.Controls.Add(this.lblCompanyNameCustomer);
            this.panelCustomer.Controls.Add(this.txtCompanyName);
            this.panelCustomer.Controls.Add(this.lblAddress);
            this.panelCustomer.Controls.Add(this.txtAddress);
            this.panelCustomer.Controls.Add(this.lblTel);
            this.panelCustomer.Controls.Add(this.txtTel);
            this.panelCustomer.Controls.Add(this.lblEmail);
            this.panelCustomer.Controls.Add(this.txtEmail);
            this.panelCustomer.Location = new System.Drawing.Point(30, 160);
            this.panelCustomer.Name = "panelCustomer";
            this.panelCustomer.Size = new System.Drawing.Size(600, 260);
            this.panelCustomer.TabIndex = 2;
            // 
            // lblBillTo
            // 
            this.lblBillTo.AutoSize = true;
            this.lblBillTo.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblBillTo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(42)))), ((int)(((byte)(64)))));
            this.lblBillTo.Location = new System.Drawing.Point(15, 15);
            this.lblBillTo.Name = "lblBillTo";
            this.lblBillTo.Size = new System.Drawing.Size(70, 25);
            this.lblBillTo.TabIndex = 0;
            this.lblBillTo.Text = "BILL TO";
            // 
            // lblCustomerName
            // 
            this.lblCustomerName.AutoSize = true;
            this.lblCustomerName.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.lblCustomerName.Location = new System.Drawing.Point(15, 55);
            this.lblCustomerName.Name = "lblCustomerName";
            this.lblCustomerName.Size = new System.Drawing.Size(55, 21);
            this.lblCustomerName.TabIndex = 1;
            this.lblCustomerName.Text = "Name:";
            // 
            // txtCustomerName
            // 
            this.txtCustomerName.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.txtCustomerName.Location = new System.Drawing.Point(140, 52);
            this.txtCustomerName.Name = "txtCustomerName";
            this.txtCustomerName.Size = new System.Drawing.Size(440, 29);
            this.txtCustomerName.TabIndex = 3;
            this.txtCustomerName.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            // 
            // lblCompanyNameCustomer
            // 
            this.lblCompanyNameCustomer.AutoSize = true;
            this.lblCompanyNameCustomer.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.lblCompanyNameCustomer.Location = new System.Drawing.Point(15, 87);
            this.lblCompanyNameCustomer.Name = "lblCompanyNameCustomer";
            this.lblCompanyNameCustomer.Size = new System.Drawing.Size(122, 21);
            this.lblCompanyNameCustomer.TabIndex = 3;
            this.lblCompanyNameCustomer.Text = "Company Name:";
            // 
            // txtCompanyName
            // 
            this.txtCompanyName.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.txtCompanyName.Location = new System.Drawing.Point(140, 84);
            this.txtCompanyName.Name = "txtCompanyName";
            this.txtCompanyName.Size = new System.Drawing.Size(440, 29);
            this.txtCompanyName.TabIndex = 4;
            this.txtCompanyName.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            // 
            // lblAddress
            // 
            this.lblAddress.AutoSize = true;
            this.lblAddress.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.lblAddress.Location = new System.Drawing.Point(15, 119);
            this.lblAddress.Name = "lblAddress";
            this.lblAddress.Size = new System.Drawing.Size(71, 21);
            this.lblAddress.TabIndex = 5;
            this.lblAddress.Text = "Address:";
            // 
            // txtAddress
            // 
            this.txtAddress.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.txtAddress.Location = new System.Drawing.Point(140, 116);
            this.txtAddress.Multiline = true;
            this.txtAddress.Name = "txtAddress";
            this.txtAddress.Size = new System.Drawing.Size(440, 70);
            this.txtAddress.TabIndex = 5;
            this.txtAddress.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            // 
            // lblTel
            // 
            this.lblTel.AutoSize = true;
            this.lblTel.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.lblTel.Location = new System.Drawing.Point(15, 194);
            this.lblTel.Name = "lblTel";
            this.lblTel.Size = new System.Drawing.Size(31, 21);
            this.lblTel.TabIndex = 7;
            this.lblTel.Text = "Tel:";
            // 
            // txtTel
            // 
            this.txtTel.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.txtTel.Location = new System.Drawing.Point(140, 191);
            this.txtTel.Name = "txtTel";
            this.txtTel.Size = new System.Drawing.Size(220, 29);
            this.txtTel.TabIndex = 6;
            this.txtTel.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            // 
            // lblEmail
            // 
            this.lblEmail.AutoSize = true;
            this.lblEmail.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.lblEmail.Location = new System.Drawing.Point(15, 227);
            this.lblEmail.Name = "lblEmail";
            this.lblEmail.Size = new System.Drawing.Size(51, 21);
            this.lblEmail.TabIndex = 9;
            this.lblEmail.Text = "Email:";
            // 
            // txtEmail
            // 
            this.txtEmail.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.txtEmail.Location = new System.Drawing.Point(140, 224);
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.Size = new System.Drawing.Size(440, 29);
            this.txtEmail.TabIndex = 7;
            this.txtEmail.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            // 
            // dgvItems
            // 
            this.dgvItems.AllowUserToDeleteRows = true;
            this.dgvItems.BackgroundColor = System.Drawing.Color.White;
            this.dgvItems.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvItems.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvItems.Location = new System.Drawing.Point(30, 440);
            this.dgvItems.Name = "dgvItems";
            this.dgvItems.Size = new System.Drawing.Size(1340, 180);
            this.dgvItems.TabIndex = 8;
            // 
            // lblFoodItems
            // 
            this.lblFoodItems.AutoSize = true;
            this.lblFoodItems.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblFoodItems.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(42)))), ((int)(((byte)(64)))));
            this.lblFoodItems.Location = new System.Drawing.Point(30, 630);
            this.lblFoodItems.Name = "lblFoodItems";
            this.lblFoodItems.Size = new System.Drawing.Size(95, 21);
            this.lblFoodItems.TabIndex = 12;
            this.lblFoodItems.Text = "Food";
            // 
            // dgvFoodItems
            // 
            this.dgvFoodItems.AllowUserToDeleteRows = true;
            this.dgvFoodItems.BackgroundColor = System.Drawing.Color.White;
            this.dgvFoodItems.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvFoodItems.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvFoodItems.Location = new System.Drawing.Point(30, 655);
            this.dgvFoodItems.Name = "dgvFoodItems";
            this.dgvFoodItems.Size = new System.Drawing.Size(920, 150);
            this.dgvFoodItems.TabIndex = 9;
            // 
            // panelTotals
            // 
            this.panelTotals.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(250)))), ((int)(((byte)(252)))));
            this.panelTotals.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelTotals.Controls.Add(this.lblSubtotal);
            this.panelTotals.Controls.Add(this.txtSubtotal);
            this.panelTotals.Controls.Add(this.lblTaxRate);
            this.panelTotals.Controls.Add(this.txtTaxRate);
            this.panelTotals.Controls.Add(this.lblPercent);
            this.panelTotals.Controls.Add(this.lblTaxes);
            this.panelTotals.Controls.Add(this.txtTaxes);
            this.panelTotals.Controls.Add(this.lblOther);
            this.panelTotals.Controls.Add(this.txtOther);
            this.panelTotals.Controls.Add(this.lblTotal);
            this.panelTotals.Controls.Add(this.txtTotal);
            this.panelTotals.Controls.Add(this.lblPaid);
            this.panelTotals.Controls.Add(this.txtPaid);
            this.panelTotals.Controls.Add(this.lblTotalDue);
            this.panelTotals.Controls.Add(this.txtTotalDue);
            this.panelTotals.Controls.Add(this.lblAdvance);
            this.panelTotals.Controls.Add(this.txtAdvance);
            this.panelTotals.Location = new System.Drawing.Point(970, 655);
            this.panelTotals.Name = "panelTotals";
            this.panelTotals.Size = new System.Drawing.Size(400, 335);
            this.panelTotals.TabIndex = 10;
            // 
            // lblSubtotal
            // 
            this.lblSubtotal.AutoSize = true;
            this.lblSubtotal.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.lblSubtotal.Location = new System.Drawing.Point(20, 25);
            this.lblSubtotal.Name = "lblSubtotal";
            this.lblSubtotal.Size = new System.Drawing.Size(86, 21);
            this.lblSubtotal.TabIndex = 0;
            this.lblSubtotal.Text = "SUBTOTAL";
            // 
            // txtSubtotal
            // 
            this.txtSubtotal.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.txtSubtotal.Location = new System.Drawing.Point(230, 22);
            this.txtSubtotal.Name = "txtSubtotal";
            this.txtSubtotal.ReadOnly = true;
            this.txtSubtotal.Size = new System.Drawing.Size(150, 29);
            this.txtSubtotal.TabIndex = 1;
            this.txtSubtotal.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // lblTaxRate
            // 
            this.lblTaxRate.AutoSize = true;
            this.lblTaxRate.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.lblTaxRate.Location = new System.Drawing.Point(20, 60);
            this.lblTaxRate.Name = "lblTaxRate";
            this.lblTaxRate.Size = new System.Drawing.Size(72, 21);
            this.lblTaxRate.TabIndex = 2;
            this.lblTaxRate.Text = "Tax Rate:";
            // 
            // txtTaxRate
            // 
            this.txtTaxRate.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.txtTaxRate.Location = new System.Drawing.Point(120, 57);
            this.txtTaxRate.Name = "txtTaxRate";
            this.txtTaxRate.Size = new System.Drawing.Size(60, 29);
            this.txtTaxRate.TabIndex = 10;
            this.txtTaxRate.Text = "8.00";
            this.txtTaxRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // lblPercent
            // 
            this.lblPercent.AutoSize = true;
            this.lblPercent.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.lblPercent.Location = new System.Drawing.Point(190, 60);
            this.lblPercent.Name = "lblPercent";
            this.lblPercent.Size = new System.Drawing.Size(23, 21);
            this.lblPercent.TabIndex = 4;
            this.lblPercent.Text = "%";
            // 
            // lblTaxes
            // 
            this.lblTaxes.AutoSize = true;
            this.lblTaxes.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.lblTaxes.Location = new System.Drawing.Point(20, 95);
            this.lblTaxes.Name = "lblTaxes";
            this.lblTaxes.Size = new System.Drawing.Size(48, 21);
            this.lblTaxes.TabIndex = 5;
            this.lblTaxes.Text = "Taxes";
            // 
            // txtTaxes
            // 
            this.txtTaxes.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.txtTaxes.Location = new System.Drawing.Point(230, 92);
            this.txtTaxes.Name = "txtTaxes";
            this.txtTaxes.ReadOnly = true;
            this.txtTaxes.Size = new System.Drawing.Size(150, 29);
            this.txtTaxes.TabIndex = 6;
            this.txtTaxes.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // lblOther
            // 
            this.lblOther.AutoSize = true;
            this.lblOther.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.lblOther.Location = new System.Drawing.Point(20, 130);
            this.lblOther.Name = "lblOther";
            this.lblOther.Size = new System.Drawing.Size(49, 21);
            this.lblOther.TabIndex = 7;
            this.lblOther.Text = "Other";
            // 
            // txtOther
            // 
            this.txtOther.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.txtOther.Location = new System.Drawing.Point(230, 127);
            this.txtOther.Name = "txtOther";
            this.txtOther.Size = new System.Drawing.Size(150, 29);
            this.txtOther.TabIndex = 11;
            this.txtOther.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // lblTotal
            // 
            this.lblTotal.AutoSize = true;
            this.lblTotal.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblTotal.Location = new System.Drawing.Point(20, 170);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(70, 25);
            this.lblTotal.TabIndex = 9;
            this.lblTotal.Text = "TOTAL";
            // 
            // txtTotal
            // 
            this.txtTotal.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.txtTotal.Location = new System.Drawing.Point(230, 167);
            this.txtTotal.Name = "txtTotal";
            this.txtTotal.ReadOnly = true;
            this.txtTotal.Size = new System.Drawing.Size(150, 32);
            this.txtTotal.TabIndex = 10;
            this.txtTotal.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // lblPaid
            // 
            this.lblPaid.AutoSize = true;
            this.lblPaid.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.lblPaid.Location = new System.Drawing.Point(20, 210);
            this.lblPaid.Name = "lblPaid";
            this.lblPaid.Size = new System.Drawing.Size(43, 21);
            this.lblPaid.TabIndex = 11;
            this.lblPaid.Text = "PAID";
            // 
            // txtPaid
            // 
            this.txtPaid.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.txtPaid.Location = new System.Drawing.Point(230, 207);
            this.txtPaid.Name = "txtPaid";
            this.txtPaid.Size = new System.Drawing.Size(150, 29);
            this.txtPaid.TabIndex = 12;
            this.txtPaid.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // lblTotalDue
            // 
            this.lblTotalDue.AutoSize = true;
            this.lblTotalDue.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblTotalDue.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.lblTotalDue.Location = new System.Drawing.Point(20, 250);
            this.lblTotalDue.Name = "lblTotalDue";
            this.lblTotalDue.Size = new System.Drawing.Size(111, 25);
            this.lblTotalDue.TabIndex = 13;
            this.lblTotalDue.Text = "TOTAL DUE";
            // 
            // txtTotalDue
            // 
            this.txtTotalDue.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.txtTotalDue.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtTotalDue.Location = new System.Drawing.Point(230, 247);
            this.txtTotalDue.Name = "txtTotalDue";
            this.txtTotalDue.ReadOnly = true;
            this.txtTotalDue.Size = new System.Drawing.Size(150, 32);
            this.txtTotalDue.TabIndex = 14;
            this.txtTotalDue.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // lblAdvance
            // 
            this.lblAdvance.AutoSize = true;
            this.lblAdvance.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.lblAdvance.Location = new System.Drawing.Point(20, 290);
            this.lblAdvance.Name = "lblAdvance";
            this.lblAdvance.Size = new System.Drawing.Size(94, 21);
            this.lblAdvance.TabIndex = 15;
            this.lblAdvance.Text = "Advance Bal";
            // 
            // txtAdvance
            // 
            this.txtAdvance.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.txtAdvance.Location = new System.Drawing.Point(230, 287);
            this.txtAdvance.Name = "txtAdvance";
            this.txtAdvance.Size = new System.Drawing.Size(150, 29);
            this.txtAdvance.TabIndex = 13;
            this.txtAdvance.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // panelNotes
            // 
            this.panelNotes.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(250)))), ((int)(((byte)(252)))));
            this.panelNotes.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelNotes.Controls.Add(this.lblNotes);
            this.panelNotes.Controls.Add(this.txtNotes);
            this.panelNotes.Location = new System.Drawing.Point(30, 820);
            this.panelNotes.Name = "panelNotes";
            this.panelNotes.Size = new System.Drawing.Size(920, 170);
            this.panelNotes.TabIndex = 14;
            // 
            // lblNotes
            // 
            this.lblNotes.AutoSize = true;
            this.lblNotes.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblNotes.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(42)))), ((int)(((byte)(64)))));
            this.lblNotes.Location = new System.Drawing.Point(20, 20);
            this.lblNotes.Name = "lblNotes";
            this.lblNotes.Size = new System.Drawing.Size(66, 25);
            this.lblNotes.TabIndex = 0;
            this.lblNotes.Text = "Notes:";
            // 
            // txtNotes (RichTextBox)
            // 
            this.txtNotes.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.txtNotes.Location = new System.Drawing.Point(20, 55);
            this.txtNotes.Name = "txtNotes";
            this.txtNotes.Size = new System.Drawing.Size(880, 100);
            this.txtNotes.TabIndex = 14;
            this.txtNotes.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            // 
            // panelButtons
            // 
            this.panelButtons.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(252)))), ((int)(((byte)(255)))));
            this.panelButtons.Controls.Add(this.btnNew);
            this.panelButtons.Controls.Add(this.btnLoad);
            this.panelButtons.Controls.Add(this.btnSave);
            this.panelButtons.Controls.Add(this.btnExportPdf);
            this.panelButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelButtons.Location = new System.Drawing.Point(0, 1030);
            this.panelButtons.Name = "panelButtons";
            this.panelButtons.Size = new System.Drawing.Size(1400, 70);
            this.panelButtons.TabIndex = 15;
            // 
            // btnNew
            // 
            this.btnNew.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(76)))), ((int)(((byte)(175)))), ((int)(((byte)(80)))));
            this.btnNew.FlatAppearance.BorderSize = 0;
            this.btnNew.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNew.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnNew.ForeColor = System.Drawing.Color.White;
            this.btnNew.Location = new System.Drawing.Point(30, 20);
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(120, 40);
            this.btnNew.TabIndex = 15;
            this.btnNew.Text = "New Invoice";
            this.btnNew.UseVisualStyleBackColor = false;
            // 
            // btnLoad
            // 
            this.btnLoad.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(150)))), ((int)(((byte)(243)))));
            this.btnLoad.FlatAppearance.BorderSize = 0;
            this.btnLoad.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLoad.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnLoad.ForeColor = System.Drawing.Color.White;
            this.btnLoad.Location = new System.Drawing.Point(170, 20);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(120, 40);
            this.btnLoad.TabIndex = 16;
            this.btnLoad.Text = "Load Invoice";
            this.btnLoad.UseVisualStyleBackColor = false;
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(152)))), ((int)(((byte)(0)))));
            this.btnSave.FlatAppearance.BorderSize = 0;
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnSave.ForeColor = System.Drawing.Color.White;
            this.btnSave.Location = new System.Drawing.Point(310, 20);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(120, 40);
            this.btnSave.TabIndex = 17;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = false;
            // 
            // btnExportPdf
            // 
            this.btnExportPdf.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(67)))), ((int)(((byte)(54)))));
            this.btnExportPdf.FlatAppearance.BorderSize = 0;
            this.btnExportPdf.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExportPdf.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnExportPdf.ForeColor = System.Drawing.Color.White;
            this.btnExportPdf.Location = new System.Drawing.Point(450, 20);
            this.btnExportPdf.Name = "btnExportPdf";
            this.btnExportPdf.Size = new System.Drawing.Size(120, 40);
            this.btnExportPdf.TabIndex = 18;
            this.btnExportPdf.Text = "Export PDF";
            this.btnExportPdf.UseVisualStyleBackColor = false;
            // 
            // MainInvoiceForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1400, 1100);
            this.Controls.Add(this.panelButtons);
            this.Controls.Add(this.panelNotes);
            this.Controls.Add(this.panelTotals);
            this.Controls.Add(this.dgvFoodItems);
            this.Controls.Add(this.lblFoodItems);
            this.Controls.Add(this.dgvItems);
            this.Controls.Add(this.panelCustomer);
            this.Controls.Add(this.panelHeader);
            this.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
            this.MaximizeBox = true;
            this.MinimumSize = new System.Drawing.Size(1420, 600);
            this.Name = "MainInvoiceForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Haran Invoice Software - Professional Invoicing Solution";
            this.panelHeader.ResumeLayout(false);
            this.panelHeader.PerformLayout();
            this.panelCustomer.ResumeLayout(false);
            this.panelCustomer.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvItems)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvFoodItems)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLogo)).EndInit();
            this.panelTotals.ResumeLayout(false);
            this.panelTotals.PerformLayout();
            this.panelNotes.ResumeLayout(false);
            this.panelNotes.PerformLayout();
            this.panelButtons.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelHeader;
        private System.Windows.Forms.PictureBox pictureBoxLogo;
        private System.Windows.Forms.Label lblCompanyName;
        private System.Windows.Forms.Label lblCompanyAddress;
        private System.Windows.Forms.Label lblCompanyPhone;
        private System.Windows.Forms.Label lblCompanyEmail;
        private System.Windows.Forms.Label lblInvoice;
        private System.Windows.Forms.Label lblDate;
        private System.Windows.Forms.Label lblInvoiceNumber;
        private System.Windows.Forms.DateTimePicker dtpInvoiceDate;
        private System.Windows.Forms.TextBox txtInvoiceNumber;
        private System.Windows.Forms.Panel panelCustomer;
        private System.Windows.Forms.Label lblBillTo;
        private System.Windows.Forms.Label lblCustomerName;
        private System.Windows.Forms.TextBox txtCustomerName;
        private System.Windows.Forms.Label lblCompanyNameCustomer;
        private System.Windows.Forms.TextBox txtCompanyName;
        private System.Windows.Forms.Label lblAddress;
        private System.Windows.Forms.TextBox txtAddress;
        private System.Windows.Forms.Label lblTel;
        private System.Windows.Forms.TextBox txtTel;
        private System.Windows.Forms.Label lblEmail;
        private System.Windows.Forms.TextBox txtEmail;
        private System.Windows.Forms.DataGridView dgvItems;
        private System.Windows.Forms.Label lblFoodItems;
        private System.Windows.Forms.DataGridView dgvFoodItems;
        private System.Windows.Forms.Panel panelTotals;
        private System.Windows.Forms.Label lblSubtotal;
        private System.Windows.Forms.TextBox txtSubtotal;
        private System.Windows.Forms.Label lblTaxRate;
        private System.Windows.Forms.TextBox txtTaxRate;
        private System.Windows.Forms.Label lblPercent;
        private System.Windows.Forms.Label lblTaxes;
        private System.Windows.Forms.TextBox txtTaxes;
        private System.Windows.Forms.Label lblOther;
        private System.Windows.Forms.TextBox txtOther;
        private System.Windows.Forms.Label lblTotal;
        private System.Windows.Forms.TextBox txtTotal;
        private System.Windows.Forms.Label lblPaid;
        private System.Windows.Forms.TextBox txtPaid;
        private System.Windows.Forms.Label lblTotalDue;
        private System.Windows.Forms.TextBox txtTotalDue;
        private System.Windows.Forms.Label lblAdvance;
        private System.Windows.Forms.TextBox txtAdvance;
        private System.Windows.Forms.Panel panelNotes;
        private System.Windows.Forms.Label lblNotes;
        private System.Windows.Forms.RichTextBox txtNotes;
        private System.Windows.Forms.Panel panelButtons;
        private System.Windows.Forms.Button btnNew;
        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnExportPdf;
        private System.Windows.Forms.Label lblCurrency;
        private System.Windows.Forms.ComboBox cboCurrency;
    }
}
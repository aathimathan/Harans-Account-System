using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using HaranInvoiceSoftware.Models;
using HaranInvoiceSoftware.Services;
using HaranInvoiceSoftware.Utils;

namespace HaranInvoiceSoftware.Forms
{
    public partial class MainInvoiceForm : Form
    {
        private Invoice _currentInvoice;
        private InvoiceDataService _dataService;
        private PdfExportService _pdfService;
        private DataTable _itemsTable;
        private DataTable _foodItemsTable;
        private int _rightClickedRowIndex = -1;
        private DataGridView _rightClickedGrid = null;

        public MainInvoiceForm()
        {
            InitializeComponent();
            _dataService = new InvoiceDataService();
            _pdfService = new PdfExportService();
            InitializeDataGrid();
            InitializeFoodDataGrid();
            LoadCompanyLogo();
            LoadLastInvoice();
            SetupEventHandlers();
            InitializeCurrencySelector();
            RefreshCurrencyDisplays();
        }

        private void InitializeCurrencySelector()
        {
            try
            {
                string code = _currentInvoice?.CurrencyCode ?? "LKR";
                if (cboCurrency != null)
                {
                    if (!cboCurrency.Items.Contains(code))
                        cboCurrency.Items.Add(code);
                    cboCurrency.SelectedItem = code;
                    cboCurrency.SelectedIndexChanged -= CboCurrency_SelectedIndexChanged; // avoid double subscription
                    cboCurrency.SelectedIndexChanged += CboCurrency_SelectedIndexChanged;
                }
            }
            catch { /* ignore UI init errors */ }
        }

        private void CboCurrency_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_currentInvoice == null) return;
            if (cboCurrency?.SelectedItem == null) return;
            string newCode = cboCurrency.SelectedItem.ToString();
            if (string.Equals(_currentInvoice.CurrencyCode, newCode, StringComparison.OrdinalIgnoreCase)) return;
            _currentInvoice.CurrencyCode = newCode;
            RefreshCurrencyDisplays();
            AutoSave();
        }

        private void RefreshCurrencyDisplays()
        {
            if (_currentInvoice == null) return;
            ReformatTotals(_currentInvoice.CurrencyCode ?? "LKR");
            dgvItems?.Refresh();
            dgvFoodItems?.Refresh();
        }

        private void ReformatTotals(string code)
        {
            // Re-parse without assuming existing symbol
            if (CurrencyHelper.TryParse(txtSubtotal.Text, code, out var subtotal))
                txtSubtotal.Text = CurrencyHelper.Format(subtotal, code);
            if (CurrencyHelper.TryParse(txtTaxes.Text, code, out var taxes))
                txtTaxes.Text = CurrencyHelper.Format(taxes, code);
            if (CurrencyHelper.TryParse(txtTotal.Text, code, out var total))
                txtTotal.Text = CurrencyHelper.Format(total, code);
            if (CurrencyHelper.TryParse(txtTotalDue.Text, code, out var totalDue))
                txtTotalDue.Text = CurrencyHelper.Format(totalDue, code);

            // Editable numeric fields keep plain numbers for easy editing
            if (CurrencyHelper.TryParse(txtPaid.Text, code, out var paid))
                txtPaid.Text = CurrencyHelper.FormatShort(paid);
            if (CurrencyHelper.TryParse(txtOther.Text, code, out var other))
                txtOther.Text = CurrencyHelper.FormatShort(other);
            if (CurrencyHelper.TryParse(txtAdvance.Text, code, out var adv))
                txtAdvance.Text = CurrencyHelper.FormatShort(adv);
        }

        private void InitializeDataGrid()
        {
            _itemsTable = new DataTable();
            _itemsTable.Columns.Add("Description", typeof(string));
            _itemsTable.Columns.Add("Check In", typeof(DateTime));
            _itemsTable.Columns.Add("Check Out", typeof(DateTime));
            _itemsTable.Columns.Add("# of Nights", typeof(int));
            _itemsTable.Columns.Add("Price / Night", typeof(decimal));
            _itemsTable.Columns.Add("Total", typeof(decimal));

            // Add event handler for DataTable changes
            _itemsTable.ColumnChanged += ItemsTable_ColumnChanged;

            dgvItems.DataSource = _itemsTable;
            dgvItems.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // Style the DataGridView
            dgvItems.EnableHeadersVisualStyles = false;
            dgvItems.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(37, 42, 64);
            dgvItems.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvItems.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            dgvItems.RowHeadersDefaultCellStyle.BackColor = Color.FromArgb(45, 50, 72);
            dgvItems.RowHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvItems.RowHeadersWidth = 30;
            dgvItems.RowHeadersVisible = true;
            dgvItems.DefaultCellStyle.Font = new Font("Segoe UI", 10);
            dgvItems.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 247, 250);
            dgvItems.SelectionMode = DataGridViewSelectionMode.CellSelect;
            dgvItems.MultiSelect = false;
            dgvItems.AllowUserToAddRows = true;
            dgvItems.AllowUserToDeleteRows = true;
            dgvItems.EditMode = DataGridViewEditMode.EditOnEnter;
            dgvItems.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;

            // Set date column formats
            foreach (DataGridViewColumn column in dgvItems.Columns)
            {
                if (column.Name == "Check In" || column.Name == "Check Out")
                {
                    column.DefaultCellStyle.Format = "dd-MMM-yy";
                }
                // Currency formatting is handled in DgvItems_CellFormatting event
            }

            // Add date picker functionality for date columns
            SetupDatePickerColumns();

            // Add a default empty row
            AddEmptyRow();
        }

        private void AddEmptyRow()
        {
            _itemsTable.Rows.Add("Entire villa", DateTime.Now, DateTime.Now.AddDays(1), 1, 45000.00m, 45000.00m);
        }

        private void InitializeFoodDataGrid()
        {
            _foodItemsTable = new DataTable();
            _foodItemsTable.Columns.Add("Description", typeof(string));
            _foodItemsTable.Columns.Add("Note", typeof(string));
            _foodItemsTable.Columns.Add("Price", typeof(decimal));

            // Add event handler for DataTable changes
            _foodItemsTable.ColumnChanged += FoodItemsTable_ColumnChanged;

            dgvFoodItems.DataSource = _foodItemsTable;
            dgvFoodItems.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // Style the DataGridView
            dgvFoodItems.EnableHeadersVisualStyles = false;
            dgvFoodItems.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(37, 42, 64);
            dgvFoodItems.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvFoodItems.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            dgvFoodItems.RowHeadersDefaultCellStyle.BackColor = Color.FromArgb(45, 50, 72);
            dgvFoodItems.RowHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvFoodItems.RowHeadersWidth = 30;
            dgvFoodItems.RowHeadersVisible = true;
            dgvFoodItems.DefaultCellStyle.Font = new Font("Segoe UI", 10);
            dgvFoodItems.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 247, 250);
            dgvFoodItems.SelectionMode = DataGridViewSelectionMode.CellSelect;
            dgvFoodItems.MultiSelect = false;
            dgvFoodItems.AllowUserToAddRows = true;
            dgvFoodItems.AllowUserToDeleteRows = true;
            dgvFoodItems.EditMode = DataGridViewEditMode.EditOnEnter;
            dgvFoodItems.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;

            // Set price column format
            foreach (DataGridViewColumn column in dgvFoodItems.Columns)
            {
                if (column.Name == "Price")
                {
                    // Currency formatting is handled in CellFormatting event
                }
            }
        }

        private void LoadCompanyLogo()
        {
            try
            {
                // Try multiple possible paths
                var possiblePaths = new[]
                {
                    System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets", "nallur-residence-logo.png"),
                    System.IO.Path.Combine(Environment.CurrentDirectory, "Assets", "nallur-residence-logo.png"),
                    System.IO.Path.Combine(@"c:\Haran Invoice Software", "Assets", "nallur-residence-logo.png"),
                    System.IO.Path.Combine(Application.StartupPath, "Assets", "nallur-residence-logo.png")
                };

                string foundPath = null;
                foreach (var path in possiblePaths)
                {
                    System.Diagnostics.Debug.WriteLine($"Checking path: {path}");
                    if (System.IO.File.Exists(path))
                    {
                        foundPath = path;
                        System.Diagnostics.Debug.WriteLine($"Found logo at: {path}");
                        break;
                    }
                }

                if (foundPath != null)
                {
                    pictureBoxLogo.Image = Image.FromFile(foundPath);
                    pictureBoxLogo.Visible = true;
                    System.Diagnostics.Debug.WriteLine("Logo loaded successfully");
                }
                else
                {
                    // Create a placeholder text if logo doesn't exist
                    var placeholder = new Bitmap(120, 110);
                    using (var g = Graphics.FromImage(placeholder))
                    {
                        g.FillRectangle(new SolidBrush(Color.FromArgb(100, 150, 200)), 0, 0, 120, 110);
                        g.DrawString("LOGO\nNOT FOUND", new Font("Segoe UI", 9, FontStyle.Bold), Brushes.White, new RectangleF(0, 0, 120, 110),
                            new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                    }
                    pictureBoxLogo.Image = placeholder;
                    pictureBoxLogo.Visible = true;
                    System.Diagnostics.Debug.WriteLine("Logo not found in any path, showing placeholder");
                }
            }
            catch (Exception ex)
            {
                // Show error in placeholder instead of hiding
                System.Diagnostics.Debug.WriteLine($"Error loading logo: {ex.Message}");
                var errorPlaceholder = new Bitmap(120, 110);
                using (var g = Graphics.FromImage(errorPlaceholder))
                {
                    g.FillRectangle(new SolidBrush(Color.FromArgb(200, 100, 100)), 0, 0, 120, 110);
                    g.DrawString("LOGO\nERROR", new Font("Segoe UI", 9, FontStyle.Bold), Brushes.White, new RectangleF(0, 0, 120, 110),
                        new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                }
                pictureBoxLogo.Image = errorPlaceholder;
                pictureBoxLogo.Visible = true;
            }
        }

        private void SetupEventHandlers()
        {
            // Button events
            btnNew.Click += BtnNew_Click;
            btnLoad.Click += BtnLoad_Click;
            btnSave.Click += BtnSave_Click;
            btnExportPdf.Click += BtnExportPdf_Click;

            // Auto-save events
            txtCustomerName.TextChanged += (s, e) => AutoSave();
            txtCompanyName.TextChanged += (s, e) => AutoSave();
            txtAddress.TextChanged += (s, e) => AutoSave();
            txtTel.TextChanged += (s, e) => AutoSave();
            txtEmail.TextChanged += (s, e) => AutoSave();
            txtOther.TextChanged += (s, e) => { CalculateTotals(); AutoSave(); };
            txtPaid.TextChanged += (s, e) => { CalculateTotals(); AutoSave(); };
            txtAdvance.TextChanged += (s, e) => { CalculateTotals(); AutoSave(); };
            txtTaxRate.TextChanged += (s, e) => { CalculateTotals(); AutoSave(); };
            txtNotes.TextChanged += (s, e) => AutoSave();
            dtpInvoiceDate.ValueChanged += (s, e) => AutoSave();
            txtInvoiceNumber.TextChanged += (s, e) => AutoSave();

            // DataGridView events
            dgvItems.CellValueChanged += DgvItems_CellValueChanged;
            dgvItems.UserAddedRow += (s, e) => { CalculateTotals(); AutoSave(); };
            dgvItems.UserDeletedRow += (s, e) => { CalculateTotals(); AutoSave(); };
            dgvItems.CellClick += DgvItems_CellClick;
            dgvItems.CellDoubleClick += DgvItems_CellDoubleClick;
            dgvItems.CellEndEdit += DgvItems_CellEndEdit;
            dgvItems.CellFormatting += DgvItems_CellFormatting;
            dgvItems.MouseClick += DgvItems_MouseClick;

            // Food DataGridView events
            dgvFoodItems.CellValueChanged += DgvFoodItems_CellValueChanged;
            dgvFoodItems.CellEndEdit += DgvFoodItems_CellEndEdit;
            dgvFoodItems.UserAddedRow += (s, e) => { CalculateTotals(); AutoSave(); };
            dgvFoodItems.UserDeletedRow += (s, e) => { CalculateTotals(); AutoSave(); };
            dgvFoodItems.CellFormatting += DgvFoodItems_CellFormatting;
            dgvFoodItems.CellClick += DgvFoodItems_CellClick;
            dgvFoodItems.MouseClick += DgvFoodItems_MouseClick;

            // Setup context menus
            SetupContextMenus();
        }

        private DateTimePicker _dateTimePicker;
        private bool _isUpdatingDatePicker = false; // Flag to prevent recursive updates

        private void SetupDatePickerColumns()
        {
            // Create a DateTimePicker control for date editing
            _dateTimePicker = new DateTimePicker();
            _dateTimePicker.Format = DateTimePickerFormat.Short;
            _dateTimePicker.Visible = false;
            _dateTimePicker.ValueChanged += DateTimePicker_ValueChanged;
            _dateTimePicker.KeyDown += DateTimePicker_KeyDown;
            _dateTimePicker.Leave += DateTimePicker_Leave;
            dgvItems.Controls.Add(_dateTimePicker);
        }

        private void SetupContextMenus()
        {
            // Create context menu for accommodation items
            var contextMenuItems = new ContextMenuStrip();
            var addItemMenuItem = new ToolStripMenuItem("Add New Row");
            addItemMenuItem.Click += AddItemMenuItem_Click;
            var deleteItemMenuItem = new ToolStripMenuItem("Delete Row");
            deleteItemMenuItem.Click += DeleteItemMenuItem_Click;
            contextMenuItems.Items.Add(addItemMenuItem);
            contextMenuItems.Items.Add(new ToolStripSeparator());
            contextMenuItems.Items.Add(deleteItemMenuItem);

            // Create context menu for food items
            var contextMenuFood = new ContextMenuStrip();
            var addFoodMenuItem = new ToolStripMenuItem("Add New Food Item");
            addFoodMenuItem.Click += AddFoodMenuItem_Click;
            var deleteFoodMenuItem = new ToolStripMenuItem("Delete Food Item");
            deleteFoodMenuItem.Click += DeleteFoodMenuItem_Click;
            contextMenuFood.Items.Add(addFoodMenuItem);
            contextMenuFood.Items.Add(new ToolStripSeparator());
            contextMenuFood.Items.Add(deleteFoodMenuItem);

            // Store context menus for later use
            dgvItems.Tag = contextMenuItems;
            dgvFoodItems.Tag = contextMenuFood;
        }

        private void DateTimePicker_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                _dateTimePicker.Visible = false;
                dgvItems.Focus();
            }
            else if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Tab)
            {
                _dateTimePicker.Visible = false;
                dgvItems.Focus();

                // Move to next cell if Tab was pressed
                if (e.KeyCode == Keys.Tab && dgvItems.CurrentCell != null)
                {
                    int nextColumn = dgvItems.CurrentCell.ColumnIndex + 1;
                    int currentRow = dgvItems.CurrentCell.RowIndex;

                    if (nextColumn < dgvItems.Columns.Count)
                    {
                        dgvItems.CurrentCell = dgvItems[nextColumn, currentRow];
                    }
                }
            }
        }

        private void DateTimePicker_Leave(object sender, EventArgs e)
        {
            // Ensure the last change is committed before hiding
            if (_dateTimePicker.Tag is Point cellPosition)
            {
                int columnIndex = cellPosition.X;
                int rowIndex = cellPosition.Y;

                // Final update of DataTable
                if (rowIndex < dgvItems.Rows.Count && columnIndex < dgvItems.Columns.Count &&
                    rowIndex < _itemsTable.Rows.Count)
                {
                    var columnName = dgvItems.Columns[columnIndex].Name;
                    _itemsTable.Rows[rowIndex][columnName] = _dateTimePicker.Value;

                    System.Diagnostics.Debug.WriteLine($"DateTimePicker leave: {columnName} = {_dateTimePicker.Value:yyyy-MM-dd}");
                }
            }

            _dateTimePicker.Visible = false;
        }

        private void DgvItems_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Switch back to cell select mode when a cell is clicked
            if (dgvItems.SelectionMode == DataGridViewSelectionMode.FullRowSelect)
            {
                dgvItems.SelectionMode = DataGridViewSelectionMode.CellSelect;
                dgvItems.MultiSelect = false;
            }

            // Show date picker for Check In and Check Out columns
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                DataGridViewColumn column = dgvItems.Columns[e.ColumnIndex];
                if (column.Name == "Check In" || column.Name == "Check Out")
                {
                    ShowDateTimePicker(e.RowIndex, e.ColumnIndex);
                }
                else
                {
                    // For non-date columns, enter edit mode immediately
                    dgvItems.BeginEdit(false);
                }
            }
        }

        private void DgvFoodItems_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Switch back to cell select mode when a cell is clicked
            if (dgvFoodItems.SelectionMode == DataGridViewSelectionMode.FullRowSelect)
            {
                dgvFoodItems.SelectionMode = DataGridViewSelectionMode.CellSelect;
                dgvFoodItems.MultiSelect = false;
            }

            // For food items grid, enter edit mode immediately on single click
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                dgvFoodItems.BeginEdit(false);
            }
        }

        private void DgvItems_MouseClick(object sender, MouseEventArgs e)
        {
            // Show context menu on right click
            if (e.Button == MouseButtons.Right)
            {
                var hit = dgvItems.HitTest(e.X, e.Y);
                if (hit.RowIndex >= 0 && !dgvItems.Rows[hit.RowIndex].IsNewRow)
                {
                    // Store the right-clicked row information
                    _rightClickedRowIndex = hit.RowIndex;
                    _rightClickedGrid = dgvItems;

                    // Select the row
                    dgvItems.ClearSelection();
                    dgvItems.Rows[hit.RowIndex].Selected = true;

                    // Show context menu
                    var contextMenu = dgvItems.Tag as ContextMenuStrip;
                    contextMenu?.Show(dgvItems, e.Location);
                }
            }
        }

        private void DgvFoodItems_MouseClick(object sender, MouseEventArgs e)
        {
            // Show context menu on right click
            if (e.Button == MouseButtons.Right)
            {
                var hit = dgvFoodItems.HitTest(e.X, e.Y);
                if (hit.RowIndex >= 0 && !dgvFoodItems.Rows[hit.RowIndex].IsNewRow)
                {
                    // Store the right-clicked row information
                    _rightClickedRowIndex = hit.RowIndex;
                    _rightClickedGrid = dgvFoodItems;

                    // Select the row
                    dgvFoodItems.ClearSelection();
                    dgvFoodItems.Rows[hit.RowIndex].Selected = true;

                    // Show context menu
                    var contextMenu = dgvFoodItems.Tag as ContextMenuStrip;
                    contextMenu?.Show(dgvFoodItems, e.Location);
                }
            }
        }

        private void DeleteItemMenuItem_Click(object sender, EventArgs e)
        {
            // Delete the right-clicked row from accommodation items
            if (_rightClickedGrid == dgvItems && _rightClickedRowIndex >= 0 && _rightClickedRowIndex < dgvItems.Rows.Count)
            {
                var row = dgvItems.Rows[_rightClickedRowIndex];
                if (!row.IsNewRow)
                {
                    var result = MessageBox.Show("Are you sure you want to delete this accommodation item?",
                        "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (result == DialogResult.Yes)
                    {
                        dgvItems.Rows.RemoveAt(_rightClickedRowIndex);
                        CalculateTotals();
                        AutoSave();
                    }
                }
            }

            // Reset the stored values
            _rightClickedRowIndex = -1;
            _rightClickedGrid = null;
        }

        private void DeleteFoodMenuItem_Click(object sender, EventArgs e)
        {
            // Delete the right-clicked row from food items
            if (_rightClickedGrid == dgvFoodItems && _rightClickedRowIndex >= 0 && _rightClickedRowIndex < dgvFoodItems.Rows.Count)
            {
                var row = dgvFoodItems.Rows[_rightClickedRowIndex];
                if (!row.IsNewRow)
                {
                    var result = MessageBox.Show("Are you sure you want to delete this food item?",
                        "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (result == DialogResult.Yes)
                    {
                        System.Diagnostics.Debug.WriteLine($"Deleting food item at row index: {_rightClickedRowIndex}");
                        dgvFoodItems.Rows.RemoveAt(_rightClickedRowIndex);
                        CalculateTotals();
                        AutoSave();
                        System.Diagnostics.Debug.WriteLine("Food item deleted successfully");
                    }
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("Cannot delete new row");
                }
            }
            else
            {
                System.Diagnostics.Debug.WriteLine($"Invalid deletion attempt - Grid: {_rightClickedGrid?.Name}, RowIndex: {_rightClickedRowIndex}");
            }

            // Reset the stored values
            _rightClickedRowIndex = -1;
            _rightClickedGrid = null;
        }

        private void AddItemMenuItem_Click(object sender, EventArgs e)
        {
            // Add a new row to accommodation items
            _itemsTable.Rows.Add("Entire villa", DateTime.Now, DateTime.Now.AddDays(1), 1, 45000.00m, 45000.00m);
            CalculateTotals();
            AutoSave();
        }

        private void AddFoodMenuItem_Click(object sender, EventArgs e)
        {
            // Add a new row to food items
            _foodItemsTable.Rows.Add("", "", 0.00m);
            CalculateTotals();
            AutoSave();
        }

        private void DgvItems_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            // Also show date picker on double-click for date columns
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                DataGridViewColumn column = dgvItems.Columns[e.ColumnIndex];
                if (column.Name == "Check In" || column.Name == "Check Out")
                {
                    ShowDateTimePicker(e.RowIndex, e.ColumnIndex);
                }
            }
        }

        private void DgvItems_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            // Hide date picker when cell editing ends
            if (_dateTimePicker.Visible)
            {
                _dateTimePicker.Visible = false;
            }
        }

        private void DgvItems_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex >= 0 && e.ColumnIndex < dgvItems.Columns.Count)
            {
                string columnName = dgvItems.Columns[e.ColumnIndex].Name;
                if ((columnName == "Price / Night" || columnName == "Total") && e.Value != null)
                {
                    string raw = e.Value.ToString();
                    // strip possible previous symbols
                    raw = raw.Replace("Rs.", "").Replace("$", "").Trim();
                    if (decimal.TryParse(raw, out decimal amount))
                    {
                        string code = _currentInvoice?.CurrencyCode ?? "LKR";
                        e.Value = CurrencyHelper.Format(amount, code);
                        e.FormattingApplied = true;
                    }
                }
            }
        }

        private void ShowDateTimePicker(int rowIndex, int columnIndex)
        {
            Rectangle cellRect = dgvItems.GetCellDisplayRectangle(columnIndex, rowIndex, false);

            // Get current cell value or use default
            DateTime currentValue = DateTime.Now;
            if (dgvItems[columnIndex, rowIndex].Value != null && dgvItems[columnIndex, rowIndex].Value != DBNull.Value)
            {
                if (DateTime.TryParse(dgvItems[columnIndex, rowIndex].Value.ToString(), out DateTime parsedDate))
                {
                    currentValue = parsedDate;
                }
            }

            // Set flag to prevent ValueChanged event when initializing
            _isUpdatingDatePicker = true;
            try
            {
                _dateTimePicker.Value = currentValue;
                _dateTimePicker.Location = new Point(cellRect.X, cellRect.Y);
                _dateTimePicker.Size = new Size(cellRect.Width, cellRect.Height);
                _dateTimePicker.Tag = new Point(columnIndex, rowIndex); // Store cell position
            }
            finally
            {
                _isUpdatingDatePicker = false;
            }

            _dateTimePicker.Visible = true;
            _dateTimePicker.Focus();
        }

        private void DateTimePicker_ValueChanged(object sender, EventArgs e)
        {
            // Prevent recursive updates
            if (_isUpdatingDatePicker) return;

            if (_dateTimePicker.Tag is Point cellPosition)
            {
                int columnIndex = cellPosition.X;
                int rowIndex = cellPosition.Y;

                // Update the cell value and DataTable
                if (rowIndex < dgvItems.Rows.Count && columnIndex < dgvItems.Columns.Count &&
                    rowIndex < _itemsTable.Rows.Count)
                {
                    var columnName = dgvItems.Columns[columnIndex].Name;

                    // Set flag to prevent recursive calls
                    _isUpdatingDatePicker = true;

                    try
                    {
                        // Update the DataTable directly (this will trigger ItemsTable_ColumnChanged)
                        _itemsTable.Rows[rowIndex][columnName] = _dateTimePicker.Value;

                        System.Diagnostics.Debug.WriteLine($"DateTimePicker changed: {columnName} = {_dateTimePicker.Value:yyyy-MM-dd}");
                    }
                    finally
                    {
                        _isUpdatingDatePicker = false;
                    }
                }
            }
        }

        private void DgvItems_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < _itemsTable.Rows.Count)
            {
                var row = _itemsTable.Rows[e.RowIndex];

                // Get column name to determine which field was changed
                string columnName = "";
                if (e.ColumnIndex >= 0 && e.ColumnIndex < dgvItems.Columns.Count)
                {
                    columnName = dgvItems.Columns[e.ColumnIndex].Name;
                }

                // Calculate nights when either check-in or check-out date changes
                if (columnName == "Check In" || columnName == "Check Out" ||
                    (row["Check In"] != DBNull.Value && row["Check Out"] != DBNull.Value))
                {
                    if (row["Check In"] != DBNull.Value && row["Check Out"] != DBNull.Value)
                    {
                        DateTime checkIn = (DateTime)row["Check In"];
                        DateTime checkOut = (DateTime)row["Check Out"];

                        // Ensure check-out is after check-in
                        if (checkOut <= checkIn)
                        {
                            checkOut = checkIn.AddDays(1);
                            row["Check Out"] = checkOut;
                            // Update the DataGridView as well
                            dgvItems["Check Out", e.RowIndex].Value = checkOut;
                        }

                        int nights = (checkOut - checkIn).Days;
                        int calculatedNights = Math.Max(1, nights);
                        row["# of Nights"] = calculatedNights;

                        // Update the DataGridView display
                        dgvItems["# of Nights", e.RowIndex].Value = calculatedNights;
                    }
                }

                // Calculate total when nights or price changes
                if (row["# of Nights"] != DBNull.Value && row["Price / Night"] != DBNull.Value)
                {
                    int nights = Convert.ToInt32(row["# of Nights"]);
                    decimal pricePerNight = Convert.ToDecimal(row["Price / Night"]);
                    decimal total = nights * pricePerNight;
                    row["Total"] = total;

                    // Update the DataGridView display
                    dgvItems["Total", e.RowIndex].Value = total;
                }

                CalculateTotals();
                AutoSave();
            }
        }

        private void ItemsTable_ColumnChanged(object sender, DataColumnChangeEventArgs e)
        {
            // This handles direct changes to the DataTable, which is what happens with data binding
            if (e.Row.RowState == DataRowState.Detached) return; // Skip detached rows

            var row = e.Row;
            string columnName = e.Column.ColumnName;

            // Calculate nights when either check-in or check-out date changes
            if (columnName == "Check In" || columnName == "Check Out")
            {
                if (row["Check In"] != DBNull.Value && row["Check Out"] != DBNull.Value)
                {
                    DateTime checkIn = (DateTime)row["Check In"];
                    DateTime checkOut = (DateTime)row["Check Out"];

                    // Ensure check-out is after check-in
                    if (checkOut <= checkIn)
                    {
                        // Temporarily remove event handler to prevent cascading
                        _itemsTable.ColumnChanged -= ItemsTable_ColumnChanged;

                        try
                        {
                            checkOut = checkIn.AddDays(1);
                            row["Check Out"] = checkOut;
                        }
                        finally
                        {
                            // Re-add event handler
                            _itemsTable.ColumnChanged += ItemsTable_ColumnChanged;
                        }
                    }

                    int nights = (checkOut - checkIn).Days;
                    int calculatedNights = Math.Max(1, nights);
                    row["# of Nights"] = calculatedNights;

                    // Debug output
                    System.Diagnostics.Debug.WriteLine($"Calculated nights: {calculatedNights} for dates {checkIn:yyyy-MM-dd} to {checkOut:yyyy-MM-dd}");
                }
            }

            // Calculate total when nights or price changes
            if ((columnName == "# of Nights" || columnName == "Price / Night") &&
                row["# of Nights"] != DBNull.Value && row["Price / Night"] != DBNull.Value)
            {
                int nights = Convert.ToInt32(row["# of Nights"]);
                decimal pricePerNight = Convert.ToDecimal(row["Price / Night"]);
                decimal total = nights * pricePerNight;
                row["Total"] = total;

                // Debug output
                System.Diagnostics.Debug.WriteLine($"Calculated total: {total} for {nights} nights at {pricePerNight}");
            }

            CalculateTotals();
            AutoSave();
        }

        private void FoodItemsTable_ColumnChanged(object sender, DataColumnChangeEventArgs e)
        {
            // This handles direct changes to the food items DataTable
            if (e.Row.RowState == DataRowState.Detached) return; // Skip detached rows

            // When any food item changes, recalculate totals
            CalculateTotals();
            AutoSave();
        }

        private void DgvFoodItems_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            // Handle food items cell value changes
            CalculateTotals();
            AutoSave();
        }

        private void DgvFoodItems_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            // Force calculation when editing food items is completed
            CalculateTotals();
            AutoSave();
        }

        private void DgvFoodItems_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex >= 0 && e.ColumnIndex < dgvFoodItems.Columns.Count)
            {
                string columnName = dgvFoodItems.Columns[e.ColumnIndex].Name;
                if (columnName == "Price" && e.Value != null)
                {
                    string valueStr = e.Value.ToString();
                    valueStr = valueStr.Replace("Rs.", "").Replace("$", "").Trim();
                    if (decimal.TryParse(valueStr, out decimal amount) && amount > 0)
                    {
                        string code = _currentInvoice?.CurrencyCode ?? "LKR";
                        e.Value = CurrencyHelper.Format(amount, code);
                        e.FormattingApplied = true;
                    }
                }
            }
        }

        private void LoadLastInvoice()
        {
            try
            {
                _currentInvoice = _dataService.LoadLastInvoice();
                UpdateCompanyInfo(); // Ensure company info is current
                PopulateForm();
            }
            catch
            {
                _currentInvoice = new Invoice();
                UpdateCompanyInfo(); // Ensure company info is current
                PopulateForm();
            }
        }

        private void UpdateCompanyInfo()
        {
            // Ensure the company information is always current
            if (_currentInvoice != null)
            {
                _currentInvoice.Company = new Company();
            }
        }

        private void PopulateForm()
        {
            if (_currentInvoice == null) return;

            // Populate invoice details
            dtpInvoiceDate.Value = _currentInvoice.InvoiceDate;
            txtInvoiceNumber.Text = _currentInvoice.InvoiceNumber;

            // Populate customer details
            txtCustomerName.Text = _currentInvoice.Customer.Name;
            txtCompanyName.Text = _currentInvoice.Customer.CompanyName;
            txtAddress.Text = _currentInvoice.Customer.Address;
            txtTel.Text = _currentInvoice.Customer.Phone;
            txtEmail.Text = _currentInvoice.Customer.Email;

            // Populate items
            _itemsTable.Clear();
            foreach (var item in _currentInvoice.Items)
            {
                _itemsTable.Rows.Add(
                    item.Description,
                    item.CheckIn,
                    item.CheckOut,
                    item.NumberOfNights,
                    item.PricePerNight,
                    item.Total
                );
            }

            if (_itemsTable.Rows.Count == 0)
            {
                AddEmptyRow();
            }

            // Populate food items
            _foodItemsTable.Clear();
            foreach (var foodItem in _currentInvoice.FoodItems)
            {
                _foodItemsTable.Rows.Add(
                    foodItem.Description,
                    foodItem.Note,
                    foodItem.Price
                );
            }

            // Populate totals
            txtTaxRate.Text = (_currentInvoice.TaxRate * 100).ToString("F2");
            txtOther.Text = CurrencyHelper.FormatShort(_currentInvoice.Other);
            txtPaid.Text = CurrencyHelper.FormatShort(_currentInvoice.Paid);
            txtAdvance.Text = CurrencyHelper.FormatShort(_currentInvoice.Advance);
            txtNotes.Text = _currentInvoice.Notes;

            CalculateTotals();
            InitializeCurrencySelector();
            RefreshCurrencyDisplays();
        }

        private void UpdateInvoiceFromForm()
        {
            if (_currentInvoice == null) return;

            // Update invoice details
            _currentInvoice.InvoiceDate = dtpInvoiceDate.Value;
            _currentInvoice.InvoiceNumber = txtInvoiceNumber.Text;

            // Update customer details
            _currentInvoice.Customer.Name = txtCustomerName.Text;
            _currentInvoice.Customer.CompanyName = txtCompanyName.Text;
            _currentInvoice.Customer.Address = txtAddress.Text;
            _currentInvoice.Customer.Phone = txtTel.Text;
            _currentInvoice.Customer.Email = txtEmail.Text;

            // Update items
            _currentInvoice.Items.Clear();
            foreach (DataRow row in _itemsTable.Rows)
            {
                if (row.RowState != DataRowState.Deleted &&
                    row["Description"] != DBNull.Value &&
                    !string.IsNullOrEmpty(row["Description"].ToString()))
                {
                    var item = new InvoiceItem
                    {
                        Description = row["Description"].ToString(),
                        CheckIn = row["Check In"] != DBNull.Value ? (DateTime)row["Check In"] : DateTime.Now,
                        CheckOut = row["Check Out"] != DBNull.Value ? (DateTime)row["Check Out"] : DateTime.Now.AddDays(1),
                        NumberOfNights = row["# of Nights"] != DBNull.Value ? Convert.ToInt32(row["# of Nights"]) : 1,
                        PricePerNight = row["Price / Night"] != DBNull.Value ? Convert.ToDecimal(row["Price / Night"]) : 0m,
                        Total = row["Total"] != DBNull.Value ? Convert.ToDecimal(row["Total"]) : 0m
                    };
                    _currentInvoice.Items.Add(item);
                }
            }

            // Update food items
            _currentInvoice.FoodItems.Clear();
            foreach (DataRow row in _foodItemsTable.Rows)
            {
                if (row.RowState != DataRowState.Deleted &&
                    row["Description"] != DBNull.Value &&
                    !string.IsNullOrEmpty(row["Description"].ToString()))
                {
                    var foodItem = new FoodItem
                    {
                        Description = row["Description"].ToString(),
                        Note = row["Note"] != DBNull.Value ? row["Note"].ToString() : "",
                        Price = row["Price"] != DBNull.Value ? Convert.ToDecimal(row["Price"]) : 0m
                    };
                    _currentInvoice.FoodItems.Add(foodItem);
                }
            }

            // Update other fields
            decimal.TryParse(txtTaxRate.Text, out decimal taxRate);
            _currentInvoice.TaxRate = taxRate / 100.0m; // Convert percentage to decimal

            decimal.TryParse(txtOther.Text, out decimal other);
            _currentInvoice.Other = other;

            decimal.TryParse(txtPaid.Text, out decimal paid);
            _currentInvoice.Paid = paid;

            decimal.TryParse(txtAdvance.Text, out decimal advance);
            _currentInvoice.Advance = advance;

            _currentInvoice.Notes = txtNotes.Text;

            // Update currency from selector if available
            if (cboCurrency?.SelectedItem != null)
            {
                _currentInvoice.CurrencyCode = cboCurrency.SelectedItem.ToString();
            }

            _currentInvoice.CalculateTotals();
        }

        private void CalculateTotals()
        {
            decimal subtotal = 0;
            foreach (DataRow row in _itemsTable.Rows)
            {
                if (row.RowState != DataRowState.Deleted && row["Total"] != DBNull.Value)
                {
                    subtotal += Convert.ToDecimal(row["Total"]);
                }
            }

            // Add food items to subtotal
            foreach (DataRow row in _foodItemsTable.Rows)
            {
                if (row.RowState != DataRowState.Deleted && row["Price"] != DBNull.Value && row["Price"].ToString() != "")
                {
                    string code = _currentInvoice?.CurrencyCode ?? "LKR";
                    if (CurrencyHelper.TryParse(row["Price"].ToString(), code, out decimal foodPrice))
                    {
                        subtotal += foodPrice;
                    }
                }
            }

            decimal.TryParse(txtTaxRate.Text, out decimal taxRatePercent);
            decimal taxRate = taxRatePercent / 100.0m; // Convert percentage to decimal

            decimal.TryParse(txtOther.Text, out decimal other);
            decimal.TryParse(txtPaid.Text, out decimal paid);
            decimal.TryParse(txtAdvance.Text, out decimal advance);

            decimal taxes = subtotal * taxRate;
            decimal total = subtotal + taxes + other;
            decimal totalDue = total - paid - advance;

            string currencyCode = _currentInvoice?.CurrencyCode ?? "LKR";
            txtSubtotal.Text = CurrencyHelper.Format(subtotal, currencyCode);
            txtTaxes.Text = CurrencyHelper.Format(taxes, currencyCode);
            txtTotal.Text = CurrencyHelper.Format(total, currencyCode);
            txtTotalDue.Text = CurrencyHelper.Format(totalDue, currencyCode);
        }

        private void AutoSave()
        {
            try
            {
                UpdateInvoiceFromForm();
                _dataService.SaveInvoice(_currentInvoice);
            }
            catch
            {
                // Ignore auto-save errors
            }
        }

        private void BtnNew_Click(object sender, EventArgs e)
        {
            // Ask user for filename first
            string fileName = GetNewInvoiceFileName();
            if (string.IsNullOrEmpty(fileName))
            {
                return; // User cancelled
            }

            _currentInvoice = new Invoice();
            UpdateCompanyInfo(); // Ensure company info is current
            _itemsTable.Clear();
            AddEmptyRow();

            // Generate automatic invoice number based on datetime with seconds
            string invoiceNumber = GenerateInvoiceNumber();

            // Clear form fields
            txtCustomerName.Clear();
            txtCompanyName.Clear();
            txtAddress.Clear();
            txtTel.Clear();
            txtEmail.Clear();
            txtInvoiceNumber.Text = invoiceNumber;
            txtTaxRate.Text = "8.00";
            txtOther.Text = "0.00";
            txtPaid.Text = "0.00";
            txtAdvance.Text = "0.00";
            txtNotes.Clear();
            dtpInvoiceDate.Value = DateTime.Now;

            // Clear calculated totals
            string code = _currentInvoice?.CurrencyCode ?? (cboCurrency?.SelectedItem?.ToString() ?? "LKR");
            _currentInvoice.CurrencyCode = code;
            txtSubtotal.Text = CurrencyHelper.Format(0, code);
            txtTaxes.Text = CurrencyHelper.Format(0, code);
            txtTotal.Text = CurrencyHelper.Format(0, code);
            txtTotalDue.Text = CurrencyHelper.Format(0, code);

            // Set the filename for this invoice
            _currentInvoice.InvoiceNumber = invoiceNumber;
            _currentInvoice.FileName = fileName;
            _currentInvoice.CurrencyCode = cboCurrency?.SelectedItem?.ToString() ?? "LKR";

            CalculateTotals();
            RefreshCurrencyDisplays();
        }

        private void BtnLoad_Click(object sender, EventArgs e)
        {
            using (var dialog = new OpenFileDialog())
            {
                dialog.Filter = "XML files (*.xml)|*.xml";
                dialog.InitialDirectory = System.IO.Path.Combine(Application.StartupPath, "Invoices");

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        _currentInvoice = _dataService.LoadInvoice(dialog.FileName);
                        UpdateCompanyInfo(); // Ensure company info is current
                        PopulateForm();
                        MessageBox.Show("Invoice loaded successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error loading invoice: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                UpdateInvoiceFromForm();
                _dataService.SaveInvoice(_currentInvoice);
                MessageBox.Show("Invoice saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving invoice: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnExportPdf_Click(object sender, EventArgs e)
        {
            using (var dialog = new SaveFileDialog())
            {
                dialog.Filter = "PDF files (*.pdf)|*.pdf";
                dialog.DefaultExt = "pdf";
                dialog.FileName = $"Invoice_{_currentInvoice.InvoiceNumber}_{DateTime.Now:yyyyMMdd}.pdf";

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        UpdateInvoiceFromForm();
                        _pdfService.ExportToPdf(_currentInvoice, dialog.FileName);
                        MessageBox.Show("PDF exported successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Ask if user wants to open the PDF
                        if (MessageBox.Show("Would you like to open the PDF file?", "Open PDF", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                            {
                                FileName = dialog.FileName,
                                UseShellExecute = true
                            });
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error exporting PDF: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private string GenerateInvoiceNumber()
        {
            // Generate invoice number using current datetime with seconds
            // Format: INV-YYYYMMDD-HHMMSS (e.g., INV-20251011-143052)
            DateTime now = DateTime.Now;
            return $"INV-{now:yyyyMMdd}-{now:HHmmss}";
        }

        private string GetNewInvoiceFileName()
        {
            using (var dialog = new SaveFileDialog())
            {
                dialog.Filter = "XML files (*.xml)|*.xml";
                dialog.DefaultExt = "xml";
                dialog.Title = "Save New Invoice As";
                dialog.InitialDirectory = System.IO.Path.Combine(Application.StartupPath, "Invoices");

                // Suggest a filename based on current datetime
                string suggestedName = $"Invoice_{DateTime.Now:yyyyMMdd_HHmmss}";
                dialog.FileName = suggestedName;

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    return dialog.FileName;
                }

                return string.Empty; // User cancelled
            }
        }
    }
}
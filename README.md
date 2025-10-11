# Haran Invoice Software

A professional C# Windows Forms invoicing application with modern UI design, PDF export functionality, and XML-based data persistence.

## Features

- **Modern UI Design**: Clean, professional interface with state-of-the-art styling
- **PDF Export**: Generate professional PDF invoices using iText7
- **XML Data Storage**: Save and load invoices in XML format
- **Auto-Save**: Automatically saves invoice data as you type
- **Auto-Restore**: Automatically loads the last worked invoice on startup
- **Invoice Management**: Create new invoices, load existing ones, and manage invoice history

## Technology Stack

- **Framework**: .NET 6.0 Windows Forms
- **PDF Generation**: iText7
- **Data Storage**: XML Serialization
- **UI**: Windows Forms with modern styling

## Installation

1. Ensure you have .NET 6.0 SDK installed
2. Clone or download this repository
3. Open the project in Visual Studio or VS Code
4. Build and run the application

## Usage

### Creating a New Invoice

1. Click "New Invoice" to start fresh
2. Fill in customer details in the "BILL TO" section
3. Add invoice items in the data grid:
   - Description (e.g., "Entire villa")
   - Check-in and check-out dates
   - Number of nights (calculated automatically)
   - Price per night
   - Total (calculated automatically)
4. Enter any additional charges in "Other"
5. Enter payment amounts in "Paid" and "Advance Bal"
6. Add notes if needed

### Auto-Save Feature

The application automatically saves your work as you type in any field. You don't need to manually save unless you want to.

### Loading Existing Invoices

1. Click "Load Invoice" to browse and select a previously saved invoice
2. The application will populate all fields with the loaded data

### Exporting to PDF

1. Click "Export PDF" when your invoice is ready
2. Choose a location to save the PDF file
3. The application will generate a professional PDF invoice

### Auto-Restore

When you start the application, it automatically loads the last invoice you were working on.

## Project Structure

```
HaranInvoiceSoftware/
├── Models/
│   ├── Company.cs          # Company information model
│   ├── Customer.cs         # Customer information model
│   ├── InvoiceItem.cs      # Invoice line item model
│   └── Invoice.cs          # Main invoice model
├── Forms/
│   ├── MainInvoiceForm.cs  # Main application form
│   └── MainInvoiceForm.Designer.cs
├── Services/
│   ├── InvoiceDataService.cs   # XML data persistence
│   └── PdfExportService.cs     # PDF generation
├── Invoices/               # Auto-created folder for XML files
└── Program.cs              # Application entry point
```

## Data Storage

- Invoice data is stored in XML format in the `Invoices/` folder
- Each invoice is saved as a separate XML file
- The application remembers the last invoice you worked on
- XML files can be manually backed up or shared

## Customization

### Company Information

To change the default company information, modify the `Company.cs` model:

```csharp
public string Name { get; set; } = "YOUR COMPANY NAME";
public string Address { get; set; } = "Your Address";
public string Phone { get; set; } = "Your Phone";
public string Email { get; set; } = "your@email.com";
```

### Tax Rate

To change the default tax rate (currently 8%), modify the `Invoice.cs` model:

```csharp
public decimal TaxRate { get; set; } = 0.08m; // Change to your tax rate
```

### UI Colors

The application uses a modern color scheme. Key colors can be changed in the form designer:

- Header Background: `Color.FromArgb(37, 42, 64)` (Dark blue-gray)
- Panel Background: `Color.FromArgb(245, 247, 250)` (Light gray)
- Button Colors: Various material design colors

## Requirements

- Windows 10 or later
- .NET 6.0 Runtime
- Approximately 50MB disk space

## License

This software is provided as-is for educational and commercial use.

## Support

For support or feature requests, please contact the development team.
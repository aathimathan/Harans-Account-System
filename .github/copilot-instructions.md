# Haran Invoice Software - C# Windows Forms Application

This is a professional invoicing software built with C# Windows Forms that includes:
- Modern, state-of-the-art UI design
- PDF export functionality using iTextSharp
- XML-based data persistence for invoices
- Auto-save and auto-restore functionality
- Professional invoice templates

## Project Structure
- `Models/` - Data models for Invoice, Customer, etc.
- `Forms/` - Windows Forms for the application UI
- `Services/` - Business logic and data services
- `Utils/` - Utility classes and helpers
- `Invoices/` - XML storage for invoice data

## Dependencies
- System.Windows.Forms
- System.Xml.Serialization
- iTextSharp (for PDF generation)

## Development Notes
- Target Framework: .NET Framework 4.8 or .NET 6.0+
- Uses modern flat design principles
- Auto-saves invoice data on field changes
- Automatically loads last worked invoice on startup
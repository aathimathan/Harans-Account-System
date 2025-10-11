using System.Globalization;

namespace HaranInvoiceSoftware.Utils
{
    public static class CurrencyHelper
    {
        public static string FormatSriLankanRupees(decimal amount)
        {
            // Format as Sri Lankan Rupees with "Rs." prefix
            return $"Rs. {amount:N2}";
        }
        
        public static string FormatSriLankanRupeesShort(decimal amount)
        {
            // Format without "Rs." prefix for calculations
            return amount.ToString("N2");
        }
        
        public static bool TryParseSriLankanRupees(string input, out decimal result)
        {
            result = 0;
            if (string.IsNullOrWhiteSpace(input))
                return false;
                
            // Remove "Rs." prefix and any extra spaces
            string cleanInput = input.Replace("Rs.", "").Replace("Rs", "").Trim();
            
            // Try to parse the cleaned input
            return decimal.TryParse(cleanInput, NumberStyles.Number, CultureInfo.InvariantCulture, out result);
        }
    }
}
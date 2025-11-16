using System;
using System.Globalization;

namespace HaranInvoiceSoftware.Utils
{
    public static class CurrencyHelper
    {
        /// <summary>
        /// Format amount with currency symbol based on ISO-like currency code.
        /// Currently supports: LKR (Rs.), USD ($)
        /// </summary>
        public static string Format(decimal amount, string currencyCode)
        {
            string symbol = GetSymbol(currencyCode);
            return $"{symbol} {amount:N2}".Trim();
        }

        /// <summary>
        /// Format amount without symbol (for editable text boxes).
        /// </summary>
        public static string FormatShort(decimal amount)
        {
            return amount.ToString("N2");
        }

        /// <summary>
        /// Attempt to parse a currency string containing an optional symbol.
        /// </summary>
        public static bool TryParse(string input, string currencyCode, out decimal result)
        {
            result = 0m;
            if (string.IsNullOrWhiteSpace(input)) return false;

            string symbol = GetSymbol(currencyCode);
            // Remove symbol variants and common prefixes/spaces
            // We cannot use string.Replace with StringComparison in older frameworks without overload; manual cleanup
            string clean = input;
            foreach (var legacy in new [] { symbol, "Rs.", "Rs", "$" })
            {
                clean = clean.Replace(legacy, "");
            }
            clean = clean.Trim();

            return decimal.TryParse(clean, NumberStyles.Number, CultureInfo.InvariantCulture, out result);
        }

        /// <summary>
        /// Map a currency code to display symbol/prefix.
        /// </summary>
        public static string GetSymbol(string currencyCode)
        {
            if (string.IsNullOrWhiteSpace(currencyCode)) return "Rs."; // default
            switch (currencyCode.ToUpperInvariant())
            {
                case "USD": return "$"; // US Dollar
                case "LKR": return "Rs."; // Sri Lankan Rupees
                default: return currencyCode.ToUpperInvariant(); // Fallback: show code
            }
        }
    }
}
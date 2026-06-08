using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using System.Text.Json;
using MySqlConnector;
using HaranInvoiceSoftware.Models;

namespace HaranInvoiceSoftware.Services
{
    public class InvoiceDataService
    {
        private readonly string _invoicesDirectory;
        private readonly string _lastInvoiceFile;
        private readonly string _mySqlDatabaseName;
        private readonly string _mySqlConnectionString;
        private readonly bool _useMySql;
        private readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        private const string LastInvoiceSettingKey = "LastInvoiceNumber";
        private const string XmlImportCompletedSettingKey = "XmlImportCompleted";

        public bool IsUsingMySql => _useMySql;

        public InvoiceDataService()
        {
            _invoicesDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Invoices");
            _lastInvoiceFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "last_invoice.txt");
            _mySqlDatabaseName = GetEnvironment("DB_NAME", "accounts");

            // Ensure invoices directory exists
            if (!Directory.Exists(_invoicesDirectory))
            {
                Directory.CreateDirectory(_invoicesDirectory);
            }

            _mySqlConnectionString = BuildMySqlConnectionString(_mySqlDatabaseName);

            try
            {
                EnsureDatabaseAndTables();
                _useMySql = true;
                ImportExistingXmlInvoicesOnce();
                MigrateLegacyLastInvoicePointer();
            }
            catch
            {
                _useMySql = false;
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

                EnsureInvoiceDefaults(invoice);

                if (_useMySql)
                {
                    SaveInvoiceToDatabase(invoice, true);
                }
                else
                {
                    SaveInvoiceToXml(invoice);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error saving invoice: {ex.Message}");
            }
        }

        public Invoice LoadInvoice(string invoiceRef)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(invoiceRef))
                {
                    return new Invoice();
                }

                if (File.Exists(invoiceRef))
                {
                    return LoadInvoiceFromXmlFile(invoiceRef);
                }

                if (_useMySql)
                {
                    return LoadInvoiceFromDatabase(invoiceRef);
                }

                return new Invoice();
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
                if (_useMySql)
                {
                    var lastInvoiceNumber = GetSetting(LastInvoiceSettingKey);
                    if (!string.IsNullOrWhiteSpace(lastInvoiceNumber))
                    {
                        return LoadInvoiceFromDatabase(lastInvoiceNumber);
                    }

                    var mostRecentInvoice = LoadMostRecentInvoiceFromDatabase();
                    if (mostRecentInvoice != null)
                    {
                        return mostRecentInvoice;
                    }

                    return new Invoice();
                }

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
                if (_useMySql)
                {
                    return GetRecentInvoiceNumbers();
                }

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
                if (_useMySql)
                {
                    if (string.IsNullOrWhiteSpace(filePath))
                    {
                        return;
                    }

                    if (File.Exists(filePath))
                    {
                        var invoice = LoadInvoiceFromXmlFile(filePath);
                        if (!string.IsNullOrWhiteSpace(invoice.InvoiceNumber))
                        {
                            SetSetting(LastInvoiceSettingKey, invoice.InvoiceNumber);
                        }
                        return;
                    }

                    SetSetting(LastInvoiceSettingKey, filePath);
                    return;
                }

                File.WriteAllText(_lastInvoiceFile, filePath);
            }
            catch
            {
                // Ignore errors when saving last invoice path
            }
        }

        public string[] GetRecentInvoiceNumbers(int maxCount = 200)
        {
            if (!_useMySql)
            {
                return new string[0];
            }

            var invoiceNumbers = new List<string>();
            int safeLimit = Math.Clamp(maxCount, 1, 5000);
            using (var connection = new MySqlConnection(_mySqlConnectionString))
            using (var command = connection.CreateCommand())
            {
                command.CommandText = @"
SELECT `InvoiceNumber`
FROM `Invoices`
ORDER BY `InvoiceDate` DESC, `UpdatedAt` DESC
LIMIT " + safeLimit;
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        invoiceNumbers.Add(reader.GetString(0));
                    }
                }
            }

            return invoiceNumbers.ToArray();
        }

        private void SaveInvoiceToXml(Invoice invoice)
        {
            string filePath;

            if (!string.IsNullOrEmpty(invoice.FileName))
            {
                filePath = invoice.FileName;
            }
            else
            {
                string fileName = $"Invoice_{invoice.InvoiceNumber}_{invoice.InvoiceDate:yyyyMMdd}.xml";
                filePath = Path.Combine(_invoicesDirectory, fileName);
                invoice.FileName = filePath;
            }

            XmlSerializer serializer = new XmlSerializer(typeof(Invoice));
            using (FileStream stream = new FileStream(filePath, FileMode.Create))
            {
                serializer.Serialize(stream, invoice);
            }

            File.WriteAllText(_lastInvoiceFile, filePath);
        }

        private Invoice LoadInvoiceFromXmlFile(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return new Invoice();
            }

            XmlSerializer serializer = new XmlSerializer(typeof(Invoice));
            using (FileStream stream = new FileStream(filePath, FileMode.Open))
            {
                var invoice = (Invoice)serializer.Deserialize(stream);
                invoice.FileName = filePath;
                EnsureInvoiceDefaults(invoice);
                return invoice;
            }
        }

        private void SaveInvoiceToDatabase(Invoice invoice, bool saveAsLastInvoice)
        {
            invoice.CalculateTotals();

            using (var connection = new MySqlConnection(_mySqlConnectionString))
            using (var command = connection.CreateCommand())
            {
                command.CommandText = @"
INSERT INTO `Invoices`
(`InvoiceNumber`, `InvoiceDate`, `CustomerName`, `Total`, `CurrencyCode`, `FileName`, `JsonData`, `CreatedAt`, `UpdatedAt`)
VALUES
(@InvoiceNumber, @InvoiceDate, @CustomerName, @Total, @CurrencyCode, @FileName, @JsonData, UTC_TIMESTAMP(), UTC_TIMESTAMP())
ON DUPLICATE KEY UPDATE
    `InvoiceDate` = VALUES(`InvoiceDate`),
    `CustomerName` = VALUES(`CustomerName`),
    `Total` = VALUES(`Total`),
    `CurrencyCode` = VALUES(`CurrencyCode`),
    `FileName` = VALUES(`FileName`),
    `JsonData` = VALUES(`JsonData`),
    `UpdatedAt` = UTC_TIMESTAMP();";

                command.Parameters.AddWithValue("@InvoiceNumber", invoice.InvoiceNumber);
                command.Parameters.AddWithValue("@InvoiceDate", invoice.InvoiceDate);
                command.Parameters.AddWithValue("@CustomerName", invoice.Customer?.Name ?? string.Empty);
                command.Parameters.AddWithValue("@Total", invoice.Total);
                command.Parameters.AddWithValue("@CurrencyCode", invoice.CurrencyCode ?? "LKR");
                command.Parameters.AddWithValue("@FileName", invoice.FileName ?? string.Empty);
                command.Parameters.AddWithValue("@JsonData", JsonSerializer.Serialize(invoice, _jsonOptions));

                connection.Open();
                command.ExecuteNonQuery();
            }

            if (saveAsLastInvoice)
            {
                SetSetting(LastInvoiceSettingKey, invoice.InvoiceNumber);
            }
        }

        private Invoice LoadInvoiceFromDatabase(string invoiceNumber)
        {
            using (var connection = new MySqlConnection(_mySqlConnectionString))
            using (var command = connection.CreateCommand())
            {
                command.CommandText = @"SELECT `JsonData` FROM `Invoices` WHERE `InvoiceNumber` = @InvoiceNumber";
                command.Parameters.AddWithValue("@InvoiceNumber", invoiceNumber);
                connection.Open();

                var jsonData = command.ExecuteScalar() as string;
                if (string.IsNullOrWhiteSpace(jsonData))
                {
                    return new Invoice();
                }

                var invoice = JsonSerializer.Deserialize<Invoice>(jsonData, _jsonOptions) ?? new Invoice();
                EnsureInvoiceDefaults(invoice);
                SetSetting(LastInvoiceSettingKey, invoice.InvoiceNumber);
                return invoice;
            }
        }

        private Invoice LoadMostRecentInvoiceFromDatabase()
        {
            using (var connection = new MySqlConnection(_mySqlConnectionString))
            using (var command = connection.CreateCommand())
            {
                command.CommandText = @"
SELECT `JsonData`
FROM `Invoices`
ORDER BY `UpdatedAt` DESC, `InvoiceDate` DESC
LIMIT 1";
                connection.Open();
                var jsonData = command.ExecuteScalar() as string;
                if (string.IsNullOrWhiteSpace(jsonData))
                {
                    return null;
                }

                var invoice = JsonSerializer.Deserialize<Invoice>(jsonData, _jsonOptions) ?? new Invoice();
                EnsureInvoiceDefaults(invoice);
                return invoice;
            }
        }

        private void EnsureDatabaseAndTables()
        {
            string host = GetEnvironment("DB_HOST", "localhost");
            uint port = GetPortEnvironment("DB_PORT", 3306);
            string dbUser = GetEnvironment("DB_USER", "root");
            string dbPass = Environment.GetEnvironmentVariable("DB_PASS") ?? string.Empty;

            string escapedDbName = EscapeMySqlIdentifier(_mySqlDatabaseName);
            string adminConnectionString = BuildMySqlConnectionString(host, port, string.Empty, dbUser, dbPass);

            using (var adminConnection = new MySqlConnection(adminConnectionString))
            using (var createDbCommand = adminConnection.CreateCommand())
            {
                createDbCommand.CommandText = $"CREATE DATABASE IF NOT EXISTS `{escapedDbName}` CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;";
                adminConnection.Open();
                createDbCommand.ExecuteNonQuery();
            }

            using (var appConnection = new MySqlConnection(_mySqlConnectionString))
            using (var createTablesCommand = appConnection.CreateCommand())
            {
                createTablesCommand.CommandText = @"
CREATE TABLE IF NOT EXISTS `Invoices`
(
    `InvoiceNumber` VARCHAR(100) NOT NULL,
    `InvoiceDate` DATETIME NOT NULL,
    `CustomerName` VARCHAR(200) NULL,
    `Total` DECIMAL(18, 2) NOT NULL DEFAULT 0,
    `CurrencyCode` VARCHAR(10) NOT NULL DEFAULT 'LKR',
    `FileName` VARCHAR(512) NULL,
    `JsonData` LONGTEXT NOT NULL,
    `CreatedAt` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    `UpdatedAt` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    PRIMARY KEY (`InvoiceNumber`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

CREATE TABLE IF NOT EXISTS `AppSettings`
(
    `SettingKey` VARCHAR(100) NOT NULL,
    `SettingValue` LONGTEXT NULL,
    `UpdatedAt` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    PRIMARY KEY (`SettingKey`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;";

                appConnection.Open();
                createTablesCommand.ExecuteNonQuery();

                using (var checkIndexCommand = appConnection.CreateCommand())
                {
                    checkIndexCommand.CommandText = @"
SELECT COUNT(1)
FROM information_schema.statistics
WHERE table_schema = DATABASE()
  AND table_name = 'Invoices'
  AND index_name = 'IX_Invoices_InvoiceDate';";

                    var indexCount = Convert.ToInt32(checkIndexCommand.ExecuteScalar());
                    if (indexCount == 0)
                    {
                        using (var createIndexCommand = appConnection.CreateCommand())
                        {
                            createIndexCommand.CommandText = @"CREATE INDEX `IX_Invoices_InvoiceDate` ON `Invoices` (`InvoiceDate`, `UpdatedAt`);";
                            createIndexCommand.ExecuteNonQuery();
                        }
                    }
                }
            }
        }

        private void ImportExistingXmlInvoicesOnce()
        {
            string completed = GetSetting(XmlImportCompletedSettingKey);
            if (string.Equals(completed, "true", StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            var xmlFiles = Directory.GetFiles(_invoicesDirectory, "*.xml");
            foreach (var xmlFile in xmlFiles)
            {
                try
                {
                    var invoice = LoadInvoiceFromXmlFile(xmlFile);
                    if (string.IsNullOrWhiteSpace(invoice.InvoiceNumber))
                    {
                        invoice.InvoiceNumber = GenerateInvoiceNumber();
                    }

                    SaveInvoiceToDatabase(invoice, false);
                }
                catch
                {
                    // Continue importing remaining files.
                }
            }

            SetSetting(XmlImportCompletedSettingKey, "true");
        }

        private void MigrateLegacyLastInvoicePointer()
        {
            if (!File.Exists(_lastInvoiceFile))
            {
                return;
            }

            try
            {
                string lastInvoicePath = File.ReadAllText(_lastInvoiceFile);
                if (!File.Exists(lastInvoicePath))
                {
                    return;
                }

                var invoice = LoadInvoiceFromXmlFile(lastInvoicePath);
                if (!string.IsNullOrWhiteSpace(invoice.InvoiceNumber))
                {
                    SetSetting(LastInvoiceSettingKey, invoice.InvoiceNumber);
                }
            }
            catch
            {
                // Ignore migration errors to avoid blocking startup.
            }
        }

        private string GetSetting(string key)
        {
            using (var connection = new MySqlConnection(_mySqlConnectionString))
            using (var command = connection.CreateCommand())
            {
                command.CommandText = @"SELECT `SettingValue` FROM `AppSettings` WHERE `SettingKey` = @SettingKey";
                command.Parameters.AddWithValue("@SettingKey", key);
                connection.Open();
                return command.ExecuteScalar() as string;
            }
        }

        private void SetSetting(string key, string value)
        {
            using (var connection = new MySqlConnection(_mySqlConnectionString))
            using (var command = connection.CreateCommand())
            {
                command.CommandText = @"
INSERT INTO `AppSettings` (`SettingKey`, `SettingValue`, `UpdatedAt`)
VALUES (@SettingKey, @SettingValue, UTC_TIMESTAMP())
ON DUPLICATE KEY UPDATE
    `SettingValue` = VALUES(`SettingValue`),
    `UpdatedAt` = UTC_TIMESTAMP();";

                command.Parameters.AddWithValue("@SettingKey", key);
                command.Parameters.AddWithValue("@SettingValue", (object)value ?? DBNull.Value);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        private string BuildMySqlConnectionString(string databaseName)
        {
            string host = GetEnvironment("DB_HOST", "localhost");
            uint port = GetPortEnvironment("DB_PORT", 3306);
            string dbUser = GetEnvironment("DB_USER", "root");
            string dbPass = Environment.GetEnvironmentVariable("DB_PASS") ?? string.Empty;

            return BuildMySqlConnectionString(host, port, databaseName, dbUser, dbPass);
        }

        private string BuildMySqlConnectionString(string host, uint port, string databaseName, string dbUser, string dbPass)
        {
            var builder = new MySqlConnectionStringBuilder
            {
                Server = host,
                Port = port,
                Database = databaseName,
                UserID = dbUser,
                Password = dbPass,
                ConnectionTimeout = 5,
                CharacterSet = "utf8mb4"
            };

            string sslModeValue = GetEnvironment("DB_SSL_MODE", "None");
            if (Enum.TryParse(sslModeValue, true, out MySqlSslMode sslMode))
            {
                builder.SslMode = sslMode;
            }
            else
            {
                builder.SslMode = MySqlSslMode.None;
            }

            return builder.ConnectionString;
        }

        private void EnsureInvoiceDefaults(Invoice invoice)
        {
            if (invoice.Customer == null)
                invoice.Customer = new Customer();
            if (invoice.Items == null)
                invoice.Items = new List<InvoiceItem>();
            if (invoice.FoodItems == null)
                invoice.FoodItems = new List<FoodItem>();
            if (invoice.Company == null)
                invoice.Company = new Company();
            if (string.IsNullOrWhiteSpace(invoice.CurrencyCode))
                invoice.CurrencyCode = "LKR";
        }

        private string EscapeMySqlIdentifier(string value)
        {
            return (value ?? string.Empty).Replace("`", "``");
        }

        private string GetEnvironment(string key, string defaultValue)
        {
            var value = Environment.GetEnvironmentVariable(key);
            if (string.IsNullOrWhiteSpace(value))
            {
                return defaultValue;
            }

            string trimmed = value.Trim();
            if (trimmed.Length >= 2)
            {
                bool wrappedWithDoubleQuotes = trimmed.StartsWith("\"", StringComparison.Ordinal) && trimmed.EndsWith("\"", StringComparison.Ordinal);
                bool wrappedWithSingleQuotes = trimmed.StartsWith("'", StringComparison.Ordinal) && trimmed.EndsWith("'", StringComparison.Ordinal);
                if (wrappedWithDoubleQuotes || wrappedWithSingleQuotes)
                {
                    trimmed = trimmed.Substring(1, trimmed.Length - 2).Trim();
                }
            }

            return string.IsNullOrWhiteSpace(trimmed) ? defaultValue : trimmed;
        }

        private uint GetPortEnvironment(string key, uint defaultValue)
        {
            string value = GetEnvironment(key, defaultValue.ToString());
            if (uint.TryParse(value, out var port) && port > 0)
            {
                return port;
            }

            return defaultValue;
        }
    }
}

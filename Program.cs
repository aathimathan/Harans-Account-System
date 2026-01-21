using System;
using System.IO;
using System.Text;
using System.Windows.Forms;
using HaranInvoiceSoftware.Forms;

namespace HaranInvoiceSoftware
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // WinForms exceptions thrown on the UI thread typically bypass outer try/catch.
            // Capture them and persist a full stack trace for troubleshooting.
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            Application.ThreadException += (sender, args) =>
            {
                var logPath = WriteCrashLog(args.Exception, "Application.ThreadException");
                MessageBox.Show(
                    $"A runtime error occurred.\n\n{args.Exception.Message}\n\nDetails saved to:\n{logPath}",
                    "Runtime Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            };

            AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
            {
                var ex = args.ExceptionObject as Exception;
                var logPath = WriteCrashLog(ex, "AppDomain.UnhandledException");
                var message = ex?.Message ?? "Unknown fatal error";
                MessageBox.Show(
                    $"A fatal runtime error occurred.\n\n{message}\n\nDetails saved to:\n{logPath}",
                    "Fatal Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            };

            try
            {
                Application.Run(new MainInvoiceForm());
            }
            catch (Exception ex)
            {
                var logPath = WriteCrashLog(ex, "Main/Run");
                MessageBox.Show(
                    $"An error occurred during startup.\n\n{ex.Message}\n\nDetails saved to:\n{logPath}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private static string WriteCrashLog(Exception ex, string source)
        {
            try
            {
                string baseDir = AppDomain.CurrentDomain.BaseDirectory;
                string logDir = Path.Combine(baseDir, "Logs");
                Directory.CreateDirectory(logDir);

                string fileName = $"crash_{DateTime.Now:yyyyMMdd_HHmmss_fff}.txt";
                string path = Path.Combine(logDir, fileName);

                var sb = new StringBuilder();
                sb.AppendLine($"Timestamp: {DateTime.Now:O}");
                sb.AppendLine($"Source: {source}");
                sb.AppendLine($"OS: {Environment.OSVersion}");
                sb.AppendLine($"AppBase: {baseDir}");
                sb.AppendLine();
                sb.AppendLine(ex?.ToString() ?? "<null exception>");

                File.WriteAllText(path, sb.ToString());
                return path;
            }
            catch
            {
                return "<failed to write crash log>";
            }
        }
    }
}
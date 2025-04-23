using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaServer.Utils
{
    public static class ConsoleLogger
    {
        private static string logFilePath = @"C:\Users\mariu\Desktop\RSI\Projekt1\CinemaServer\Logs\log.txt"; // Ścieżka do pliku logu (zmień na odpowiednią)

        // Metoda logująca, która wyświetla wiadomość w konsoli oraz zapisuje ją do pliku
        public static void Log(string message, LogLevel level = LogLevel.Info)
        {
            string logMessage = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [{level}] - {message}";

            // Wyświetlenie komunikatu w konsoli
            Console.WriteLine(logMessage);

            // Zapisanie logu do pliku
            try
            {
                File.AppendAllText(logFilePath, logMessage + Environment.NewLine);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd zapisu do pliku logu: {ex.Message}");
            }
        }

        // Enum do określenia poziomu logowania (Info, Warning, Error)
        public enum LogLevel
        {
            Info,
            Warning,
            Error
        }
    }
}

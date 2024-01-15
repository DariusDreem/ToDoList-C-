using System;
using System.IO;
using System.IO.Compression;
using System.Text.RegularExpressions;

namespace Framework.Controller
{
    public class LogWriter
    {
        private const string LogFileNameFormat = "yyyy-MM-dd HH";
        private const string LogFileExtension = "txt";
        private bool IsFileCreated = false;
        private string zipFileName = "";
        private DateTime lastDate = DateTime.MinValue;


        // Instance unique de la classe
        private static Lazy<LogWriter> instance = new Lazy<LogWriter>(() => new LogWriter());

        // Propriété pour accéder à l'instance unique
        public static LogWriter Instance => instance.Value;

        private LogWriter()
        {
          
        }

        /// <summary>
        /// This method writes in a log file the action of the user.
        /// </summary>
        /// <param name="message"></param>
        public void LogAction(string message)
        {
            // Modification : Vérifier si le fichier a été créé au début de chaque action
            if (!IsFileCreated)
            {
                CreateLogFile();
                IsFileCreated = true;
            }

            string logMessage = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}";

            string logFilePath = GetLogFilePath(DateTime.Now.ToString(LogFileNameFormat));
            File.AppendAllText(logFilePath, logMessage + Environment.NewLine);
        }

        /// <summary>
        /// Create a log file for the day if it doesn't exist.
        /// </summary>
        private void CreateLogFile()
        {
            // Modification : Ne créer le fichier que si ce n'est pas déjà fait
            if (IsFileCreated) return;

            string today = DateTime.Now.ToString(LogFileNameFormat);
            string logFilePath = GetLogFilePath(today);

            if (!File.Exists(logFilePath))
            {
                File.Create(logFilePath).Close();
            }
            zipFileName = Path.Combine(Directory.GetCurrentDirectory(), $"{today}_log.zip");
        }

        /// <summary>
        /// Zip the log file for the day and delete the log file.
        /// </summary>
        /// <param name="date"></param>
        public static void ZipLogForDay(string targetDate)
        {
            string zipFileName = Path.Combine(Directory.GetCurrentDirectory(), $"{targetDate}_log.zip");
            string[] filesToZip = Directory.GetFiles(".", $"*{targetDate}*.txt")
                                            .ToArray();

            if (filesToZip.Length == 0)
            {
                throw new FileNotFoundException($"Aucun fichier .txt contenant la date {targetDate} dans le nom n'a été trouvé.");
            }

            if (File.Exists(zipFileName))
            {
                using (ZipArchive archive = ZipFile.Open(zipFileName, ZipArchiveMode.Update))
                {
                    foreach (string file in filesToZip)
                    {
                        string entryName = Path.GetFileName(file);
                        archive.CreateEntryFromFile(file, entryName);
                    }
                }
            }
            else
            {
                using (ZipArchive archive = ZipFile.Open(zipFileName, ZipArchiveMode.Create))
                {
                    foreach (string file in filesToZip)
                    {
                        string entryName = Path.GetFileName(file);
                        archive.CreateEntryFromFile(file, entryName);
                    }
                }
            }

            // Supprimez les fichiers .txt après les avoir ajoutés ou mis à jour dans le zip
            foreach (string fileToDelete in filesToZip)
            {
                File.Delete(fileToDelete);
            }
        }

    /// <summary>
    /// Get the path of the log file.
    /// </summary>
    /// <param name="date"></param> Is the date of the log file.
    /// <returns></returns>
    private static string GetLogFilePath(string date)
        {
            return Path.Combine(Directory.GetCurrentDirectory(), $"{date}_log.{LogFileExtension}");
        }

        /// <summary>
        /// Test the LogAction method.
        /// </summary>
        /// <returns></returns>
        public static bool Loggertest()
        {
            DateTime customDate = new DateTime(2002, 2, 2);
            string formattedDate = customDate.ToString("dd-MM-yyyy");

           // CreateLogFile(formattedDate);

            string logFilePath = GetLogFilePath(formattedDate);
            return File.Exists(logFilePath);
        }

        /// <summary>
        /// tests the ZipLogForDay method.
        /// </summary>
        /// <returns></returns>
        public static bool ZipLogTest()
        {
            DateTime customDate = new DateTime(2002, 2, 2);
            string formattedDate = customDate.ToString("dd-MM-yyyy");

            string logFilePath = GetLogFilePath(formattedDate);

            if (File.Exists(logFilePath))
            {
                ZipLogForDay(formattedDate);

                string zipFilePath = Path.Combine(Directory.GetCurrentDirectory(), $"{formattedDate}_log.zip");
                if (File.Exists(zipFilePath))
                {
                    File.Delete(zipFilePath);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
    }
}

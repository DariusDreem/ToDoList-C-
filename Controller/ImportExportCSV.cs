using Framework;
using Framework.Controller;
using Framework.Model;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace Framework.Controller
{
    public class CsvImportExporter
    {
        /// <summary>
        /// Export the tasks from the database to a csv file
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="filePath"></param>
        public static void ExportToCsv<T>(IEnumerable<T> data, string filePath)
        {
            var csvContent = new StringBuilder();

            var propertiesToInclude = typeof(T).GetProperties().SkipLast(2);

            var headerLine = string.Join(',', propertiesToInclude.Select(p => p.Name));
            csvContent.AppendLine(headerLine);

            foreach (var item in data)
            {
                var dataLine = string.Join(',', propertiesToInclude.Select(p => p.GetValue(item)?.ToString() ?? ""));
                csvContent.AppendLine(dataLine);
            }

            File.WriteAllText(filePath, csvContent.ToString());
        }

        /// <summary>
        /// Add the tasks from the csv file to the database
        /// </summary>
        /// <param name="filePath"></param>
        public static void ImportFromCsv(string filePath)
        {
            string name = "", description = "", priority = "", dueDate = "", ownerID = "";

            var lines = File.ReadAllLines(filePath);

            var header = lines.First();
            var dataLines = lines.Skip(1);

            var columnNames = header.Split(',');

            foreach (var line in dataLines)
            {
                UserMenuModel.Instance.UserCommand = "";
                var values = line.Split(',');

                for (int i = 0; i < values.Length; i++)
                {
                    values[i] = values[i].Trim();
                    switch (columnNames[i].Trim())
                    {
                        case "Priority":
                            priority = values[i];
                            break;
                        case "DueDate":
                            dueDate = values[i].Split(' ').First();
                            break;
                        case "Name":
                            name = values[i];
                            break;
                        case "Description":
                            description = values[i];
                            break;
                        case "OwnerID":
                            ownerID = values[i];
                            break;
                        default:
                            continue;
                    }
                }

                UserMenuModel.Instance.UserCommand += $" {name} {priority} {dueDate} {ownerID} {description}";
                UserMenuModel.Instance.Controller.CommandToCommands();
                UserMenuModel.Instance.Controller.Add();
            }
        }

        /// <summary>
        /// Overwrite the database with the csv file
        /// </summary>
        /// <param name="filePath"></param>
        public static void OverwriteFromCsv(string filePath)
        {
            using (var dbContext = EFContextModel.Instance)
            {
                var filteredTasks = dbContext.ToDoTasks.ToList();
                foreach (var task in filteredTasks)
                {
                    dbContext.Remove(task);
                }
                dbContext.SaveChanges();
            }

            CsvImportExporter.ImportFromCsv(filePath);
        }
    }
}
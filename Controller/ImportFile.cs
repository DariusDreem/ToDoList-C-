using Framework.Model;
using System;
using System.IO;

namespace Framework.Controller
{
    public class ImportFile
    {
        /// <summary>
        /// Execute the commands from the file
        /// </summary>
        public static void ExecuteCommandsFromFile()
        {
            UserMenuModel menu = UserMenuModel.Instance;
            string filePath = Path.Combine(Environment.CurrentDirectory, "AutoRead.txt");

            try
            {
                using (StreamReader file = new StreamReader(filePath))
                {
                    string line;
                    while ((line = file.ReadLine()) != null)
                    {
                        menu.UserCommand = line;
                        Console.WriteLine(line);
                        menu.Controller.StartMenu();
                    }
                }
            }
            catch (IOException ex)
            {
                Console.WriteLine($"Error reading file: {ex.Message}");
            }
            finally
            {
                menu.AutoRead = false;
            }
        }
    }
}

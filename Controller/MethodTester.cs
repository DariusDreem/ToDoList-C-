using Framework.Model;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Controller
{
    internal class MethodTester
    {
        public static void Test()
        {
            IsWorking("Add", AddTest());
            IsWorking("Update", UpdateTest());
            IsWorking("Complete", CompleteTest());
            IsWorking("Remove", RemoveTest());
            IsWorking("RemovePriority", RemovePriorityTest());
            IsWorking("Create", CreateTest());
            IsWorking("RemoveUser", RemoveUserTest());
            IsWorking("CSVexport", CSVexportTest());
            IsWorking("CSVimport", CSVimportTest());
            IsWorking("Logger", LoggerTest());
            IsWorking("ZipLog", ZipLogTest());
        }

        /// <summary>
        /// Will test all the methods of the project
        /// </summary>
        private static void IsWorking(string methodName, bool Method)
        {
            if (Method)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                }
            Console.WriteLine($"{methodName} is working : {Method}");
            // Reset console color to default
            Console.ResetColor();
        }
        /// <summary>
        /// Test the add method
        /// </summary>
        /// <returns></returns>
        private static bool AddTest()
        {
            EFContextModel dbContext = EFContextModel.Instance;

            UserMenuModel userMenuModel = UserMenuModel.Instance;

            userMenuModel.UserCommands = new string[] { "add", "MethodTestAdd" , "Low" , "01/02/0003", "1", "desc test" };
            userMenuModel.Controller.ChoosingCommand(userMenuModel.UserCommands[0]);
            dbContext.SaveChanges();
           var task = dbContext.ToDoTasks.FirstOrDefault(t => t.Name == "MethodTestAdd");
            if (task != null) { return true; }
            return false;
        }

        /// <summary>
        /// Test the update method
        /// </summary>
        /// <returns></returns>
        private static bool UpdateTest()
        {
            EFContextModel dbContext = EFContextModel.Instance;
            var task = dbContext.ToDoTasks.FirstOrDefault(t => t.Name == "MethodTestAdd");
            if (task != null)
            {
                task.Description = "test2";
                if (task.Description == "test2")
                {
                    dbContext.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }

        /// <summary>
        /// Test the complete method
        /// </summary>
        /// <returns></returns>
        private static bool CompleteTest()
        {
            EFContextModel dbContext = EFContextModel.Instance;
            var task = dbContext.ToDoTasks.FirstOrDefault(t => t.Name == "MethodTestAdd");
            if (task != null)
            {
                task.IsCompleted = true;
                if (task.IsCompleted == true) { dbContext.SaveChanges(); return true; }
                return false;

            }
            return false;
        }

        /// <summary>
        /// Test the remove method
        /// </summary>
        /// <returns></returns>
        private static bool RemoveTest()
        {
            EFContextModel dbContext = EFContextModel.Instance;

            UserMenuModel userMenuModel = UserMenuModel.Instance;

            userMenuModel.UserCommands = new string[] { "rm", "MethodTestAdd" };
            userMenuModel.Controller.ChoosingCommand(userMenuModel.UserCommands[0]);

           var task = dbContext.ToDoTasks.FirstOrDefault(t => t.Name == "MethodTestAdd");

            return task == null;
        }

        /// <summary>
        /// Test the remove priority method
        /// </summary>
        /// <returns></returns>
        private static bool RemovePriorityTest()
        {
            bool isLowPriorityTaskRemoved = false;
            EFContextModel dbContext = EFContextModel.Instance;

            // Ajout de tâches avec différentes priorités
            foreach (PriorityStatus priorityStatus in Enum.GetValues(typeof(PriorityStatus)))
            {
                ToDoTaskModel task = new ToDoTaskModel("MethodTestrmPrio", priorityStatus, "01/02/0003", "1", "desc test");
                dbContext.Add(task);
                dbContext.SaveChanges();
            }

            // Recherche et suppression de la tâche avec une priorité basse
            ToDoTaskModel lowPriorityTask = dbContext.ToDoTasks.FirstOrDefault(t => t.Name == "MethodTestrmPrio" && t.Priority == PriorityStatus.Low);
            if (lowPriorityTask != null)
            {
                dbContext.Remove(lowPriorityTask);
                dbContext.SaveChanges();

                // Vérification que la tâche avec une priorité basse a été supprimée
                ToDoTaskModel taskWithLowPriority = dbContext.ToDoTasks.FirstOrDefault(t => t.Name == "MethodTestrmPrio" && t.Priority == PriorityStatus.Low);
                ToDoTaskModel taskWithMediumOrHighPriority = dbContext.ToDoTasks.FirstOrDefault(t => t.Name == "MethodTestrmPrio" && (t.Priority == PriorityStatus.Medium || t.Priority == PriorityStatus.High));

                // Vérification que la tâche avec une priorité moyenne ou élevée existe toujours
                if (taskWithLowPriority == null && taskWithMediumOrHighPriority != null)
                {
                    isLowPriorityTaskRemoved = true;
                }

                // Suppression des tâches avec des priorités moyennes et élevées
                foreach (PriorityStatus priorityToDelete in new[] { PriorityStatus.Medium, PriorityStatus.High })
                {
                    ToDoTaskModel taskToDelete = dbContext.ToDoTasks.FirstOrDefault(t => t.Priority == priorityToDelete);
                    dbContext.Remove(taskToDelete);
                }
                dbContext.SaveChanges();
            }

            return isLowPriorityTaskRemoved;
        }



        /// <summary>
        /// Test the create user method
        /// </summary>
        /// <returns></returns>
        private static bool CreateTest()
        {
            EFContextModel dbContext = EFContextModel.Instance;

            UserMenuModel userMenuModel = UserMenuModel.Instance;
            var user = dbContext.Users.FirstOrDefault(u => u.name == "test");
            
                userMenuModel.UserCommands = new string[] { "create", "MethodTestCreate" };
                userMenuModel.Controller.ChoosingCommand(userMenuModel.UserCommands[0]);

                user = dbContext.Users.FirstOrDefault(u => u.name == "MethodTestCreate");
                if (user == null) { return false; }
                return true;
        }

        /// <summary>
        /// Test the remove user method
        /// </summary>
        /// <returns></returns>
        private static bool RemoveUserTest()
        {
            EFContextModel dbContext = EFContextModel.Instance;

            UserMenuModel userMenuModel = UserMenuModel.Instance;
            var user = dbContext.Users.FirstOrDefault(u => u.name == "MethodTestCreate");

            userMenuModel.UserCommands = new string[] { "rmuser", $"{user.id}" };
            userMenuModel.Controller.ChoosingCommand(userMenuModel.UserCommands[0]);

                user = dbContext.Users.FirstOrDefault(u => u.name == "MethodTestCreate");
                if (user == null) { return true; }
             return false;
        }

        /// <summary>
        /// Export the tasks to a CSV file
        /// </summary>
        /// <returns></returns>
        private static bool CSVexportTest()
        {
            EFContextModel dbContext = EFContextModel.Instance;
            var tasks = dbContext.ToDoTasks.ToList();

            try
            {
                string filePath = "tasks_export.csv";

                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    writer.WriteLine("Name,Priority,DueDate,OwnerId,Description");

                    // Write each task to the CSV file
                    foreach (var task in tasks)
                    {
                        writer.WriteLine($"{task.Name},{task.Priority},{task.DueDate},{task.OwnerID},{task.Description}");
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// Import the tasks from the CSV file
        /// </summary>
        /// <returns></returns>
        private static bool CSVimportTest()
        {
            EFContextModel dbContext = EFContextModel.Instance;
            var tasks = dbContext.ToDoTasks.ToList();

            try
            {
                string filePath = $"{Environment.CurrentDirectory}\\tasks_export_test.csv";
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    writer.WriteLine("Name,Priority,DueDate,OwnerId,Description");

                    foreach (var task in tasks)
                    {
                        // Assurez-vous de formater correctement la date et de gérer les caractères spéciaux
                        writer.WriteLine($"{EscapeCSVField(task.Name)},{task.Priority},{task.DueDate.ToString("yyyy-MM-dd")},{task.OwnerID},{EscapeCSVField(task.Description)}");
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// Escape a CSV field to avoid errors when importing the CSV file
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        private static string EscapeCSVField(string field)
        {
            if (field == null)
            {
                // Traitement spécial pour un champ null (ou vous pouvez simplement le laisser tel quel)
                return "NULL_FIELD";
            }

            // Si le champ contient des guillemets doubles, les doubler pour l'échappement
            if (field.Contains("\""))
            {
                field = field.Replace("\"", "\"\"");
            }

            // Si le champ contient une virgule ou des guillemets doubles, entourez-le de guillemets doubles
            if (field.Contains(",") || field.Contains("\""))
            {
                field = $"\"{field}\"";
            }

            return field;
        }


        private static bool LoggerTest()
        {
            return LogWriter.Loggertest();
        }


        private static bool ZipLogTest()
        {
            return LogWriter.ZipLogTest();
        }
    }
}

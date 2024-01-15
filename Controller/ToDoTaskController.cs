using Framework.Model;
using System;
using System.Globalization;

namespace Framework.Controller
{

    public class ToDoTaskController
    {
        public ToDoTaskModel Model;
        public ToDoTaskController (ToDoTaskModel model)
        {
            Model = model;
        }
        /// <summary>
        /// Parse the date to a DateTime object
        /// </summary>
        /// <param name="date"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static DateTime ParseDueDate(string date,string format)
        {
            if (DateTime.TryParseExact(date, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dateTime))
            {
                return dateTime;
            }
            else
            {
                throw new ArgumentException($"Format de date invalide. Utilisez le format {format}.");
            }
        }
        
        /// <summary>
        /// Add a task to the database
        /// </summary>
        public void AddToDoTask()
        {
            EFContextModel dbContext = EFContextModel.Instance;
            dbContext.Add(Model);
                dbContext.SaveChanges();
        }
        /// <summary>
        /// Remove a task from the database
        /// </summary>
        /// <param name="name"></param>
        public static void RemoveTask(string name)
        {
            EFContextModel dbContext = EFContextModel.Instance;
            var task = dbContext.ToDoTasks.FirstOrDefault(t => t.Name == name);
                if (task != null)
                {
                    dbContext.Remove(task);
                    dbContext.SaveChanges();
                }
        }
        /// <summary>
        /// Update the description of a task
        /// </summary>
        /// <param name="name"></param>
        /// <param name="description"></param>
        public static void UpdateTaskDescription(string name, string description)
        {
            EFContextModel dbContext = EFContextModel.Instance;
            var task = dbContext.ToDoTasks.FirstOrDefault(t => t.Name == name);
                if (task != null)
                {
                    task.Description = description;
                    dbContext.SaveChanges();
            }
        }
        /// <summary>
        /// Remove all tasks with a specific priority
        /// </summary>
        /// <param name="priority"></param>
        public static void RemoveTasksByPriority(PriorityStatus priority)
        {
            EFContextModel dbContext = EFContextModel.Instance;
                var tasks = dbContext.ToDoTasks.Where(t => t.Priority == priority).ToList();
                foreach (var task in tasks)
                {
                    dbContext.Remove(task);
                }
                dbContext.SaveChanges();
        }
        /// <summary>
        /// Display all tasks
        /// </summary>
        public static void DisplayToDoList()
        {
            EFContextModel dbContext = EFContextModel.Instance;
            var toDoList = dbContext.ToDoTasks.ToList();
                foreach (var task in toDoList)   
                {
                    task.TaskView.ViewTask(task);
                }
        }
        /// <summary>
        /// Set a task as completed
        /// </summary>
        /// <param name="name"></param>
        public static void MarkTaskAsCompleted(string name)
        {
            EFContextModel dbContext = EFContextModel.Instance;
            var task = dbContext.ToDoTasks.FirstOrDefault(t => t.Name == name);
                if (task != null)
                {
                    task.IsCompleted = true;
                    dbContext.SaveChanges();
                }
            }
        
        /// <summary>
        /// Check if it should be completed or not
        /// </summary>
        /// <param name="completion"></param>
        public static void FilterTaskByCompletionChoice(string completion)
        {
            bool IsCompleted;
            EFContextModel dbContext = EFContextModel.Instance;
            //dbContext.EFCView.FilterChoice();
                if (completion == "1")
                {
                    IsCompleted = true;
                }
                else
                {
                    IsCompleted = false;
                }
            FilterTasksByCompletionStatus(IsCompleted);
        }
        /// <summary>
        /// Filter the tasks by completion status
        /// </summary>
        /// <param name="isCompleted"></param>
        public static void FilterTasksByCompletionStatus(bool isCompleted)
        {
            EFContextModel dbContext = EFContextModel.Instance;
                var filteredTasks = dbContext.ToDoTasks.Where(t => t.IsCompleted == isCompleted).ToList();
                foreach (var task in filteredTasks)
                {
                    task.TaskView.ViewTask(task);
                }
        }
        /// <summary>
        /// Filter the tasks by priority
        /// </summary>
        /// <param name="priority"></param>
        public static void FilterTasksByPriority(PriorityStatus priority)
        {
            EFContextModel dbContext = EFContextModel.Instance;
                var filteredTasks = dbContext.ToDoTasks.Where(t => t.Priority == priority).ToList();
                foreach (var task in filteredTasks)
                {
                    task.TaskView.ViewTask(task);
                }
        }
        /// <summary>
        /// Filter the tasks by due date
        /// </summary>
        public static void SortTasksByDueDate()
        {
            EFContextModel dbContext = EFContextModel.Instance;
                var sortedTasks = dbContext.ToDoTasks.OrderBy(t => t.DueDate).ToList();
                foreach (var task in sortedTasks)
                {
                    task.TaskView.ViewTask(task);
                }
        }
        /// <summary>
        /// Show a specific task
        /// </summary>
        /// <param name="name"></param>
        public static void ShowTask(string name)
        {
            EFContextModel dbContext = EFContextModel.Instance;
            var task = dbContext.ToDoTasks.FirstOrDefault(t => t.Name == name);
            if (task != null)
            {
                task.TaskView.ViewTask(task);
            }
        }
        /// <summary>
        /// 
        /// Display the stats of the tasks
        /// </summary>
        public static void DisplayToDoStats()
        {
            EFContextModel dbContext = EFContextModel.Instance;
            var tasks = dbContext.ToDoTasks.ToList();
            int totalTasks = tasks.Count;
            int completedTasks = tasks.Count(t => t.IsCompleted);
            int uncompletedTasks = totalTasks - completedTasks;
            int highPriorityTasks = tasks.Count(t => t.Priority == PriorityStatus.High);
            int mediumPriorityTasks = tasks.Count(t => t.Priority == PriorityStatus.Medium);
            int lowPriorityTasks = tasks.Count(t => t.Priority == PriorityStatus.Low);

            dbContext.EFCView.StatsDisplay(completedTasks, uncompletedTasks, highPriorityTasks, mediumPriorityTasks, lowPriorityTasks, totalTasks);
        }

    }
}

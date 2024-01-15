using Framework.Model;
using Framework.Views;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Globalization;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Framework.Controller
{
    public class UserMenuController
    {
        private readonly UserMenuModel _model;
        private const string CsvFileName = "file.csv";

        public UserMenuController(UserMenuModel model)
        {
            _model = model;
        }
        /// <summary>
        /// This method is the main menu of the application
        /// </summary>
        public void StartMenu()
        {
            if (!_model.AutoRead) { AskCommand(); };
            CommandToCommands();
            ChoosingCommand(_model.UserCommands[0]);
            logAction();
            if (!_model.AutoRead) { StartMenu();};
        }

        private void logAction()
        {
            LogWriter.Instance.LogAction($"L'utilisateur a effectué l'action: {_model.UserCommands[0]}.");
            if (_model.UserCommands.Length >= 2)
            {
                string valuesLog = "L'utilisateur a entré les valeurs : " + string.Join(", ", _model.UserCommands.Skip(1));
                LogWriter.Instance.LogAction($"L'utilisateur a effectué l'action: {valuesLog} .");
            }
        }
        /// <summary>
        /// Split the user command into an array of strings
        /// </summary>
        public void CommandToCommands()
        {
            _model.UserCommands = _model.UserCommand.ToLower().Split(' ');
        }
        /// <summary>
        /// Ask the user to enter a command
        /// </summary>
        public void AskCommand()
        {
            _model.UserCommand = Console.ReadLine();
        }
        /// <summary>
        /// Add a new task in the database
        /// </summary>
        public void Add()
        {
            

            ToDoTaskModel todo ;
            if (!CheckUserCommandsLength(5)) { _model.MainMenu.InvalidFormat();StartMenu(); } ;
            CheckStringToPriority(_model.UserCommands[2]);
            CheckDateFormat();
            DescriptionCompleter(_model.UserCommands, 5);
            if (!UserController.CheckInputId(_model.UserCommands[4])) { UserView. ValidUser(); StartMenu();} ;
            todo = new ToDoTaskModel(_model.UserCommands[1], _model.Priority, _model.UserCommands[3], _model.UserCommands[4] , _model.Description);
            todo.TaskController.AddToDoTask();
        }
        /// <summary>
        /// Check if the date format is valid
        /// </summary>
        public void CheckDateFormat()
        {
            if (!DateTime.TryParseExact(_model.UserCommands[3], "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
            {
                _model.MainMenu.InvalidDateFormat("dd/MM/yyyy");
                StartMenu();
            }
        }
        /// <summary>
        /// Check if the user commands is the right length
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public bool CheckUserCommandsLength(int length)
        {
            if (_model.UserCommands.Length > length-1) { return true; }
            return false; 
        }
        /// <summary>
        /// Check if the user command is a valid priority
        /// </summary>
        /// <param name="prioString"></param>
        public void CheckStringToPriority(string prioString)
        {
            _model.Priority = StringToPriority(prioString);
            if (_model.Priority == PriorityStatus.Null)
            {
                _model.MainMenu.ValidPrio();
            }
        }
        
        /// <summary>
        /// Put differents words of the user command into the description so the user can enter a description with spaces
        /// </summary>
        /// <param name="userCommands"></param>
        /// <param name="place"></param>
        public void DescriptionCompleter(string[] userCommands, int place)
        {
            if (_model.UserCommands.Length > place)
            {
                _model.Description = "";
                while (place < _model.UserCommands.Length)
                {
                    place++;
                    _model.Description += " " + _model.UserCommands[place - 1];
                }
            }
            else
            {
                
            }
        }
        /// <summary>
        /// Convert a string into a priority
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static PriorityStatus StringToPriority(string str)
        {
            str = str.ToLower();
            switch (str)
            {
                case ("low"):
                    return PriorityStatus.Low;
                case ("medium"):
                    return PriorityStatus.Medium;
                case ("high"):
                    return PriorityStatus.High;
                default:
                    return PriorityStatus.Null;
            }
        }
        /// <summary>
        /// Show the list of commands
        /// </summary>
        public void Help()
        {    
            _model.MainMenu.Help();
        }
        /// <summary>
        /// Update a task description
        /// </summary>
        public void Update()
        {
            if (!CheckUserCommandsLength(3)) { _model.MainMenu.InvalidFormat(); StartMenu(); };
            DescriptionCompleter(_model.UserCommands, 2);
            ToDoTaskController.UpdateTaskDescription(_model.UserCommands[1], _model.Description);
        }
        /// <summary>
        /// Show the list of tasks
        /// </summary>
        public static void Show()
        {
            ToDoTaskController.DisplayToDoList();
        }
        /// <summary>
        /// Remove a task by it's name
        /// </summary>
        public void Remove()
        {
            if (!CheckUserCommandsLength(2)) { _model.MainMenu.InvalidFormat(); StartMenu(); };
            ToDoTaskController.RemoveTask($"{_model.UserCommands[1]}");
        }
        /// <summary>
        /// Remove all the tasks with the specified priority
        /// </summary>
        public void RemovePrio()
        {
            if (!CheckUserCommandsLength(2)) { _model.MainMenu.InvalidFormat(); StartMenu(); };
            CheckStringToPriority(_model.UserCommands[1]);
            ToDoTaskController.RemoveTasksByPriority(_model.Priority);
        }
        /// <summary>
        /// Mark a task as completed by it's name
        /// </summary>
        public void Complete()
        {
            if (!CheckUserCommandsLength(2)) { 
                _model.MainMenu.InvalidFormat();
                StartMenu(); 
            }
            ToDoTaskController.MarkTaskAsCompleted(_model.UserCommands[1]);
        }
        /// <summary>
        /// Manage commands to apply the adapted filter 
        /// </summary>
        public void Filter()
        {
            if (!CheckUserCommandsLength(3)){_model.MainMenu.InvalidFormat();StartMenu();}

            string command2Value = (_model.UserCommands.Length > 2) ? _model.UserCommands[2] : "";

            Filters(_model.UserCommands[1], command2Value);
        }

        /// <summary>
        /// Show the stats of the to do list
        /// </summary>
        public void Stats()
        {
            ToDoTaskController.DisplayToDoStats();
        }
        /// <summary>
        /// Show the list of tasks by priority or by date or by completion or by owner or the list of users without tasks
        /// </summary>
        public void Filters(string choice,string? underChoice)
        {
            switch (choice)
            {
                case ("prio"):
                    CheckStringToPriority(underChoice);
                    ToDoTaskController.FilterTasksByPriority(_model.Priority);
                    break;
                case ("date"):
                    ToDoTaskController.SortTasksByDueDate();
                    break;
                case ("complete"):
                    ToDoTaskController.FilterTaskByCompletionChoice(underChoice);
                    break;
                case ("owner"):
                    if (!UserController.CheckInputId(_model.UserCommands[4])) { UserView.ValidUser(); StartMenu(); };
                    UserController.GetUserNameById(int.Parse(_model.UserCommands[4]));
                    break;
                case ("usersw/otasks"):
                    UserController.GetUsersWithoutTasks();
                    break;
                default:
                    _model.MainMenu.ValidFilters();
                    StartMenu();
                    break;
            }
        }
        /// <summary>
        /// Create a new user and add it to the database
        /// </summary>
        public void Create()
        {
            if (!CheckUserCommandsLength(2)) { _model.MainMenu.InvalidFormat(); StartMenu(); };
            UserController.AddUser(_model.UserCommands[1]);
        }
        /// <summary>
        /// Show the list of users
        /// </summary>
        public void ShowUsers()
        {
            UserController.ShowUser();
        }
        /// <summary>
        /// Remove a user by it's id
        /// </summary>
        public void RemoveUser()
        {
            if (!CheckUserCommandsLength(2)) { _model.MainMenu.InvalidFormat(); StartMenu(); };
            UserController.RemoveUser(_model.UserCommands[1]);
        }
        /// <summary>
        /// Show a task by it's name
        /// </summary>
        public void ShowTask()
        {
            if (!CheckUserCommandsLength(2)) { _model.MainMenu.InvalidFormat(); StartMenu(); };
            ToDoTaskController.ShowTask($"{_model.UserCommands[1]}");
        }
        /// <summary>
        /// Activate the auto read
        /// </summary>
        public void AutoRead()
        {
            _model.AutoRead = true;
            ImportFile.ExecuteCommandsFromFile();
        }
        /// <summary>
        /// Export the database to a CSV file
        /// </summary>
        public void ExportToCsv()
        {
            var dataToExport = EFContextModel.Instance.ToDoTasks.ToList();
            CsvImportExporter.ExportToCsv(dataToExport, $"{Environment.CurrentDirectory}/{CsvFileName}");
        }
        /// <summary>
        /// Import the CSV file into the database
        /// </summary>
        private void ImportFromCsv()
        {
            CsvImportExporter.ImportFromCsv($"{Environment.CurrentDirectory}/{CsvFileName}");
            EFContextModel.Instance.SaveChanges();
        }
        /// <summary>
        /// Overwrite the database with the CSV file
        /// </summary>
        private void OverwriteFromCsv()
        {
            CsvImportExporter.OverwriteFromCsv($"{Environment.CurrentDirectory}/{CsvFileName}");
        }
        /// <summary>
        /// Exit the application
        /// </summary>
        private void _Exit()
        {
            Environment.Exit(0);
        }
        /// <summary>
        /// Zip the log file for the day and delete the log file.
        /// </summary>
        private void ZipLog()
        {
            if (!CheckUserCommandsLength(2)) { _model.MainMenu.InvalidFormat(); StartMenu(); };
            if (DateTime.TryParseExact(_model.UserCommands[1], "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
            {
                ToDoTaskController.ParseDueDate(_model.UserCommands[1], "yyyy-MM-dd");
                LogWriter.ZipLogForDay(_model.UserCommands[1]);
            }
            else { _model.MainMenu.InvalidDateFormat("yyyy-MM-dd"); StartMenu(); }
        }

        /// <summary>
        /// Choose the command to execute
        /// </summary>
        /// <param name="firstUserCommand"></param>
        public void ChoosingCommand(string firstUserCommand)
        {
            switch (firstUserCommand)
            {
                case "add":Add();break;
                case "help":Help();break;
                case "update":Update();break;
                case "showtasks":Show();break;
                case "showtask":ShowTask();break;
                case "rm":Remove();break;
                case "rmprio":RemovePrio();break;
                case "complete":Complete();break;
                case "filter":Filter();break;
                case "stats":Stats();break;
                case "create":Create();break;
                case "showusers":ShowUsers();break;
                case "rmuser":RemoveUser();break;
                case "autoread":AutoRead();break;
                case "exportdb": ExportToCsv();break;
                case "importdb": ImportFromCsv();break;
                case "overwritedb": OverwriteFromCsv();break;
                case "ziplog":ZipLog();break;
                case "methodtest" :MethodTester.Test();break;
                case "exit":_Exit();break;
                default:
                    _model.MainMenu.Default();
                    _model.UserCommands = new string[] { };
                    StartMenu();
                    break;
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework.Controller;
using Framework.Model;
using Framework.Views;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Client;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Framework
{
    public class UserMenu
    {
        public UserMenu()
        {

        }
        public void MainMenu()
        {

        }

        /// <summary>
        /// This inform the user that the command help can be used to see the list of commands
        /// </summary>
        public void Start()
        {
            Console.WriteLine("Use \"help\" to see commands.");
        }

        /// <summary>
        /// This method inform the user that the command is not recognized
        /// </summary>
        public void Default()
        {
            Console.WriteLine("Command not recognized.");
        }
        public void InvalidDateFormat(string format)
        {
            Console.WriteLine($"Invalid date format. Valid format : {format} ");
        }
        /// <summary>
        /// This method inform the user that the user intered an invalid command format
        /// </summary>
        public void InvalidFormat()
        {
            Console.WriteLine("Invalid command format.");
        }
        /// <summary>
        /// This method inform the user of the commands available
        /// </summary>
        public void Help()
        {

            Console.WriteLine(
    @"
-------------- USER MANAGEMENT ---------------

-stats                  : Show the to-do list statistics.
-Create <UserName>      : Create a new user.
-showUsers              : Display existing users.
-rmUser                  : Remove a user by their ID.

-------------- TASK MANAGEMENT ---------------

-add <name> <Priority> <date> <OwnerId> <description> : Add a to-do task.
    Priorities: (Low, Medium, High)
    Date format: (day/month/year)
-update <name> <description> : Update a task's description by its name.
-showTasks              : Show the entire to-do list.
-showTask <name>        : Show a task by its name.
-rm <name>              : Remove a task by its name.
-rmPrio <Priority>      : Remove all tasks with the specified priority.
    Priorities: (Low, Medium, High)
-complete <name>        : Mark a task as complete by its name.
-filter <filter> <value> : Show tasks based on filters.
    Available filters: (prio, date, complete, owner, usersw/otasks)
    Example: -filter prio High

-------------- PROJECT MANAGEMENT ---------------

-autoRead              : Execute commands from a file.
-exportdb              : Export the database to a file.
-importdb              : Import the database from a file.
-overwritedb           : Delete the database and import from a file.
-ziplog <yyyy-MM-dd>   : Zip the log file for the specified date (day-month-year).
-methodtest            : Test each method.
-exit                  : Quit the application.
");

        }
        /// <summary>
        /// This method ask the user to enter a valid priority name
        /// </summary>
        public void ValidPrio()
        {
            Console.WriteLine("invalid priority");
        }
        /// <summary>
        /// This method ask the user to enter a filter between the different options
        /// </summary>
        /*public void Filter()
        { 
            Console.WriteLine("What do you want to filter ?");
            Console.WriteLine("By priority ?(prio)");
            Console.WriteLine("By Date ?(date)");
            Console.WriteLine("By completion ?(complete)");
            Console.WriteLine("By Owner's name ?(owner)");
            Console.WriteLine("By User without tasks ?(UsersW/oTasks)");
        }*/

        /// <summary>
        /// This method ask the user to enter a priority between the different options(low/medium/high)
        /// </summary>
        public void AskFilterPrio()
        {
            Console.WriteLine("Enter a priority. (low/medium/high)");
        }
        /// <summary>
        /// This method ask the user to enter a valid filter
        /// </summary>
        public void ValidFilters()
        {
            Console.WriteLine("Please enter a valid filter");
        }
        /// <summary>
        /// This method inform the user that the file is not found
        /// </summary>
        /// <param name="date"></param>
        public static void NoExistingFile(string date)
        {
            Console.WriteLine($"Aucun fichier log.txt existant pour la date {date}");
        }


    }
}

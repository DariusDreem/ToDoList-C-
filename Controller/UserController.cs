using Framework.Model;
using Framework.Views;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Controller
{
    internal class UserController
    {
        /// <summary>
        /// Add a user to the database
        /// </summary>
        /// <param name="nameUser"></param>
        public static void AddUser(string nameUser)
        {
            EFContextModel dbContext = EFContextModel.Instance;
            dbContext.Users.Add(new UserModel(nameUser));
                dbContext.SaveChanges();
        }

        /// <summary>
        /// Check if the user input is a number and if it is a valid user
        /// </summary>
        /// <returns></returns>
        public static bool CheckInputId(string number)
        {
            EFContextModel dbContext = EFContextModel.Instance;
            while (true)
                {
                    if (!int.TryParse(number, out int userID))
                    {
                    Console.WriteLine(number);
                        return false;
                    }
                    else
                    {
                        if (!CheckExistingUser(userID))
                        {
                        return false;
                        }
                        else
                        {
                            return true;
                        }
                    }     
            }
        }
         /// <summary>
         /// Check if the user exist in the database
         /// </summary>
         /// <param name="UserId"></param>
         /// <returns></returns>
        public static bool CheckExistingUser(int UserId)
        {
            EFContextModel dbContext = EFContextModel.Instance;
            if (dbContext.Users.FirstOrDefault(u => u.id == UserId) != null)
                {
                    return true;
                }
                else
                { 
                    return false;
            }
        }
        /// <summary>
        /// Show all the users in the database
        /// </summary>
        public static void ShowUser()
        {
            EFContextModel dbContext = EFContextModel.Instance;
            var users = dbContext.Users.ToList();
                foreach (var user in users)
                {
                    UserView.ViewUser(user);
                }
        }
        /// <summary>
        /// Remove a user from the database
        /// </summary>
        public static void RemoveUser(string userId)
        {
            EFContextModel dbContext = EFContextModel.Instance;
            UserMenuModel menu = UserMenuModel.Instance;
            if (!UserController.CheckInputId(menu.UserCommands[1])) { UserView.ValidUser();  menu.Controller.StartMenu(); };
            var User = dbContext.Users.FirstOrDefault(t => t.id == int.Parse(userId));
                if (User != null)
                {
                    dbContext.Users.Remove(User);
                    dbContext.SaveChanges();
            }
        }
        /// <summary>
        /// Get the name of a user by his id
        /// </summary>
        /// <param name="userId"></param>
        public static void GetUserNameById(int userId)
        {
            EFContextModel dbContext = EFContextModel.Instance;

            var user = dbContext.Users.FirstOrDefault(u => u.id == userId);

            if (user != null)
            {
                UserView.ViewUser(user);
            }
        }

        /// <summary>
        /// Get all users without tasks
        /// </summary>
        public static void GetUsersWithoutTasks()
        {
            EFContextModel dbContext = EFContextModel.Instance;
            var usersWithoutTasks = dbContext.Users
                .Where(u => !dbContext.ToDoTasks.Any(t => t.OwnerID == u.id))
                .ToList();
            foreach (var user in usersWithoutTasks)
            {
                UserView.ViewUser(user);
            }
        }

        

    }
}

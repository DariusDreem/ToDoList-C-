using Framework.Controller;
using Framework.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Views
{
    internal class UserView
    {
        /// <summary>
        /// This method ask the user to enter an ID for the user he's seeking
        /// </summary>
        public static void AskingUser()
        {
            Console.WriteLine("enter the ID of the user you're seeking");
        }
        /// <summary>
        /// This method ask the user to enter a valid number
        /// </summary>
        public static void ValidNumber()
        {
            Console.WriteLine("Enter a valid number");
        }
        /// <summary>
        /// This method ask the user to enter an existing ID
        /// </summary>
        public static void ValidUser()
        {
            Console.WriteLine("Invalid ID");
        }
        /// <summary>
        /// This method display the user informations
        /// </summary>
        /// <param name="user"></param>
        public static void ViewUser(UserModel user)
        {
            Console.WriteLine($"{user.name} , " +
                $"User Id : {user.id}");
        }
    }
}

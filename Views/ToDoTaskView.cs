using Framework.Model;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Views
{
    public class ToDoTaskView
    {
        public void ViewTask(ToDoTaskModel todo)
        {
            Console.WriteLine($"{todo.Name} , " +
                $"{todo.Priority} , " +
                $"Creation Date : {todo.CreationDate} , " +
                $"Due Date : {todo.DueDate} ," +
                $"{todo.Description} , " +
                $"OwnerId : {todo.OwnerID} , " +
                $"Completion : {todo.IsCompleted}");
        }

    }
}

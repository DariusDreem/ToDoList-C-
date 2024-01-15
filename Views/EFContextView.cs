using System;

namespace Framework.Views
{
    public class EFContextView
    {
        /// <summary>
        /// This method ask the user to choose between the different options(completed/uncompleted)
        /// </summary>
        /*public void FilterChoice()
        {
            Console.WriteLine("Do you want to see the completed (1) or the uncompleted tasks (2)?");
        }*/
        /// <summary>
        /// This method ask the user to choose between the different options (low/medium/high)
        /// </summary>
        public void PrioChoice()
        {
            Console.WriteLine("Which priority do you want to see? (low/medium/high)");
        }
        /// <summary>
        /// This method show the results of the differents stats
        /// </summary>
        /// <param name="completed"></param>
        /// <param name="uncompleted"></param>
        /// <param name="high"></param>
        /// <param name="medium"></param>
        /// <param name="low"></param>
        /// <param name="total"></param>
        public void StatsDisplay(float completed, float uncompleted, float high, float medium, float low, float total)
        {
            Console.WriteLine($"Tasks completed at {Math.Floor((completed / total) * 100)}%");
            Console.WriteLine($"Tasks uncompleted at {Math.Floor((uncompleted / total) * 100)}%");
            Console.WriteLine($"Tasks in High priority at {Math.Floor((high / total) * 100)}%");
            Console.WriteLine($"Tasks in Medium priority at {Math.Floor((medium / total) * 100)}%");
            Console.WriteLine($"Tasks in Low priority at {Math.Floor((low / total) * 100)}%");
        }
    }
}

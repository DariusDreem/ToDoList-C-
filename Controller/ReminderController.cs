using System;
using System.Timers;
using Timer = System.Timers.Timer;

namespace Framework.Controller
{
    public class MinuteTimer
    {
        private readonly Timer timer;

        /// <summary>
        /// Creates a timer with a specified interval (in milliseconds).
        /// </summary>
        public MinuteTimer(int intervalInMilliseconds = 60000)
        {
            timer = new Timer(intervalInMilliseconds);
            timer.Elapsed += OnTimedEvent;
        }

        /// <summary>
        /// Starts the timer.
        /// </summary>
        public void Start()
        {
            timer.Start();
        }

        /// <summary>
        /// Stops the timer.
        /// </summary>
        public void Stop()
        {
            timer.Stop();
        }

        /// <summary>
        /// Reminds the user to add a description to their tasks at the end of the timer.
        /// </summary>
        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            Console.WriteLine("Pensez à ajouter une description à vos tâches.");
            Stop();
        }
    }
}

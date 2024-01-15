using Framework.Controller;
using Framework.Views;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Framework.Model
{
    public class ToDoTaskModel
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public PriorityStatus Priority { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime DueDate { get; set; }
        public bool IsCompleted { get; set; }
        public int OwnerID { get; set; }
        [NotMapped]
        public ToDoTaskView TaskView { get; } = new ToDoTaskView();
        [NotMapped]
        public ToDoTaskController TaskController { get; }
        public ToDoTaskModel()
        {
        }

        public ToDoTaskModel(string name, PriorityStatus priority, string dueDate, string ownerID, string? description)
        {
            this.Name = name;
            this.Description = description;
            this.Priority = priority;
            this.DueDate = DateTime.ParseExact(dueDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
            CreationDate = DateTime.Now;
            TaskController = new ToDoTaskController(this);
            this.OwnerID = int.Parse(ownerID);
            if (description == null)
            {
                MinuteTimer reminder = new MinuteTimer();
                reminder.Start();
            }
            
        }
    }
}
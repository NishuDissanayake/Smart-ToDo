using ToDoApp.Client.Models;

namespace ToDoApp.Client.Services
{
    public class ToDoService
    {
        private readonly ILogger<ToDoService> _logger;
        public List<ToDoItem> Tasks { get; set; } = new();

        public void AddTask(ToDoItem task)
        {
            Tasks.Add(task);
            _logger.LogInformation("New task successfully added to the to do list!");
        }

        public void DeleteTask(int taskId)
        {
            Tasks.RemoveAll(t => t.Id == taskId);
            _logger.LogInformation("Task removed from the to do list!");
        }

        public void StateToggle(int taskId)
        {
            var task = Tasks.FirstOrDefault(t => t.Id == taskId);
            if (task != null)
            {
                task.IsDone = !task.IsDone;
            }
            _logger.LogInformation("Completion state of the task updated!");
        }
    }
}

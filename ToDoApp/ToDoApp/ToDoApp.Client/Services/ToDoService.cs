using ToDoApp.Client.Models;

namespace ToDoApp.Client.Services
{
    public class ToDoService
    {
        private readonly ILogger<ToDoService> _logger;

        public ToDoService(ILogger<ToDoService> logger)
        {
            _logger = logger;
        }

        public List<ToDoItem> Tasks { get; set; } = new();

        /// <summary>
        /// Adds a new task to the list.
        /// </summary>
        public void AddTask(ToDoItem task)
        {
            Tasks.Add(task);
            _logger.LogInformation("New task successfully added to the to do list!");
        }

        /// <summary>
        /// Deletes a task by ID.
        /// </summary>
        public void DeleteTask(int taskId)
        {
            Tasks.RemoveAll(t => t.Id == taskId);
            _logger.LogInformation("Task removed from the to do list!");
        }

        /// <summary>
        /// Toggles the completion state of a task.
        /// </summary>
        public void StateToggle(int taskId)
        {
            var task = Tasks.FirstOrDefault(t => t.Id == taskId);
            if (task != null)
            {
                task.IsDone = !task.IsDone;
            }
            _logger.LogInformation("Completion state of the task updated!");
        }

        /// <summary>
        /// Edits an existing task.
        /// </summary>
        public void EditTask(int taskId, string newTitle, string newDescription, DateTime newDueDate)
        {
            var task = Tasks.FirstOrDefault(t => t.Id == taskId);
            if (task != null)
            {
                task.Title = newTitle;
                task.Description = newDescription;
                task.DueDate = newDueDate;
                _logger.LogInformation("Task updated.");
            }
        }

        /// <summary>
        /// Retrieves all tasks.
        /// </summary>
        public List<ToDoItem> GetAllTasks()
        {
            return Tasks.ToList();
        }

        /// <summary>
        /// Gets the count of incomplete tasks.
        /// </summary>
        public int GetIncompleteTaskCount()
        {
            return Tasks.Count(t => !t.IsDone);
        }
    }
}

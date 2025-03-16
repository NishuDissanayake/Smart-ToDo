using Blazored.LocalStorage;
using System.Text.Json;
using ToDoApp.Client.Models;

namespace ToDoApp.Client.Services
{
    public enum TaskFilter
    {
        All = 0,
        Active = 1,
        Completed = 2
    }

    public class ToDoService
    {
        private readonly ILogger<ToDoService> _logger;
        private readonly ILocalStorageService _localStorage;
        private List<ToDoItem> Tasks = new();

        public ToDoService(ILogger<ToDoService> logger, ILocalStorageService localStorage)
        {
            _logger = logger;
            _localStorage = localStorage;
        }

        public List<ToDoItem> Tasks { get; set; } = new();

        /// <summary>
        /// Add a new task to the list.
        /// </summary>
        public void AddTask(ToDoItem task)
        {
            task.Id = Tasks.Any() ? Tasks.Max(t => t.Id) + 1 : 1;
            Tasks.Add(task);
            _logger.LogInformation("New task successfully added to the to do list!");
        }

        /// <summary>
        /// Delete a task by ID.
        /// </summary>
        public void DeleteTask(int taskId)
        {
            Tasks.RemoveAll(t => t.Id == taskId);
            _logger.LogInformation("Task removed from the to do list!");
        }

        /// <summary>
        /// Toggle the completion state of a task.
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
        /// Edit an existing task.
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
        /// Retrieve all tasks.
        /// </summary>
        public List<ToDoItem> GetAllTasks()
        {
            return Tasks.ToList();
        }

        /// <summary>
        /// Retrieve filtered tasks.
        /// </summary>
        public List<ToDoItem> GetFilteredTasks(TaskFilter filter)
        {
            return filter switch
            {
                TaskFilter.Active => Tasks.Where(t => !t.IsDone).ToList(),
                TaskFilter.Completed => Tasks.Where(t => t.IsDone).ToList(),
                _ => Tasks.ToList()
            };
        }

        /// <summary>
        /// Get the count of incomplete tasks.
        /// </summary>
        public int GetIncompleteTaskCount()
        {
            return Tasks.Count(t => !t.IsDone);
        }
    }
}

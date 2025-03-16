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
        private readonly AuthService _authService;
        private List<ToDoItem> Tasks = new();

        public ToDoService(ILogger<ToDoService> logger, ILocalStorageService localStorage, AuthService authService)
        {
            _logger = logger;
            _localStorage = localStorage;
            _authService = authService;
        }

        /// <summary>
        /// Load tasks from LocalStorage for the logged-in user
        /// </summary>
        public async Task LoadTasksAsync()
        {
            if (!_authService.IsAuthenticated) return;

            try
            {
                var storedTasks = await _localStorage.GetItemAsync<string>("tasks");
                if (!string.IsNullOrEmpty(storedTasks))
                {
                    var allTasks = JsonSerializer.Deserialize<List<ToDoItem>>(storedTasks) ?? new List<ToDoItem>();

                    Tasks = allTasks.Where(t => t.UserId.ToString() == _authService.CurrentUser!.UserId).ToList();
                }
                _logger.LogInformation("Tasks loaded from LocalStorage.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error loading tasks from LocalStorage: {ex.Message}");
            }
        }

        /// <summary>
        /// Save tasks to LocalStorage for the logged-in user
        /// </summary>
        private async Task SaveTasksAsync()
        {
            if (!_authService.IsAuthenticated) return;

            var storedTasks = await _localStorage.GetItemAsync<string>("tasks") ?? "[]";
            var allTasks = JsonSerializer.Deserialize<List<ToDoItem>>(storedTasks) ?? new List<ToDoItem>();

            allTasks.RemoveAll(t => t.UserId.ToString() == _authService.CurrentUser!.UserId);
            allTasks.AddRange(Tasks);

            await _localStorage.SetItemAsync("tasks", JsonSerializer.Serialize(allTasks));
            _logger.LogInformation("Tasks saved to LocalStorage!");
        }

        /// <summary>
        /// Add a new task to the list
        /// </summary>
        public async Task AddTaskAsync(ToDoItem task)
        {
            if (!_authService.IsAuthenticated) return;

            task.Id = Tasks.Any() ? Tasks.Max(t => t.Id) + 1 : 1;
            task.UserId = Guid.Parse(_authService.CurrentUser!.UserId);
            Tasks.Add(task);

            await SaveTasksAsync();
            _logger.LogInformation("New task successfully added to the to do list!");
        }

        /// <summary>
        /// Delete a task by ID
        /// </summary>
        public async Task DeleteTaskAsync(int taskId)
        {
            var task = Tasks.FirstOrDefault(t => t.Id == taskId);
            if (task != null && task.UserId.ToString() == _authService.CurrentUser!.UserId)
            {
                Tasks.Remove(task);
                await SaveTasksAsync();
                _logger.LogInformation("Task removed from the to do list!");
            }
        }

        /// <summary>
        /// Toggle the completion state of a task
        /// </summary>
        public async Task StateToggleAsync(int taskId)
        {
            var task = Tasks.FirstOrDefault(t => t.Id == taskId);
            if (task != null && task.UserId.ToString() == _authService.CurrentUser!.UserId)
            {
                task.IsDone = !task.IsDone;
                await SaveTasksAsync();
                _logger.LogInformation("Completion state of the task updated!");
            }
        }

        /// <summary>
        /// Edit an existing task
        /// </summary>
        public async Task EditTaskAsync(int taskId, string newTitle, string newDescription, DateTime newDueDate)
        {
            var task = Tasks.FirstOrDefault(t => t.Id == taskId);
            if (task != null && task.UserId.ToString() == _authService.CurrentUser!.UserId)
            {
                task.Title = newTitle;
                task.Description = newDescription;
                task.DueDate = newDueDate;
                await SaveTasksAsync();
                _logger.LogInformation("Task updated.");
            }
        }

        /// <summary>
        /// Retrieve all tasks
        /// </summary>
        public async Task<List<ToDoItem>> GetAllTasksAsync()
        {
            if (Tasks.Count == 0)
            {
                await LoadTasksAsync();
            }
            return Tasks;
        }

        /// <summary>
        /// Retrieve filtered tasks
        /// </summary>
        public async Task<List<ToDoItem>> GetFilteredTasksAsync(TaskFilter filter)
        {
            await LoadTasksAsync();
            return filter switch
            {
                TaskFilter.Active => Tasks.Where(t => !t.IsDone).ToList(),
                TaskFilter.Completed => Tasks.Where(t => t.IsDone).ToList(),
                _ => Tasks.ToList()
            };
        }

        /// <summary>
        /// Get the count of incomplete tasks
        /// </summary>
        public async Task<int> GetIncompleteTaskCountAsync()
        {
            await LoadTasksAsync();
            return Tasks.Count(t => !t.IsDone);
        }
    }
}

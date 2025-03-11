using ToDoApp.Client.Models;

namespace ToDoApp.Client.Services
{
    public class ToDoService
    {
        public List<ToDoItem> Tasks { get; set; } = new();

        public void AddTask(ToDoItem task)
        {
            Tasks.Add(task);
        }

        public void DeleteTask(int taskId)
        {
            Tasks.RemoveAll(t => t.Id == taskId);
        }

        public void StateToggle(int taskId)
        {
            var task = Tasks.FirstOrDefault(t => t.Id == taskId);
            if (task != null)
            {
                task.IsDone = !task.IsDone;
            }
        }
    }
}

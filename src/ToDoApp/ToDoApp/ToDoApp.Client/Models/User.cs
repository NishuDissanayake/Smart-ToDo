namespace ToDoApp.Client.Models
{
    public class User
    {
        public string UserId { get; set; } = Guid.NewGuid().ToString();
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}

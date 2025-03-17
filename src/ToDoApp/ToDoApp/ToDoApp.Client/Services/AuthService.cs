using Blazored.LocalStorage;
using ToDoApp.Client.Models;

namespace ToDoApp.Client.Services
{
    public class AuthService
    {
        private readonly ILocalStorageService _localStorage;

        private readonly List<User> dummyUsers = new()
        {
            new() { Username = "admin", Password = "admin123", UserId = Guid.NewGuid().ToString() },
            new() { Username = "user1", Password = "password1", UserId = Guid.NewGuid().ToString() },
            new() { Username = "user2", Password = "password2", UserId = Guid.NewGuid().ToString() }
        };

        public User? CurrentUser { get; private set; }

        public AuthService(ILocalStorageService localStorage)
        {
            _localStorage = localStorage;
        }

        /// <summary>
        /// Load user session from LocalStorage
        /// </summary>
        public async Task LoadUserAsync()
        {
            CurrentUser = await _localStorage.GetItemAsync<User>("loggedInUser");
        }

        /// <summary>
        /// Authenticate user and store session in LocalStorage
        /// </summary>
        public async Task<bool> LoginAsync(string username, string password)
        {
            var user = dummyUsers.FirstOrDefault(u => u.Username == username && u.Password == password);
            if (user == null) return false;

            CurrentUser = user;
            await _localStorage.SetItemAsync("loggedInUser", user);
            return true;
        }

        /// <summary>
        /// Logout user
        /// </summary>
        public async Task LogoutAsync()
        {
            CurrentUser = null;
            await _localStorage.RemoveItemAsync("loggedInUser");
        }

        public bool IsAuthenticated => CurrentUser != null;
    }
}

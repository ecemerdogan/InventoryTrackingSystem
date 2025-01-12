namespace BookstoreInventoryTracking.Models
{
    public class User
    {
        public string UserId { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;

        public User(string userId, string name, string role)
        {
            UserId = userId;
            Name = name;
            Role = role;
            Password = "";
        }

        public User(string userId, string password, string name, string role)
        {
            UserId = userId;
            Password = password;
            Name = name;
            Role = role;
        }

        public User()
        {
            UserId = string.Empty;
            Password = string.Empty;
            Name = string.Empty;
            Role = string.Empty;
        }
    }
}

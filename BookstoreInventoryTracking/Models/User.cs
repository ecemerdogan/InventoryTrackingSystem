using System.ComponentModel;
using System.Security.Cryptography;

namespace BookstoreInventoryTracking.Models
{
    public class User : LoginWindow
    {
        public string UserName { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;

        public User(string UserName, string Name, string Role)
        {
            this.UserName = UserName;
            this.Name = Name;
            this.Role = Role;
        }
            
    }
}

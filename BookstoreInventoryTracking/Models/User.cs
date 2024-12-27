using System.ComponentModel;

namespace BookstoreInventory.Models
{
    public class User : LoginWindow
    {

        public string UserId { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

    }
}

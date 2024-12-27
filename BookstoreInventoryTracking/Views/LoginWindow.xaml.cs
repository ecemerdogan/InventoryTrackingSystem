using BookstoreInventory.Models;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace BookstoreInventory
{
    public partial class LoginWindow : Window
    {
        private ObservableCollection<User> allUsers = [];
        public LoginWindow()
        {
            InitializeComponent();
        }
        private void TxtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) // Check if Enter is pressed
            {
                BtnLogin_Click(sender, e); // Trigger the Login button click
            }
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            string userId = txtUserId.Text;
            string password = txtPassword.Password;

            if (ValidateUser(userId, password))
            {
                MainWindow.IsLoggedIn = true; // Successful Login
                MainWindow mainWindow = new MainWindow(); // Form Main Window
                mainWindow.Show(); // Show Main Window
                this.Close(); // Close the Login Page
            }
            else
            {
                MessageBox.Show("Invalid User ID or Password. Please try again.", "Login Failed");
            }
        }

        private bool ValidateUser(string userId, string password)
        {
            // Validation of the user.
            allUsers = new ObservableCollection<User> 
            {
                new User{UserId = "admin", Password = "password123" },
                new User{UserId = "user1", Password = "password456" },
                new User{UserId = "user2", Password = "password789"}
            };

            return allUsers.Any(u => u.UserId == userId && u.Password == password); ;
        }
    }
}

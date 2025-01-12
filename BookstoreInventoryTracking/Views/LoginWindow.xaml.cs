using BookstoreInventoryTracking.Models;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using BookstoreInventoryTracking.Helpers;

namespace BookstoreInventoryTracking.Views
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
            string userName = txtUserId.Text;
            string password = txtPassword.Password;

            if (DatabaseHelper.ValidateUser(userName, password))
            {
                // Oturum açmış kullanıcıyı kaydet
                UserSession.CurrentUser = DatabaseHelper.GetUserInfo(userName);
                MessageBox.Show($"Welcome, {UserSession.CurrentUser.Name}!", "Login Successful", MessageBoxButton.OK, MessageBoxImage.Information);
                MainWindow.IsLoggedIn = true; // Successful Login
                MainWindow mainWindow = new MainWindow(); // Form Main Window
                mainWindow.UpdateUserNameAndRole(DatabaseHelper.GetUserInfo(userName));
                mainWindow.Show(); // Show Main Window
                this.Close(); // Close the Login Page
            }
            else
            {
                MessageBox.Show("Invalid User ID or Password. Please try again.", "Login Failed");
            }
        }

        // private bool ValidateUser(string userId, string password)
        // {
        //     // Validation of the user.
        //     allUsers = new ObservableCollection<User> 
        //     {
        //         new User{UserId = "admin", Password = "password123" },
        //         new User{UserId = "user1", Password = "password456" },
        //         new User{UserId = "user2", Password = "password789"}
        //     };
        //
        //     return allUsers.Any(u => u.UserId == userId && u.Password == password);
        // }
    }
}

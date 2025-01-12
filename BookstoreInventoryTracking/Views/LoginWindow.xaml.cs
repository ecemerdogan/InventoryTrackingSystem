using System.Windows;
using System.Windows.Input;
using BookstoreInventoryTracking.Helpers;

namespace BookstoreInventoryTracking.Views
{
    // Login window class for the Bookstore Inventory Tracking application
    public partial class LoginWindow
    {
        public LoginWindow()
        {
            InitializeComponent(); // Initialize the UI components
        }

        // Event handler for detecting when a key is pressed in the password textbox
        private void TxtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) // Check if the "Enter" key was pressed
            {
                BtnLogin_Click(sender, e); // Trigger the Login button click event
            }
        }

        // Event handler for the Login button click
        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            string userName = TxtUserId.Text; // Retrieve the entered username
            string password = TxtPassword.Password; // Retrieve the entered password

            // Validate the user's credentials using the database helper
            if (DatabaseHelper.ValidateUser(userName, password))
            {
                // Save the logged-in user's information
                UserSession.CurrentUser = DatabaseHelper.GetUserInfo(userName);
                MessageBox.Show($"Welcome, {UserSession.CurrentUser.Name}!", "Login Successful", MessageBoxButton.OK, MessageBoxImage.Information);

                MainWindow.IsLoggedIn = true; // Indicate successful login
                MainWindow mainWindow = new MainWindow(); // Create the main window
                mainWindow.UpdateUserNameAndRole(DatabaseHelper.GetUserInfo(userName)); // Update user info on the main window
                mainWindow.Show(); // Display the main window
                this.Close(); // Close the login window
            }
            else
            {
                // Show an error message for invalid login credentials
                MessageBox.Show("Invalid User ID or Password. Please try again.", "Login Failed");
            }
        }
    }
}

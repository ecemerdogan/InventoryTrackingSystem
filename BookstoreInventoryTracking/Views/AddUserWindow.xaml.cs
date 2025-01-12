using BookstoreInventoryTracking.Models;
using System.Windows;
using BookstoreInventoryTracking.Helpers;

namespace BookstoreInventoryTracking.Views
{
    /// <summary>
    /// Interaction logic for AddUserWindow.xaml
    /// </summary>
    public partial class AddUserWindow
    {
        public AddUserWindow()
        {
            InitializeComponent(); // Initialize UI components
        }

        // Event handler for the Confirm button click
        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Check if all required fields are filled
                if (string.IsNullOrWhiteSpace(UserIdTextBox.Text) ||
                        string.IsNullOrWhiteSpace(NameTextBox.Text))
                {
                    MessageBox.Show("All fields are required. Please complete them before saving.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Check if a role is selected
                if (AdminRadioButton.IsChecked == false && ModRadioButton.IsChecked == false)
                {
                    MessageBox.Show("Please select a role (Admin or Mod).", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Determine the role based on the selected radio button
                string role = AdminRadioButton.IsChecked == true ? "Admin" : "Mod";

                // Save the new user and set dialog result to true if successful
                DialogResult = DatabaseHelper.InsertUser(new User(UserIdTextBox.Text, DatabaseHelper.Encrypt(PasswordBox.Password), NameTextBox.Text, role));
            }
            catch (Exception ex)
            {
                // Display an error message if an exception occurs
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Event handler for the Cancel button click
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            // Close the dialog without saving
            DialogResult = false;
        }
    }
}

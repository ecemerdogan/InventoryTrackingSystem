using BookstoreInventoryTracking.Models;
using System.Windows;
using BookstoreInventoryTracking.Helpers;

namespace BookstoreInventoryTracking.Views
{
    /// <summary>
    /// AddUserWindow.xaml etkileşim mantığı
    /// </summary>
    public partial class AddUserWindow : Window
    {
        public string UserName { get; private set; }
        public new string Name { get; private set; }
        public string Password { get; private set; }

        public AddUserWindow()
        {
            InitializeComponent();
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Alanların doldurulup doldurulmadığını kontrol edin
                if (string.IsNullOrWhiteSpace(UserIdTextBox.Text) ||
                        string.IsNullOrWhiteSpace(NameTextBox.Text))
                {
                    MessageBox.Show("All fields are required. Please complete them before saving.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Rol seçim kontrolü
                if (AdminRadioButton.IsChecked == false && ModRadioButton.IsChecked == false)
                {
                    MessageBox.Show("Please select a role (Admin or Mod).", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Rolü belirle
                string role = AdminRadioButton.IsChecked == true ? "Admin" : "Mod";

                // Başarılı bir şekilde kaydedildiğini bildir ve pencereyi kapat
                DialogResult = DatabaseHelper.InsertUser(new User(UserName = UserIdTextBox.Text, Password = DatabaseHelper.Encrypt(PasswordBox.Password), Name = NameTextBox.Text, role));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            // Close the dialog without saving
            DialogResult = false;
        }
    }


}


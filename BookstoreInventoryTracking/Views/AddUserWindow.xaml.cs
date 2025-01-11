using BookstoreInventoryTracking.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BookstoreInventoryTracking.Views
{
    /// <summary>
    /// AddUserWindow.xaml etkileşim mantığı
    /// </summary>
    public partial class AddUserWindow : Window
    {
        public User NewUser { get; private set; }
        public string UserName { get; private set; }
        public new string Name { get; private set; }
        public string Password { get; private set; }

        public AddUserWindow()
        {
            InitializeComponent();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
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
                if (AdminCheckBox.IsChecked == false && UserCheckBox.IsChecked == false)
                {
                    MessageBox.Show("Please select a role (Admin or User).", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Rolü belirle
                string role = AdminCheckBox.IsChecked == true ? "admin" : "user";

                // Kullanıcı nesnesini oluştur
                NewUser = new User(UserName = UserIdTextBox.Text, Name = NameTextBox.Text, role);

                // Başarılı bir şekilde kaydedildiğini bildir ve pencereyi kapat
                DialogResult = true;
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


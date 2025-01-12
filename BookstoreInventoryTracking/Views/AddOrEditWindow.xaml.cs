using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using BookstoreInventoryTracking.Models;

namespace BookstoreInventoryTracking.Views
{
    public partial class AddItemWindow
    {
        public Book? NewBook { get; private set; }

        public AddItemWindow(Book? bookToEdit = null)
        {
            InitializeComponent();

            if (bookToEdit != null)
            {
                IsbnTextBox.Text = bookToEdit.Isbn;
                NameTextBox.Text = bookToEdit.Name;
                AuthorTextBox.Text = bookToEdit.Author;
                LocationTextBox.Text = bookToEdit.Location;
                PriceTextBox.Text = bookToEdit.Price.ToString(CultureInfo.InvariantCulture);
                QuantityTextBox.Text = bookToEdit.Quantity.ToString();

                // ISBN alanını sadece okunabilir yap
                IsbnTextBox.IsReadOnly = true;
            }
        }
        
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!AreTextBoxesFilled())
                {
                    MessageBox.Show("All fields are required. Please complete them before saving.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                NewBook = new Book
                {
                    Isbn = IsbnTextBox.Text,
                    Name = NameTextBox.Text,
                    Author = AuthorTextBox.Text,
                    Location = LocationTextBox.Text,
                    Price  = double.Parse(PriceTextBox.Text),
                    Quantity = int.Parse(QuantityTextBox.Text)
                };
                DialogResult = true; // Pencereyi kapatır ve kaydetme işlemini onaylar.
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hata: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false; // Pencereyi kapatır ve hiçbir değişiklik yapılmaz.
        }

        private void QuantityTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !Regex.IsMatch(e.Text, "^[0-9]+$");
        }
        
        private void PriceTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !Regex.IsMatch(e.Text, @"^[0-9]*(\.[0-9]*)?$");
        }

        private bool AreTextBoxesFilled()
        {
            if (string.IsNullOrWhiteSpace(IsbnTextBox.Text) ||
                string.IsNullOrWhiteSpace(NameTextBox.Text) ||
                string.IsNullOrWhiteSpace(AuthorTextBox.Text) ||
                string.IsNullOrWhiteSpace(LocationTextBox.Text) ||
                string.IsNullOrWhiteSpace(PriceTextBox.Text) ||
                string.IsNullOrWhiteSpace(QuantityTextBox.Text))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
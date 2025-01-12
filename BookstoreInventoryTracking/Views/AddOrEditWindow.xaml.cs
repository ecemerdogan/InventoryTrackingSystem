using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using BookstoreInventoryTracking.Models;

namespace BookstoreInventoryTracking.Views
{
    // Add or edit item window for the Bookstore Inventory Tracking application
    public partial class AddItemWindow
    {
        public Book? NewBook { get; private set; } // Holds the book object created or edited

        public AddItemWindow(Book? bookToEdit = null)
        {
            InitializeComponent();

            // Populate fields if editing an existing book
            if (bookToEdit != null)
            {
                IsbnTextBox.Text = bookToEdit.Isbn;
                NameTextBox.Text = bookToEdit.Name;
                AuthorTextBox.Text = bookToEdit.Author;
                LocationTextBox.Text = bookToEdit.Location;
                PriceTextBox.Text = bookToEdit.Price.ToString(CultureInfo.InvariantCulture);
                QuantityTextBox.Text = bookToEdit.Quantity.ToString();

                // Make the ISBN field read-only when editing
                IsbnTextBox.IsReadOnly = true;
            }
        }
        
        // Event handler for the Save button click
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Ensure all fields are filled
                if (!AreTextBoxesFilled())
                {
                    MessageBox.Show("All fields are required. Please complete them before saving.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Create or update a book object with the entered data
                NewBook = new Book
                {
                    Isbn = IsbnTextBox.Text,
                    Name = NameTextBox.Text,
                    Author = AuthorTextBox.Text,
                    Location = LocationTextBox.Text,
                    Price  = double.Parse(PriceTextBox.Text),
                    Quantity = int.Parse(QuantityTextBox.Text)
                };

                DialogResult = true; // Close the window and confirm the save
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
            DialogResult = false; // Close the window without saving changes
        }

        // Input validation for the Quantity field (allow only numbers)
        private void QuantityTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !Regex.IsMatch(e.Text, "^[0-9]+$"); // Block non-numeric input
        }
        
        // Input validation for the Price field (allow numbers with an optional decimal point)
        private void PriceTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !Regex.IsMatch(e.Text, @"^[0-9]*(\.[0-9]*)?$"); // Block invalid numeric input
        }

        // Helper method to check if all text boxes are filled
        private bool AreTextBoxesFilled()
        {
            // Return false if any field is empty or contains only whitespace
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

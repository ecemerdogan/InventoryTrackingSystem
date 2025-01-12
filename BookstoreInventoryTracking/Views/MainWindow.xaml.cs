using System.Windows;
using System.Collections.ObjectModel;
using System.DirectoryServices.ActiveDirectory;
using System.Windows.Controls;
using System.Windows.Media;
using BookstoreInventoryTracking.Models;
using BookstoreInventoryTracking.Helpers;

namespace BookstoreInventoryTracking.Views
{
    // Main window class for the Bookstore Inventory Tracking application
    public partial class MainWindow
    {
        public static bool IsLoggedIn = false; // Tracks login status

        private ObservableCollection<Book> _allBooks = []; // Collection of all books in the inventory
        private bool isAllComponentInitialized = false;
        
        public MainWindow()
        {
            InitializeComponent();
            isAllComponentInitialized = true;
            // Redirect to login page if user is not logged in
            if (!IsLoggedIn)
            {
                MessageBox.Show("Unauthorized access detected. Redirecting to login page.");
                var loginWindow = new LoginWindow();
                loginWindow.Show();
                this.Close(); // Close the main window
            }
            LoadInventoryData(); // Load data into the table
        }

        // Handles text changes in the search box
        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Do not filter if the text is empty or "Search..."
            if (string.IsNullOrWhiteSpace(SearchBox.Text) || SearchBox.Text == "Search...")
            {
                // Show all items by default
                RefreshGrids();
                return;
            }

            string searchText = SearchBox.Text.ToLower(); // Convert search text to lowercase

            if (InventoryTabControl == null) return; // Check to avoid errors

            // Perform filtering
            if (InventoryTabControl.SelectedIndex == 0)
            {
                InventoryGrid.ItemsSource = _allBooks
                    .Where(book => book.Quantity > 0 &&
                                   (book.Name.ToLower().Contains(searchText) ||
                                    book.Author.ToLower().Contains(searchText) ||
                                    book.Isbn.ToLower().Contains(searchText)))
                    .ToList();
            }
            else if (InventoryTabControl.SelectedIndex == 1)
            {
                OutOfStockGrid.ItemsSource = _allBooks
                    .Where(book => book.Quantity == 0 &&
                                   (book.Name.ToLower().Contains(searchText) ||
                                    book.Author.ToLower().Contains(searchText) ||
                                    book.Isbn.ToLower().Contains(searchText)))
                    .ToList();
            }
        }

        private void SearchBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(SearchBox.Text))
            {
                SearchBox.Text = "Search...";
                SearchBox.Foreground = Brushes.Gray;
                
                RefreshGrids();
            }
        }

        private void SearchBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (SearchBox.Text == "Search...")
            {
                SearchBox.Text = "";
                SearchBox.Foreground = Brushes.Black;
            }
        }

        private void ResetSearchBox()
        {
            SearchBox.Text = "Search...";
            SearchBox.Foreground = Brushes.Gray;
        }

        // Event handler for the "Add" button click to add a new item
        private void BtnAddNewItem_Click(object sender, RoutedEventArgs e)
        {
            var addWindow = new AddItemWindow();
            if (addWindow.ShowDialog() == true)
            {
                if (addWindow.NewBook != null)
                {
                    DatabaseHelper.InsertBook(addWindow.NewBook); // Add the new book to the database
                    LoadInventoryData(); // Refresh the inventory data
                }
            }
        }

        // Event handler for the "Edit" button click to edit an existing item
        private void BtnEditAnItem_Click(object sender, RoutedEventArgs e)
        {
            DataGrid selectedGrid = GetSelectedDataGrid(); // Get the active data grid

            var selectedBooks = selectedGrid.Items
                .OfType<Book>()
                .Where(book => book.IsSelected) // Check for selected items
                .ToList();

            // Ensure only one book is selected for editing
            if (selectedBooks.Count == 1)
            {
                var selectedBook = selectedBooks.First();
                var editWindow = new AddItemWindow(selectedBook);

                if (editWindow.ShowDialog() == true)
                {
                    if (editWindow.NewBook != null)
                    {
                        DatabaseHelper.UpdateBook(editWindow.NewBook); // Update book in the database
                        LoadInventoryData(); // Refresh inventory data
                    }
                }
            }
            else if (selectedBooks.Count > 1)
            {
                MessageBox.Show("Please select only one book to edit.");
            }
            else
            {
                MessageBox.Show("Please select a book to edit.");
            }
        }

        // Event handler for the "Delete" button click to remove selected items
        private void BtnDeleteAnItem_Click(object sender, RoutedEventArgs e)
        {
            DataGrid selectedGrid = GetSelectedDataGrid(); // Get the active data grid

            var selectedBooks = selectedGrid.Items
                .OfType<Book>()
                .Where(book => book.IsSelected) // Check for selected items
                .ToList();

            // Delete selected books if any are selected
            if (selectedBooks.Count > 0)
            {
                string deletedBooks = "";
                foreach (var book in selectedBooks)
                {
                    DatabaseHelper.DeleteBook(book.Isbn); // Delete book from database
                    deletedBooks = book.Name + ", " + deletedBooks;
                }
                MessageBox.Show(deletedBooks + " are deleted!!");
                LoadInventoryData(); // Refresh inventory data
            }
            else
            {
                MessageBox.Show("Please select a book to delete.");
            }
        }

        // Event handler for the "Add User" button click
        private void BtnAddUser_Click(object sender, RoutedEventArgs e)
        {
            // Check the user's role before allowing access
            if (UserSession.CurrentUser.Role.ToLower() == "admin")
            {
                // Open the "Add User" window as a modal
                AddUserWindow addUserWindow = new AddUserWindow();
                addUserWindow.ShowDialog();
            }
            else
            {
                // Display an error message for unauthorized users
                MessageBox.Show("You do not have the required permissions to add a user.", "Access Denied", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        // Load inventory data from the database
        private void LoadInventoryData()
        {
            _allBooks = DatabaseHelper.GetAllBooks(); // Fetch all books
            
            if (_allBooks != null)
                RefreshGrids(true); // Refresh the data grids
        }

        // Refresh the grids for in-stock and out-of-stock items
        private void RefreshGrids(bool willSearchBoxReset = false)
        {
            if (isAllComponentInitialized)
            {
                InventoryGrid.ItemsSource = _allBooks.Where(book => book.Quantity > 0).ToList();
                OutOfStockGrid.ItemsSource = _allBooks.Where(book => book.Quantity == 0).ToList();
            }

            if (willSearchBoxReset)
            {
                ResetSearchBox();
            }
        }

        // Get the currently selected data grid based on the active tab
        private DataGrid GetSelectedDataGrid()
        {
            switch (InventoryTabControl.SelectedIndex)
            {
                case 0:
                    ResetSearchBox();
                    return InventoryGrid; // "In Stock" grid
                case 1:
                    ResetSearchBox();
                    return OutOfStockGrid; // "Out of Stock" grid
                default:
                    throw new InvalidOperationException("No tab is selected!");
            }
        }

        // Update the UI with the user's name and role
        public void UpdateUserNameAndRole(User user)
        {
            NameTextBlock.Text = user.Name; // Update the displayed name
            UserRoleTextBlock.Text = user.Role; // Update the displayed role
        }
    }
}

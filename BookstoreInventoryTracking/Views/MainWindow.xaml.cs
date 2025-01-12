
using System.Windows;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using BookstoreInventoryTracking.Models;
using BookstoreInventoryTracking.Helpers;

namespace BookstoreInventoryTracking.Views
{
    
    public partial class MainWindow
    {
        public static bool IsLoggedIn = false; // Giriş durumunu takip etmek için

        private ObservableCollection<Book> _allBooks = []; // Tüm ürünlerin listesi
        public MainWindow()
        {
            InitializeComponent();
            if (!IsLoggedIn)
            {
                MessageBox.Show("Unauthorized access detected. Redirecting to login page.");
                var loginWindow = new LoginWindow();
                loginWindow.Show();
                this.Close(); // Ana pencereyi kapat
            }

            LoadInventoryData(); // Tabloyu veriyle doldurmak için bir metot çağrısı
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchText = SearchBox.Text.ToLower();

            if (InventoryTabControl == null) return; // Null kontrolü


            if (InventoryTabControl.SelectedIndex == 0) // "In Stock" sekmesi
            {
                InventoryGrid.ItemsSource = _allBooks
                    .Where(book => book.Quantity > 0 &&
                                   (book.Name.ToLower().Contains(searchText) ||
                                    book.Author.ToLower().Contains(searchText) ||
                                    book.Isbn.ToLower().Contains(searchText)))
                    .ToList();
            }
            else if (InventoryTabControl.SelectedIndex == 1) // "Out of Stock" sekmesi
            {
                OutOfStockGrid.ItemsSource = _allBooks
                    .Where(book => book.Quantity == 0 &&
                                   (book.Name.ToLower().Contains(searchText) ||
                                    book.Author.ToLower().Contains(searchText) ||
                                    book.Isbn.ToLower().Contains(searchText)))
                    .ToList();
            }
        }

        // "Add" butonuna tıklandığında
        private void BtnAddNewItem_Click(object sender, RoutedEventArgs e)
        {
            var addWindow = new AddItemWindow();
            if (addWindow.ShowDialog() == true)
            {
                if (addWindow.NewBook != null)
                {
                    DatabaseHelper.InsertBook(addWindow.NewBook);
                    LoadInventoryData();
                }
            }
        }

        // "Edit" butonuna tıklandığında
        private void BtnEditAnItem_Click(object sender, RoutedEventArgs e)
        {
            DataGrid selectedGrid = GetSelectedDataGrid();

            var selectedBooks = selectedGrid.Items
                .OfType<Book>()
                .Where(book => book.IsSelected)
                .ToList();

            // Seçili kitap sayısını kontrol et
            if (selectedBooks.Count == 1)
            {
                var selectedBook = selectedBooks.First();
                var editWindow = new AddItemWindow(selectedBook);

                if (editWindow.ShowDialog() == true)
                {
                    if (editWindow.NewBook != null)
                    {
                        DatabaseHelper.UpdateBook(editWindow.NewBook);
                        LoadInventoryData();
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

        // "Delete" butonuna tıklandığında
        private void BtnDeleteAnItem_Click(object sender, RoutedEventArgs e)
        {
            DataGrid selectedGrid = GetSelectedDataGrid();

            var selectedBooks = selectedGrid.Items
                .OfType<Book>()
                .Where(book => book.IsSelected)
                .ToList();

            if (selectedBooks.Count > 0)
            {
                string deletedBooks = "";
                foreach (var book in selectedBooks)
                {
                    DatabaseHelper.DeleteBook(book.Isbn);
                    deletedBooks = book.Name + ", " + deletedBooks;
                }
                MessageBox.Show(deletedBooks + " are deleted!!");
                LoadInventoryData();
            }
            else
            {
                MessageBox.Show("Please select a book to delete.");
            }
        }

        private void BtnAddUser_Click(object sender, RoutedEventArgs e)
        {
            // Kullanıcının rolünü kontrol edin
            if (UserSession.CurrentUser.Role.ToLower() == "admin")
            {
                // Yeni kullanıcı ekleme penceresini aç
                AddUserWindow addUserWindow = new AddUserWindow();
                addUserWindow.ShowDialog(); // Modal pencere olarak aç
            }
            else
            {
                // Yetkisiz kullanıcı için hata mesajı göster
                MessageBox.Show("You do not have the required permissions to add a user.", "Access Denied", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        


        // Tabloyu dolduracak örnek veri
        private void LoadInventoryData()
        {
            _allBooks = DatabaseHelper.GetAllBooks();
            RefreshGrids();
        }

        private void RefreshGrids()
        {
            InventoryGrid.ItemsSource = _allBooks.Where(book => book.Quantity > 0).ToList();
            OutOfStockGrid.ItemsSource = _allBooks.Where(book => book.Quantity == 0).ToList();
        }
        
        private DataGrid GetSelectedDataGrid()
        {
            switch (InventoryTabControl.SelectedIndex)
            {
                case 0:
                    return InventoryGrid;
                case 1:
                    return OutOfStockGrid;
                default: 
                    throw new InvalidOperationException("No tab is selected!");
            }
        }
        public void UpdateUserNameAndRole(User user)
        {
            NameTextBlock.Text = user.Name;  // Change the name text
            UserRoleTextBlock.Text = user.Role;  // Change the user role text
        }
    }
}

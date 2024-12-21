using System.Windows;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;
using BookstoreInventory.Models;

namespace BookstoreInventory
{
    public partial class MainWindow : Window
    {

        private ObservableCollection<Book> allBooks; // Tüm ürünlerin listesi
        public MainWindow()
        {
            InitializeComponent();
            LoadInventoryData(); // Tabloyu veriyle doldurmak için bir metot çağrısı
        }

        private void SearchBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            string searchText = SearchBox.Text.ToLower();

            if (InventoryTabControl == null) return; // Null kontrolü


            if (InventoryTabControl.SelectedIndex == 0) // "In Stock" sekmesi
            {
                InventoryGrid.ItemsSource = allBooks
                    .Where(book => book.Quantity > 0 &&
                                   (book.Name.ToLower().Contains(searchText) ||
                                    book.Author.ToLower().Contains(searchText) ||
                                    book.ISBN.ToLower().Contains(searchText)))
                    .ToList();
            }
            else if (InventoryTabControl.SelectedIndex == 1) // "Out of Stock" sekmesi
            {
                OutOfStockGrid.ItemsSource = allBooks
                    .Where(book => book.Quantity == 0 &&
                                   (book.Name.ToLower().Contains(searchText) ||
                                    book.Author.ToLower().Contains(searchText) ||
                                    book.ISBN.ToLower().Contains(searchText)))
                    .ToList();
            }
        }

        // "Add New Item" butonuna tıklandığında
        private void BtnAddNewItem_Click(object sender, RoutedEventArgs e)
        {
            var addWindow = new AddItemWindow();
            if (addWindow.ShowDialog() == true)
            {
                allBooks.Add(addWindow.NewBook);
            }
            RefreshGrids();
        }

        // "Add An Item" butonuna tıklandığında
        private void BtnEditAnItem_Click(object sender, RoutedEventArgs e)
        {
            DataGrid SelectedGrid = GetSelectedDataGrid();

            var SelectedBooks = SelectedGrid.Items
                .OfType<Book>()
                .Where(book => book.IsSelected)
                .ToList();

            // Seçili kitap sayısını kontrol et
            if (SelectedBooks.Count == 1)
            {
                var selectedBook = SelectedBooks.First();
                var editWindow = new AddItemWindow(selectedBook);
        
                if (editWindow.ShowDialog() == true)
                {
                    // Seçilen kitabı güncelle
                    var index = allBooks.IndexOf(selectedBook);
                    allBooks[index] = editWindow.NewBook;
                    RefreshGrids();
                }
            }
            else if (SelectedBooks.Count > 1)
            {
                MessageBox.Show("Please select only one book to edit.");
            }
            else
            {
                MessageBox.Show("Please select a book to edit.");
            }
        }
        
        // "Delete An Item" butonuna tıklandığında
        private void BtnDeleteAnItem_Click(object sender, RoutedEventArgs e)
        {
            DataGrid SelectedGrid = GetSelectedDataGrid();

            var SelectedBooks = SelectedGrid.Items
                .OfType<Book>()
                .Where(book => book.IsSelected)
                .ToList();

            if (SelectedBooks.Count > 0)
            {
                foreach (var book in SelectedBooks)
                {
                    allBooks.Remove(book);
                    MessageBox.Show(book.Name + " which is written by " + book.Author + " is deleted!!");
                }
            }
            else
            {
                MessageBox.Show("Please select a book to delete.");
            }

            // Refresh the grids
            RefreshGrids();
        }

        // Tabloyu dolduracak örnek veri
        private void LoadInventoryData()
        {
            allBooks = new ObservableCollection<Book>
            {
                new Book { ISBN = "978-3-16-148410-0", Name = "Book 1", Author = "Author A", Location = "Aisle 1", Price = 12.99, Quantity = 5 },
                new Book { ISBN = "978-1-4028-9462-6", Name = "Book 2", Author = "Author B", Location = "Aisle 2", Price = 15.50, Quantity = 2 },
                new Book { ISBN = "978-0-596-52068-7", Name = "Book 3", Author = "Author C", Location = "Aisle 3", Price = 18.75, Quantity = 0 },
                new Book { ISBN = "978-0-201-53082-4", Name = "Book 4", Author = "Author D", Location = "Aisle 1", Price = 22.00, Quantity = 8 },
                new Book { ISBN = "978-3-16-148410-1", Name = "Book 5", Author = "Author E", Location = "Aisle 2", Price = 14.50, Quantity = 0 },
                new Book { ISBN = "978-1-4028-9462-7", Name = "Book 6", Author = "Author F", Location = "Aisle 3", Price = 19.99, Quantity = 4 },
                new Book { ISBN = "978-0-596-52068-8", Name = "Book 7", Author = "Author G", Location = "Aisle 4", Price = 9.99, Quantity = 3 },
                new Book { ISBN = "978-0-201-53082-5", Name = "Book 8", Author = "Author H", Location = "Aisle 1", Price = 7.50, Quantity = 0 },
                new Book { ISBN = "978-3-16-148410-2", Name = "Book 9", Author = "Author I", Location = "Aisle 2", Price = 25.00, Quantity = 1 },
                new Book { ISBN = "978-1-4028-9462-8", Name = "Book 10", Author = "Author J", Location = "Aisle 3", Price = 11.50, Quantity = 6 },
                new Book { ISBN = "978-0-596-52068-9", Name = "Book 11", Author = "Author K", Location = "Aisle 4", Price = 13.25, Quantity = 0 },
                new Book { ISBN = "978-0-201-53082-6", Name = "Book 12", Author = "Author L", Location = "Aisle 1", Price = 20.00, Quantity = 2 },
                new Book { ISBN = "978-3-16-148410-3", Name = "Book 13", Author = "Author M", Location = "Aisle 2", Price = 17.50, Quantity = 3 },
                new Book { ISBN = "978-1-4028-9462-9", Name = "Book 14", Author = "Author N", Location = "Aisle 3", Price = 10.99, Quantity = 5 },
                new Book { ISBN = "978-0-596-52068-10", Name = "Book 15", Author = "Author O", Location = "Aisle 4", Price = 12.00, Quantity = 0 },
                new Book { ISBN = "978-0-201-53082-7", Name = "Book 16", Author = "Author P", Location = "Aisle 1", Price = 21.50, Quantity = 7 },
                new Book { ISBN = "978-3-16-148410-4", Name = "Book 17", Author = "Author Q", Location = "Aisle 2", Price = 15.75, Quantity = 0 },
                new Book { ISBN = "978-1-4028-9463-0", Name = "Book 18", Author = "Author R", Location = "Aisle 3", Price = 8.25, Quantity = 2 },
                new Book { ISBN = "978-0-596-52068-11", Name = "Book 19", Author = "Author S", Location = "Aisle 4", Price = 5.99, Quantity = 9 },
                new Book { ISBN = "978-0-201-53082-8", Name = "Book 20", Author = "Author T", Location = "Aisle 1", Price = 18.00, Quantity = 0 }
            };

            RefreshGrids();
        }

        private void RefreshGrids()
        {
            InventoryGrid.ItemsSource = allBooks.Where(book => book.Quantity > 0).ToList();
            OutOfStockGrid.ItemsSource = allBooks.Where(book => book.Quantity == 0).ToList();
        }

        private void BtnRaiseItem_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Raise button clicked!");
        }

        private void BtnClc_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Clear button clicked!");
        }
        private DataGrid GetSelectedDataGrid()
        {
            if (InventoryTabControl.SelectedIndex == 0) // "In Stock" sekmesi
            {
                return InventoryGrid;
            }
            else if (InventoryTabControl.SelectedIndex == 1) // "Out of Stock" sekmesi
            {
                return OutOfStockGrid;
            }
            return null; // Hiçbir sekme seçilmemişse
        }
    }
}



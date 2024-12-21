﻿using System.Windows;
using System.Collections.Generic;
using System.Linq;

namespace BookstoreInventory
{
    public partial class MainWindow : Window
    {

        private List<Book> allBooks; // Tüm ürünlerin listesi
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
            MessageBox.Show("Add New Item clicked!");
            // Refresh the grids
            RefreshGrids();
        }

        // "Add An Item" butonuna tıklandığında
        private void BtnEditAnItem_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Edit An Item clicked!");
        }

        // "Delete An Item" butonuna tıklandığında
        private void BtnDeleteAnItem_Click(object sender, RoutedEventArgs e)
        {
            

            if (InventoryTabControl.SelectedIndex == 0) // In Stock tab
            {
                var selectedBooks = InventoryGrid.SelectedItems.Cast<Book>().ToList();

                foreach (var book in selectedBooks)
                {
                    allBooks.Remove(book);
                    MessageBox.Show(book.Name + " which is written by " + book.Author + " is deleted!!");
                }
            }
            else if (InventoryTabControl.SelectedIndex == 1) // Out of Stock tab
            {
                var selectedBooks = OutOfStockGrid.SelectedItems.Cast<Book>().ToList();

                foreach (var book in selectedBooks)
                {
                    allBooks.Remove(book);
                    MessageBox.Show(book.Name + " which is written by " + book.Author + " is deleted!!");
                }
            }

            // Refresh the grids
            RefreshGrids();
        }

        // Tabloyu dolduracak örnek veri
        private void LoadInventoryData()
        {
            allBooks = new List<Book>
            {
                new Book { ISBN = "978-3-16-148410-0", Name = "Book 1", Author = "Author A", Location = "Aisle 1", Price = 12.99, Quantity = 5 },
                new Book { ISBN = "978-1-4028-9462-6", Name = "Book 2", Author = "Author B", Location = "Aisle 2", Price = 15.50, Quantity = 2 },
                new Book { ISBN = "978-0-596-52068-7", Name = "Book 3", Author = "Author C", Location = "Aisle 3", Price = 18.75, Quantity = 0 } // Out of Stock
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
    }
}
// Kitap sınıfı
public class Book
{
    public string ISBN { get; set; }
    public string Name { get; set; }
    public string Author { get; set; }
    public string Location { get; set; }
    public double Price { get; set; }
    public int Quantity { get; set; }
}



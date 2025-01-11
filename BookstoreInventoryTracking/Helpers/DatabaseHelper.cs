using System.Collections.ObjectModel;
using BookstoreInventoryTracking.Models;
using Npgsql;

namespace BookstoreInventoryTracking.Helpers
{
    public static class DatabaseHelper
    {
        private static readonly string _connectionString =
            "Host=database-1.clcqga86m19q.eu-central-1.rds.amazonaws.com;Port=5432;Database=BookstoreInventory;Username=postgres;Password=M3hIRREHBYkdrhPJrKJR;";

        public static void InsertBook(Book book)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                string query =
                    "INSERT INTO Books (ISBN, Name, Author, Location, Price, Quantity) VALUES (@isbn, @name, @author, @location, @price, @quantity)";

                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("isbn", book.ISBN);
                    command.Parameters.AddWithValue("name", book.Name);
                    command.Parameters.AddWithValue("author", book.Author);
                    command.Parameters.AddWithValue("location", book.Location);
                    command.Parameters.AddWithValue("price", book.Price);
                    command.Parameters.AddWithValue("quantity", book.Quantity);
                    command.ExecuteNonQuery();
                }
            }
        }

        public static ObservableCollection<Book> GetAllBooks()
        {
            var books = new ObservableCollection<Book>();

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM Books";

                using (var command = new NpgsqlCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var book = new Book
                            {
                                ISBN = reader["ISBN"].ToString(),
                                Name = reader["Name"].ToString(),
                                Author = reader["Author"].ToString(),
                                Location = reader["Location"].ToString(),
                                Price = Convert.ToDouble(reader["Price"]),
                                Quantity = Convert.ToInt32(reader["Quantity"])
                            };
                            books.Add(book);
                        }
                    }
                }
            }

            return books;
        }

        public static void UpdateBook(Book book)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                string query =
                    "UPDATE Books SET Name = @name, Author = @author, Location = @location, Price = @price, Quantity = @quantity WHERE ISBN = @isbn";

                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("isbn", book.ISBN);
                    command.Parameters.AddWithValue("name", book.Name);
                    command.Parameters.AddWithValue("author", book.Author);
                    command.Parameters.AddWithValue("location", book.Location);
                    command.Parameters.AddWithValue("price", book.Price);
                    command.Parameters.AddWithValue("quantity", book.Quantity);
                    command.ExecuteNonQuery();
                }
            }
        }

        public static void DeleteBook(string isbn)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                string query = "DELETE FROM Books WHERE ISBN = @isbn";

                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("isbn", isbn);
                    command.ExecuteNonQuery();
                }
            }
        }

        // Method to validate login
        public static bool ValidateUser(string username, string password)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                using (var command =
                       new NpgsqlCommand("SELECT password FROM users WHERE username = @username", connection))
                {
                    command.Parameters.AddWithValue("@username", username);
                    var storedHashedPassword = command.ExecuteScalar() as string;

                    if (storedHashedPassword == null)
                    {
                        return false; // User does not exist
                    }

                    return BCrypt.Net.BCrypt.Verify(password, storedHashedPassword);
                }
            }
        }

        public static User GetUserInfo(string username)
        {
            string userName = string.Empty;
            string name = string.Empty;
            string role = string.Empty;
            
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                using (var command =
                       new NpgsqlCommand("SELECT username, name, role FROM users WHERE username = @username",
                           connection))
                {
                    command.Parameters.AddWithValue("username", username);

                    var reader = command.ExecuteReader();
                    if (reader.Read()) // Check if there is a result
                    {
                        userName = reader["username"].ToString(); // Get the username
                        name = reader["name"].ToString(); // Get the name
                        role = reader["role"].ToString(); // Get the role
                    }
                }
            }
            return new User(userName, name, role);
        }


    }
}

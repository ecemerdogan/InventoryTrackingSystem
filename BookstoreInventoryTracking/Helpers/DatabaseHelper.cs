using System.Collections.ObjectModel;
using System.Data;
using BookstoreInventoryTracking.Models;
using Npgsql;

namespace BookstoreInventoryTracking.Helpers
{
    // Helper class to manage database operations
    public static class DatabaseHelper
    {
        private static readonly string ConnectionString = ""; // Database connection string

        // Insert a new book into the database
        public static void InsertBook(Book book)
        {
            using (var connection = new NpgsqlConnection(ConnectionString))
            {
                connection.Open();
                string query =
                    "INSERT INTO Books (ISBN, Name, Author, Location, Price, Quantity) VALUES (@isbn, @name, @author, @location, @price, @quantity)";
                
                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("isbn", book.Isbn);
                    command.Parameters.AddWithValue("name", book.Name);
                    command.Parameters.AddWithValue("author", book.Author);
                    command.Parameters.AddWithValue("location", book.Location);
                    command.Parameters.AddWithValue("price", book.Price);
                    command.Parameters.AddWithValue("quantity", book.Quantity);
                    command.ExecuteNonQuery(); // Execute the query
                }
            }
        }

        // Retrieve all books from the database
        public static ObservableCollection<Book> GetAllBooks()
        {
            var books = new ObservableCollection<Book>();

            using (var connection = new NpgsqlConnection(ConnectionString))
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
                                Isbn = reader.GetStringSafe("ISBN"),
                                Name = reader.GetStringSafe("Name"),
                                Author = reader.GetStringSafe("Author"),
                                Location = reader.GetStringSafe("Location"),
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

        // Update an existing book in the database
        public static void UpdateBook(Book book)
        {
            using (var connection = new NpgsqlConnection(ConnectionString))
            {
                connection.Open();
                string query =
                    "UPDATE Books SET Name = @name, Author = @author, Location = @location, Price = @price, Quantity = @quantity WHERE ISBN = @isbn";
                
                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("isbn", book.Isbn);
                    command.Parameters.AddWithValue("name", book.Name);
                    command.Parameters.AddWithValue("author", book.Author);
                    command.Parameters.AddWithValue("location", book.Location);
                    command.Parameters.AddWithValue("price", book.Price);
                    command.Parameters.AddWithValue("quantity", book.Quantity);
                    command.ExecuteNonQuery(); // Execute the query
                }
            }
        }

        // Delete a book by its ISBN
        public static void DeleteBook(string isbn)
        {
            using (var connection = new NpgsqlConnection(ConnectionString))
            {
                connection.Open();
                string query = "DELETE FROM Books WHERE ISBN = @isbn";

                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("isbn", isbn);
                    command.ExecuteNonQuery(); // Execute the query
                }
            }
        }

        // Validate user credentials during login
        public static bool ValidateUser(string userId, string password)
        {
            using (var connection = new NpgsqlConnection(ConnectionString))
            {
                connection.Open();
                using (var command =
                       new NpgsqlCommand("SELECT password FROM users WHERE username = @username", connection))
                {
                    command.Parameters.AddWithValue("@username", userId);
                    var storedHashedPassword = command.ExecuteScalar() as string;

                    if (storedHashedPassword == null)
                    {
                        return false; // User does not exist
                    }

                    // Verify the password using BCrypt
                    return BCrypt.Net.BCrypt.Verify(password, storedHashedPassword);
                }
            }
        }

        // Retrieve user information by user ID
        public static User GetUserInfo(string userId)
        {
            string name = string.Empty;
            string role = string.Empty;

            using (var connection = new NpgsqlConnection(ConnectionString))
            {
                connection.Open();
                using (var command =
                       new NpgsqlCommand("SELECT username, name, role FROM users WHERE username = @username",
                           connection))
                {
                    command.Parameters.AddWithValue("username", userId);

                    var reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        name = reader.GetStringSafe("name"); // Get the name
                        role = reader.GetStringSafe("role"); // Get the role
                    }
                }
            }

            return new User(userId, name, role);
        }

        // Insert a new user into the database
        public static bool InsertUser(User newUser)
        {
            try
            {
                using (var connection = new NpgsqlConnection(ConnectionString))
                {
                    connection.Open();
                    string query =
                        "INSERT INTO Users (Username, Password, Name, Role) VALUES (@username, @password, @name, @role)";
                    
                    using (var command = new NpgsqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("username", newUser.UserId);
                        command.Parameters.AddWithValue("password", newUser.Password);
                        command.Parameters.AddWithValue("name", newUser.Name);
                        command.Parameters.AddWithValue("role", newUser.Role);
                        command.ExecuteNonQuery(); // Execute the query
                    }
                }

                return true; // User successfully added
            }
            catch (Exception e)
            {
                throw new InvalidOperationException("An error occurred while inserting the user.", e);
            }
        }

        // Encrypt text using BCrypt
        public static string Encrypt(string text)
        {
            return BCrypt.Net.BCrypt.HashPassword(text);
        }

        // Extension method to safely retrieve string values from a database record
        public static string GetStringSafe(this IDataRecord reader, string columnName)
        {
            return reader[columnName] != DBNull.Value ? reader[columnName].ToString() : string.Empty;
        }
    }
}

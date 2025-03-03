using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using ContactList.model.Utilities;
using Dapper;
using Microsoft.Data.Sqlite;

namespace ContactList.model.Services
{
    // Service class to handle user authentication.
    public class AuthService
    {
        // Property to store the currently logged-in user, nullable.
        public User? CurrentUser { get; private set; }

        // Method to register a new user with a username and password.
        public bool Register(string username, string password)
        {
            try
            {
                // Establish a connection to the database.
                using var connection = DatabaseConnection.GetConnection();

                // Insert the new user into the Users table and get the inserted row ID.
                var result = connection.ExecuteScalar<int>(
                    "INSERT INTO Users(Username, Password) VALUES (@Username, @Password); SELECT last_insert_rowid();",
                    new { Username = username, Password = password });

                // Return true if the user was successfully registered.
                return result > 0;
            }
            catch (SqliteException ex)
            {
                // Handle any SQLite exceptions that occur during registration.
                Console.WriteLine($"Registration failed: {ex.Message}");
                ContactList.Services.Logger.log(ex.Message);
                return false;
                
            }
        }

        // Method to login a user with a username and password.
        public bool Login(string username, string password)
        {
            try
            {
                // Establish a connection to the database.
                using var connection = DatabaseConnection.GetConnection();

                // Query the Users table for a user with the provided username and password.
                CurrentUser = connection.QueryFirstOrDefault<User>(
                    "SELECT * FROM Users WHERE Username = @Username AND Password = @Password",
                    new { Username = username, Password = password });

                // Return true if the user was successfully logged in.
                return CurrentUser != null;
            }
            catch (Exception ex)
            {
                // Handle any exceptions that occur during login.
                Console.WriteLine($"Login Error: {ex.Message}");
                ContactList.Services.Logger.log(ex.Message);
                return false;
            }
        }


    }
}
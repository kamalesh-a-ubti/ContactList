using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.Sqlite;

namespace ContactList.model.Utilities
{
    // Class to manage the database connection and initialization.
    public class DatabaseConnection
    {
        // Connection string to the SQLite database.
        private static readonly string _connectionString = $"Data Source=contacts.db";

        // Method to get a new SQLite connection.
        public static SqliteConnection GetConnection()
        {
            return new SqliteConnection(_connectionString);
        }

        // Method to initialize the database by creating necessary tables.
        public static void IntializeDatabase()
        {
            // Establish a connection to the database.
            using var connection = GetConnection();

            // Open the database connection.
            connection.Open();

            // Create the Users table if it does not exist.
            connection.Execute(@"
                CREATE TABLE IF NOT EXISTS Users(
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Username TEXT UNIQUE NOT NULL,
                Password TEXT NOT NULL
            )");

            // Create the Contacts table if it does not exist.
            connection.Execute(@"
                CREATE TABLE IF NOT EXISTS Contacts (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                UserId INTEGER NOT NULL,
                Name TEXT NOT NULL,
                Phone TEXT,
                FOREIGN KEY(UserId) REFERENCES Users(Id)
            )");
        }
    }

}
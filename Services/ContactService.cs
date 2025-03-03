using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ContactList.model.Utilities;
using Dapper;

namespace ContactList.model.Services
{
    // Service class to handle contact management.
    public class ContactService
    {
        // Field to store the current user's ID.
        private readonly int _currentUserId;

        // Constructor to initialize the contact service with the user's ID.
        public ContactService(int userId)
        {
            _currentUserId = userId;
        }

        // Method to add a new contact.
        public bool AddContact(Contact contact)
        {
            try
            {
                // Establish a connection to the database.
                using var connection = DatabaseConnection.GetConnection();
                
                // Insert the new contact into the Contacts table.
                var result = connection.Execute(
                    "INSERT INTO Contacts (UserId, Name, Phone) VALUES (@UserId, @Name, @PhoneNo)",
                    new { UserId = _currentUserId, contact.Name, contact.PhoneNo });
                
                // Return true if the contact was successfully added.
                return result > 0;
            }
            catch (Exception ex)
            {
                // Handle any exceptions that occur during the addition of the contact.
                Console.WriteLine($"Error adding contact: {ex.Message}");
                ContactList.Services.Logger.log(ex.Message);
                return false;
            }
        }

        // Method to retrieve all contacts for the current user.
        public List<Contact> GetAllContacts()
        {
            try
            {
                // Establish a connection to the database.
                using var connection = DatabaseConnection.GetConnection();
                
                // Query the Contacts table for all contacts belonging to the current user.
                return connection.Query<Contact>(
                    "SELECT * FROM Contacts WHERE UserId = @UserId",
                    new { UserId = _currentUserId }).ToList();
            }
            catch (Exception ex)
            {
                // Handle any exceptions that occur during the retrieval of contacts.
                Console.WriteLine($"Error retrieving contacts: {ex.Message}");
                ContactList.Services.Logger.log(ex.Message);
                return new List<Contact>();
            }
        }

        // Method to update an existing contact.
        public bool UpdateContact(int contactId, Contact updatedContact)
        {
            try
            {
                // Establish a connection to the database.
                using var connection = DatabaseConnection.GetConnection();
                
                // Update the contact in the Contacts table.
                var affectedRows = connection.Execute(
                    @"UPDATE Contacts SET
                        Name = @Name,
                        Phone = @PhoneNo
                    WHERE Id = @ContactId AND UserId = @UserId",
                    new
                    {
                        ContactId = contactId,
                        UserId = _currentUserId,
                        updatedContact.Name,
                        updatedContact.PhoneNo
                    });

                // Check if the contact was found and updated.
                if (affectedRows == 0)
                {
                    Console.WriteLine("Contact not found or access denied");
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                // Handle any exceptions that occur during the update of the contact.
                Console.WriteLine($"Error updating contact: {ex.Message}");
                ContactList.Services.Logger.log(ex.Message);
                return false;
            }
        }

        // Method to delete a contact.
        public bool DeleteContact(int contactId)
        {
            try
            {
                // Establish a connection to the database.
                using var connection = DatabaseConnection.GetConnection();
                
                // Delete the contact from the Contacts table.
                var affectedRows = connection.Execute(
                    "DELETE FROM Contacts WHERE Id = @Id AND UserId = @UserId",
                    new { Id = contactId, UserId = _currentUserId });

                // Check if the contact was found and deleted.
                if (affectedRows == 0)
                {
                    Console.WriteLine("Contact not found or access denied");
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                // Handle any exceptions that occur during the deletion of the contact.
                Console.WriteLine($"Error deleting contact: {ex.Message}");
                ContactList.Services.Logger.log(ex.Message);
                return false;
            }
        }

        // Method to search for contacts by a search term.
        public List<Contact> SearchContacts(string searchTerm)
        {
            try
            {
                // Establish a connection to the database.
                using var connection = DatabaseConnection.GetConnection();
                
                // Query the Contacts table for contacts matching the search term.
                return connection.Query<Contact>(
                    @"SELECT * FROM Contacts WHERE UserId = @UserId AND 
                    (Name LIKE @SearchTerm OR Phone LIKE @SearchTerm)",
                    new
                    {
                        UserId = _currentUserId,
                        SearchTerm = $"%{searchTerm}%"
                    }).ToList();
            }
            catch (Exception ex)
            {
                // Handle any exceptions that occur during the search for contacts.
                Console.WriteLine($"Search error: {ex.Message}");
                ContactList.Services.Logger.log(ex.Message);
                return new List<Contact>();
            }
        }
    }

}
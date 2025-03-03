using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ContactList.model;
using ContactList.model.Services;

namespace ContactList.Services
{
    public class MenuOptionsServices
    {

        private static ContactService? _contactService;
        // Method to handle the flow for adding a new contact.
        public void AddContactFlow()
        {
            Console.WriteLine("=== Add New Contact ===");
            var contact = new Contact
            {
                // Get the name of the contact from the user.
                Name = GetValidInput("Name: ", s => s.Trim()),
                // Get the phone number of the contact from the user.
                PhoneNo = GetValidInput("Phone: ", s => s.Trim())
            };

            // Attempt to add the contact to the contact service.
            if (_contactService.AddContact(contact))
                Console.WriteLine("\nContact added successfully!");
            else
                Console.WriteLine("\nFailed to add contact!");
        }

        // Method to handle the flow for viewing all contacts.
        public  void ViewContactsFlow()
        {
        #pragma warning disable CS8602 // Dereference of a possibly null reference.
            // Get all contacts from the contact service.
            var contacts = _contactService.GetAllContacts();
        #pragma warning restore CS8602 // Dereference of a possibly null reference.
            Console.WriteLine("=== Your Contacts ===");

            // Check if there are any contacts to display.
            if (!contacts.Any())
            {
                Console.WriteLine("No contacts found!");
                return;
            }

            // Display each contact's details.
            foreach (var contact in contacts)
            {
                Console.WriteLine($"\nID: {contact.Id}");
                Console.WriteLine($"Name: {contact.Name}");
                Console.WriteLine($"Phone: {contact.PhoneNo}");
            }
        }
        // Method to handle the flow for updating an existing contact.
        public void UpdateContactFlow()
        {
            // Display all contacts to the user.
            ViewContactsFlow();
            
            // Get the contact ID to update from the user.
            int contactId = GetValidInput<int>("\nEnter contact ID to update: ", int.Parse);
            
            // Find the existing contact by ID.
            var existingContact = _contactService.GetAllContacts()
                .FirstOrDefault(c => c.Id == contactId);

            // If the contact ID is invalid, inform the user and return.
            if (existingContact == null)
            {
                Console.WriteLine("Invalid contact ID!");
                return;
            }

            // Get the updated contact details from the user.
            var updatedContact = new Contact
            {
                Name = GetValidInput($"Name ({existingContact.Name}): ", s =>
                    string.IsNullOrWhiteSpace(s) ? existingContact.Name : s.Trim()),
                PhoneNo = GetValidInput($"Phone ({existingContact.PhoneNo}): ", s =>
                    string.IsNullOrWhiteSpace(s) ? existingContact.PhoneNo : s.Trim())
            };

            // Attempt to update the contact in the contact service.
            if (_contactService.UpdateContact(contactId, updatedContact))
                Console.WriteLine("\nContact updated successfully!");
            else
                Console.WriteLine("\nFailed to update contact!");
        }

        // Method to handle the flow for deleting a contact.
        public void DeleteContactFlow()
        {
            // Display all contacts to the user.
            ViewContactsFlow();
            
            // Get the contact ID to delete from the user.
            int contactId = GetValidInput<int>("\nEnter contact ID to delete: ", int.Parse);

        #pragma warning disable CS8602 // Dereference of a possibly null reference.
            // Attempt to delete the contact from the contact service.
            if (_contactService.DeleteContact(contactId))
                Console.WriteLine("\nContact deleted successfully!");
            else
                Console.WriteLine("\nFailed to delete contact!");
        #pragma warning restore CS8602 // Dereference of a possibly null reference.
        }

        // Method to handle the flow for searching contacts.
        public void SearchContactFlow()
        {
            // Get the search term from the user.
            var searchTerm = GetValidInput("Enter search term: ", s => s.Trim());
            
            // Search for contacts matching the search term.
            var results = _contactService.SearchContacts(searchTerm);

            Console.WriteLine("\n=== Search Results ===");
            
            // Display the search results.
            foreach (var contact in results)
            {
                Console.WriteLine($"\n{contact.Name} - {contact.PhoneNo} ");
            }
        }

                // Generic method to get valid input from the user.
        private static T GetValidInput<T>(string prompt, Func<string, T> converter)
        {
            while (true)
            {
                try
                {
                    Console.Write(prompt);
                    var input = Console.ReadLine();
                    return converter(input);
                }
                catch
                {
                    // Inform the user of invalid input and prompt again.
                    Console.WriteLine("Invalid input. Please try again.");
                    Logger.log("Invalid input");
                }
            }
        }
    }
}
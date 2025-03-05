using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ContactList.model;
using ContactList.model.Enums;
using ContactList.model.Services;
using ContactList.Services;

namespace ContactList.handlers{
    public class MenuHandler{
        // field to hold an instance of AuthService.
        private readonly AuthService _authService;
        //field to hold an instance of ContactService.
        private readonly ContactService _contactService;

        //Constructir the give the values to the instances.

        public MenuHandler(AuthService authService, ContactService contactService)
        {
            _authService = authService;
            _contactService = contactService;
        }

        // Method to handle the main menu operations.
        public void HandleMainMenu(){
            while (true)
            {
                Console.Clear();
        #pragma warning disable CS8602 // Dereference of a possibly null reference.
                // Display the main menu with the current user's username.
                Console.WriteLine($"Main Menu [User: {_authService.CurrentUser.Username}]\n");
        #pragma warning restore CS8602 // Dereference of a possibly null reference.

                // Display the menu options.
                foreach (MenuOptions option in Enum.GetValues(typeof(MenuOptions))){
                    Console.WriteLine($"{(int)option}. {option}");
                }

                // Get a valid input choice from the user.
                var choice = GetValidInput<int>("\nEnter choice: ", int.Parse);

                // Exit the loop if the user chooses to exit.
                if (choice == (int)MenuOptions.Exit) break;

                try{
                    // Handle the selected menu choice.
                    HandleMenuChoice(choice);
                }
                catch (Exception ex){
                    // Inform the user if an operation fails.
                    Console.WriteLine($"Operation failed: {ex.Message}");
                    Console.ReadKey();
                    Logger.log(ex.Message);
                }
            }
        }
        // Method to handle the menu choice based on user input.
        private  void HandleMenuChoice(int choice){
            Console.Clear();
            switch (choice){
                case (int)MenuOptions.AddContact:
                    // Handle the flow for adding a new contact.
                    AddContactFlow();
                    break;
                case (int)MenuOptions.ViewContacts:
                    // Handle the flow for viewing all contacts.
                    ViewContactsFlow();
                    break;
                case (int)MenuOptions.UpdateContact:
                    // Handle the flow for updating an existing contact.
                    UpdateContactFlow();
                    break;
                case (int)MenuOptions.DeleteContact:
                    // Handle the flow for deleting a contact.
                    DeleteContactFlow();
                    break;
                case (int)MenuOptions.SearchContact:
                    // Handle the flow for searching a contact.
                    SearchContactFlow();
                    break;
                default:
                    // Inform the user of an invalid choice.
                    Console.WriteLine("Invalid choice!");
                    break;
            }
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }

        // Method to handle the flow for adding a new contact.
        private  void AddContactFlow(){
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
        private  void ViewContactsFlow(){
        #pragma warning disable CS8602 // Dereference of a possibly null reference.
            // Get all contacts from the contact service.
            var contacts = _contactService.GetAllContacts();
        #pragma warning restore CS8602 // Dereference of a possibly null reference.
            Console.WriteLine("=== Your Contacts ===");

            // Check if there are any contacts to display.
            if (!contacts.Any()){
                Console.WriteLine("No contacts found!");
                return;
            }

            // Display each contact's details.
            foreach (var contact in contacts){
                Console.WriteLine($"Name: {contact.Name}");
                Console.WriteLine($"Phone: {contact.PhoneNo}");
                Console.WriteLine("");
            }
        }
        // Method to handle the flow for updating an existing contact.
        private  void UpdateContactFlow(){
            // Display all contacts to the user.
            ViewContactsFlow();
            
            // Get the contact ID to update from the user.
            int contactId = GetValidInput<int>("\nEnter contact ID to update: ", int.Parse);
            
            // Find the existing contact by ID.
            var existingContact = _contactService.GetAllContacts()
                .FirstOrDefault(c => c.Id == contactId);

            // If the contact ID is invalid, inform the user and return.
            if (existingContact == null){
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
        private  void DeleteContactFlow(){
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
        private  void SearchContactFlow(){
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
        private  T GetValidInput<T>(string prompt, Func<string, T> converter){
            while (true)
            {
                try{
                    Console.Write(prompt);
                    var input = Console.ReadLine();
                    return converter(input);
                }
                catch{
                    // Inform the user of invalid input and prompt again.
                    Console.WriteLine("Invalid input. Please try again.");
                    Logger.log("Invalid");
                }
            }
        }
    }
}
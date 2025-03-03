using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ContactList.model;
using ContactList.model.Enums;
using ContactList.model.Services;
using ContactList.model.Utilities;
using ContactList.Services;

namespace ContactList{
    class Program{
        // Static field to hold an instance of AuthService, nullable.
        private static AuthService? _authService;
        // Static field to hold an instance of ContactService, nullable.
        private static ContactService? _contactService;
        private static readonly Authentication? auth;
        private static MenuOptionsServices? menu;
        // Main method, the entry point of the application.
        static void Main(string[] args)
        {
            try{
                // Initialize the database connection.
                DatabaseConnection.IntializeDatabase();
                
                // Run the main application logic.
                RunApplication();
            }
            catch (Exception ex){
                // Handle any critical errors that occur during execution.
                Console.WriteLine($"Critical error: {ex.Message}");
                Logger.log(ex.Message);

            }
        }
        // Method to run the main application logic.
        private static void RunApplication()
        {
            // Initialize the authentication service.
            _authService = new AuthService();
            
            // Handle user authentication. If authentication fails, exit the application.
            if (!HandleAuthentication())
            {
                Console.WriteLine("Exiting application...");
                return;
            }

            // Initialize the contact service with the current user's ID.
            _contactService = new ContactService(_authService.CurrentUser.Id);         
            // Handle the main menu operations.
            HandleMainMenu();
        }
        // Method to handle user authentication.
        private  static bool HandleAuthentication()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Contact List Manager\n");
                Console.WriteLine("1. Login");
                Console.WriteLine("2. Register");
                Console.WriteLine("3. Exit");
                
                // Get a valid input choice from the user.
                var choice = GetValidInput<int>("Enter choice: ", int.Parse);               
                switch (choice)
                {
                    case 1:
                        // Handle login and return true if successful.
                        if (auth.HandleLogin()) return true;
                        break;
                    case 2:
                        // Handle registration and return true if successful.
                        if (auth.HandleRegistration()) return true;
                        break;
                    case 3:
                        // Exit the application.
                        return false;
                    default:
                        // Inform the user of an invalid choice.
                        Console.WriteLine("Invalid choice!");
                        break;
                }
            }
        }
        // Method to handle the main menu operations.
        private static void HandleMainMenu()
        {
            while (true)
            {
                Console.Clear();
        #pragma warning disable CS8602 // Dereference of a possibly null reference.
                // Display the main menu with the current user's username.
                Console.WriteLine($"Main Menu [User: {_authService.CurrentUser.Username}]\n");
        #pragma warning restore CS8602 // Dereference of a possibly null reference.

                // Display the menu options.
                foreach (MenuOptions option in Enum.GetValues(typeof(MenuOptions)))
                {
                    Console.WriteLine($"{(int)option}. {option}");
                }

                // Get a valid input choice from the user.
                var choice = GetValidInput<int>("\nEnter choice: ", int.Parse);

                // Exit the loop if the user chooses to exit.
                if (choice == (int)MenuOptions.Exit) break;

                try
                {
                    // Handle the selected menu choice.
                    HandleMenuChoice(choice);
                }
                catch (Exception ex)
                {
                    // Inform the user if an operation fails.
                    Console.WriteLine($"Operation failed: {ex.Message}");
                    Console.ReadKey();
                    Logger.log(ex.Message);
                }
            }
        }
        // Method to handle the menu choice based on user input.
        private static void HandleMenuChoice(int choice)
        {
            Console.Clear();
            switch (choice)
            {
                case (int)MenuOptions.AddContact:
                    // Handle the flow for adding a new contact.
                    menu.AddContactFlow();
                    break;
                case (int)MenuOptions.ViewContacts:
                    // Handle the flow for viewing all contacts.
                    menu.ViewContactsFlow();
                    break;
                case (int)MenuOptions.UpdateContact:
                    // Handle the flow for updating an existing contact.
                    menu.UpdateContactFlow();
                    break;
                case (int)MenuOptions.DeleteContact:
                    // Handle the flow for deleting a contact.
                    menu.DeleteContactFlow();
                    break;
                case (int)MenuOptions.SearchContact:
                    // Handle the flow for searching a contact.
                    menu.SearchContactFlow();
                    break;
                default:
                    // Inform the user of an invalid choice.
                    Console.WriteLine("Invalid choice!");
                    break;
            }
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ContactList.model.Services;
using ContactList.Services;

namespace ContactList.Handler
{
    public class AuthenticationHandler
    {
        private readonly AuthService _authService;
        //Constructor used to get the authservice 
        public AuthenticationHandler(AuthService authService)
        {
            _authService = authService;
        }

        // Method to handle user authentication.
        public bool HandleAuthentication()
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
                        if ( HandleLogin()) return true;
                        break;
                    case 2:
                        // Handle registration and return true if successful.
                        if ( HandleRegistration()) return true;
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

        // Method to handle user login.
        private  bool HandleLogin()
        {
            Console.Clear();
            Console.WriteLine("=== Login ===");
            
            // Get the username and password from the user.
            var username = GetValidInput("Username: ", s => s.Trim());
            var password = GetValidInput("Password: ", s => s.Trim());

            // Attempt to login with the provided credentials.
            if ( _authService.Login(username, password))
            {
                Console.WriteLine($"\nWelcome {username}!");
                Console.ReadKey();
                return true;
            }

            // Inform the user of invalid credentials.
            Console.WriteLine("Invalid credentials!");
            Console.ReadKey();
            return false;
        }
        // Method to handle user registration.
        private bool HandleRegistration()
        {
            Console.Clear();
            Console.WriteLine("=== Registration ===");

            // Get the username from the user with validation.
            var username = GetValidInput("Username: ", s =>
            {
                var input = s.Trim();
                return input.Length >= 3 ? input : throw new Exception("Username must be at least 3 characters");
            });

            // Get the password from the user with validation.
            var password = GetValidInput("Password: ", s =>
            {
                var input = s.Trim();
                return input.Length >= 6 ? input : throw new Exception("Password must be at least 6 characters");
            });

            // Attempt to register the user with the provided credentials.
            if (_authService.Register(username, password))
            {
                Console.WriteLine("\nRegistration successful! Please login.");
                Console.ReadKey();
                return HandleLogin();
            }

            // Inform the user if registration fails.
            Console.WriteLine("Registration failed! Username might be taken.");
            Console.ReadKey();
            return false;
        }

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
                    Console.WriteLine("Invalid input. Please try again.");
                    Logger.log("Invalid");
                }
            }
        }
    }
}
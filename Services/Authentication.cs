using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ContactList.model.Services;

namespace ContactList.Services
{
    public class Authentication
    {

        AuthService auth = new AuthService();
        // Method to handle user login.
        public  bool HandleLogin()
        {
            Console.Clear();
            Console.WriteLine("=== Login ===");
            
            // Get the username and password from the user.
            var username = GetValidInput("Username: ", s => s.Trim());
            var password = GetValidInput("Password: ", s => s.Trim());

            // Attempt to login with the provided credentials.
            if (auth.Login(username, password))
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
        public bool HandleRegistration()
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
            if (auth.Register(username, password))
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
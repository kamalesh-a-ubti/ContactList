using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ContactList.handlers;
using ContactList.model;
using ContactList.model.Enums;
using ContactList.model.Services;
using ContactList.model.Utilities;
using ContactList.Services;

namespace ContactList{
    class Program{
        private static AuthService? _authService;
        private static ContactService? _contactService;

        static async Task Main(string[] args)
        {
            try
            {
                await DatabaseConnection.IntializeDatabase();
                await RunApplication();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Critical error: {ex.Message}");
                Logger.log(ex.Message);
            }
        }

        private static async Task RunApplication()
        {
            _authService = new AuthService();
            var authHandler = new AuthenticationHandler(_authService);

            if (!await authHandler.HandleAuthentication())
            {
                Console.WriteLine("Exiting application...");
                return;
            }

            _contactService = new ContactService(_authService.CurrentUser.Id);
            var menuHandler = new MenuHandler(_authService, _contactService);
            menuHandler.HandleMainMenu();
        }
       

    }
}
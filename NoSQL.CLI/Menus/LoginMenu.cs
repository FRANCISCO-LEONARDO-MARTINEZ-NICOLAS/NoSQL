using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using NoSQL.Application.Services.Interfaces;

namespace NoSQL.CLI.Menus
{
    public class LoginMenu : BaseMenu
    {
        private new string _currentUserEmail;
        private new string _currentUserRole;

        public LoginMenu(IServiceProvider serviceProvider) 
            : base(serviceProvider, null, null)
        {
        }

        public override async Task ShowAsync()
        {
            ClearScreen();
            ShowHeader("Inicio de Sesión");

            string email = GetUserInput("Correo: ");
            string password = GetUserInput("Contraseña: ");

            Console.WriteLine("\nSeleccione su rol:");
            Console.WriteLine("1. Administrador");
            Console.WriteLine("2. Optometrista");
            string roleOption = GetUserInput("Opción: ");

            string role = roleOption == "1" ? "Admin" : "Optometrista";

            var authService = _serviceProvider.GetRequiredService<IAuthService>();
            var loginResult = await authService.LoginAsync(email, password, role);

            if (loginResult.Success)
            {
                _currentUserEmail = email;
                _currentUserRole = role;
                ShowSuccess($"Bienvenido {email} ({role})");
            }
            else
            {
                ShowError(loginResult.Message);
            }

            ShowFooter();
        }

        public (string Email, string Role) GetCurrentUser()
        {
            return (_currentUserEmail, _currentUserRole);
        }
    }
} 
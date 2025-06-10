using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using NoSQL.CLI.Services;
using NoSQL.Domain.Entities;
using NoSQL.Application.Interfaces;
using NoSQL.CLI.Services.Adapters;

namespace NoSQL.CLI.Menus
{
    public class LoginMenu
    {
        private readonly IAuthService _authService;

        public LoginMenu(IAuthService authService)
        {
            _authService = authService;
        }

        public async Task<(bool Success, Usuario? User, string? Token)> ShowAsync()
        {
            Console.Clear();
            Console.WriteLine("=== INICIO DE SESIÓN ===");
            
            Console.Write("Email: ");
            var email = Console.ReadLine() ?? string.Empty;
            email = email.Trim().ToLower(); // <-- Normaliza el correo a minúsculas y sin espacios
            
            Console.Write("Contraseña: ");
            var password = ReadPassword();
            
            Console.Write("Rol (Admin/Optometrista): ");
            var rol = Console.ReadLine()?.ToLower() ?? string.Empty;

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(rol))
            {
                Console.WriteLine("Error: Todos los campos son requeridos.");
                return (false, null, null);
            }

            if (rol != "admin" && rol != "optometrista")
            {
                Console.WriteLine("Error: Rol inválido. Debe ser 'Admin' u 'Optometrista'.");
                return (false, null, null);
            }

            var (success, message, user, token) = await _authService.LoginAsync(email, password, rol);
            
            if (!success)
            {
                Console.WriteLine($"Error: {message}");
                return (false, null, null);
            }

            Console.WriteLine($"Bienvenido, {user?.Nombre}!");
            return (true, user, token);
        }

        private string ReadPassword()
        {
            var password = new System.Text.StringBuilder();
            while (true)
            {
                var keyInfo = Console.ReadKey(true);
                if (keyInfo.Key == ConsoleKey.Enter)
                    break;
                if (keyInfo.Key == ConsoleKey.Backspace)
                {
                    if (password.Length > 0)
                    {
                        password.Length--;
                        Console.Write("\b \b");
                    }
                }
                else
                {
                    password.Append(keyInfo.KeyChar);
                    Console.Write("*");
                }
            }
            Console.WriteLine();
            return password.ToString();
        }
    }
}
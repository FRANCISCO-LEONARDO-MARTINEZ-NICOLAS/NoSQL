using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace NoSQL.CLI.Menus
{
    public abstract class BaseMenu
    {
        protected readonly IServiceProvider _serviceProvider;
        protected string _currentUserEmail;
        protected string _currentUserRole;

        protected BaseMenu(IServiceProvider serviceProvider, string currentUserEmail, string currentUserRole)
        {
            _serviceProvider = serviceProvider;
            _currentUserEmail = currentUserEmail;
            _currentUserRole = currentUserRole;
        }

        protected void ClearScreen()
        {
            Console.Clear();
        }

        protected void ShowHeader(string title)
        {
            Console.WriteLine($"\n=== {title} ===");
        }

        protected void ShowFooter()
        {
            Console.WriteLine("\nPresione cualquier tecla para continuar...");
            Console.ReadKey();
        }

        protected string GetUserInput(string prompt)
        {
            Console.Write(prompt);
            return Console.ReadLine()?.Trim() ?? string.Empty;
        }

        protected void ShowError(string message)
        {
            Console.WriteLine($"\nError: {message}");
        }

        protected void ShowSuccess(string message)
        {
            Console.WriteLine($"\nÉxito: {message}");
        }

        public abstract Task ShowAsync();
    }
} 
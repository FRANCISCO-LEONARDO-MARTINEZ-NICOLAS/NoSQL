using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace NoSQL.CLI.Menus
{
    public class AdminMenu : BaseMenu
    {
        public AdminMenu(IServiceProvider serviceProvider, string currentUserEmail, string currentUserRole)
            : base(serviceProvider, currentUserEmail, currentUserRole)
        {
        }

        public override async Task ShowAsync()
        {
            while (true)
            {
                ClearScreen();
                ShowHeader("Panel de Administración");
                Console.WriteLine("1. Gestión de Optometristas");
                Console.WriteLine("2. Configuración del Sistema");
                Console.WriteLine("3. Monitoreo de Actividades");
                Console.WriteLine("4. Generación de Reportes");
                Console.WriteLine("5. Cerrar Sesión");
                Console.Write("\nSeleccione una opción: ");

                string option = Console.ReadLine();

                switch (option)
                {
                    case "1":
                        await ShowOptometristaManagementMenu();
                        break;
                    case "2":
                        await ShowSystemConfigMenu();
                        break;
                    case "3":
                        await ShowMonitoringMenu();
                        break;
                    case "4":
                        await ShowReportsMenu();
                        break;
                    case "5":
                        _currentUserEmail = null;
                        _currentUserRole = null;
                        return;
                }
            }
        }

        private async Task ShowOptometristaManagementMenu()
        {
            ClearScreen();
            ShowHeader("Gestión de Optometristas");
            Console.WriteLine("1. Agregar Optometrista");
            Console.WriteLine("2. Eliminar Optometrista");
            Console.WriteLine("3. Modificar Optometrista");
            Console.WriteLine("4. Consultar Optometristas");
            Console.WriteLine("5. Volver al menú principal");
            Console.Write("\nSeleccione una opción: ");

            string option = Console.ReadLine();

            var optometristaMenu = new OptometristaManagementMenu(_serviceProvider, _currentUserEmail, _currentUserRole);
            await optometristaMenu.HandleOptionAsync(option);
        }

        private async Task ShowSystemConfigMenu()
        {
            ClearScreen();
            ShowHeader("Configuración del Sistema");
            // Implementar menú de configuración
            ShowFooter();
        }

        private async Task ShowMonitoringMenu()
        {
            ClearScreen();
            ShowHeader("Monitoreo de Actividades");
            // Implementar menú de monitoreo
            ShowFooter();
        }

        private async Task ShowReportsMenu()
        {
            ClearScreen();
            ShowHeader("Generación de Reportes");
            // Implementar menú de reportes
            ShowFooter();
        }
    }
} 
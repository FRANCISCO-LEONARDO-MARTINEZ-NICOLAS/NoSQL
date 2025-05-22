using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace NoSQL.CLI.Menus
{
    public class OptometristaMenu : BaseMenu
    {
        public OptometristaMenu(IServiceProvider serviceProvider, string currentUserEmail, string currentUserRole)
            : base(serviceProvider, currentUserEmail, currentUserRole)
        {
        }

        public override async Task ShowAsync()
        {
            while (true)
            {
                ClearScreen();
                ShowHeader("Panel de Optometrista");
                Console.WriteLine("1. Gestión de Pacientes");
                Console.WriteLine("2. Realización de Consultas");
                Console.WriteLine("3. Venta de Productos");
                Console.WriteLine("4. Programación de Citas");
                Console.WriteLine("5. Historial Clínico");
                Console.WriteLine("6. Cerrar Sesión");
                Console.Write("\nSeleccione una opción: ");

                string option = Console.ReadLine();

                switch (option)
                {
                    case "1":
                        await ShowPatientManagementMenu();
                        break;
                    case "2":
                        await ShowConsultationMenu();
                        break;
                    case "3":
                        await ShowProductSalesMenu();
                        break;
                    case "4":
                        await ShowAppointmentMenu();
                        break;
                    case "5":
                        await ShowClinicalHistoryMenu();
                        break;
                    case "6":
                        _currentUserEmail = null;
                        _currentUserRole = null;
                        return;
                }
            }
        }

        private async Task ShowPatientManagementMenu()
        {
            ClearScreen();
            ShowHeader("Gestión de Pacientes");
            Console.WriteLine("1. Registrar Nuevo Paciente");
            Console.WriteLine("2. Buscar Paciente");
            Console.WriteLine("3. Ver Historial de Paciente");
            Console.WriteLine("4. Volver al menú principal");
            Console.Write("\nSeleccione una opción: ");

            string option = Console.ReadLine();

            var patientMenu = new PatientManagementMenu(_serviceProvider, _currentUserEmail, _currentUserRole);
            await patientMenu.HandleOptionAsync(option);
        }

        private async Task ShowConsultationMenu()
        {
            ClearScreen();
            ShowHeader("Realización de Consultas");
            Console.WriteLine("1. Nueva Consulta");
            Console.WriteLine("2. Ver Consultas Anteriores");
            Console.WriteLine("3. Volver al menú principal");
            Console.Write("\nSeleccione una opción: ");

            string option = Console.ReadLine();

            var consultationMenu = new ConsultationMenu(_serviceProvider, _currentUserEmail, _currentUserRole);
            await consultationMenu.HandleOptionAsync(option);
        }

        private async Task ShowProductSalesMenu()
        {
            ClearScreen();
            ShowHeader("Venta de Productos");
            Console.WriteLine("1. Nueva Venta");
            Console.WriteLine("2. Ver Ventas Realizadas");
            Console.WriteLine("3. Marcar Producto como Entregado");
            Console.WriteLine("4. Volver al menú principal");
            Console.Write("\nSeleccione una opción: ");

            string option = Console.ReadLine();

            var productMenu = new ProductSalesMenu(_serviceProvider, _currentUserEmail, _currentUserRole);
            await productMenu.HandleOptionAsync(option);
        }

        private async Task ShowAppointmentMenu()
        {
            ClearScreen();
            ShowHeader("Programación de Citas");
            Console.WriteLine("1. Agendar Cita de Seguimiento");
            Console.WriteLine("2. Agendar Cita de Entrega");
            Console.WriteLine("3. Ver Citas Programadas");
            Console.WriteLine("4. Volver al menú principal");
            Console.Write("\nSeleccione una opción: ");

            string option = Console.ReadLine();

            var appointmentMenu = new AppointmentMenu(_serviceProvider, _currentUserEmail, _currentUserRole);
            await appointmentMenu.HandleOptionAsync(option);
        }

        private async Task ShowClinicalHistoryMenu()
        {
            ClearScreen();
            ShowHeader("Historial Clínico");
            // Implementar menú de historial clínico
            ShowFooter();
        }
    }
} 
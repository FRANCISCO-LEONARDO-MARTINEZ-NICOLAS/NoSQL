using System;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using NoSQL.Application.Interfaces;
using NoSQL.CLI.Services.Adapters;
using NoSQL.Domain.Entities;

namespace NoSQL.CLI.Menus
{
    public class OptometristaMenu : BaseMenu
    {
        private readonly IOptometristaService _optometristaService;
        private readonly IPacienteService _pacienteService;
        private readonly IConsultaService _consultaService;
        private readonly IProductoService _productoService;
        private readonly ICitaService _citaService;
        private readonly string _userEmail;
        private readonly string _userRole;

        public OptometristaMenu(
            IOptometristaService optometristaService,
            IPacienteService pacienteService,
            IConsultaService consultaService,
            IProductoService productoService,
            ICitaService citaService,
            string userEmail,
            string userRole)
        {
            _optometristaService = optometristaService;
            _pacienteService = pacienteService;
            _consultaService = consultaService;
            _productoService = productoService;
            _citaService = citaService;
            _userEmail = userEmail;
            _userRole = userRole;
        }

        public override async Task ShowAsync()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== Panel de Optometrista ===\n");
                Console.WriteLine("1. Gestión de Pacientes");
                Console.WriteLine("2. Realización de Consultas");
                Console.WriteLine("3. Venta de Productos");
                Console.WriteLine("4. Programación de Citas");
                Console.WriteLine("5. Historial Clínico");
                Console.WriteLine("0. Cerrar sesión");

                var opcion = Console.ReadLine()?.Trim();

                switch (opcion)
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
                    case "0":
                        return;
                    default:
                        Console.WriteLine("\nOpción no válida. Presione cualquier tecla para continuar...");
                        Console.ReadKey();
                        break;
                }
            }
        }

        private async Task ShowPatientManagementMenu()
        {
            var patientMenu = new PatientManagementMenu(_pacienteService, _userEmail, _userRole);
            await patientMenu.ShowAsync();
        }

        private async Task ShowConsultationMenu()
        {
            var consultationMenu = new ConsultationMenu(
                _consultaService,
                _pacienteService,
                _optometristaService,
                _userEmail,
                _userRole);
            await consultationMenu.ShowAsync();
        }

        private async Task ShowProductSalesMenu()
        {
            var productMenu = new ProductSalesMenu(_productoService, _pacienteService, _userEmail, _userRole);
            await productMenu.ShowAsync();
        }

        private async Task ShowAppointmentMenu()
        {
            var appointmentMenu = new AppointmentMenu(_citaService, _pacienteService, _optometristaService, _userEmail, _userRole);
            await appointmentMenu.ShowAsync();
        }

        private async Task ShowClinicalHistoryMenu()
        {
            Console.Clear();
            Console.WriteLine("=== Historial Clínico ===\n");

            Console.Write("Correo del paciente: ");
            var pacienteEmail = Console.ReadLine()?.Trim();
            if (string.IsNullOrEmpty(pacienteEmail))
            {
                Console.WriteLine("\nCorreo de paciente requerido. Presione cualquier tecla para continuar...");
                Console.ReadKey();
                return;
            }

            var paciente = (await _pacienteService.GetAllAsync()).FirstOrDefault(p => p.Correo == pacienteEmail);
            if (paciente == null)
            {
                Console.WriteLine("\nPaciente no encontrado. Presione cualquier tecla para continuar...");
                Console.ReadKey();
                return;
            }

            Console.WriteLine($"\nHistorial clínico de {paciente.Nombre} {paciente.Apellido}:\n");

            var consultas = (await _consultaService.GetAllAsync()).Where(c => c.PacienteEmail == pacienteEmail).ToList();
            if (!consultas.Any())
            {
                Console.WriteLine("No hay consultas registradas.");
            }
            else
            {
                foreach (var consulta in consultas.OrderByDescending(c => c.Fecha))
                {
                    Console.WriteLine($"Fecha: {consulta.Fecha:dd/MM/yyyy HH:mm}");
                    Console.WriteLine($"Optometrista: {consulta.OptometristaEmail}");
                    Console.WriteLine($"Motivo: {consulta.Motivo}");
                    Console.WriteLine($"Diagnóstico: {consulta.Diagnostico}");
                    Console.WriteLine($"Tratamiento: {consulta.Tratamiento}");
                    Console.WriteLine("------------------------");
                }
            }

            var citas = (await _citaService.GetAllAsync()).Where(c => c.PacienteEmail == pacienteEmail).ToList();
            if (!citas.Any())
            {
                Console.WriteLine("\nNo hay citas registradas.");
            }
            else
            {
                Console.WriteLine("\nCitas programadas:");
                foreach (var cita in citas.OrderBy(c => c.FechaHora))
                {
                    Console.WriteLine($"Fecha: {cita.FechaHora:dd/MM/yyyy HH:mm}");
                    Console.WriteLine($"Tipo: {cita.Tipo}");
                    Console.WriteLine($"Motivo: {cita.Motivo}");
                    Console.WriteLine($"Estado: {cita.Estado}");
                    Console.WriteLine("------------------------");
                }
            }

            var productos = (await _productoService.GetAllAsync()).Where(p => p.PacienteEmail == pacienteEmail).ToList();
            if (!productos.Any())
            {
                Console.WriteLine("\nNo hay productos registrados.");
            }
            else
            {
                Console.WriteLine("\nProductos adquiridos:");
                foreach (var producto in productos.OrderByDescending(p => p.FechaVenta))
                {
                    Console.WriteLine($"Producto: {producto.Nombre}");
                    Console.WriteLine($"Tipo: {producto.Tipo}");
                    Console.WriteLine($"Fecha de venta: {producto.FechaVenta:dd/MM/yyyy}");
                    Console.WriteLine($"Fecha de entrega estimada: {producto.FechaEntregaEstimada:dd/MM/yyyy}");
                    Console.WriteLine($"Estado: {producto.Estado}");
                    Console.WriteLine("------------------------");
                }
            }

            Console.WriteLine("\nPresione cualquier tecla para continuar...");
            Console.ReadKey();
        }
    }
}
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using NoSQL.Application.Services.Interfaces;
using NoSQL.Domain.Entities;

namespace NoSQL.CLI.Menus
{
    public class AppointmentMenu : BaseMenu
    {
        private readonly ICitaService _citaService;
        private readonly IPacienteService _pacienteService;
        private readonly IProductoService _productoService;

        public AppointmentMenu(IServiceProvider serviceProvider, string currentUserEmail, string currentUserRole)
            : base(serviceProvider, currentUserEmail, currentUserRole)
        {
            _citaService = serviceProvider.GetRequiredService<ICitaService>();
            _pacienteService = serviceProvider.GetRequiredService<IPacienteService>();
            _productoService = serviceProvider.GetRequiredService<IProductoService>();
        }

        public async Task HandleOptionAsync(string option)
        {
            switch (option)
            {
                case "1":
                    await ScheduleFollowUpAppointment();
                    break;
                case "2":
                    await ScheduleDeliveryAppointment();
                    break;
                case "3":
                    await ViewScheduledAppointments();
                    break;
            }
        }

        private async Task ScheduleFollowUpAppointment()
        {
            ClearScreen();
            ShowHeader("Agendar Cita de Seguimiento");

            string pacienteEmail = GetUserInput("Correo del paciente: ");
            var paciente = await _pacienteService.GetByEmailAsync(pacienteEmail);
            
            if (paciente == null)
            {
                ShowError("Paciente no encontrado");
                ShowFooter();
                return;
            }

            Console.WriteLine($"\nPaciente: {paciente.Nombre}");
            Console.WriteLine("------------------------");

            var fechaHora = DateTime.Parse(GetUserInput("Fecha y hora (dd/MM/yyyy HH:mm): "));
            var motivo = GetUserInput("Motivo de la cita: ");

            var optometrista = await _serviceProvider.GetRequiredService<IOptometristaService>().GetByEmailAsync(_currentUserEmail);
            var cita = new Cita
            {
                PacienteEmail = pacienteEmail,
                OptometristaEmail = _currentUserEmail,
                PacienteId = paciente.Id,
                OptometristaId = optometrista?.Id ?? Guid.Empty,
                FechaHora = fechaHora,
                Tipo = "Seguimiento",
                Motivo = motivo,
                Estado = "Programada"
            };

            var result = await _citaService.CreateAsync(cita);
            if (result.Success)
            {
                ShowSuccess(result.Message);
            }
            else
            {
                ShowError(result.Message);
            }

            ShowFooter();
        }

        private async Task ScheduleDeliveryAppointment()
        {
            ClearScreen();
            ShowHeader("Agendar Cita de Entrega");

            string pacienteEmail = GetUserInput("Correo del paciente: ");
            var paciente = await _pacienteService.GetByEmailAsync(pacienteEmail);
            if (paciente == null)
            {
                ShowError("Paciente no encontrado");
                ShowFooter();
                return;
            }

            var productos = await _productoService.GetByPacienteEmailAsync(pacienteEmail);
            var productosPendientes = productos?.Where(p => p.Estado == "Listo para entrega").ToList();

            if (productosPendientes == null || !productosPendientes.Any())
            {
                ShowError("No hay productos listos para entrega para este paciente");
                ShowFooter();
                return;
            }

            Console.WriteLine($"\nPaciente: {paciente.Nombre}");
            Console.WriteLine("Productos listos para entrega:");
            foreach (var producto in productosPendientes)
            {
                Console.WriteLine($"- {producto.Tipo}: {producto.Descripcion}");
            }
            Console.WriteLine("------------------------");

            var fechaHora = DateTime.Parse(GetUserInput("Fecha y hora (dd/MM/yyyy HH:mm): "));

            var optometrista = await _serviceProvider.GetRequiredService<IOptometristaService>().GetByEmailAsync(_currentUserEmail);
            var cita = new Cita
            {
                PacienteEmail = pacienteEmail,
                OptometristaEmail = _currentUserEmail,
                PacienteId = paciente.Id,
                OptometristaId = optometrista?.Id ?? Guid.Empty,
                FechaHora = fechaHora,
                Tipo = "Entrega",
                Motivo = "Entrega de productos",
                Estado = "Programada"
            };

            var result = await _citaService.CreateAsync(cita);
            if (result.Success)
            {
                ShowSuccess(result.Message);
            }
            else
            {
                ShowError(result.Message);
            }

            ShowFooter();
        }

        private async Task ViewScheduledAppointments()
        {
            ClearScreen();
            ShowHeader("Citas Programadas");

            Console.WriteLine("1. Ver todas mis citas");
            Console.WriteLine("2. Buscar por paciente");
            string option = GetUserInput("\nSeleccione una opción: ");

            IEnumerable<Cita> citas;
            if (option == "1")
            {
                citas = await _citaService.GetByOptometristaEmailAsync(_currentUserEmail);
            }
            else
            {
                string pacienteEmail = GetUserInput("\nCorreo del paciente: ");
                citas = await _citaService.GetByPacienteEmailAsync(pacienteEmail);
            }

            if (citas != null && citas.Any())
            {
                foreach (var cita in citas.OrderBy(c => c.FechaHora))
                {
                    Console.WriteLine("\n----------------------------------------");
                    Console.WriteLine($"Fecha y hora: {cita.FechaHora:dd/MM/yyyy HH:mm}");
                    Console.WriteLine($"Paciente: {cita.PacienteEmail}");
                    Console.WriteLine($"Tipo: {cita.Tipo}");
                    Console.WriteLine($"Motivo: {cita.Motivo}");
                    Console.WriteLine($"Estado: {cita.Estado}");
                    Console.WriteLine("----------------------------------------");
                }
            }
            else
            {
                Console.WriteLine("\nNo se encontraron citas programadas.");
            }

            ShowFooter();
        }

        public override async Task ShowAsync()
        {
            while (true)
            {
                ClearScreen();
                ShowHeader("Gestión de Citas");
                Console.WriteLine("1. Agendar Cita de Seguimiento");
                Console.WriteLine("2. Agendar Cita de Entrega");
                Console.WriteLine("3. Ver Citas Programadas");
                Console.WriteLine("4. Volver al menú principal");
                Console.Write("\nSeleccione una opción: ");

                string option = Console.ReadLine();
                if (option == "4") break;

                await HandleOptionAsync(option);
            }
        }
    }
} 
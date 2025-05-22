using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using NoSQL.Application.Services.Interfaces;
using NoSQL.Domain.Entities;

namespace NoSQL.CLI.Menus
{
    public class PatientManagementMenu : BaseMenu
    {
        private readonly IPacienteService _pacienteService;
        private readonly IConsultaService _consultaService;

        public PatientManagementMenu(IServiceProvider serviceProvider, string currentUserEmail, string currentUserRole)
            : base(serviceProvider, currentUserEmail, currentUserRole)
        {
            _pacienteService = serviceProvider.GetRequiredService<IPacienteService>();
            _consultaService = serviceProvider.GetRequiredService<IConsultaService>();
        }

        public async Task HandleOptionAsync(string option)
        {
            switch (option)
            {
                case "1":
                    await RegisterPatient();
                    break;
                case "2":
                    await QueryPatient();
                    break;
                case "3":
                    await ViewPatientHistory();
                    break;
            }
        }

        private async Task RegisterPatient()
        {
            ClearScreen();
            ShowHeader("Registrar Paciente");

            var paciente = new Paciente
            {
                Nombre = GetUserInput("Nombre: "),
                Apellido = GetUserInput("Apellido: "),
                FechaNacimiento = DateTime.Parse(GetUserInput("Fecha de Nacimiento (dd/MM/yyyy): ")),
                Genero = GetUserInput("Género (M/F): "),
                Direccion = GetUserInput("Dirección: "),
                Telefono = GetUserInput("Teléfono: "),
                Correo = GetUserInput("Correo: "),
                Dni = GetUserInput("DNI: ")
            };

            var result = await _pacienteService.CreateAsync(paciente);
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

        private async Task QueryPatient()
        {
            ClearScreen();
            ShowHeader("Consultar Paciente");

            string email = GetUserInput("Correo del paciente: ");
            var paciente = await _pacienteService.GetByEmailAsync(email);

            if (paciente != null)
            {
                Console.WriteLine($"\nNombre: {paciente.Nombre}");
                Console.WriteLine($"Fecha de Nacimiento: {paciente.FechaNacimiento:dd/MM/yyyy}");
                Console.WriteLine($"Género: {paciente.Genero}");
                Console.WriteLine($"Dirección: {paciente.Direccion}");
                Console.WriteLine($"Teléfono: {paciente.Telefono}");
                Console.WriteLine($"Correo: {paciente.Correo}");
                Console.WriteLine($"DNI: {paciente.Dni}");
            }
            else
            {
                ShowError("Paciente no encontrado");
            }

            ShowFooter();
        }

        private async Task ViewPatientHistory()
        {
            ClearScreen();
            ShowHeader("Historial del Paciente");

            string email = GetUserInput("Correo del paciente: ");
            var paciente = await _pacienteService.GetByEmailAsync(email);
            
            if (paciente == null)
            {
                ShowError("Paciente no encontrado");
                ShowFooter();
                return;
            }

            var consultas = await _consultaService.GetByPacienteEmailAsync(email);
            
            Console.WriteLine($"\nHistorial de consultas para: {paciente.Nombre}");
            Console.WriteLine("----------------------------------------");

            if (consultas != null && consultas.Any())
            {
                foreach (var consulta in consultas.OrderByDescending(c => c.Fecha))
                {
                    Console.WriteLine($"\nFecha: {consulta.Fecha:dd/MM/yyyy HH:mm}");
                    Console.WriteLine($"Optometrista: {consulta.OptometristaEmail}");
                    Console.WriteLine($"Diagnóstico: {consulta.Diagnostico}");
                    Console.WriteLine($"Tratamiento: {consulta.Tratamiento}");
                    Console.WriteLine("----------------------------------------");
                }
            }
            else
            {
                Console.WriteLine("\nNo hay consultas registradas para este paciente.");
            }

            ShowFooter();
        }

        public override async Task ShowAsync()
        {
            while (true)
            {
                ClearScreen();
                ShowHeader("Gestión de Pacientes");
                Console.WriteLine("1. Registrar Nuevo Paciente");
                Console.WriteLine("2. Buscar Paciente");
                Console.WriteLine("3. Ver Historial de Paciente");
                Console.WriteLine("4. Volver al menú principal");
                Console.Write("\nSeleccione una opción: ");

                string option = Console.ReadLine();
                if (option == "4") break;

                await HandleOptionAsync(option);
            }
        }
    }
} 
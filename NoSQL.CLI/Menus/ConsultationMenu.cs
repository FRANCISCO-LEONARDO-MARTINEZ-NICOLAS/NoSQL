using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using NoSQL.Application.Services.Interfaces;
using NoSQL.Domain.Entities;

namespace NoSQL.CLI.Menus
{
    public class ConsultationMenu : BaseMenu
    {
        private readonly IConsultaService _consultaService;
        private readonly IPacienteService _pacienteService;

        public ConsultationMenu(IServiceProvider serviceProvider, string currentUserEmail, string currentUserRole)
            : base(serviceProvider, currentUserEmail, currentUserRole)
        {
            _consultaService = serviceProvider.GetRequiredService<IConsultaService>();
            _pacienteService = serviceProvider.GetRequiredService<IPacienteService>();
        }

        public async Task HandleOptionAsync(string option)
        {
            switch (option)
            {
                case "1":
                    await CreateConsultation();
                    break;
                case "2":
                    await ViewPreviousConsultations();
                    break;
            }
        }

        private async Task CreateConsultation()
        {
            ClearScreen();
            ShowHeader("Nueva Consulta");

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

            var optometrista = await _serviceProvider.GetRequiredService<IOptometristaService>().GetByEmailAsync(_currentUserEmail);
            var consulta = new Consulta
            {
                PacienteEmail = pacienteEmail,
                OptometristaEmail = _currentUserEmail,
                PacienteId = paciente.Id,
                OptometristaId = optometrista?.Id ?? Guid.Empty,
                Fecha = DateTime.Now,
                Motivo = GetUserInput("Motivo de la consulta: "),
                Sintomas = GetUserInput("Síntomas: "),
                Diagnostico = GetUserInput("Diagnóstico: "),
                Tratamiento = GetUserInput("Tratamiento: "),
                Observaciones = GetUserInput("Observaciones: "),
                Recomendaciones = GetUserInput("Recomendaciones: ")
            };

            var result = await _consultaService.CreateAsync(consulta);
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

        private async Task ViewPreviousConsultations()
        {
            ClearScreen();
            ShowHeader("Consultas Anteriores");

            Console.WriteLine("1. Ver todas mis consultas");
            Console.WriteLine("2. Buscar por paciente");
            string option = GetUserInput("\nSeleccione una opción: ");

            IEnumerable<Consulta> consultas;
            if (option == "1")
            {
                consultas = await _consultaService.GetByOptometristaEmailAsync(_currentUserEmail);
            }
            else
            {
                string pacienteEmail = GetUserInput("\nCorreo del paciente: ");
                consultas = await _consultaService.GetByPacienteEmailAsync(pacienteEmail);
            }

            if (consultas != null && consultas.Any())
            {
                foreach (var consulta in consultas.OrderByDescending(c => c.Fecha))
                {
                    Console.WriteLine("\n----------------------------------------");
                    Console.WriteLine($"Fecha: {consulta.Fecha:dd/MM/yyyy HH:mm}");
                    Console.WriteLine($"Paciente: {consulta.PacienteEmail}");
                    Console.WriteLine($"Motivo: {consulta.Motivo}");
                    Console.WriteLine($"Diagnóstico: {consulta.Diagnostico}");
                    Console.WriteLine($"Tratamiento: {consulta.Tratamiento}");
                    if (!string.IsNullOrWhiteSpace(consulta.Observaciones))
                    {
                        Console.WriteLine($"Observaciones: {consulta.Observaciones}");
                    }
                    Console.WriteLine("----------------------------------------");
                }
            }
            else
            {
                Console.WriteLine("\nNo se encontraron consultas.");
            }

            ShowFooter();
        }

        public override async Task ShowAsync()
        {
            while (true)
            {
                ClearScreen();
                ShowHeader("Gestión de Consultas");
                Console.WriteLine("1. Nueva Consulta");
                Console.WriteLine("2. Ver Consultas Anteriores");
                Console.WriteLine("3. Volver al menú principal");
                Console.Write("\nSeleccione una opción: ");

                string option = Console.ReadLine();
                if (option == "3") break;

                await HandleOptionAsync(option);
            }
        }
    }
} 
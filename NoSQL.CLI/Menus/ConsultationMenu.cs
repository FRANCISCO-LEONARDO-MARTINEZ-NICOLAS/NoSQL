using System;
using System.Threading.Tasks;
using System.Linq;
using NoSQL.Domain.Entities;
using NoSQL.Application.Interfaces;

namespace NoSQL.CLI.Menus
{
    public class ConsultationMenu : BaseMenu
    {
        private readonly IConsultaService _consultaService;
        private readonly IPacienteService _pacienteService;
        private readonly IOptometristaService _optometristaService;
        private readonly string _usercorreo;
        private readonly string _userRole;

        public ConsultationMenu(
            IConsultaService consultaService,
            IPacienteService pacienteService,
            IOptometristaService optometristaService,
            string usercorreo,
            string userRole)
        {
            _consultaService = consultaService;
            _pacienteService = pacienteService;
            _optometristaService = optometristaService;
            _usercorreo = usercorreo;
            _userRole = userRole;
        }

        public override async Task ShowAsync()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== Gestión de Consultas ===\n");
                Console.WriteLine("1. Ver todas las consultas");
                Console.WriteLine("2. Ver consultas por fecha");
                Console.WriteLine("3. Registrar nueva consulta");
                Console.WriteLine("4. Actualizar consulta");
                Console.WriteLine("0. Volver al menú principal");

                var opcion = Console.ReadLine()?.Trim();

                switch (opcion)
                {
                    case "1":
                        await MostrarTodasLasConsultasAsync();
                        break;
                    case "2":
                        await MostrarConsultasPorFechaAsync();
                        break;
                    case "3":
                        await RegistrarConsultaAsync();
                        break;
                    case "4":
                        await ActualizarConsultaAsync();
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

        private async Task MostrarTodasLasConsultasAsync()
        {
            Console.Clear();
            Console.WriteLine("=== Todas las Consultas ===\n");

            var consultas = await _consultaService.GetAllAsync();

            if (_userRole == "Paciente")
                consultas = consultas.Where(c => c.Pacientecorreo == _usercorreo).ToList();
            else if (_userRole == "Optometrista")
                consultas = consultas.Where(c => c.Optometristacorreo == _usercorreo).ToList();

            if (!consultas.Any())
            {
                Console.WriteLine("No hay consultas registradas.");
            }
            else
            {
                foreach (var consulta in consultas)
                {
                    Console.WriteLine($"ID: {consulta.Id}");
                    Console.WriteLine($"Paciente: {consulta.Pacientecorreo}");
                    Console.WriteLine($"Optometrista: {consulta.Optometristacorreo}");
                    Console.WriteLine($"Fecha: {consulta.Fecha}");
                    Console.WriteLine($"Motivo: {consulta.Motivo}");
                    Console.WriteLine($"Diagnóstico: {consulta.Diagnostico}");
                    Console.WriteLine($"Tratamiento: {consulta.Tratamiento}");
                    Console.WriteLine("------------------------");
                }
            }

            Console.WriteLine("\nPresione cualquier tecla para continuar...");
            Console.ReadKey();
        }

        private async Task MostrarConsultasPorFechaAsync()
        {
            Console.Clear();
            Console.WriteLine("=== Consultas por Fecha ===\n");

            Console.Write("Ingrese la fecha (dd/MM/yyyy): ");
            if (!DateTime.TryParse(Console.ReadLine(), out var fecha))
            {
                Console.WriteLine("\nFecha inválida. Presione cualquier tecla para continuar...");
                Console.ReadKey();
                return;
            }

            var consultas = await _consultaService.GetAllAsync();
            var filtradas = consultas.Where(c => c.Fecha.Date == fecha.Date).ToList();

            if (!filtradas.Any())
            {
                Console.WriteLine("No hay consultas para la fecha especificada.");
            }
            else
            {
                foreach (var consulta in filtradas)
                {
                    Console.WriteLine($"ID: {consulta.Id}");
                    Console.WriteLine($"Paciente: {consulta.Pacientecorreo}");
                    Console.WriteLine($"Optometrista: {consulta.Optometristacorreo}");
                    Console.WriteLine($"Fecha: {consulta.Fecha}");
                    Console.WriteLine($"Motivo: {consulta.Motivo}");
                    Console.WriteLine($"Diagnóstico: {consulta.Diagnostico}");
                    Console.WriteLine($"Tratamiento: {consulta.Tratamiento}");
                    Console.WriteLine("------------------------");
                }
            }

            Console.WriteLine("\nPresione cualquier tecla para continuar...");
            Console.ReadKey();
        }

        private async Task RegistrarConsultaAsync()
        {
            Console.Clear();
            Console.WriteLine("=== Registrar Nueva Consulta ===\n");

            // Obtener datos de paciente
            string pacientecorreo;
            string pacienteId;
            if (_userRole == "Paciente")
            {
                pacientecorreo = _usercorreo;
                var paciente = (await _pacienteService.GetAllAsync()).FirstOrDefault(p => p.correo == pacientecorreo);
                if (paciente == null)
                {
                    Console.WriteLine("\nPaciente no encontrado. Presione cualquier tecla para continuar...");
                    Console.ReadKey();
                    return;
                }
                pacienteId = paciente.Id;
            }
            else
            {
                Console.Write("correo del paciente: ");
                pacientecorreo = Console.ReadLine()?.Trim() ?? "";
                var paciente = (await _pacienteService.GetAllAsync()).FirstOrDefault(p => p.correo == pacientecorreo);
                if (paciente == null)
                {
                    Console.WriteLine("\nPaciente no encontrado. Presione cualquier tecla para continuar...");
                    Console.ReadKey();
                    return;
                }
                pacienteId = paciente.Id;
            }

            // Obtener datos de optometrista
            string optometristacorreo;
            string optometristaId;
            if (_userRole == "Optometrista")
            {
                optometristacorreo = _usercorreo;
                var optometrista = (await _optometristaService.GetAllAsync()).FirstOrDefault(o => o.correo == optometristacorreo);
                if (optometrista == null)
                {
                    Console.WriteLine("\nOptometrista no encontrado. Presione cualquier tecla para continuar...");
                    Console.ReadKey();
                    return;
                }
                optometristaId = optometrista.Id;
            }
            else
            {
                Console.Write("correo del optometrista: ");
                optometristacorreo = Console.ReadLine()?.Trim() ?? "";
                var optometrista = (await _optometristaService.GetAllAsync()).FirstOrDefault(o => o.correo == optometristacorreo);
                if (optometrista == null)
                {
                    Console.WriteLine("\nOptometrista no encontrado. Presione cualquier tecla para continuar...");
                    Console.ReadKey();
                    return;
                }
                optometristaId = optometrista.Id;
            }

            // Obtener datos de la consulta
            Console.Write("Motivo de la consulta: ");
            var motivo = Console.ReadLine()?.Trim() ?? "";

            Console.Write("Síntomas: ");
            var sintomas = Console.ReadLine()?.Trim() ?? "";

            Console.Write("Diagnóstico: ");
            var diagnostico = Console.ReadLine()?.Trim() ?? "";

            Console.Write("Tratamiento: ");
            var tratamiento = Console.ReadLine()?.Trim() ?? "";

            Console.Write("Recomendaciones: ");
            var recomendaciones = Console.ReadLine()?.Trim() ?? "";

            Console.Write("Observaciones (opcional): ");
            var observaciones = Console.ReadLine()?.Trim();

            // Crear y guardar la consulta
            var consulta = new Consulta
            {
                Id = Guid.NewGuid().ToString(),
                PacienteId = pacienteId,
                OptometristaId = optometristaId,
                Pacientecorreo = pacientecorreo,
                Optometristacorreo = optometristacorreo,
                Fecha = DateTime.UtcNow,
                Motivo = motivo,
                Sintomas = sintomas,
                Diagnostico = diagnostico,
                Tratamiento = tratamiento,
                Recomendaciones = recomendaciones,
                Observaciones = observaciones
            };

            await _consultaService.CreateAsync(consulta);
            Console.WriteLine("\nConsulta registrada exitosamente. Presione cualquier tecla para continuar...");
            Console.ReadKey();
        }

        private async Task ActualizarConsultaAsync()
        {
            Console.Clear();
            Console.WriteLine("=== Actualizar Consulta ===\n");

            // Mostrar consultas disponibles
            var consultas = await _consultaService.GetAllAsync();
            if (_userRole == "Paciente")
                consultas = consultas.Where(c => c.Pacientecorreo == _usercorreo).ToList();
            else if (_userRole == "Optometrista")
                consultas = consultas.Where(c => c.Optometristacorreo == _usercorreo).ToList();

            if (!consultas.Any())
            {
                Console.WriteLine("No hay consultas disponibles.");
                Console.WriteLine("\nPresione cualquier tecla para continuar...");
                Console.ReadKey();
                return;
            }

            foreach (var consulta in consultas)
            {
                Console.WriteLine($"ID: {consulta.Id}");
                Console.WriteLine($"Paciente: {consulta.Pacientecorreo}");
                Console.WriteLine($"Optometrista: {consulta.Optometristacorreo}");
                Console.WriteLine($"Fecha: {consulta.Fecha}");
                Console.WriteLine($"Motivo: {consulta.Motivo}");
                Console.WriteLine("------------------------");
            }

            // Seleccionar consulta
            Console.Write("\nIngrese el ID de la consulta a actualizar: ");
            var consultaId = Console.ReadLine()?.Trim() ?? "";

            var consultaSeleccionada = consultas.FirstOrDefault(c => c.Id == consultaId);
            if (consultaSeleccionada == null)
            {
                Console.WriteLine("\nConsulta no encontrada. Presione cualquier tecla para continuar...");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("\nIngrese los nuevos datos (deje en blanco para mantener el valor actual):");

            Console.Write($"Motivo [{consultaSeleccionada.Motivo}]: ");
            var motivo = Console.ReadLine()?.Trim();
            if (!string.IsNullOrEmpty(motivo))
                consultaSeleccionada.Motivo = motivo;

            Console.Write($"Síntomas [{consultaSeleccionada.Sintomas}]: ");
            var sintomas = Console.ReadLine()?.Trim();
            if (!string.IsNullOrEmpty(sintomas))
                consultaSeleccionada.Sintomas = sintomas;

            Console.Write($"Diagnóstico [{consultaSeleccionada.Diagnostico}]: ");
            var diagnostico = Console.ReadLine()?.Trim();
            if (!string.IsNullOrEmpty(diagnostico))
                consultaSeleccionada.Diagnostico = diagnostico;

            Console.Write($"Tratamiento [{consultaSeleccionada.Tratamiento}]: ");
            var tratamiento = Console.ReadLine()?.Trim();
            if (!string.IsNullOrEmpty(tratamiento))
                consultaSeleccionada.Tratamiento = tratamiento;

            Console.Write($"Recomendaciones [{consultaSeleccionada.Recomendaciones}]: ");
            var recomendaciones = Console.ReadLine()?.Trim();
            if (!string.IsNullOrEmpty(recomendaciones))
                consultaSeleccionada.Recomendaciones = recomendaciones;

            Console.Write($"Observaciones [{consultaSeleccionada.Observaciones}]: ");
            var observaciones = Console.ReadLine()?.Trim();
            if (observaciones != null)
                consultaSeleccionada.Observaciones = observaciones;

            var (success, message) = await _consultaService.UpdateAsync(consultaId, consultaSeleccionada);
            if (success)
            {
                Console.WriteLine("\nConsulta actualizada exitosamente. Presione cualquier tecla para continuar...");
            }
            else
            {
                Console.WriteLine($"\nError al actualizar la consulta: {message}");
            }
            Console.ReadKey();
        }
    }
}
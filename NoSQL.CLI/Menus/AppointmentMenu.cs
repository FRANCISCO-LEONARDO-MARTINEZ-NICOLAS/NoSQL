using System;
using System.Threading.Tasks;
using System.Linq;
using NoSQL.Domain.Entities;
using NoSQL.Application.Interfaces;

namespace NoSQL.CLI.Menus
{
    public class AppointmentMenu : BaseMenu
    {
        private readonly ICitaService _citaService;
        private readonly IPacienteService _pacienteService;
        private readonly IOptometristaService _optometristaService;
        private readonly string _userEmail;
        private readonly string _userRole;

        public AppointmentMenu(
            ICitaService citaService,
            IPacienteService pacienteService,
            IOptometristaService optometristaService,
            string userEmail,
            string userRole)
        {
            _citaService = citaService;
            _pacienteService = pacienteService;
            _optometristaService = optometristaService;
            _userEmail = userEmail;
            _userRole = userRole;
        }

        public override async Task ShowAsync()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== Gestión de Citas ===\n");
                Console.WriteLine("1. Ver todas las citas");
                Console.WriteLine("2. Ver citas por fecha");
                Console.WriteLine("3. Ver citas por estado");
                Console.WriteLine("4. Agendar nueva cita");
                Console.WriteLine("5. Actualizar estado de cita");
                Console.WriteLine("0. Volver al menú principal");

                var opcion = Console.ReadLine()?.Trim();

                switch (opcion)
                {
                    case "1":
                        await MostrarTodasLasCitasAsync();
                        break;
                    case "2":
                        await MostrarCitasPorFechaAsync();
                        break;
                    case "3":
                        await MostrarCitasPorEstadoAsync();
                        break;
                    case "4":
                        await AgendarCitaAsync();
                        break;
                    case "5":
                        await ActualizarEstadoCitaAsync();
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

        private async Task MostrarTodasLasCitasAsync()
        {
            Console.Clear();
            Console.WriteLine("=== Todas las Citas ===\n");

            var citas = await _citaService.GetAllAsync();

            if (_userRole == "Paciente")
                citas = citas.Where(c => c.PacienteEmail == _userEmail).ToList();
            else if (_userRole == "Optometrista")
                citas = citas.Where(c => c.OptometristaEmail == _userEmail).ToList();

            if (!citas.Any())
            {
                Console.WriteLine("No hay citas registradas.");
            }
            else
            {
                foreach (var cita in citas)
                {
                    Console.WriteLine($"ID: {cita.Id}");
                    Console.WriteLine($"Paciente: {cita.PacienteEmail}");
                    Console.WriteLine($"Optometrista: {cita.OptometristaEmail}");
                    Console.WriteLine($"Fecha: {cita.FechaHora}");
                    Console.WriteLine($"Tipo: {cita.Tipo}");
                    Console.WriteLine($"Motivo: {cita.Motivo}");
                    Console.WriteLine($"Estado: {cita.Estado}");
                    Console.WriteLine("------------------------");
                }
            }

            Console.WriteLine("\nPresione cualquier tecla para continuar...");
            Console.ReadKey();
        }

        private async Task MostrarCitasPorFechaAsync()
        {
            Console.Clear();
            Console.WriteLine("=== Citas por Fecha ===\n");

            Console.Write("Ingrese la fecha (dd/MM/yyyy): ");
            if (!DateTime.TryParse(Console.ReadLine(), out var fecha))
            {
                Console.WriteLine("\nFecha inválida. Presione cualquier tecla para continuar...");
                Console.ReadKey();
                return;
            }

            var citas = await _citaService.GetAllAsync();
            var filtradas = citas.Where(c => c.FechaHora.Date == fecha.Date).ToList();

            if (!filtradas.Any())
            {
                Console.WriteLine("No hay citas para la fecha especificada.");
            }
            else
            {
                foreach (var cita in filtradas)
                {
                    Console.WriteLine($"ID: {cita.Id}");
                    Console.WriteLine($"Paciente: {cita.PacienteEmail}");
                    Console.WriteLine($"Optometrista: {cita.OptometristaEmail}");
                    Console.WriteLine($"Fecha: {cita.FechaHora}");
                    Console.WriteLine($"Tipo: {cita.Tipo}");
                    Console.WriteLine($"Motivo: {cita.Motivo}");
                    Console.WriteLine($"Estado: {cita.Estado}");
                    Console.WriteLine("------------------------");
                }
            }

            Console.WriteLine("\nPresione cualquier tecla para continuar...");
            Console.ReadKey();
        }

        private async Task MostrarCitasPorEstadoAsync()
        {
            Console.Clear();
            Console.WriteLine("=== Citas por Estado ===\n");

            Console.WriteLine("Estados disponibles:");
            Console.WriteLine("1. Pendiente");
            Console.WriteLine("2. Confirmada");
            Console.WriteLine("3. Cancelada");
            Console.WriteLine("4. Completada");

            var estado = Console.ReadLine()?.Trim() switch
            {
                "1" => "Pendiente",
                "2" => "Confirmada",
                "3" => "Cancelada",
                "4" => "Completada",
                _ => null
            };

            if (estado == null)
            {
                Console.WriteLine("\nEstado inválido. Presione cualquier tecla para continuar...");
                Console.ReadKey();
                return;
            }

            var citas = await _citaService.GetAllAsync();
            var filtradas = citas.Where(c => c.Estado == estado).ToList();

            if (!filtradas.Any())
            {
                Console.WriteLine($"No hay citas en estado {estado}.");
            }
            else
            {
                foreach (var cita in filtradas)
                {
                    Console.WriteLine($"ID: {cita.Id}");
                    Console.WriteLine($"Paciente: {cita.PacienteEmail}");
                    Console.WriteLine($"Optometrista: {cita.OptometristaEmail}");
                    Console.WriteLine($"Fecha: {cita.FechaHora}");
                    Console.WriteLine($"Tipo: {cita.Tipo}");
                    Console.WriteLine($"Motivo: {cita.Motivo}");
                    Console.WriteLine($"Estado: {cita.Estado}");
                    Console.WriteLine("------------------------");
                }
            }

            Console.WriteLine("\nPresione cualquier tecla para continuar...");
            Console.ReadKey();
        }

        private async Task AgendarCitaAsync()
        {
            Console.Clear();
            Console.WriteLine("=== Agendar Nueva Cita ===\n");

            // Obtener datos de paciente
            string pacienteEmail;
            string pacienteId;
            if (_userRole == "Paciente")
            {
                pacienteEmail = _userEmail;
                var paciente = (await _pacienteService.GetAllAsync()).FirstOrDefault(p => p.Correo == pacienteEmail);
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
                Console.Write("Email del paciente: ");
                pacienteEmail = Console.ReadLine()?.Trim() ?? "";
                var paciente = (await _pacienteService.GetAllAsync()).FirstOrDefault(p => p.Correo == pacienteEmail);
                if (paciente == null)
                {
                    Console.WriteLine("\nPaciente no encontrado. Presione cualquier tecla para continuar...");
                    Console.ReadKey();
                    return;
                }
                pacienteId = paciente.Id;
            }

            // Obtener datos de optometrista
            string optometristaEmail;
            string optometristaId;
            if (_userRole == "Optometrista")
            {
                optometristaEmail = _userEmail;
                var optometrista = (await _optometristaService.GetAllAsync()).FirstOrDefault(o => o.Correo == optometristaEmail);
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
                Console.Write("Email del optometrista: ");
                optometristaEmail = Console.ReadLine()?.Trim() ?? "";
                var optometrista = (await _optometristaService.GetAllAsync()).FirstOrDefault(o => o.Correo == optometristaEmail);
                if (optometrista == null)
                {
                    Console.WriteLine("\nOptometrista no encontrado. Presione cualquier tecla para continuar...");
                    Console.ReadKey();
                    return;
                }
                optometristaId = optometrista.Id;
            }

            // Obtener fecha y hora
            Console.Write("Fecha (dd/MM/yyyy): ");
            if (!DateTime.TryParse(Console.ReadLine(), out var fecha))
            {
                Console.WriteLine("\nFecha inválida. Presione cualquier tecla para continuar...");
                Console.ReadKey();
                return;
            }

            Console.Write("Hora (HH:mm): ");
            if (!TimeSpan.TryParse(Console.ReadLine(), out var hora))
            {
                Console.WriteLine("\nHora inválida. Presione cualquier tecla para continuar...");
                Console.ReadKey();
                return;
            }

            var fechaHora = fecha.Date + hora;

            // Obtener tipo de cita
            Console.WriteLine("\nTipos de cita disponibles:");
            Console.WriteLine("1. Consulta General");
            Console.WriteLine("2. Examen de la Vista");
            Console.WriteLine("3. Adaptación de Lentes");
            Console.WriteLine("4. Revisión de Lentes de Contacto");

            var tipo = Console.ReadLine()?.Trim() switch
            {
                "1" => "Consulta General",
                "2" => "Examen de la Vista",
                "3" => "Adaptación de Lentes",
                "4" => "Revisión de Lentes de Contacto",
                _ => null
            };

            if (tipo == null)
            {
                Console.WriteLine("\nTipo de cita inválido. Presione cualquier tecla para continuar...");
                Console.ReadKey();
                return;
            }

            // Obtener motivo
            Console.Write("\nMotivo de la cita: ");
            var motivo = Console.ReadLine()?.Trim() ?? "";

            // Obtener observaciones
            Console.Write("\nObservaciones de la cita: ");
            var observaciones = Console.ReadLine()?.Trim() ?? "";

            // Crear y guardar la cita
            var cita = new Cita
            {
                Id = Guid.NewGuid().ToString(),
                PacienteId = pacienteId,
                OptometristaId = optometristaId,
                PacienteEmail = pacienteEmail,
                OptometristaEmail = optometristaEmail,
                FechaHora = fechaHora,
                Tipo = tipo,
                Motivo = motivo,
                Estado = "Pendiente",
                Observaciones = observaciones
            };

            await _citaService.CreateAsync(cita);
            Console.WriteLine("\nCita agendada exitosamente. Presione cualquier tecla para continuar...");
            Console.ReadKey();
        }

        private async Task ActualizarEstadoCitaAsync()
        {
            Console.Clear();
            Console.WriteLine("=== Actualizar Estado de Cita ===\n");

            // Mostrar citas disponibles
            var citas = await _citaService.GetAllAsync();
            if (_userRole == "Paciente")
                citas = citas.Where(c => c.PacienteEmail == _userEmail).ToList();
            else if (_userRole == "Optometrista")
                citas = citas.Where(c => c.OptometristaEmail == _userEmail).ToList();

            if (!citas.Any())
            {
                Console.WriteLine("No hay citas disponibles.");
                Console.WriteLine("\nPresione cualquier tecla para continuar...");
                Console.ReadKey();
                return;
            }

            foreach (var cita in citas)
            {
                Console.WriteLine($"ID: {cita.Id}");
                Console.WriteLine($"Paciente: {cita.PacienteEmail}");
                Console.WriteLine($"Optometrista: {cita.OptometristaEmail}");
                Console.WriteLine($"Fecha: {cita.FechaHora}");
                Console.WriteLine($"Tipo: {cita.Tipo}");
                Console.WriteLine($"Estado actual: {cita.Estado}");
                Console.WriteLine("------------------------");
            }

            // Seleccionar cita
            Console.Write("\nIngrese el ID de la cita a actualizar: ");
            var citaId = Console.ReadLine()?.Trim() ?? "";

            var citaSeleccionada = citas.FirstOrDefault(c => c.Id == citaId);
            if (citaSeleccionada == null)
            {
                Console.WriteLine("\nCita no encontrada. Presione cualquier tecla para continuar...");
                Console.ReadKey();
                return;
            }

            // Seleccionar nuevo estado
            Console.WriteLine("\nEstados disponibles:");
            Console.WriteLine("1. Pendiente");
            Console.WriteLine("2. Confirmada");
            Console.WriteLine("3. Cancelada");
            Console.WriteLine("4. Completada");

            var nuevoEstado = Console.ReadLine()?.Trim() switch
            {
                "1" => "Pendiente",
                "2" => "Confirmada",
                "3" => "Cancelada",
                "4" => "Completada",
                _ => null
            };

            if (nuevoEstado == null)
            {
                Console.WriteLine("\nEstado inválido. Presione cualquier tecla para continuar...");
                Console.ReadKey();
                return;
            }

            // Actualizar estado
            citaSeleccionada.Estado = nuevoEstado;
            var (success, message) = await _citaService.UpdateAsync(citaId, citaSeleccionada);

            Console.WriteLine($"\nEstado de la cita actualizado exitosamente. {message}");
            Console.WriteLine("\nPresione cualquier tecla para continuar...");
            Console.ReadKey();
        }
    }
}
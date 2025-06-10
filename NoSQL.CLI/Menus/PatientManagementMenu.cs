using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using NoSQL.CLI.Services;
using NoSQL.Domain.Entities;
using NoSQL.Application.Interfaces;
using NoSQL.CLI.Services.Adapters;

namespace NoSQL.CLI.Menus
{
    public class PatientManagementMenu : BaseMenu
    {
        private readonly IPacienteService _pacienteService;
        private readonly string _userEmail;
        private readonly string _userRole;

        public PatientManagementMenu(
            IPacienteService pacienteService,
            string userEmail,
            string userRole)
        {
            _pacienteService = pacienteService;
            _userEmail = userEmail;
            _userRole = userRole;
        }

        public override async Task ShowAsync()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== Gestión de Pacientes ===\n");
                Console.WriteLine("1. Ver todos los pacientes");
                Console.WriteLine("2. Buscar paciente por email");
                Console.WriteLine("3. Buscar paciente por DNI");
                Console.WriteLine("4. Registrar nuevo paciente");
                Console.WriteLine("5. Actualizar paciente");
                Console.WriteLine("0. Volver al menú principal");

                var opcion = Console.ReadLine()?.Trim();

                switch (opcion)
                {
                    case "1":
                        await MostrarTodosLosPacientesAsync();
                        break;
                    case "2":
                        await BuscarPacientePorEmailAsync();
                        break;
                    case "3":
                        await BuscarPacientePorDniAsync();
                        break;
                    case "4":
                        await RegistrarPacienteAsync();
                        break;
                    case "5":
                        await ActualizarPacienteAsync();
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

        private async Task MostrarTodosLosPacientesAsync()
        {
            Console.Clear();
            Console.WriteLine("=== Todos los Pacientes ===\n");

            var pacientes = await _pacienteService.GetAllAsync();
            if (!pacientes.Any())
            {
                Console.WriteLine("No hay pacientes registrados.");
            }
            else
            {
                foreach (var paciente in pacientes)
                {
                    Console.WriteLine($"ID: {paciente.Id}");
                    Console.WriteLine($"Nombre: {paciente.Nombre} {paciente.Apellido}");
                    Console.WriteLine($"Email: {paciente.Correo}");
                    Console.WriteLine($"DNI: {paciente.Dni}");
                    Console.WriteLine($"Teléfono: {paciente.Telefono}");
                    Console.WriteLine($"Dirección: {paciente.Direccion}");
                    Console.WriteLine($"Fecha de nacimiento: {paciente.FechaNacimiento:dd/MM/yyyy}");
                    Console.WriteLine($"Género: {paciente.Genero}");
                    Console.WriteLine($"Ocupación: {paciente.Ocupacion}");
                    Console.WriteLine($"Seguro médico: {paciente.SeguroMedico}");
                    Console.WriteLine("------------------------");
                }
            }

            Console.WriteLine("\nPresione cualquier tecla para continuar...");
            Console.ReadKey();
        }

        private async Task BuscarPacientePorEmailAsync()
        {
            Console.Clear();
            Console.WriteLine("=== Buscar Paciente por Email ===\n");

            Console.Write("Email del paciente: ");
            var email = Console.ReadLine()?.Trim();
            if (string.IsNullOrEmpty(email))
            {
                Console.WriteLine("\nEmail requerido. Presione cualquier tecla para continuar...");
                Console.ReadKey();
                return;
            }

            var paciente = (await _pacienteService.GetAllAsync()).FirstOrDefault(p => p.Correo == email);
            if (paciente == null)
            {
                Console.WriteLine("\nPaciente no encontrado. Presione cualquier tecla para continuar...");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("\nDatos del paciente:");
            Console.WriteLine($"ID: {paciente.Id}");
            Console.WriteLine($"Nombre: {paciente.Nombre} {paciente.Apellido}");
            Console.WriteLine($"Email: {paciente.Correo}");
            Console.WriteLine($"DNI: {paciente.Dni}");
            Console.WriteLine($"Teléfono: {paciente.Telefono}");
            Console.WriteLine($"Dirección: {paciente.Direccion}");
            Console.WriteLine($"Fecha de nacimiento: {paciente.FechaNacimiento:dd/MM/yyyy}");
            Console.WriteLine($"Género: {paciente.Genero}");
            Console.WriteLine($"Ocupación: {paciente.Ocupacion}");
            Console.WriteLine($"Seguro médico: {paciente.SeguroMedico}");

            Console.WriteLine("\nPresione cualquier tecla para continuar...");
            Console.ReadKey();
        }

        private async Task BuscarPacientePorDniAsync()
        {
            Console.Clear();
            Console.WriteLine("=== Buscar Paciente por DNI ===\n");

            Console.Write("DNI del paciente: ");
            var dni = Console.ReadLine()?.Trim();
            if (string.IsNullOrEmpty(dni))
            {
                Console.WriteLine("\nDNI requerido. Presione cualquier tecla para continuar...");
                Console.ReadKey();
                return;
            }

            var paciente = (await _pacienteService.GetAllAsync()).FirstOrDefault(p => p.Dni == dni);
            if (paciente == null)
            {
                Console.WriteLine("\nPaciente no encontrado. Presione cualquier tecla para continuar...");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("\nDatos del paciente:");
            Console.WriteLine($"ID: {paciente.Id}");
            Console.WriteLine($"Nombre: {paciente.Nombre} {paciente.Apellido}");
            Console.WriteLine($"Email: {paciente.Correo}");
            Console.WriteLine($"DNI: {paciente.Dni}");
            Console.WriteLine($"Teléfono: {paciente.Telefono}");
            Console.WriteLine($"Dirección: {paciente.Direccion}");
            Console.WriteLine($"Fecha de nacimiento: {paciente.FechaNacimiento:dd/MM/yyyy}");
            Console.WriteLine($"Género: {paciente.Genero}");
            Console.WriteLine($"Ocupación: {paciente.Ocupacion}");
            Console.WriteLine($"Seguro médico: {paciente.SeguroMedico}");

            Console.WriteLine("\nPresione cualquier tecla para continuar...");
            Console.ReadKey();
        }

        private async Task RegistrarPacienteAsync()
        {
            Console.Clear();
            Console.WriteLine("=== Registrar Nuevo Paciente ===\n");

            Console.Write("Nombre: ");
            var nombre = Console.ReadLine()?.Trim();
            if (string.IsNullOrEmpty(nombre))
            {
                Console.WriteLine("\nNombre requerido. Presione cualquier tecla para continuar...");
                Console.ReadKey();
                return;
            }

            Console.Write("Apellido: ");
            var apellido = Console.ReadLine()?.Trim();
            if (string.IsNullOrEmpty(apellido))
            {
                Console.WriteLine("\nApellido requerido. Presione cualquier tecla para continuar...");
                Console.ReadKey();
                return;
            }

            Console.Write("Email: ");
            var email = Console.ReadLine()?.Trim();
            if (string.IsNullOrEmpty(email))
            {
                Console.WriteLine("\nEmail requerido. Presione cualquier tecla para continuar...");
                Console.ReadKey();
                return;
            }

            Console.Write("DNI: ");
            var dni = Console.ReadLine()?.Trim();
            if (string.IsNullOrEmpty(dni))
            {
                Console.WriteLine("\nDNI requerido. Presione cualquier tecla para continuar...");
                Console.ReadKey();
                return;
            }

            Console.Write("Teléfono: ");
            var telefono = Console.ReadLine()?.Trim();
            if (string.IsNullOrEmpty(telefono))
            {
                Console.WriteLine("\nTeléfono requerido. Presione cualquier tecla para continuar...");
                Console.ReadKey();
                return;
            }

            Console.Write("Dirección: ");
            var direccion = Console.ReadLine()?.Trim();
            if (string.IsNullOrEmpty(direccion))
            {
                Console.WriteLine("\nDirección requerida. Presione cualquier tecla para continuar...");
                Console.ReadKey();
                return;
            }

            Console.Write("Fecha de nacimiento (dd/MM/yyyy): ");
            if (!DateTime.TryParse(Console.ReadLine(), out var fechaNacimiento))
            {
                Console.WriteLine("\nFecha de nacimiento inválida. Presione cualquier tecla para continuar...");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("\nGénero:");
            Console.WriteLine("1. Masculino");
            Console.WriteLine("2. Femenino");
            Console.WriteLine("3. Otro");
            var genero = Console.ReadLine()?.Trim() switch
            {
                "1" => "Masculino",
                "2" => "Femenino",
                "3" => "Otro",
                _ => null
            };

            if (genero == null)
            {
                Console.WriteLine("\nGénero inválido. Presione cualquier tecla para continuar...");
                Console.ReadKey();
                return;
            }

            Console.Write("Ocupación: ");
            var ocupacion = Console.ReadLine()?.Trim();
            if (string.IsNullOrEmpty(ocupacion))
            {
                Console.WriteLine("\nOcupación requerida. Presione cualquier tecla para continuar...");
                Console.ReadKey();
                return;
            }

            Console.Write("Seguro médico: ");
            var seguroMedico = Console.ReadLine()?.Trim();
            if (string.IsNullOrEmpty(seguroMedico))
            {
                Console.WriteLine("\nSeguro médico requerido. Presione cualquier tecla para continuar...");
                Console.ReadKey();
                return;
            }

            var paciente = new Paciente
            {
                Id = Guid.NewGuid().ToString(),
                Nombre = nombre,
                Apellido = apellido,
                Dni = dni,
                Correo = email,
                Telefono = telefono,
                Direccion = direccion,
                FechaNacimiento = fechaNacimiento,
                Genero = genero,
                Ocupacion = ocupacion,
                SeguroMedico = seguroMedico,
                type = "Paciente"
            };

            await _pacienteService.CreateAsync(paciente);
            Console.WriteLine("\nPaciente registrado exitosamente. Presione cualquier tecla para continuar...");
            Console.ReadKey();
        }

        private async Task ActualizarPacienteAsync()
        {
            Console.Clear();
            Console.WriteLine("=== Actualizar Paciente ===\n");

            Console.Write("ID del paciente: ");
            var pacienteId = Console.ReadLine()?.Trim();
            if (string.IsNullOrEmpty(pacienteId))
            {
                Console.WriteLine("\nID de paciente inválido. Presione cualquier tecla para continuar...");
                Console.ReadKey();
                return;
            }

            var paciente = await _pacienteService.GetByIdAsync(pacienteId);
            if (paciente == null)
            {
                Console.WriteLine("\nPaciente no encontrado. Presione cualquier tecla para continuar...");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("\nIngrese los nuevos datos (deje en blanco para mantener el valor actual):");

            Console.Write($"Nombre [{paciente.Nombre}]: ");
            var nombre = Console.ReadLine()?.Trim();
            if (!string.IsNullOrEmpty(nombre))
                paciente.Nombre = nombre;

            Console.Write($"Apellido [{paciente.Apellido}]: ");
            var apellido = Console.ReadLine()?.Trim();
            if (!string.IsNullOrEmpty(apellido))
                paciente.Apellido = apellido;

            Console.Write($"Email [{paciente.Correo}]: ");
            var email = Console.ReadLine()?.Trim();
            if (!string.IsNullOrEmpty(email))
                paciente.Correo = email;

            Console.Write($"DNI [{paciente.Dni}]: ");
            var dni = Console.ReadLine()?.Trim();
            if (!string.IsNullOrEmpty(dni))
                paciente.Dni = dni;

            Console.Write($"Teléfono [{paciente.Telefono}]: ");
            var telefono = Console.ReadLine()?.Trim();
            if (!string.IsNullOrEmpty(telefono))
                paciente.Telefono = telefono;

            Console.Write($"Dirección [{paciente.Direccion}]: ");
            var direccion = Console.ReadLine()?.Trim();
            if (!string.IsNullOrEmpty(direccion))
                paciente.Direccion = direccion;

            Console.Write($"Fecha de nacimiento [{paciente.FechaNacimiento:dd/MM/yyyy}]: ");
            if (DateTime.TryParse(Console.ReadLine(), out var fechaNacimiento))
                paciente.FechaNacimiento = fechaNacimiento;

            Console.WriteLine($"\nGénero actual: {paciente.Genero}");
            Console.WriteLine("Nuevo género:");
            Console.WriteLine("1. Masculino");
            Console.WriteLine("2. Femenino");
            Console.WriteLine("3. Otro");
            var genero = Console.ReadLine()?.Trim() switch
            {
                "1" => "Masculino",
                "2" => "Femenino",
                "3" => "Otro",
                _ => null
            };
            if (genero != null)
                paciente.Genero = genero;

            Console.Write($"Ocupación [{paciente.Ocupacion}]: ");
            var ocupacion = Console.ReadLine()?.Trim();
            if (!string.IsNullOrEmpty(ocupacion))
                paciente.Ocupacion = ocupacion;

            Console.Write($"Seguro médico [{paciente.SeguroMedico}]: ");
            var seguroMedico = Console.ReadLine()?.Trim();
            if (!string.IsNullOrEmpty(seguroMedico))
                paciente.SeguroMedico = seguroMedico;

            var (success, message) = await _pacienteService.UpdateAsync(pacienteId, paciente);
            Console.WriteLine(message);
            Console.WriteLine("\nPaciente actualizado exitosamente. Presione cualquier tecla para continuar...");
            Console.ReadKey();
        }
    }
}
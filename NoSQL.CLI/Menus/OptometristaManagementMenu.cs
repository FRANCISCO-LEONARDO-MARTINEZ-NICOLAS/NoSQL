using System;
using System.Threading.Tasks;
using System.Linq;
using NoSQL.Application.Interfaces;
using NoSQL.Domain.Entities;

namespace NoSQL.CLI.Menus
{
    public class OptometristaManagementMenu : BaseMenu
    {
        private readonly IOptometristaService _optometristaService;
        private readonly string _usercorreo;
        private readonly string _userRole;

        public OptometristaManagementMenu(
            IOptometristaService optometristaService,
            string usercorreo,
            string userRole)
        {
            _optometristaService = optometristaService;
            _usercorreo = usercorreo;
            _userRole = userRole;
        }

        public override async Task ShowAsync()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== Gestión de Optometristas ===\n");
                Console.WriteLine("1. Ver todos los optometristas");
                Console.WriteLine("2. Buscar optometrista por nombre");
                Console.WriteLine("3. Registrar nuevo optometrista");
                Console.WriteLine("4. Actualizar optometrista");
                Console.WriteLine("0. Volver al menú principal");

                var opcion = Console.ReadLine()?.Trim();

                switch (opcion)
                {
                    case "1":
                        await MostrarTodosLosOptometristasAsync();
                        break;
                    case "2":
                        await BuscarOptometristaPorNombreAsync();
                        break;
                    case "3":
                        await RegistrarOptometristaAsync();
                        break;
                    case "4":
                        await ActualizarOptometristaAsync();
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

        private async Task MostrarTodosLosOptometristasAsync()
        {
            Console.Clear();
            Console.WriteLine("=== Todos los Optometristas ===\n");

            var optometristas = await _optometristaService.GetAllAsync();
            if (optometristas == null || !optometristas.Any())
            {
                Console.WriteLine("No hay optometristas registrados.");
            }
            else
            {
                foreach (var optometrista in optometristas)
                {
                    Console.WriteLine($"ID: {optometrista.Id}");
                    Console.WriteLine($"Nombre: {optometrista.Nombre} {optometrista.Apellido}");
                    Console.WriteLine($"Cédula: {optometrista.CedulaProfesional}");
                    Console.WriteLine($"Especialidad: {optometrista.Especialidad}");
                    Console.WriteLine($"correo: {optometrista.correo}");
                    Console.WriteLine($"Celular: {optometrista.Celular}");
                    Console.WriteLine($"Emergencia: {optometrista.NumeroEmergencia}");
                    Console.WriteLine($"Teléfono: {optometrista.Telefono}");
                    Console.WriteLine($"Dirección: {optometrista.Direccion}");
                    Console.WriteLine("------------------------");
                }
            }

            Console.WriteLine("\nPresione cualquier tecla para continuar...");
            Console.ReadKey();
        }

        private async Task BuscarOptometristaPorNombreAsync()
        {
            Console.Clear();
            Console.WriteLine("=== Buscar Optometrista por Nombre ===\n");

            Console.Write("Nombre del optometrista: ");
            var nombre = Console.ReadLine()?.Trim();
            if (string.IsNullOrEmpty(nombre))
            {
                Console.WriteLine("\nNombre requerido. Presione cualquier tecla para continuar...");
                Console.ReadKey();
                return;
            }

            var optometristas = await _optometristaService.GetAllAsync();
            var encontrados = optometristas.Where(o => o.Nombre != null && o.Nombre.Contains(nombre, StringComparison.OrdinalIgnoreCase)).ToList();

            if (!encontrados.Any())
            {
                Console.WriteLine("\nOptometrista no encontrado. Presione cualquier tecla para continuar...");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("\nResultados:");
            foreach (var optometrista in encontrados)
            {
                Console.WriteLine($"ID: {optometrista.Id}");
                Console.WriteLine($"Nombre: {optometrista.Nombre} {optometrista.Apellido}");
                Console.WriteLine($"Cédula: {optometrista.CedulaProfesional}");
                Console.WriteLine($"Especialidad: {optometrista.Especialidad}");
                Console.WriteLine($"correo: {optometrista.correo}");
                Console.WriteLine($"Celular: {optometrista.Celular}");
                Console.WriteLine($"Emergencia: {optometrista.NumeroEmergencia}");
                Console.WriteLine($"Teléfono: {optometrista.Telefono}");
                Console.WriteLine($"Dirección: {optometrista.Direccion}");
                Console.WriteLine("------------------------");
            }

            Console.WriteLine("\nPresione cualquier tecla para continuar...");
            Console.ReadKey();
        }

        private async Task RegistrarOptometristaAsync()
        {
            Console.Clear();
            Console.WriteLine("=== Registrar Nuevo Optometrista ===\n");

            Console.Write("Nombre: ");
            var nombre = Console.ReadLine()?.Trim();
            Console.Write("Apellido: ");
            var apellido = Console.ReadLine()?.Trim();
            Console.Write("Cédula profesional: ");
            var cedula = Console.ReadLine()?.Trim();
            Console.Write("Especialidad: ");
            var especialidad = Console.ReadLine()?.Trim();
            Console.Write("correo: ");
            var correo = Console.ReadLine()?.Trim();
            Console.Write("Celular: ");
            var celular = Console.ReadLine()?.Trim();
            Console.Write("Número de emergencia: ");
            var emergencia = Console.ReadLine()?.Trim();
            Console.Write("Teléfono (opcional): ");
            var telefono = Console.ReadLine()?.Trim();
            Console.Write("Dirección (opcional): ");
            var direccion = Console.ReadLine()?.Trim();

            if (string.IsNullOrEmpty(nombre) || string.IsNullOrEmpty(apellido) || string.IsNullOrEmpty(cedula) ||
                string.IsNullOrEmpty(especialidad) || string.IsNullOrEmpty(correo) || string.IsNullOrEmpty(celular) ||
                string.IsNullOrEmpty(emergencia))
            {
                Console.WriteLine("\nTodos los campos obligatorios deben ser llenados. Presione cualquier tecla para continuar...");
                Console.ReadKey();
                return;
            }

            var optometrista = new Optometrista
            {
                Id = Guid.NewGuid().ToString(),
                Nombre = nombre,
                Apellido = apellido,
                CedulaProfesional = cedula,
                Especialidad = especialidad,
                correo = correo,
                Celular = celular,
                NumeroEmergencia = emergencia,
                Telefono = telefono,
                Direccion = direccion,
                FechaContratacion = DateTime.UtcNow,
                Activo = true,
                type = "Optometrista"
            };

            await _optometristaService.CreateAsync(optometrista);
            Console.WriteLine("\nOptometrista registrado exitosamente. Presione cualquier tecla para continuar...");
            Console.ReadKey();
        }

        private async Task ActualizarOptometristaAsync()
        {
            Console.Clear();
            Console.WriteLine("=== Actualizar Optometrista ===\n");

            Console.Write("ID del optometrista: ");
            var optometristaId = Console.ReadLine()?.Trim();

            if (string.IsNullOrEmpty(optometristaId))
            {
                Console.WriteLine("\nID de optometrista inválido. Presione cualquier tecla para continuar...");
                Console.ReadKey();
                return;
            }

            var optometrista = await _optometristaService.GetByIdAsync(optometristaId);
            if (optometrista == null)
            {
                Console.WriteLine("\nOptometrista no encontrado. Presione cualquier tecla para continuar...");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("\nIngrese los nuevos datos (deje en blanco para mantener el valor actual):");

            Console.Write($"Nombre [{optometrista.Nombre}]: ");
            var nombre = Console.ReadLine()?.Trim();
            if (!string.IsNullOrEmpty(nombre))
                optometrista.Nombre = nombre;

            Console.Write($"Apellido [{optometrista.Apellido}]: ");
            var apellido = Console.ReadLine()?.Trim();
            if (!string.IsNullOrEmpty(apellido))
                optometrista.Apellido = apellido;

            Console.Write($"Cédula profesional [{optometrista.CedulaProfesional}]: ");
            var cedula = Console.ReadLine()?.Trim();
            if (!string.IsNullOrEmpty(cedula))
                optometrista.CedulaProfesional = cedula;

            Console.Write($"Especialidad [{optometrista.Especialidad}]: ");
            var especialidad = Console.ReadLine()?.Trim();
            if (!string.IsNullOrEmpty(especialidad))
                optometrista.Especialidad = especialidad;

            Console.Write($"correo [{optometrista.correo}]: ");
            var correo = Console.ReadLine()?.Trim();
            if (!string.IsNullOrEmpty(correo))
                optometrista.correo = correo;

            Console.Write($"Celular [{optometrista.Celular}]: ");
            var celular = Console.ReadLine()?.Trim();
            if (!string.IsNullOrEmpty(celular))
                optometrista.Celular = celular;

            Console.Write($"Número de emergencia [{optometrista.NumeroEmergencia}]: ");
            var emergencia = Console.ReadLine()?.Trim();
            if (!string.IsNullOrEmpty(emergencia))
                optometrista.NumeroEmergencia = emergencia;

            Console.Write($"Teléfono [{optometrista.Telefono}]: ");
            var telefono = Console.ReadLine()?.Trim();
            if (!string.IsNullOrEmpty(telefono))
                optometrista.Telefono = telefono;

            Console.Write($"Dirección [{optometrista.Direccion}]: ");
            var direccion = Console.ReadLine()?.Trim();
            if (!string.IsNullOrEmpty(direccion))
                optometrista.Direccion = direccion;

            var (success, message) = await _optometristaService.UpdateAsync(optometristaId, optometrista);
            Console.WriteLine("\nOptometrista actualizado exitosamente. Presione cualquier tecla para continuar...");
            Console.ReadKey();
        }
    }
}
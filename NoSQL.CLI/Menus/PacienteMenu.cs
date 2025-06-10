using System;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using NoSQL.Application.Interfaces;
using NoSQL.CLI.Services.Adapters;
using NoSQL.Domain.Entities;

namespace NoSQL.CLI.Menus
{
    public class PacienteMenu : BaseMenu
    {
        private readonly IPacienteService _pacienteService;
        private readonly IConsultaService _consultaService;
        private readonly IProductoService _productoService;
        private readonly ICitaService _citaService;
        private readonly string _userEmail;
        private readonly string _userRole;

        public PacienteMenu(
            IPacienteService pacienteService,
            IConsultaService consultaService,
            IProductoService productoService,
            ICitaService citaService,
            string userEmail,
            string userRole)
        {
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
                Console.WriteLine("=== Panel de Paciente ===\n");
                Console.WriteLine("1. Ver mis consultas");
                Console.WriteLine("2. Ver mis citas");
                Console.WriteLine("3. Ver mis productos");
                Console.WriteLine("4. Actualizar mis datos");
                Console.WriteLine("0. Cerrar sesión");

                var opcion = Console.ReadLine()?.Trim();

                switch (opcion)
                {
                    case "1":
                        await MostrarMisConsultasAsync();
                        break;
                    case "2":
                        await MostrarMisCitasAsync();
                        break;
                    case "3":
                        await MostrarMisProductosAsync();
                        break;
                    case "4":
                        await ActualizarMisDatosAsync();
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

        private async Task MostrarMisConsultasAsync()
        {
            Console.Clear();
            Console.WriteLine("=== Mis Consultas ===\n");

            var consultas = (await _consultaService.GetAllAsync()).Where(c => c.PacienteEmail == _userEmail).ToList();
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

            Console.WriteLine("\nPresione cualquier tecla para continuar...");
            Console.ReadKey();
        }

        private async Task MostrarMisCitasAsync()
        {
            Console.Clear();
            Console.WriteLine("=== Mis Citas ===\n");

            var citas = (await _citaService.GetAllAsync()).Where(c => c.PacienteEmail == _userEmail).ToList();
            if (!citas.Any())
            {
                Console.WriteLine("No hay citas registradas.");
            }
            else
            {
                foreach (var cita in citas.OrderBy(c => c.FechaHora))
                {
                    Console.WriteLine($"Fecha: {cita.FechaHora:dd/MM/yyyy HH:mm}");
                    Console.WriteLine($"Tipo: {cita.Tipo}");
                    Console.WriteLine($"Motivo: {cita.Motivo}");
                    Console.WriteLine($"Estado: {cita.Estado}");
                    Console.WriteLine("------------------------");
                }
            }

            Console.WriteLine("\nPresione cualquier tecla para continuar...");
            Console.ReadKey();
        }

        private async Task MostrarMisProductosAsync()
        {
            Console.Clear();
            Console.WriteLine("=== Mis Productos ===\n");

            var productos = (await _productoService.GetAllAsync()).Where(p => p.PacienteEmail == _userEmail).ToList();
            if (!productos.Any())
            {
                Console.WriteLine("No hay productos registrados.");
            }
            else
            {
                foreach (var producto in productos.OrderByDescending(p => p.FechaVenta))
                {
                    Console.WriteLine($"Producto: {producto.Nombre}");
                    Console.WriteLine($"Tipo: {producto.Tipo}");
                    Console.WriteLine($"Descripción: {producto.Descripcion}");
                    Console.WriteLine($"Precio: {producto.Precio:C}");
                    Console.WriteLine($"Fecha de venta: {producto.FechaVenta:dd/MM/yyyy}");
                    Console.WriteLine($"Fecha de entrega estimada: {producto.FechaEntregaEstimada:dd/MM/yyyy}");
                    Console.WriteLine($"Estado: {producto.Estado}");
                    Console.WriteLine("------------------------");
                }
            }

            Console.WriteLine("\nPresione cualquier tecla para continuar...");
            Console.ReadKey();
        }

        private async Task ActualizarMisDatosAsync()
        {
            Console.Clear();
            Console.WriteLine("=== Actualizar Mis Datos ===\n");

            var paciente = (await _pacienteService.GetAllAsync()).FirstOrDefault(p => p.Correo == _userEmail);
            if (paciente == null)
            {
                Console.WriteLine("Error al obtener datos del paciente. Presione cualquier tecla para continuar...");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("Ingrese los nuevos datos (deje en blanco para mantener el valor actual):");

            Console.Write($"Nombre [{paciente.Nombre}]: ");
            var nombre = Console.ReadLine()?.Trim();
            if (!string.IsNullOrEmpty(nombre))
                paciente.Nombre = nombre;

            Console.Write($"Apellido [{paciente.Apellido}]: ");
            var apellido = Console.ReadLine()?.Trim();
            if (!string.IsNullOrEmpty(apellido))
                paciente.Apellido = apellido;

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

            Console.Write($"Género [{paciente.Genero}]: ");
            var genero = Console.ReadLine()?.Trim();
            if (!string.IsNullOrEmpty(genero))
                paciente.Genero = genero;

            Console.Write($"Ocupación [{paciente.Ocupacion}]: ");
            var ocupacion = Console.ReadLine()?.Trim();
            if (!string.IsNullOrEmpty(ocupacion))
                paciente.Ocupacion = ocupacion;

            Console.Write($"Seguro médico [{paciente.SeguroMedico}]: ");
            var seguroMedico = Console.ReadLine()?.Trim();
            if (!string.IsNullOrEmpty(seguroMedico))
                paciente.SeguroMedico = seguroMedico;

            await _pacienteService.UpdateAsync(paciente.Id, paciente);
            Console.WriteLine("\nDatos actualizados exitosamente. Presione cualquier tecla para continuar...");
            Console.ReadKey();
        }
    }
}
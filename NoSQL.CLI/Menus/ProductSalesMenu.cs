using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using NoSQL.CLI.Services;
using NoSQL.Domain.Entities;
using System.Linq;
using NoSQL.Application.Interfaces;
using NoSQL.CLI.Services.Adapters;

namespace NoSQL.CLI.Menus
{
    public class ProductSalesMenu : BaseMenu
    {
        private readonly IProductoService _productoService;
        private readonly IPacienteService _pacienteService;
        private readonly string _usercorreo;
        private readonly string _userRole;

        public ProductSalesMenu(
            IProductoService productoService,
            IPacienteService pacienteService,
            string usercorreo,
            string userRole)
        {
            _productoService = productoService;
            _pacienteService = pacienteService;
            _usercorreo = usercorreo;
            _userRole = userRole;
        }

        public override async Task ShowAsync()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== Gestión de Productos ===\n");
                Console.WriteLine("1. Ver todos los productos");
                Console.WriteLine("2. Ver productos por paciente");
                Console.WriteLine("3. Registrar nuevo producto");
                Console.WriteLine("4. Actualizar producto");
                Console.WriteLine("5. Actualizar stock");
                Console.WriteLine("0. Volver al menú principal");

                var opcion = Console.ReadLine()?.Trim();

                switch (opcion)
                {
                    case "1":
                        await MostrarTodosLosProductosAsync();
                        break;
                    case "2":
                        await MostrarProductosPorPacienteAsync();
                        break;
                    case "3":
                        await RegistrarProductoAsync();
                        break;
                    case "4":
                        await ActualizarProductoAsync();
                        break;
                    case "5":
                        await ActualizarStockAsync();
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

        private async Task MostrarTodosLosProductosAsync()
        {
            Console.Clear();
            Console.WriteLine("=== Todos los Productos ===\n");

            var productos = await _productoService.GetAllAsync();
            if (!productos.Any())
            {
                Console.WriteLine("No hay productos registrados.");
            }
            else
            {
                foreach (var producto in productos)
                {
                    Console.WriteLine($"ID: {producto.Id}");
                    Console.WriteLine($"Nombre: {producto.Nombre}");
                    Console.WriteLine($"Tipo: {producto.Tipo}");
                    Console.WriteLine($"Descripción: {producto.Descripcion}");
                    Console.WriteLine($"Precio: {producto.Precio:C}");
                    Console.WriteLine($"Paciente: {producto.Pacientecorreo}");
                    Console.WriteLine($"Optometrista: {producto.Optometristacorreo}");
                    Console.WriteLine($"Fecha de venta: {producto.FechaVenta}");
                    Console.WriteLine($"Fecha de entrega estimada: {producto.FechaEntregaEstimada}");
                    Console.WriteLine($"Estado: {producto.Estado}");
                    Console.WriteLine($"Stock: {producto.Stock}");
                    Console.WriteLine("------------------------");
                }
            }

            Console.WriteLine("\nPresione cualquier tecla para continuar...");
            Console.ReadKey();
        }

        private async Task MostrarProductosPorPacienteAsync()
        {
            Console.Clear();
            Console.WriteLine("=== Productos por Paciente ===\n");

            string? pacientecorreo;
            if (_userRole == "Paciente")
            {
                pacientecorreo = _usercorreo;
            }
            else
            {
                Console.Write("correo del paciente: ");
                pacientecorreo = Console.ReadLine()?.Trim();
                if (string.IsNullOrEmpty(pacientecorreo))
                {
                    Console.WriteLine("\ncorreo de paciente requerido. Presione cualquier tecla para continuar...");
                    Console.ReadKey();
                    return;
                }
            }

            // Filtra en memoria usando el campo correcto
            var productos = (await _productoService.GetAllAsync())
                .Where(p => p.Pacientecorreo == pacientecorreo)
                .ToList();

            if (!productos.Any())
            {
                Console.WriteLine("No hay productos registrados para este paciente.");
            }
            else
            {
                foreach (var producto in productos)
                {
                    Console.WriteLine($"ID: {producto.Id}");
                    Console.WriteLine($"Nombre: {producto.Nombre}");
                    Console.WriteLine($"Tipo: {producto.Tipo}");
                    Console.WriteLine($"Descripción: {producto.Descripcion}");
                    Console.WriteLine($"Precio: {producto.Precio:C}");
                    Console.WriteLine($"Optometrista: {producto.Optometristacorreo}");
                    Console.WriteLine($"Fecha de venta: {producto.FechaVenta}");
                    Console.WriteLine($"Fecha de entrega estimada: {producto.FechaEntregaEstimada}");
                    Console.WriteLine($"Estado: {producto.Estado}");
                    Console.WriteLine($"Stock: {producto.Stock}");
                    Console.WriteLine("------------------------");
                }
            }

            Console.WriteLine("\nPresione cualquier tecla para continuar...");
            Console.ReadKey();
        }

        private async Task RegistrarProductoAsync()
        {
            Console.Clear();
            Console.WriteLine("=== Registrar Nuevo Producto ===\n");

            string? pacientecorreo;
            if (_userRole == "Paciente")
            {
                pacientecorreo = _usercorreo;
            }
            else
            {
                Console.Write("correo del paciente: ");
                pacientecorreo = Console.ReadLine()?.Trim();
                if (string.IsNullOrEmpty(pacientecorreo))
                {
                    Console.WriteLine("\ncorreo de paciente requerido. Presione cualquier tecla para continuar...");
                    Console.ReadKey();
                    return;
                }
            }

            string? optometristacorreo;
            if (_userRole == "Optometrista")
            {
                optometristacorreo = _usercorreo;
            }
            else
            {
                Console.Write("correo del optometrista: ");
                optometristacorreo = Console.ReadLine()?.Trim();
                if (string.IsNullOrEmpty(optometristacorreo))
                {
                    Console.WriteLine("\ncorreo de optometrista requerido. Presione cualquier tecla para continuar...");
                    Console.ReadKey();
                    return;
                }
            }

            Console.Write("Nombre del producto: ");
            var nombre = Console.ReadLine()?.Trim();
            if (string.IsNullOrEmpty(nombre))
            {
                Console.WriteLine("\nNombre requerido. Presione cualquier tecla para continuar...");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("\nTipo de producto:");
            Console.WriteLine("1. Lentes");
            Console.WriteLine("2. Armazón");
            Console.WriteLine("3. Accesorio");
            var tipo = Console.ReadLine()?.Trim() switch
            {
                "1" => "Lentes",
                "2" => "Armazón",
                "3" => "Accesorio",
                _ => null
            };

            if (tipo == null)
            {
                Console.WriteLine("\nTipo de producto inválido. Presione cualquier tecla para continuar...");
                Console.ReadKey();
                return;
            }

            Console.Write("Descripción: ");
            var descripcion = Console.ReadLine()?.Trim();
            if (string.IsNullOrEmpty(descripcion))
            {
                Console.WriteLine("\nDescripción requerida. Presione cualquier tecla para continuar...");
                Console.ReadKey();
                return;
            }

            Console.Write("Precio: ");
            if (!decimal.TryParse(Console.ReadLine(), out var precio))
            {
                Console.WriteLine("\nPrecio inválido. Presione cualquier tecla para continuar...");
                Console.ReadKey();
                return;
            }

            Console.Write("Fecha de entrega estimada (dd/MM/yyyy): ");
            if (!DateTime.TryParse(Console.ReadLine(), out var fechaEntrega))
            {
                Console.WriteLine("\nFecha inválida. Presione cualquier tecla para continuar...");
                Console.ReadKey();
                return;
            }

            Console.Write("Stock inicial: ");
            if (!int.TryParse(Console.ReadLine(), out var stock))
            {
                Console.WriteLine("\nStock inválido. Presione cualquier tecla para continuar...");
                Console.ReadKey();
                return;
            }

            Console.Write("Observaciones: ");
            var observaciones = Console.ReadLine()?.Trim();

            var producto = new Producto
            {
                Id = Guid.NewGuid().ToString(),
                Nombre = nombre,
                Tipo = tipo,
                Descripcion = descripcion,
                Precio = precio,
                Pacientecorreo = pacientecorreo,
                Optometristacorreo = optometristacorreo,
                FechaVenta = DateTime.UtcNow,
                FechaEntregaEstimada = fechaEntrega,
                Estado = "Pendiente",
                Stock = stock,
                Observaciones = observaciones ?? ""
            };

            await _productoService.CreateAsync(producto);
            Console.WriteLine("\nProducto registrado exitosamente. Presione cualquier tecla para continuar...");
            Console.ReadKey();
        }

        private async Task ActualizarProductoAsync()
        {
            Console.Clear();
            Console.WriteLine("=== Actualizar Producto ===\n");

            Console.Write("ID del producto: ");
            var productoId = Console.ReadLine()?.Trim();
            if (string.IsNullOrEmpty(productoId))
            {
                Console.WriteLine("\nID de producto inválido. Presione cualquier tecla para continuar...");
                Console.ReadKey();
                return;
            }

            var producto = await _productoService.GetByIdAsync(productoId);
            if (producto == null)
            {
                Console.WriteLine("\nProducto no encontrado. Presione cualquier tecla para continuar...");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("\nIngrese los nuevos datos (deje en blanco para mantener el valor actual):");

            Console.Write($"Nombre [{producto.Nombre}]: ");
            var nombre = Console.ReadLine()?.Trim();
            if (!string.IsNullOrEmpty(nombre))
                producto.Nombre = nombre;

            Console.WriteLine($"\nTipo actual: {producto.Tipo}");
            Console.WriteLine("Nuevo tipo:");
            Console.WriteLine("1. Lentes");
            Console.WriteLine("2. Armazón");
            Console.WriteLine("3. Accesorio");
            var tipo = Console.ReadLine()?.Trim() switch
            {
                "1" => "Lentes",
                "2" => "Armazón",
                "3" => "Accesorio",
                _ => null
            };
            if (tipo != null)
                producto.Tipo = tipo;

            Console.Write($"Descripción [{producto.Descripcion}]: ");
            var descripcion = Console.ReadLine()?.Trim();
            if (!string.IsNullOrEmpty(descripcion))
                producto.Descripcion = descripcion;

            Console.Write($"Precio [{producto.Precio:C}]: ");
            if (decimal.TryParse(Console.ReadLine(), out var precio))
                producto.Precio = precio;

            Console.Write($"Fecha de entrega estimada [{producto.FechaEntregaEstimada:dd/MM/yyyy}]: ");
            if (DateTime.TryParse(Console.ReadLine(), out var fechaEntrega))
                producto.FechaEntregaEstimada = fechaEntrega;

            Console.WriteLine($"\nEstado actual: {producto.Estado}");
            Console.WriteLine("Nuevo estado:");
            Console.WriteLine("1. En proceso");
            Console.WriteLine("2. Listo para entrega");
            Console.WriteLine("3. Entregado");
            Console.WriteLine("4. Cancelado");
            var estado = Console.ReadLine()?.Trim() switch
            {
                "1" => "En proceso",
                "2" => "Listo para entrega",
                "3" => "Entregado",
                "4" => "Cancelado",
                _ => null
            };
            if (estado != null)
                producto.Estado = estado;

            Console.Write($"Observaciones [{producto.Observaciones}]: ");
            var observaciones = Console.ReadLine()?.Trim();
            if (!string.IsNullOrEmpty(observaciones))
                producto.Observaciones = observaciones;

            var (success, message) = await _productoService.UpdateAsync(productoId, producto);
            if (success)
            {
                Console.WriteLine("\nProducto actualizado exitosamente. Presione cualquier tecla para continuar...");
            }
            else
            {
                Console.WriteLine($"\nError al actualizar el producto: {message}");
            }
            Console.ReadKey();
        }

        private async Task ActualizarStockAsync()
        {
            Console.Clear();
            Console.WriteLine("=== Actualizar Stock ===\n");

            Console.Write("ID del producto: ");
            var productoId = Console.ReadLine()?.Trim();
            if (string.IsNullOrEmpty(productoId))
            {
                Console.WriteLine("\nID de producto inválido. Presione cualquier tecla para continuar...");
                Console.ReadKey();
                return;
            }

            var producto = await _productoService.GetByIdAsync(productoId);
            if (producto == null)
            {
                Console.WriteLine("\nProducto no encontrado. Presione cualquier tecla para continuar...");
                Console.ReadKey();
                return;
            }

            Console.WriteLine($"\nStock actual: {producto.Stock}");
            Console.Write("Nuevo stock: ");
            if (!int.TryParse(Console.ReadLine(), out var nuevoStock))
            {
                Console.WriteLine("\nStock inválido. Presione cualquier tecla para continuar...");
                Console.ReadKey();
                return;
            }

            var (success, message) = await _productoService.UpdateStockAsync(productoId, nuevoStock);
            if (success)
            {
                Console.WriteLine("\nStock actualizado exitosamente. Presione cualquier tecla para continuar...");
            }
            else
            {
                Console.WriteLine($"\nError al actualizar el stock: {message}");
            }
            Console.ReadKey();
        }
    }
}
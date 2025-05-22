using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using NoSQL.Application.Services.Interfaces;
using NoSQL.Domain.Entities;

namespace NoSQL.CLI.Menus
{
    public class ProductSalesMenu : BaseMenu
    {
        private readonly IProductoService _productoService;
        private readonly IPacienteService _pacienteService;

        public ProductSalesMenu(IServiceProvider serviceProvider, string currentUserEmail, string currentUserRole)
            : base(serviceProvider, currentUserEmail, currentUserRole)
        {
            _productoService = serviceProvider.GetRequiredService<IProductoService>();
            _pacienteService = serviceProvider.GetRequiredService<IPacienteService>();
        }

        public async Task HandleOptionAsync(string option)
        {
            switch (option)
            {
                case "1":
                    await CreateProductSale();
                    break;
                case "2":
                    await ViewProductSales();
                    break;
                case "3":
                    await MarkProductAsReady();
                    break;
            }
        }

        private async Task CreateProductSale()
        {
            ClearScreen();
            ShowHeader("Nueva Venta de Producto");

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

            var producto = new Producto
            {
                PacienteEmail = pacienteEmail,
                OptometristaEmail = _currentUserEmail,
                Nombre = GetUserInput("Nombre del producto: "),
                Tipo = GetUserInput("Tipo de producto (Lentes/Armazón/Accesorio): "),
                Descripcion = GetUserInput("Descripción: "),
                Precio = decimal.Parse(GetUserInput("Precio: ")),
                FechaVenta = DateTime.Now,
                FechaEntregaEstimada = DateTime.Parse(GetUserInput("Fecha estimada de entrega (dd/MM/yyyy): ")),
                Estado = "En proceso"
            };

            var result = await _productoService.CreateAsync(producto);
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

        private async Task ViewProductSales()
        {
            ClearScreen();
            ShowHeader("Ventas de Productos");

            Console.WriteLine("1. Ver todas mis ventas");
            Console.WriteLine("2. Buscar por paciente");
            string option = GetUserInput("\nSeleccione una opción: ");

            IEnumerable<Producto> productos;
            if (option == "1")
            {
                productos = await _productoService.GetByOptometristaEmailAsync(_currentUserEmail);
            }
            else
            {
                string pacienteEmail = GetUserInput("\nCorreo del paciente: ");
                productos = await _productoService.GetByPacienteEmailAsync(pacienteEmail);
            }

            if (productos != null && productos.Any())
            {
                foreach (var producto in productos.OrderByDescending(p => p.FechaVenta))
                {
                    Console.WriteLine("\n----------------------------------------");
                    Console.WriteLine($"Fecha de venta: {producto.FechaVenta:dd/MM/yyyy HH:mm}");
                    Console.WriteLine($"Paciente: {producto.PacienteEmail}");
                    Console.WriteLine($"Tipo: {producto.Tipo}");
                    Console.WriteLine($"Descripción: {producto.Descripcion}");
                    Console.WriteLine($"Precio: ${producto.Precio:N2}");
                    Console.WriteLine($"Fecha estimada de entrega: {producto.FechaEntregaEstimada:dd/MM/yyyy}");
                    Console.WriteLine($"Estado: {producto.Estado}");
                    Console.WriteLine("----------------------------------------");
                }
            }
            else
            {
                Console.WriteLine("\nNo se encontraron ventas.");
            }

            ShowFooter();
        }

        private async Task MarkProductAsReady()
        {
            ClearScreen();
            ShowHeader("Marcar Producto como Listo");

            string id = GetUserInput("ID del producto: ");
            var result = await _productoService.UpdateStatusAsync(id, "Listo para entrega");

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

        public override async Task ShowAsync()
        {
            while (true)
            {
                ClearScreen();
                ShowHeader("Gestión de Ventas de Productos");
                Console.WriteLine("1. Nueva Venta");
                Console.WriteLine("2. Ver Ventas Realizadas");
                Console.WriteLine("3. Marcar Producto como Entregado");
                Console.WriteLine("4. Volver al menú principal");
                Console.Write("\nSeleccione una opción: ");

                string option = Console.ReadLine();
                if (option == "4") break;

                await HandleOptionAsync(option);
            }
        }
    }
} 
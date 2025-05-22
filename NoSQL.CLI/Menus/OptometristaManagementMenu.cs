using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using NoSQL.Application.Services.Interfaces;
using NoSQL.Domain.Entities;

namespace NoSQL.CLI.Menus
{
    public class OptometristaManagementMenu : BaseMenu
    {
        private readonly IOptometristaService _optometristaService;

        public OptometristaManagementMenu(IServiceProvider serviceProvider, string currentUserEmail, string currentUserRole)
            : base(serviceProvider, currentUserEmail, currentUserRole)
        {
            _optometristaService = serviceProvider.GetRequiredService<IOptometristaService>();
        }

        public async Task HandleOptionAsync(string option)
        {
            switch (option)
            {
                case "1":
                    await AddOptometrista();
                    break;
                case "2":
                    await DeleteOptometrista();
                    break;
                case "3":
                    await ModifyOptometrista();
                    break;
                case "4":
                    await ListOptometristas();
                    break;
            }
        }

        private async Task AddOptometrista()
        {
            ClearScreen();
            ShowHeader("Agregar Optometrista");

            var optometrista = new Optometrista
            {
                Nombre = GetUserInput("Nombre: "),
                Apellido = GetUserInput("Apellido: "),
                CedulaProfesional = GetUserInput("Cédula Profesional: "),
                Especialidad = GetUserInput("Especialidad: "),
                Correo = GetUserInput("Correo: "),
                Telefono = GetUserInput("Teléfono: "),
                Celular = GetUserInput("Celular: "),
                NumeroEmergencia = GetUserInput("Número de Emergencia: ")
            };

            var result = await _optometristaService.CreateAsync(optometrista);
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

        private async Task DeleteOptometrista()
        {
            ClearScreen();
            ShowHeader("Eliminar Optometrista");

            string email = GetUserInput("Correo del optometrista a eliminar: ");
            var result = await _optometristaService.DeleteAsync(email);

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

        private async Task ModifyOptometrista()
        {
            ClearScreen();
            ShowHeader("Modificar Optometrista");

            string email = GetUserInput("Correo del optometrista a modificar: ");
            var existing = await _optometristaService.GetByEmailAsync(email);

            if (existing == null)
            {
                ShowError("Optometrista no encontrado");
                ShowFooter();
                return;
            }

            var optometrista = new Optometrista
            {
                Nombre = GetUserInput($"Nombre ({existing.Nombre}): ") ?? existing.Nombre,
                Apellido = GetUserInput($"Apellido ({existing.Apellido}): ") ?? existing.Apellido,
                CedulaProfesional = GetUserInput($"Cédula Profesional ({existing.CedulaProfesional}): ") ?? existing.CedulaProfesional,
                Especialidad = GetUserInput($"Especialidad ({existing.Especialidad}): ") ?? existing.Especialidad,
                Correo = GetUserInput($"Correo ({existing.Correo}): ") ?? existing.Correo,
                Telefono = GetUserInput($"Teléfono ({existing.Telefono}): ") ?? existing.Telefono,
                Celular = GetUserInput($"Celular ({existing.Celular}): ") ?? existing.Celular,
                NumeroEmergencia = GetUserInput($"Número de Emergencia ({existing.NumeroEmergencia}): ") ?? existing.NumeroEmergencia
            };

            var result = await _optometristaService.UpdateAsync(email, optometrista);
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

        private async Task ListOptometristas()
        {
            ClearScreen();
            ShowHeader("Lista de Optometristas");

            var optometristas = await _optometristaService.GetAllAsync();
            foreach (var opt in optometristas)
            {
                Console.WriteLine($"\nNombre: {opt.Nombre}");
                Console.WriteLine($"Cédula: {opt.CedulaProfesional}");
                Console.WriteLine($"Especialidad: {opt.Especialidad}");
                Console.WriteLine($"Correo: {opt.Correo}");
                Console.WriteLine($"Teléfono: {opt.Telefono}");
                Console.WriteLine(new string('-', 40));
            }

            ShowFooter();
        }

        public override async Task ShowAsync()
        {
            while (true)
            {
                ClearScreen();
                ShowHeader("Gestión de Optometristas");
                Console.WriteLine("1. Agregar Optometrista");
                Console.WriteLine("2. Eliminar Optometrista");
                Console.WriteLine("3. Modificar Optometrista");
                Console.WriteLine("4. Consultar Optometristas");
                Console.WriteLine("5. Volver al menú principal");
                Console.Write("\nSeleccione una opción: ");

                string option = Console.ReadLine();
                if (option == "5") break;

                await HandleOptionAsync(option);
            }
        }
    }
} 
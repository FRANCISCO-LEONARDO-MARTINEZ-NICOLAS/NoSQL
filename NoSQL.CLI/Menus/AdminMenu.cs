using System;
using System.Threading.Tasks;
using NoSQL.Application.Interfaces;
using NoSQL.Domain.Entities;

namespace NoSQL.CLI.Menus
{
    public class AdminMenu : BaseMenu
    {
        private readonly IUsuarioService _usuarioService;

        public AdminMenu(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        public override async Task ShowAsync()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== Menú de Administración ===\n");
                Console.WriteLine("1. Ver todos los usuarios");
                Console.WriteLine("2. Buscar usuario por correo");
                Console.WriteLine("3. Registrar nuevo usuario");
                Console.WriteLine("4. Actualizar usuario");
                Console.WriteLine("5. Cambiar contraseña de usuario");
                Console.WriteLine("0. Cerrar sesión");

                var opcion = Console.ReadLine()?.Trim();

                switch (opcion)
                {
                    case "1":
                        await MostrarTodosLosUsuariosAsync();
                        break;
                    case "2":
                        await BuscarUsuarioPorcorreoAsync();
                        break;
                    case "3":
                        await RegistrarUsuarioAsync();
                        break;
                    case "4":
                        await ActualizarUsuarioAsync();
                        break;
                    case "5":
                        await CambiarContrasenaUsuarioAsync();
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

        private async Task MostrarTodosLosUsuariosAsync()
        {
            Console.Clear();
            Console.WriteLine("=== Todos los Usuarios ===\n");

            var usuarios = await _usuarioService.GetAllAsync();
            if (usuarios == null || !usuarios.Any())
            {
                Console.WriteLine("No hay usuarios registrados.");
            }
            else
            {
                foreach (var usuario in usuarios)
                {
                    Console.WriteLine($"ID: {usuario.Id}");
                    Console.WriteLine($"Nombre: {usuario.Nombre}");
                    Console.WriteLine($"correo: {usuario.correo}");
                    Console.WriteLine($"Rol: {usuario.Rol}");
                    Console.WriteLine($"Estado: {(usuario.Activo ? "Activo" : "Inactivo")}");
                    Console.WriteLine($"Fecha de creación: {usuario.FechaCreacion:dd/MM/yyyy HH:mm}");
                    Console.WriteLine($"Último acceso: {usuario.UltimoAcceso:dd/MM/yyyy HH:mm}");
                    Console.WriteLine("------------------------");
                }
            }

            Console.WriteLine("\nPresione cualquier tecla para continuar...");
            Console.ReadKey();
        }

        private async Task BuscarUsuarioPorcorreoAsync()
        {
            Console.Clear();
            Console.WriteLine("=== Buscar Usuario por correo ===\n");

            Console.Write("correo del usuario: ");
            var correo = Console.ReadLine()?.Trim();
            if (string.IsNullOrEmpty(correo))
            {
                Console.WriteLine("\ncorreo requerido. Presione cualquier tecla para continuar...");
                Console.ReadKey();
                return;
            }

            var usuario = await _usuarioService.GetBycorreoAsync(correo);
            if (usuario == null)
            {
                Console.WriteLine("\nUsuario no encontrado. Presione cualquier tecla para continuar...");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("\nDatos del usuario:");
            Console.WriteLine($"ID: {usuario.Id}");
            Console.WriteLine($"Nombre: {usuario.Nombre}");
            Console.WriteLine($"correo: {usuario.correo}");
            Console.WriteLine($"Rol: {usuario.Rol}");
            Console.WriteLine($"Estado: {(usuario.Activo ? "Activo" : "Inactivo")}");
            Console.WriteLine($"Fecha de creación: {usuario.FechaCreacion:dd/MM/yyyy HH:mm}");
            Console.WriteLine($"Último acceso: {usuario.UltimoAcceso:dd/MM/yyyy HH:mm}");

            Console.WriteLine("\nPresione cualquier tecla para continuar...");
            Console.ReadKey();
        }

        private async Task RegistrarUsuarioAsync()
        {
            Console.Clear();
            Console.WriteLine("=== Registrar Nuevo Usuario ===\n");

            Console.Write("Nombre: ");
            var nombre = Console.ReadLine()?.Trim();
            if (string.IsNullOrEmpty(nombre))
            {
                Console.WriteLine("\nNombre requerido. Presione cualquier tecla para continuar...");
                Console.ReadKey();
                return;
            }

            Console.Write("correo: ");
            var correo = Console.ReadLine()?.Trim();
            if (string.IsNullOrEmpty(correo))
            {
                Console.WriteLine("\ncorreo requerido. Presione cualquier tecla para continuar...");
                Console.ReadKey();
                return;
            }

            Console.Write("Contraseña (se guardará como hash): ");
            var password = Console.ReadLine()?.Trim();
            if (string.IsNullOrEmpty(password))
            {
                Console.WriteLine("\nContraseña requerida. Presione cualquier tecla para continuar...");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("\nRol:");
            Console.WriteLine("1. Administrador");
            Console.WriteLine("2. Optometrista");
            Console.WriteLine("3. Recepcionista");
            var rol = Console.ReadLine()?.Trim() switch
            {
                "1" => "Administrador",
                "2" => "Optometrista",
                "3" => "Recepcionista",
                _ => null
            };

            if (rol == null)
            {
                Console.WriteLine("\nRol inválido. Presione cualquier tecla para continuar...");
                Console.ReadKey();
                return;
            }

            var usuario = new Usuario
            {
                Id = Guid.NewGuid().ToString(),
                Nombre = nombre,
                correo = correo,
                Rol = rol,
                Activo = true,
                FechaCreacion = DateTime.UtcNow,
                UltimoAcceso = DateTime.UtcNow,
                type = "Usuario",
                PasswordHash = password
            };

            await _usuarioService.CreateAsync(usuario);
            // Si tienes notificación, descomenta la siguiente línea:
            // await _notificacionService.EnviarcorreoBienvenidaAsync(usuario.correo, usuario.Nombre);
            Console.WriteLine("\nUsuario registrado exitosamente. Presione cualquier tecla para continuar...");
            Console.ReadKey();
        }

        private async Task ActualizarUsuarioAsync()
        {
            Console.Clear();
            Console.WriteLine("=== Actualizar Usuario ===\n");

            Console.Write("ID del usuario: ");
            var usuarioId = Console.ReadLine()?.Trim();
            if (string.IsNullOrEmpty(usuarioId))
            {
                Console.WriteLine("\nID de usuario inválido. Presione cualquier tecla para continuar...");
                Console.ReadKey();
                return;
            }

            var usuario = await _usuarioService.GetByIdAsync(usuarioId);
            if (usuario == null)
            {
                Console.WriteLine("\nUsuario no encontrado. Presione cualquier tecla para continuar...");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("\nIngrese los nuevos datos (deje en blanco para mantener el valor actual):");

            Console.Write($"Nombre [{usuario.Nombre}]: ");
            var nombre = Console.ReadLine()?.Trim();
            if (!string.IsNullOrEmpty(nombre))
                usuario.Nombre = nombre;

            Console.Write($"correo [{usuario.correo}]: ");
            var correo = Console.ReadLine()?.Trim();
            if (!string.IsNullOrEmpty(correo))
                usuario.correo = correo;

            Console.WriteLine($"\nRol actual: {usuario.Rol}");
            Console.WriteLine("Nuevo rol:");
            Console.WriteLine("1. Administrador");
            Console.WriteLine("2. Optometrista");
            Console.WriteLine("3. Recepcionista");
            var rol = Console.ReadLine()?.Trim() switch
            {
                "1" => "Administrador",
                "2" => "Optometrista",
                "3" => "Recepcionista",
                _ => null
            };
            if (rol != null)
                usuario.Rol = rol;

            Console.WriteLine($"\nEstado actual: {(usuario.Activo ? "Activo" : "Inactivo")}");
            Console.WriteLine("Nuevo estado:");
            Console.WriteLine("1. Activo");
            Console.WriteLine("2. Inactivo");
            var estado = Console.ReadLine()?.Trim() switch
            {
                "1" => true,
                "2" => false,
                _ => usuario.Activo
            };
            usuario.Activo = estado;

            var (success, message) = await _usuarioService.UpdateAsync(usuarioId, usuario);
            Console.WriteLine(message);
            Console.WriteLine("\nUsuario actualizado exitosamente. Presione cualquier tecla para continuar...");
            Console.ReadKey();
        }

        private async Task CambiarContrasenaUsuarioAsync()
        {
            Console.Clear();
            Console.WriteLine("=== Cambiar Contraseña de Usuario ===\n");

            Console.Write("ID del usuario: ");
            var usuarioId = Console.ReadLine()?.Trim();
            if (string.IsNullOrEmpty(usuarioId))
            {
                Console.WriteLine("\nID de usuario inválido. Presione cualquier tecla para continuar...");
                Console.ReadKey();
                return;
            }

            var usuario = await _usuarioService.GetByIdAsync(usuarioId);
            if (usuario == null)
            {
                Console.WriteLine("\nUsuario no encontrado. Presione cualquier tecla para continuar...");
                Console.ReadKey();
                return;
            }

            Console.Write("Nueva contraseña (se guardará como hash): ");
            var nuevaPassword = Console.ReadLine()?.Trim();
            if (string.IsNullOrEmpty(nuevaPassword))
            {
                Console.WriteLine("\nContraseña requerida. Presione cualquier tecla para continuar...");
                Console.ReadKey();
                return;
            }

            usuario.PasswordHash = nuevaPassword;
            var (success, message) = await _usuarioService.UpdateAsync(usuarioId, usuario);
            Console.WriteLine(message);
            Console.WriteLine("\nContraseña actualizada exitosamente. Presione cualquier tecla para continuar...");
            Console.ReadKey();
        }
    }
}
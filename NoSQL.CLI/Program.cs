using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using NoSQL.CLI.Menus;
using NoSQL.Application.Services;
using NoSQL.Application.Services.Interfaces;
using NoSQL.Infrastructure;
using NoSQL.Infrastructure.Repositories;
using NoSQL.Domain.Interfaces;

namespace NoSQL.CLI
{
    class Program
    {
        private static IServiceProvider _serviceProvider;
        private static IConfiguration _configuration;
        private static string _currentUserEmail;
        private static string _currentUserRole;

        static async Task Main(string[] args)
        {
            Console.WriteLine("=== Sistema de Gestión de Clínica Optométrica ===");
            
            ConfigureServices();

            while (true)
            {
                if (string.IsNullOrEmpty(_currentUserEmail))
                {
                    var loginMenu = new LoginMenu(_serviceProvider);
                    await loginMenu.ShowAsync();
                    var (email, role) = loginMenu.GetCurrentUser();
                    _currentUserEmail = email;
                    _currentUserRole = role;
                }
                else
                {
                    if (_currentUserRole == "Admin")
                    {
                        var adminMenu = new AdminMenu(_serviceProvider, _currentUserEmail, _currentUserRole);
                        await adminMenu.ShowAsync();
                    }
                    else
                    {
                        var optometristaMenu = new OptometristaMenu(_serviceProvider, _currentUserEmail, _currentUserRole);
                        await optometristaMenu.ShowAsync();
                    }
                }
            }
        }

        private static void ConfigureServices()
        {
            _configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            var services = new ServiceCollection();

            // Registrar configuración y contexto de base de datos
            services.AddSingleton<IConfiguration>(_configuration);
            services.AddSingleton<CouchbaseDbContext>();

            // Registrar repositorios
            services.AddScoped<IUsuarioRepository, UsuarioRepository>();
            services.AddScoped<IVentaRepository, VentaRepository>();
            services.AddScoped<INotificacionRepository, NotificacionRepository>();
            services.AddScoped<PacienteRepository>();
            services.AddScoped<OptometristaRepository>();
            services.AddScoped<ConsultaRepository>();
            services.AddScoped<ProductoRepository>();
            services.AddScoped<CitaRepository>();

            // Registrar servicios de aplicación
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IOptometristaService, OptometristaService>();
            services.AddScoped<IPacienteService, PacienteService>();
            services.AddScoped<IConsultaService, ConsultaService>();
            services.AddScoped<IProductoService, ProductoService>();
            services.AddScoped<ICitaService, CitaService>();

            _serviceProvider = services.BuildServiceProvider();
        }
    }
}

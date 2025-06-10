using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NoSQL.CLI.Menus;
using NoSQL.CLI.Services;
using NoSQL.CLI.Services.Adapters;
using NoSQL.Application.Services;
using NoSQL.Infrastructure;
using NoSQL.Infrastructure.Repositories;
using NoSQL.Domain.Interfaces;
using NoSQL.Application.Interfaces;
using Microsoft.Extensions.Options;

namespace NoSQL.CLI
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var services = ConfigureServices();
            var serviceProvider = services.BuildServiceProvider();

            while (true)
            {
                var loginMenu = serviceProvider.GetRequiredService<LoginMenu>();
                var (success, user, token) = await loginMenu.ShowAsync();

                if (!success || user == null)
                {
                    Console.WriteLine("\nPresione cualquier tecla para intentar nuevamente...");
                    Console.ReadKey();
                    continue;
                }

                switch (user.Rol.ToLower())
                {
                    case "admin":
                        var adminMenu = serviceProvider.GetRequiredService<AdminMenu>();
                        await adminMenu.ShowAsync();
                        break;
                    case "optometrista":
                        var optometristaMenu = serviceProvider.GetRequiredService<OptometristaMenu>();
                        await optometristaMenu.ShowAsync();
                        break;
                    default:
                        Console.WriteLine("Rol no válido.");
                        break;
                }
            }
        }

        private static IServiceCollection ConfigureServices()
        {
            var services = new ServiceCollection();

            // Logging
            services.AddLogging();

            // Configuración
            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false)
                .Build();

            services.AddSingleton<IConfiguration>(configuration);

            // Notificaciones
            services.Configure<NoSQL.Application.Services.NotificacionOptions>(
                configuration.GetSection("Notificaciones"));

            // Couchbase
            services.AddSingleton<CouchbaseDbContext>();

            // Repositories
            services.AddScoped<IUsuarioRepository, UsuarioRepository>();
            services.AddScoped<IPacienteRepository, PacienteRepository>();
            services.AddScoped<IOptometristaRepository, OptometristaRepository>();
            services.AddScoped<IConsultaRepository, ConsultaRepository>();
            services.AddScoped<IProductoRepository, ProductoRepository>();
            services.AddScoped<ICitaRepository, CitaRepository>();
            services.AddScoped<IVentaRepository, VentaRepository>();
            services.AddScoped<INotificacionRepository, NotificacionRepository>();

            // Application Services
            services.AddScoped<AuthService>();
            services.AddScoped<PacienteService>();
            services.AddScoped<OptometristaService>();
            services.AddScoped<ConsultaService>();
            services.AddScoped<ProductoService>();
            services.AddScoped<CitaService>();
            services.AddScoped<VentaService>();
            services.AddScoped<NotificacionService>();
            services.AddScoped<INotificacionService, NotificacionService>();
            services.AddScoped<UsuarioService>();
            services.AddScoped<IUsuarioService, UsuarioService>();

            // CLI Adapters
            services.AddScoped<IAuthService, AuthServiceAdapter>();
            services.AddScoped<IPacienteService, PacienteServiceAdapter>();
            services.AddScoped<IOptometristaService, OptometristaServiceAdapter>();
            services.AddScoped<IConsultaService, ConsultaServiceAdapter>();
            services.AddScoped<IProductoService, ProductoServiceAdapter>();
            services.AddScoped<ICitaService, CitaServiceAdapter>();

            // Menus
            services.AddTransient<LoginMenu>();
            services.AddTransient<AdminMenu>();
            services.AddTransient<OptometristaMenu>();
            services.AddTransient<PacienteMenu>();
            services.AddTransient<OptometristaManagementMenu>();
            services.AddTransient<PatientManagementMenu>();
            services.AddTransient<ConsultationMenu>();
            services.AddTransient<ProductSalesMenu>();
            services.AddTransient<AppointmentMenu>();

            return services;
        }
    }
}

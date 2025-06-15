using Microsoft.AspNetCore.Mvc;
using NoSQL.Application.Interfaces;
using NoSQL.Domain.Entities;

namespace NoSQL.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DashboardController : ControllerBase
    {
        private readonly IPacienteService _pacienteService;
        private readonly ICitaService _citaService;
        private readonly IVentaService _ventaService;
        private readonly IOptometristaService _optometristaService;
        private readonly IConsultaService _consultaService;

        public DashboardController(
            IPacienteService pacienteService,
            ICitaService citaService,
            IVentaService ventaService,
            IOptometristaService optometristaService,
            IConsultaService consultaService)
        {
            _pacienteService = pacienteService;
            _citaService = citaService;
            _ventaService = ventaService;
            _optometristaService = optometristaService;
            _consultaService = consultaService;
        }

        [HttpGet("metrics")]
        public async Task<IActionResult> GetMetrics()
        {
            try
            {
                var pacientes = await _pacienteService.GetAllAsync();
                var citas = await _citaService.GetAllAsync();
                var ventas = await _ventaService.GetAllAsync();
                var optometristas = await _optometristaService.GetAllAsync();
                var consultas = await _consultaService.GetAllAsync();

                var today = DateTime.Today;
                var thisMonth = new DateTime(today.Year, today.Month, 1);

                var metrics = new
                {
                    totalPatients = pacientes.Count(),
                    todayAppointments = citas.Count(c => c.FechaHora.Date == today),
                    monthlyRevenue = ventas.Where(v => v.Fecha >= thisMonth).Sum(v => v.MontoTotal),
                    pendingOrders = 0, // Simplificado por ahora
                    completedConsultations = consultas.Count(),
                    activeOptometrists = optometristas.Count()
                };

                return Ok(metrics);
            }
            catch (Exception)
            {
                // Retornar métricas por defecto en caso de error
                var defaultMetrics = new
                {
                    totalPatients = 0,
                    todayAppointments = 0,
                    monthlyRevenue = 0,
                    pendingOrders = 0,
                    completedConsultations = 0,
                    activeOptometrists = 0
                };
                
                return Ok(defaultMetrics);
            }
        }
    }
} 
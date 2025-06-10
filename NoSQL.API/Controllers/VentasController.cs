using Microsoft.AspNetCore.Mvc;
using NoSQL.API.DTOs;
using NoSQL.Application.Interfaces;
using NoSQL.Domain.Entities;

namespace NoSQL.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VentasController : ControllerBase
    {
        private readonly IVentaService _ventaService;

        public VentasController(IVentaService ventaService)
        {
            _ventaService = ventaService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<VentaResponseDto>>> GetAll()
        {
            var ventas = await _ventaService.GetAllAsync();
            var response = ventas.Select(v => MapToResponseDto(v));
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<VentaResponseDto>> GetById(string id)
        {
            var venta = await _ventaService.GetByIdAsync(id);
            if (venta == null)
                return NotFound();

            return Ok(MapToResponseDto(venta));
        }

        [HttpGet("paciente/{pacienteId}")]
        public async Task<ActionResult<IEnumerable<VentaResponseDto>>> GetByPacienteId(string pacienteId)
        {
            var ventas = await _ventaService.GetByPacienteIdAsync(pacienteId);
            var response = ventas.Select(v => MapToResponseDto(v));
            return Ok(response);
        }

        [HttpGet("optometrista/{optometristaId}")]
        public async Task<ActionResult<IEnumerable<VentaResponseDto>>> GetByOptometristaId(string optometristaId)
        {
            var ventas = await _ventaService.GetByOptometristaIdAsync(optometristaId);
            var response = ventas.Select(v => MapToResponseDto(v));
            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<VentaResponseDto>> Create(CrearVentaDto dto)
        {
            var venta = new Venta
            {
                PacienteId = dto.PacienteId,
                OptometristaId = dto.OptometristaId,
                MetodoPago = dto.MetodoPago,
                Productos = dto.Productos.Select(p => new ProductoVenta
                {
                    Nombre = p.Nombre,
                    Cantidad = p.Cantidad,
                    PrecioUnitario = p.PrecioUnitario
                }).ToList()
            };

            var (success, message) = await _ventaService.CreateAsync(venta);
            if (!success)
                return BadRequest(message);

            return CreatedAtAction(nameof(GetById), new { id = venta.Id }, MapToResponseDto(venta));
        }

        [HttpPut("{id}/estado")]
        public async Task<ActionResult> UpdateEstado(string id, ActualizarEstadoVentaDto dto)
        {
            var (success, message) = await _ventaService.ActualizarEstadoAsync(id, dto.NuevoEstado);
            if (!success)
                return NotFound(message);

            return NoContent();
        }

        [HttpPost("{id}/productos")]
        public async Task<ActionResult> AddProducto(string id, CrearProductoVentaDto dto)
        {
            var producto = new ProductoVenta
            {
                Nombre = dto.Nombre,
                Cantidad = dto.Cantidad,
                PrecioUnitario = dto.PrecioUnitario
            };

            var (success, message) = await _ventaService.AgregarProductoAsync(id, producto);
            if (!success)
                return NotFound(message);

            return NoContent();
        }

        [HttpGet("{id}/total")]
        public async Task<ActionResult<decimal>> GetTotal(string id)
        {
            try
            {
                var total = await _ventaService.CalcularTotalVentaAsync(id);
                return Ok(total);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        private static VentaResponseDto MapToResponseDto(Venta venta)
        {
            return new VentaResponseDto
            {
                Id = venta.Id,
                PacienteId = venta.PacienteId,
                OptometristaId = venta.OptometristaId,
                Fecha = venta.Fecha,
                MontoTotal = venta.MontoTotal,
                MetodoPago = venta.MetodoPago,
                Estado = venta.Estado,
                Productos = venta.Productos.Select(p => new ProductoVentaResponseDto
                {
                    Nombre = p.Nombre,
                    Cantidad = p.Cantidad,
                    PrecioUnitario = p.PrecioUnitario,
                    Subtotal = p.Subtotal
                }).ToList()
            };
        }
    }
}
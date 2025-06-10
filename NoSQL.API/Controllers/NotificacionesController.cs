using Microsoft.AspNetCore.Mvc;
using NoSQL.API.DTOs;
using NoSQL.Application.Interfaces;
using NoSQL.Domain.Entities;

namespace NoSQL.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotificacionesController : ControllerBase
    {
        private readonly INotificacionService _notificacionService;

        public NotificacionesController(INotificacionService notificacionService)
        {
            _notificacionService = notificacionService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<NotificacionResponseDto>>> GetAll()
        {
            var notificaciones = await _notificacionService.GetAllAsync();
            var response = notificaciones.Select(n => MapToResponseDto(n));
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<NotificacionResponseDto>> GetById(string id)
        {
            var notificacion = await _notificacionService.GetByIdAsync(id);
            if (notificacion == null)
                return NotFound();

            return Ok(MapToResponseDto(notificacion));
        }

        [HttpGet("paciente/{pacienteId}")]
        public async Task<ActionResult<IEnumerable<NotificacionResponseDto>>> GetByPacienteId(string pacienteId)
        {
            var notificaciones = await _notificacionService.GetByPacienteIdAsync(pacienteId);
            var response = notificaciones.Select(n => MapToResponseDto(n));
            return Ok(response);
        }

        [HttpGet("pendientes")]
        public async Task<ActionResult<IEnumerable<NotificacionResponseDto>>> GetPendientes()
        {
            var notificaciones = await _notificacionService.GetPendientesAsync();
            var response = notificaciones.Select(n => MapToResponseDto(n));
            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<NotificacionResponseDto>> Create(CrearNotificacionDto dto)
        {
            var notificacion = new Notificacion
            {
                PacienteId = dto.PacienteId,
                Correo = dto.Correo,
                Mensaje = dto.Mensaje
            };

            var (success, message) = await _notificacionService.CreateAsync(notificacion);
            if (!success)
                return BadRequest(message);

            return CreatedAtAction(nameof(GetById), new { id = notificacion.Id }, MapToResponseDto(notificacion));
        }

        [HttpPost("{id}/enviar")]
        public async Task<ActionResult> EnviarNotificacion(string id)
        {
            var (success, message) = await _notificacionService.EnviarNotificacionAsync(id);
            if (!success)
                return NotFound(message);

            return NoContent();
        }

        [HttpPost("producto-listo")]
        public async Task<ActionResult> NotificarProductoListo(string pacienteId, string nombreProducto)
        {
            var (success, message) = await _notificacionService.CrearNotificacionProductoListoAsync(pacienteId, nombreProducto);
            if (!success)
                return BadRequest(message);

            return NoContent();
        }

        private static NotificacionResponseDto MapToResponseDto(Notificacion notificacion)
        {
            return new NotificacionResponseDto
            {
                Id = notificacion.Id,
                PacienteId = notificacion.PacienteId,
                Correo = notificacion.Correo,
                Mensaje = notificacion.Mensaje,
                FechaEnvio = notificacion.FechaEnvio,
                Estado = notificacion.Estado,
                Error = notificacion.Error
            };
        }
    }
}
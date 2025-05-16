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
        public async Task<ActionResult<NotificacionResponseDto>> GetById(Guid id)
        {
            var notificacion = await _notificacionService.GetByIdAsync(id);
            if (notificacion == null)
                return NotFound();

            return Ok(MapToResponseDto(notificacion));
        }

        [HttpGet("paciente/{pacienteId}")]
        public async Task<ActionResult<IEnumerable<NotificacionResponseDto>>> GetByPacienteId(Guid pacienteId)
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

            var notificacionCreada = await _notificacionService.CreateAsync(notificacion);
            return CreatedAtAction(nameof(GetById), new { id = notificacionCreada.Id }, MapToResponseDto(notificacionCreada));
        }

        [HttpPost("{id}/enviar")]
        public async Task<ActionResult> EnviarNotificacion(Guid id)
        {
            var resultado = await _notificacionService.EnviarNotificacionAsync(id);
            if (!resultado)
                return NotFound();

            return NoContent();
        }

        [HttpPost("producto-listo")]
        public async Task<ActionResult> NotificarProductoListo(Guid pacienteId, string nombreProducto)
        {
            var resultado = await _notificacionService.CrearNotificacionProductoListoAsync(pacienteId, nombreProducto);
            if (!resultado)
                return BadRequest("No se pudo enviar la notificación");

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
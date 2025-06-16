using Microsoft.AspNetCore.Mvc;
using NoSQL.Application.Interfaces;
using NoSQL.Domain.Entities;
using System.Linq;

namespace NoSQL.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AppointmentsController : ControllerBase
    {
        private readonly ICitaService _citaService;
        private readonly IPacienteService _pacienteService;
        private readonly IOptometristaService _optometristaService;

        public AppointmentsController(
            ICitaService citaService,
            IPacienteService pacienteService,
            IOptometristaService optometristaService)
        {
            _citaService = citaService;
            _pacienteService = pacienteService;
            _optometristaService = optometristaService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var citas = await _citaService.GetAllAsync();
                var appointments = await MapCitasToAppointments(citas.ToList());
                return Ok(appointments);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error interno del servidor", error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            try
            {
                var cita = await _citaService.GetByIdAsync(id);
                if (cita == null)
                    return NotFound();

                var appointment = await MapCitaToAppointment(cita);
                return Ok(appointment);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error interno del servidor", error = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateAppointmentRequest request)
        {
            try
            {
                // Obtener datos del paciente y optometrista
                var paciente = await _pacienteService.GetByIdAsync(request.PatientId);
                var optometrista = await _optometristaService.GetByIdAsync(request.OptometristId);

                if (paciente == null)
                    return BadRequest("Paciente no encontrado");
                if (optometrista == null)
                    return BadRequest("Optometrista no encontrado");

                var cita = new Cita
                {
                    Id = Guid.NewGuid().ToString(),
                    PacienteId = request.PatientId,
                    OptometristaId = request.OptometristId,
                    Pacientecorreo = paciente.correo,
                    Optometristacorreo = optometrista.Correo,
                    FechaHora = DateTime.Parse($"{request.Date} {request.Time}"),
                    Tipo = MapTypeToSpanish(request.Type),
                    Motivo = request.Notes ?? "Consulta programada",
                    Estado = "Programada",
                    Observaciones = request.Notes,
                    Notas = request.Notes,
                    type = "cita"
                };

                var (success, message) = await _citaService.CreateAsync(cita);
                if (!success)
                    return BadRequest(message);

                var appointment = await MapCitaToAppointment(cita);
                return CreatedAtAction(nameof(GetById), new { id = cita.Id }, appointment);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error interno del servidor", error = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody] UpdateAppointmentRequest request)
        {
            try
            {
                var existingCita = await _citaService.GetByIdAsync(id);
                if (existingCita == null)
                    return NotFound();

                if (request.Date != null && request.Time != null)
                    existingCita.FechaHora = DateTime.Parse($"{request.Date} {request.Time}");
                if (request.Type != null)
                    existingCita.Tipo = MapTypeToSpanish(request.Type);
                if (request.Status != null)
                    existingCita.Estado = MapStatusToSpanish(request.Status);
                if (request.Notes != null)
                    existingCita.Notas = request.Notes;

                await _citaService.UpdateAsync(id, existingCita);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error interno del servidor", error = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                await _citaService.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error interno del servidor", error = ex.Message });
            }
        }

        [HttpPatch("{id}/status")]
        public async Task<IActionResult> UpdateStatus(string id, [FromBody] UpdateStatusRequest request)
        {
            try
            {
                var existingCita = await _citaService.GetByIdAsync(id);
                if (existingCita == null)
                    return NotFound();

                existingCita.Estado = MapStatusToSpanish(request.Status);
                await _citaService.UpdateAsync(id, existingCita);

                var appointment = await MapCitaToAppointment(existingCita);
                return Ok(appointment);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error interno del servidor", error = ex.Message });
            }
        }

        [HttpGet("date/{date}")]
        public async Task<IActionResult> GetByDate(string date)
        {
            try
            {
                if (!DateTime.TryParse(date, out var parsedDate))
                    return BadRequest("Fecha inválida");

                var citas = await _citaService.GetAllAsync();
                var dateCitas = citas.Where(c => c.FechaHora.Date == parsedDate.Date).ToList();
                var appointments = await MapCitasToAppointments(dateCitas);
                return Ok(appointments);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error interno del servidor", error = ex.Message });
            }
        }

        [HttpGet("patient/{patientId}")]
        public async Task<IActionResult> GetByPatient(string patientId)
        {
            try
            {
                var citas = await _citaService.GetAllAsync();
                var patientCitas = citas.Where(c => c.PacienteId == patientId).ToList();
                var appointments = await MapCitasToAppointments(patientCitas);
                return Ok(appointments);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error interno del servidor", error = ex.Message });
            }
        }

        private async Task<List<AppointmentDto>> MapCitasToAppointments(List<Cita> citas)
        {
            var appointments = new List<AppointmentDto>();
            foreach (var cita in citas)
            {
                appointments.Add(await MapCitaToAppointment(cita));
            }
            return appointments;
        }

        private async Task<AppointmentDto> MapCitaToAppointment(Cita cita)
        {
            var paciente = await _pacienteService.GetByIdAsync(cita.PacienteId);
            var optometrista = await _optometristaService.GetByIdAsync(cita.OptometristaId);

            return new AppointmentDto
            {
                Id = cita.Id,
                PatientId = cita.PacienteId,
                PatientName = paciente != null ? $"{paciente.Nombre} {paciente.Apellido}" : "Paciente Desconocido",
                OptometristId = cita.OptometristaId,
                OptometristName = optometrista != null ? $"Dr. {optometrista.Nombre} {optometrista.Apellido}" : "Optometrista Desconocido",
                Date = cita.FechaHora.ToString("yyyy-MM-dd"),
                Time = cita.FechaHora.ToString("HH:mm"),
                Type = MapTypeToEnglish(cita.Tipo),
                Status = MapStatusToEnglish(cita.Estado),
                Notes = cita.Notas
            };
        }

        private string MapTypeToEnglish(string tipo)
        {
            return tipo?.ToLower() switch
            {
                "consulta" => "consultation",
                "seguimiento" => "follow-up",
                "urgencia" => "emergency",
                _ => "consultation"
            };
        }

        private string MapTypeToSpanish(string type)
        {
            return type?.ToLower() switch
            {
                "consultation" => "Consulta",
                "follow-up" => "Seguimiento",
                "emergency" => "Urgencia",
                _ => "Consulta"
            };
        }

        private string MapStatusToEnglish(string estado)
        {
            return estado?.ToLower() switch
            {
                "programada" => "scheduled",
                "en curso" => "in-progress",
                "completada" => "completed",
                "cancelada" => "cancelled",
                _ => "scheduled"
            };
        }

        private string MapStatusToSpanish(string status)
        {
            return status?.ToLower() switch
            {
                "scheduled" => "Programada",
                "in-progress" => "En Curso",
                "completed" => "Completada",
                "cancelled" => "Cancelada",
                _ => "Programada"
            };
        }
    }

    public class CreateAppointmentRequest
    {
        public string PatientId { get; set; } = string.Empty;
        public string OptometristId { get; set; } = string.Empty;
        public string Date { get; set; } = string.Empty;
        public string Time { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string? Notes { get; set; }
    }

    public class UpdateAppointmentRequest
    {
        public string? Date { get; set; }
        public string? Time { get; set; }
        public string? Type { get; set; }
        public string? Status { get; set; }
        public string? Notes { get; set; }
    }

    public class UpdateStatusRequest
    {
        public string Status { get; set; } = string.Empty;
    }

    public class AppointmentDto
    {
        public string Id { get; set; } = string.Empty;
        public string PatientId { get; set; } = string.Empty;
        public string PatientName { get; set; } = string.Empty;
        public string OptometristId { get; set; } = string.Empty;
        public string OptometristName { get; set; } = string.Empty;
        public string Date { get; set; } = string.Empty;
        public string Time { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string? Notes { get; set; }
    }
} 
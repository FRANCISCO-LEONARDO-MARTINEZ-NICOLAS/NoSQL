using Microsoft.AspNetCore.Mvc;
using NoSQL.Application.Interfaces;
using NoSQL.Domain.Entities;

namespace NoSQL.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CitasController : ControllerBase
    {
        private readonly ICitaService _service;

        public CitasController(ICitaService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var citas = await _service.GetAllAsync();
            return Ok(citas);
        }

        [HttpGet("today")]
        public async Task<IActionResult> GetToday()
        {
            try
            {
                var citas = await _service.GetAllAsync();
                var today = DateTime.Today;
                var todayCitas = citas.Where(c => c.FechaHora.Date == today).ToList();

                var result = todayCitas.Select(c => new
                {
                    id = c.Id,
                    time = c.FechaHora.ToString("HH:mm"),
                    patientName = "Paciente", // Simplificado por ahora
                    type = c.Tipo ?? "Consulta",
                    status = c.Estado?.ToLower() ?? "programada",
                    duration = "30 min"
                });

                return Ok(result);
            }
            catch (Exception)
            {
                // Retornar lista vacía en caso de error
                return Ok(new List<object>());
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var cita = await _service.GetByIdAsync(id);
            if (cita == null)
                return NotFound();

            return Ok(cita);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Cita cita)
        {
            var (success, message) = await _service.CreateAsync(cita);
            if (!success)
                return BadRequest(message);

            return CreatedAtAction(nameof(GetById), new { id = cita.Id }, cita);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody] Cita cita)
        {
            await _service.UpdateAsync(id, cita);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}
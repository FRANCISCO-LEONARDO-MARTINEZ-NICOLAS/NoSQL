using Microsoft.AspNetCore.Mvc;
using NoSQL.Application.Interfaces;
using NoSQL.Domain.Entities;

namespace NoSQL.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PacientesController : ControllerBase
    {
        private readonly IPacienteService _service;

        public PacientesController(IPacienteService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var pacientes = await _service.GetAllAsync();
            return Ok(pacientes);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var paciente = await _service.GetByIdAsync(id);
            if (paciente == null)
                return NotFound();

            return Ok(paciente);
        }

        [HttpGet("email/{correo}")]
        public async Task<IActionResult> GetByEmail(string correo)
        {
            var paciente = await _service.GetByEmailAsync(correo);
            if (paciente == null)
                return NotFound();

            return Ok(paciente);
        }

        [HttpGet("dni/{dni}")]
        public async Task<IActionResult> GetByDni(string dni)
        {
            var paciente = await _service.GetByDniAsync(dni);
            if (paciente == null)
                return NotFound();

            return Ok(paciente);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Paciente paciente)
        {
            var (success, message) = await _service.CreateAsync(paciente);
            if (!success)
                return BadRequest(message);

            return CreatedAtAction(nameof(GetById), new { id = paciente.Id }, paciente);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody] Paciente paciente)
        {
            var (success, message) = await _service.UpdateAsync(id, paciente);
            if (!success)
                return BadRequest(message);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var (success, message) = await _service.DeleteAsync(id);
            if (!success)
                return BadRequest(message);

            return NoContent();
        }
    }
}
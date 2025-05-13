using Microsoft.AspNetCore.Mvc;
using NoSQL.Application.Services;
using NoSQL.Domain.Entities;

namespace NoSQL.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CitasController : ControllerBase
    {
        private readonly CitaService _service;

        public CitasController(CitaService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var citas = await _service.GetAllAsync();
            return Ok(citas);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var cita = await _service.GetByIdAsync(id);
            if (cita == null)
                return NotFound();

            return Ok(cita);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Cita cita)
        {
            await _service.AddAsync(cita);
            return CreatedAtAction(nameof(GetById), new { id = cita.Id }, cita);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, [FromBody] Cita cita)
        {
            await _service.UpdateAsync(id, cita);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}
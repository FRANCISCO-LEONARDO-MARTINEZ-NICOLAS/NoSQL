using Microsoft.AspNetCore.Mvc;
using NoSQL.Application.Services;
using NoSQL.Domain.Entities;

namespace NoSQL.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ConsultasController : ControllerBase
    {
        private readonly ConsultaService _service;

        public ConsultasController(ConsultaService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var consultas = await _service.GetAllAsync();
            return Ok(consultas);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var consulta = await _service.GetByIdAsync(id);
            if (consulta == null)
                return NotFound();

            return Ok(consulta);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Consulta consulta)
        {
            await _service.AddAsync(consulta);
            return CreatedAtAction(nameof(GetById), new { id = consulta.Id }, consulta);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, [FromBody] Consulta consulta)
        {
            await _service.UpdateAsync(id, consulta);
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
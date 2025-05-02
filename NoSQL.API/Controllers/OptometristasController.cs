using Microsoft.AspNetCore.Mvc;
using NoSQL.Application.Services;
using NoSQL.Domain.Entities;

namespace NoSQL.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OptometristasController : ControllerBase
    {
        private readonly OptometristaService _service;

        public OptometristasController(OptometristaService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var optometristas = await _service.GetAllAsync();
            return Ok(optometristas);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var optometrista = await _service.GetByIdAsync(id);
            if (optometrista == null)
                return NotFound();

            return Ok(optometrista);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Optometrista optometrista)
        {
            await _service.AddAsync(optometrista);
            return CreatedAtAction(nameof(GetById), new { id = optometrista.Id }, optometrista);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, [FromBody] Optometrista optometrista)
        {
            await _service.UpdateAsync(id, optometrista);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }

        [HttpGet("test-connection")]
        public IActionResult TestConnection()
        {
            return Ok("Conexión a Couchbase exitosa");
        }
    }
}
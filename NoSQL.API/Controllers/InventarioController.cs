using Microsoft.AspNetCore.Mvc;
using NoSQL.Application.Interfaces;
using NoSQL.Domain.Entities;

namespace NoSQL.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InventarioController : ControllerBase
    {
        private readonly IProductoInventarioService _service;

        public InventarioController(IProductoInventarioService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var productos = await _service.GetAllAsync();
            return Ok(productos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var producto = await _service.GetByIdAsync(id);
            if (producto == null)
                return NotFound();

            return Ok(producto);
        }

        [HttpGet("tipo/{tipo}")]
        public async Task<IActionResult> GetByTipo(string tipo)
        {
            var productos = await _service.GetByTipoAsync(tipo);
            return Ok(productos);
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string q)
        {
            if (string.IsNullOrWhiteSpace(q))
                return BadRequest("Query parameter 'q' is required");

            var productos = await _service.SearchAsync(q);
            return Ok(productos);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ProductoInventario producto)
        {
            var (success, message) = await _service.CreateAsync(producto);
            if (!success)
                return BadRequest(message);

            return CreatedAtAction(nameof(GetById), new { id = producto.Id }, producto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody] ProductoInventario producto)
        {
            var (success, message) = await _service.UpdateAsync(id, producto);
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

        [HttpPut("{id}/stock")]
        public async Task<IActionResult> UpdateStock(string id, [FromBody] UpdateStockRequest request)
        {
            var (success, message) = await _service.UpdateStockAsync(id, request.Cantidad);
            if (!success)
                return BadRequest(message);

            return NoContent();
        }
    }

    public class UpdateStockRequest
    {
        public int Cantidad { get; set; }
    }
} 
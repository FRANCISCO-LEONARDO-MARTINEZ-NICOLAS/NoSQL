using Microsoft.AspNetCore.Mvc;
using NoSQL.Application.Interfaces;
using NoSQL.Domain.Entities;
using System.Security.Cryptography;
using System.Text;
using BCrypt.Net;

namespace NoSQL.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OptometristasController : ControllerBase
    {
        private readonly IOptometristaService _service;

        public OptometristasController(IOptometristaService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var optometristas = await _service.GetAllAsync();
                return Ok(optometristas);
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
                if (string.IsNullOrEmpty(id))
                    return BadRequest(new { message = "ID es requerido" });

                var optometrista = await _service.GetByIdAsync(id);
                if (optometrista == null)
                    return NotFound(new { message = "Optometrista no encontrado" });

                return Ok(optometrista);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error interno del servidor", error = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Optometrista optometrista)
        {
            try
            {
                if (optometrista == null)
                    return BadRequest(new { message = "Datos del optometrista son requeridos" });

                // Validaciones básicas
                if (string.IsNullOrEmpty(optometrista.Nombre) || string.IsNullOrEmpty(optometrista.Apellido))
                    return BadRequest(new { message = "Nombre y apellido son requeridos" });

                if (string.IsNullOrEmpty(optometrista.CedulaProfesional))
                    return BadRequest(new { message = "Cédula profesional es requerida" });

                if (string.IsNullOrEmpty(optometrista.Correo))
                    return BadRequest(new { message = "Correo electrónico es requerido" });

                var (success, message) = await _service.CreateAsync(optometrista);
                if (!success)
                    return BadRequest(new { message });

                return CreatedAtAction(nameof(GetById), new { id = optometrista.Id }, optometrista);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error interno del servidor", error = ex.Message });
            }
        }

        [HttpPost("{id}/credentials")]
        public async Task<IActionResult> CreateCredentials(string id, [FromBody] CreateCredentialsRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                    return BadRequest(new { message = "ID es requerido" });

                if (string.IsNullOrEmpty(request.Username) || string.IsNullOrEmpty(request.Password))
                    return BadRequest(new { message = "Usuario y contraseña son requeridos" });

                var optometrista = await _service.GetByIdAsync(id);
                if (optometrista == null)
                    return NotFound(new { message = "Optometrista no encontrado" });

                // Generar hash de la contraseña
                var passwordHash = HashPassword(request.Password);
                
                optometrista.Username = request.Username;
                optometrista.PasswordHash = passwordHash;
                optometrista.HasLoginCredentials = true;

                var (success, message) = await _service.UpdateAsync(id, optometrista);
                if (!success)
                    return BadRequest(new { message });

                return Ok(new { message = "Credenciales creadas exitosamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error interno del servidor", error = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody] Optometrista optometrista)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                    return BadRequest(new { message = "ID es requerido" });

                if (optometrista == null)
                    return BadRequest(new { message = "Datos del optometrista son requeridos" });

                var (success, message) = await _service.UpdateAsync(id, optometrista);
                if (!success)
                    return BadRequest(new { message });

                return Ok(new { message = "Optometrista actualizado exitosamente" });
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
                if (string.IsNullOrEmpty(id))
                    return BadRequest(new { message = "ID es requerido" });

                var (success, message) = await _service.DeleteAsync(id);
                if (!success)
                    return BadRequest(new { message });

                return Ok(new { message = "Optometrista eliminado exitosamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error interno del servidor", error = ex.Message });
            }
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string? name, [FromQuery] string? specialty, [FromQuery] string? email)
        {
            try
            {
                var allOptometristas = await _service.GetAllAsync();
                var results = allOptometristas.AsEnumerable();

                if (!string.IsNullOrEmpty(name))
                {
                    var searchTerm = name.ToLower();
                    results = results.Where(o => 
                        o.Nombre.ToLower().Contains(searchTerm) ||
                        o.Apellido.ToLower().Contains(searchTerm) ||
                        $"{o.Nombre} {o.Apellido}".ToLower().Contains(searchTerm)
                    );
                }

                if (!string.IsNullOrEmpty(specialty))
                {
                    var searchTerm = specialty.ToLower();
                    results = results.Where(o => o.Especialidad.ToLower().Contains(searchTerm));
                }

                if (!string.IsNullOrEmpty(email))
                {
                    var searchTerm = email.ToLower();
                    results = results.Where(o => o.Correo.ToLower().Contains(searchTerm));
                }

                return Ok(results.ToList());
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error interno del servidor", error = ex.Message });
            }
        }

        [HttpGet("active")]
        public async Task<IActionResult> GetActive()
        {
            try
            {
                var allOptometristas = await _service.GetAllAsync();
                var activeOptometristas = allOptometristas.Where(o => o.Activo).ToList();
                return Ok(activeOptometristas);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error interno del servidor", error = ex.Message });
            }
        }

        [HttpGet("test-connection")]
        public IActionResult TestConnection()
        {
            return Ok(new { message = "Conexión a Couchbase exitosa", timestamp = DateTime.UtcNow });
        }

        [HttpPost("test-create")]
        public async Task<IActionResult> TestCreate()
        {
            try
            {
                var testOptometrista = new Optometrista
                {
                    Nombre = "Dr. Test",
                    Apellido = "Optometrista",
                    CedulaProfesional = "TEST123",
                    Especialidad = "Optometría General",
                    Correo = "test@opticare.com",
                    Celular = "555-1234",
                    NumeroEmergencia = "555-5678",
                    Telefono = "555-9999",
                    Direccion = "Calle Test 123",
                    FechaContratacion = DateTime.UtcNow,
                    Activo = true,
                    type = "Optometrista"
                };

                var (success, message) = await _service.CreateAsync(testOptometrista);
                if (success)
                {
                    return Ok(new { message = "Optometrista de prueba creado exitosamente", id = testOptometrista.Id });
                }
                else
                {
                    return BadRequest(new { message });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error interno del servidor", error = ex.Message });
            }
        }

        [HttpPost("{id}/update-password")]
        public async Task<IActionResult> UpdatePassword(string id, [FromBody] UpdatePasswordRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                    return BadRequest(new { message = "ID es requerido" });

                if (string.IsNullOrEmpty(request.Password))
                    return BadRequest(new { message = "Contraseña es requerida" });

                var optometrista = await _service.GetByIdAsync(id);
                if (optometrista == null)
                    return NotFound(new { message = "Optometrista no encontrado" });

                // Generar hash de la contraseña con BCrypt
                var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
                
                optometrista.PasswordHash = passwordHash;
                optometrista.HasLoginCredentials = true;

                var (success, message) = await _service.UpdateAsync(id, optometrista);
                if (!success)
                    return BadRequest(new { message });

                return Ok(new { message = "Contraseña actualizada exitosamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error interno del servidor", error = ex.Message });
            }
        }

        private string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }
    }

    public class CreateCredentialsRequest
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class UpdatePasswordRequest
    {
        public string Password { get; set; } = string.Empty;
    }
}
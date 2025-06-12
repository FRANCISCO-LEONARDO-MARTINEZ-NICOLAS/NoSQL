using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NoSQL.Application.Interfaces;
using NoSQL.Domain.Entities;

namespace NoSQL.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (string.IsNullOrEmpty(request.Rol))
                return BadRequest(new { message = "El rol es requerido (Admin u Optometrista)" });

            var (success, message, user, token) = await _authService.LoginAsync(request.Correo, request.Password, request.Rol);
            if (!success)
                return BadRequest(new { message });

            return Ok(new { token, message, user });
        }

        [HttpPost("register")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            var usuario = new Usuario
            {
                Nombre = request.Nombre,
                Correo = request.Correo,
                Rol = request.Rol,
                PasswordHash = "temp",
            };

            var (success, message) = await _authService.RegisterAsync(usuario, request.Password);
            if (!success)
                return BadRequest(new { message });

            return Ok(new { message });
        }

        [HttpPost("create-test-user")]
        public async Task<IActionResult> CreateTestUser()
        {
            var usuario = new Usuario
            {
                Nombre = "Usuario de Prueba",
                Correo = "test@opticare.com",
                Rol = "admin",
                PasswordHash = "temp",
            };

            var (success, message) = await _authService.RegisterAsync(usuario, "123456");
            if (!success)
                return BadRequest(new { message });

            return Ok(new { message, user = new { correo = usuario.Correo, password = "123456" } });
        }

        [HttpPost("change-password")]
        [Authorize]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
        {
            var correo = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;
            if (string.IsNullOrEmpty(correo))
                return BadRequest(new { message = "No se pudo obtener el correo del usuario autenticado." });

            var (success, message) = await _authService.ChangePasswordAsync(correo, request.CurrentPassword, request.NewPassword);
            if (!success)
                return BadRequest(new { message });

            return Ok(new { message });
        }
    }

    public class LoginRequest
    {
        public string Correo { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Rol { get; set; } = string.Empty;
    }

    public class RegisterRequest
    {
        public string Nombre { get; set; } = string.Empty;
        public string Correo { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Rol { get; set; } = string.Empty;
    }

    public class ChangePasswordRequest
    {
        public string CurrentPassword { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
    }
} 
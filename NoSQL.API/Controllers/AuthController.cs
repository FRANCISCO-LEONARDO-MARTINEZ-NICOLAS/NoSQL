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

            var (success, message, user, token) = await _authService.LoginAsync(request.Email, request.Password, request.Rol);
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
                Email = request.Email,
                Rol = request.Rol,
                PasswordHash = "temp",
            };

            var (success, message) = await _authService.RegisterAsync(usuario, request.Password);
            if (!success)
                return BadRequest(new { message });

            return Ok(new { message });
        }

        [HttpPost("change-password")]
        [Authorize]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
        {
            var email = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;
            if (string.IsNullOrEmpty(email))
                return BadRequest(new { message = "No se pudo obtener el email del usuario autenticado." });

            var (success, message) = await _authService.ChangePasswordAsync(email, request.CurrentPassword, request.NewPassword);
            if (!success)
                return BadRequest(new { message });

            return Ok(new { message });
        }
    }

    public class LoginRequest
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Rol { get; set; } = string.Empty;
    }

    public class RegisterRequest
    {
        public string Nombre { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Rol { get; set; } = string.Empty;
    }

    public class ChangePasswordRequest
    {
        public string CurrentPassword { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
    }
} 
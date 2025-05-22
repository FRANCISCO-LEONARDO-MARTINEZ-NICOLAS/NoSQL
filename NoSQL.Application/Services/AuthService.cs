using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using NoSQL.Application.Interfaces;
using NoSQL.Domain.Entities;
using NoSQL.Domain.Interfaces;
using NoSQL.Application.Services.Interfaces;

namespace NoSQL.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IConfiguration _configuration;

        public AuthService(IUsuarioRepository usuarioRepository, IConfiguration configuration)
        {
            _usuarioRepository = usuarioRepository;
            _configuration = configuration;
        }

        public async Task<(bool Success, string Message)> LoginAsync(string email, string password, string role)
        {
            var usuario = await _usuarioRepository.GetByEmailAsync(email);
            if (usuario == null)
                return (false, "Usuario no encontrado");
            if (!usuario.Activo)
                return (false, "Usuario inactivo");
            if (usuario.Password != password)
                return (false, "Contraseña incorrecta");
            if (!string.Equals(usuario.Rol, role, StringComparison.OrdinalIgnoreCase))
                return (false, "Rol incorrecto");
            usuario.UltimoAcceso = DateTime.UtcNow;
            await _usuarioRepository.UpdateAsync(usuario);
            return (true, "Login exitoso");
        }

        public async Task<(bool Success, string Message)> RegisterAsync(string email, string password, string role)
        {
            if (await _usuarioRepository.ExistsByEmailAsync(email))
                return (false, "El email ya está registrado");
            var usuario = new Usuario
            {
                Email = email,
                Password = password,
                Rol = role,
                FechaCreacion = DateTime.UtcNow,
                Activo = true,
                Nombre = email // O pide el nombre en el registro real
            };
            await _usuarioRepository.CreateAsync(usuario);
            return (true, "Usuario registrado exitosamente");
        }

        public async Task<bool> ChangePasswordAsync(Guid userId, string currentPassword, string newPassword)
        {
            var usuario = await _usuarioRepository.GetByIdAsync(userId);
            if (usuario == null)
                return false;

            // En un entorno real, deberíamos usar BCrypt o similar para verificar y hashear las contraseñas
            if (usuario.Password != currentPassword)
                return false;

            usuario.Password = newPassword;
            await _usuarioRepository.UpdateAsync(usuario);
            return true;
        }

        private string GenerateJwtToken(Usuario usuario)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"] ?? "your-secret-key-min-16-chars"));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new Claim(ClaimTypes.Email, usuario.Email),
                new Claim(ClaimTypes.Role, usuario.Rol),
                new Claim(ClaimTypes.Name, usuario.Nombre)
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["JwtSettings:Issuer"] ?? "OpticaNoSQL",
                audience: _configuration["JwtSettings:Audience"] ?? "OpticaNoSQL",
                claims: claims,
                expires: DateTime.UtcNow.AddHours(8),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
} 
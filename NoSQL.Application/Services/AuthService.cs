using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using NoSQL.Application.Interfaces;
using NoSQL.Domain.Entities;
using NoSQL.Domain.Interfaces;

namespace NoSQL.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IOptometristaRepository _optometristaRepository;
        private readonly IConfiguration _configuration;
        private readonly INotificacionService _notificacionService;

        public AuthService(
            IUsuarioRepository usuarioRepository,
            IOptometristaRepository optometristaRepository,
            IConfiguration configuration,
            INotificacionService notificacionService)
        {
            _usuarioRepository = usuarioRepository;
            _optometristaRepository = optometristaRepository;
            _configuration = configuration;
            _notificacionService = notificacionService;
        }

        public async Task<(bool Success, string Message, Usuario? User, string? Token)> LoginAsync(string correo, string password, string rol)
        {
            // Buscar primero en usuarios normales
            var usuario = await _usuarioRepository.GetBycorreoAsync(correo);
            
            // Si no se encuentra y el rol es Optometrista, buscar en optometristas
            if (usuario == null && rol.ToLower() == "optometrista")
            {
                var optometrista = await _optometristaRepository.GetBycorreoAsync(correo);
                if (optometrista != null)
                {
                    // Convertir Optometrista a Usuario para el login
                    usuario = new Usuario
                    {
                        Id = optometrista.Id,
                        Nombre = $"{optometrista.Nombre} {optometrista.Apellido}",
                        Correo = optometrista.Correo,
                        Rol = "Optometrista",
                        PasswordHash = optometrista.PasswordHash
                    };
                }
            }

            if (usuario == null)
            {
                return (false, "Usuario no encontrado.", null, null);
            }

            if (usuario.Rol.ToLower() != rol.ToLower())
            {
                return (false, "Rol incorrecto.", null, null);
            }

            if (!VerifyPassword(password, usuario.PasswordHash))
            {
                return (false, "Contraseña incorrecta.", null, null);
            }

            var token = GenerateJwtToken(usuario);
            return (true, "Login exitoso.", usuario, token);
        }

        public async Task<(bool Success, string Message)> RegisterAsync(Usuario usuario, string password)
        {
            var existingUser = await _usuarioRepository.GetBycorreoAsync(usuario.Correo);
            if (existingUser != null)
            {
                return (false, "El correo ya está registrado.");
            }

            usuario.PasswordHash = HashPassword(password);
            await _usuarioRepository.AddAsync(usuario);

            // Enviar correo de bienvenida
            await _notificacionService.EnviarcorreoBienvenidaAsync(usuario.Correo, usuario.Nombre);

            return (true, "Usuario registrado exitosamente.");
        }

        public async Task<(bool Success, string Message)> ChangePasswordAsync(string correo, string currentPassword, string newPassword)
        {
            var usuario = await _usuarioRepository.GetBycorreoAsync(correo);
            if (usuario == null)
            {
                return (false, "Usuario no encontrado.");
            }

            if (!VerifyPassword(currentPassword, usuario.PasswordHash))
            {
                return (false, "Contraseña actual incorrecta.");
            }

            usuario.PasswordHash = HashPassword(newPassword);
            await _usuarioRepository.UpdateAsync(usuario.Id, usuario);

            // Enviar correo de confirmación
            await _notificacionService.EnviarcorreoCambioPasswordAsync(usuario.Correo, usuario.Nombre);

            return (true, "Contraseña cambiada exitosamente.");
        }

        public async Task<(bool Success, string Message)> ResetPasswordAsync(string correo)
        {
            var usuario = await _usuarioRepository.GetBycorreoAsync(correo);
            if (usuario == null)
            {
                return (false, "Usuario no encontrado.");
            }

            var newPassword = GenerateRandomPassword();
            usuario.PasswordHash = HashPassword(newPassword);
            await _usuarioRepository.UpdateAsync(usuario.Id, usuario);

            // Enviar correo con nueva contraseña
            await _notificacionService.EnviarcorreoResetPasswordAsync(usuario.Correo, usuario.Nombre, newPassword);

            return (true, "Se ha enviado una nueva contraseña a tu correo electrónico.");
        }

        public (bool Success, string Message) ValidateTokenAsync(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_configuration["JwtSettings:Key"] ?? throw new InvalidOperationException("JWT Key not found"));
                
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = _configuration["JwtSettings:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = _configuration["JwtSettings:Audience"],
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                }, out _);

                return (true, "Token válido.");
            }
            catch
            {
                return (false, "Token inválido.");
            }
        }

        private string GenerateJwtToken(Usuario usuario)
        {
            if (string.IsNullOrEmpty(usuario.Id))
                throw new ArgumentNullException(nameof(usuario.Id), "El Id del usuario no puede ser nulo.");
            if (string.IsNullOrEmpty(usuario.Correo))
                throw new ArgumentNullException(nameof(usuario.Correo), "El Correo del usuario no puede ser nulo.");
            if (string.IsNullOrEmpty(usuario.Rol))
                throw new ArgumentNullException(nameof(usuario.Rol), "El Rol del usuario no puede ser nulo.");

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["JwtSettings:Key"] ?? throw new InvalidOperationException("JWT Key not found"));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, usuario.Id),
                    new Claim(ClaimTypes.Email, usuario.Correo),
                    new Claim(ClaimTypes.Role, usuario.Rol)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = _configuration["JwtSettings:Issuer"],
                Audience = _configuration["JwtSettings:Audience"]
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        private bool VerifyPassword(string password, string hash)
        {
            return BCrypt.Net.BCrypt.Verify(password, hash);
        }

        private string GenerateRandomPassword()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, 12)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
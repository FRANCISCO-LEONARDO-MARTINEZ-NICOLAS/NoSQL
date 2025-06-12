using NoSQL.Application.Interfaces;
using NoSQL.Domain.Entities;
using NoSQL.Application.Services; // Agrega este using

namespace NoSQL.CLI.Services.Adapters
{
    public class AuthServiceAdapter : IAuthService
    {
        private readonly AuthService _authService;

        public AuthServiceAdapter(AuthService authService)
        {
            _authService = authService;
        }

        public async Task<(bool Success, string Message, Usuario? User, string? Token)> LoginAsync(string correo, string password, string rol)
        {
            // Implementación base que usa el servicio de aplicación
            return await _authService.LoginAsync(correo, password, rol);
        }

        public async Task<(bool Success, string Message)> RegisterAsync(Usuario usuario, string password)
        {
            return await _authService.RegisterAsync(usuario, password);
        }

        public async Task<(bool Success, string Message)> ChangePasswordAsync(string correo, string currentPassword, string newPassword)
        {
            return await _authService.ChangePasswordAsync(correo, currentPassword, newPassword);
        }

        public async Task<(bool Success, string Message)> ResetPasswordAsync(string correo)
        {
            return await _authService.ResetPasswordAsync(correo);
        }

        public async Task<(bool Success, string Message)> ValidateTokenAsync(string token)
        {
            return await _authService.ValidateTokenAsync(token);
        }
    }
}
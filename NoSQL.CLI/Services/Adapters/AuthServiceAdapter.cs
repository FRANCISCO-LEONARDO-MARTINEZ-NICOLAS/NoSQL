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

        public async Task<(bool Success, string Message, Usuario? User, string? Token)> LoginAsync(string email, string password, string rol)
        {
            // Implementación base que usa el servicio de aplicación
            return await _authService.LoginAsync(email, password, rol);
        }

        public async Task<(bool Success, string Message)> RegisterAsync(Usuario usuario, string password)
        {
            return await _authService.RegisterAsync(usuario, password);
        }

        public async Task<(bool Success, string Message)> ChangePasswordAsync(string email, string currentPassword, string newPassword)
        {
            return await _authService.ChangePasswordAsync(email, currentPassword, newPassword);
        }

        public async Task<(bool Success, string Message)> ResetPasswordAsync(string email)
        {
            return await _authService.ResetPasswordAsync(email);
        }

        public async Task<(bool Success, string Message)> ValidateTokenAsync(string token)
        {
            return await _authService.ValidateTokenAsync(token);
        }
    }
}
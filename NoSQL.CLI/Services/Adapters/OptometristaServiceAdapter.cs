using NoSQL.Application.Interfaces;
using NoSQL.Application.Services;
using NoSQL.CLI.Services;
using NoSQL.Domain.Entities;

namespace NoSQL.CLI.Services.Adapters
{
    public class OptometristaServiceAdapter : IOptometristaService
    {
        private readonly OptometristaService _optometristaService;

        public OptometristaServiceAdapter(OptometristaService optometristaService)
        {
            _optometristaService = optometristaService;
        }

        public async Task<IEnumerable<Optometrista>> GetAllAsync()
        {
            return await _optometristaService.GetAllAsync();
        }

        public async Task<Optometrista?> GetByIdAsync(string id)
        {
            return await _optometristaService.GetByIdAsync(id);
        }

        public async Task<Optometrista?> GetByEmailAsync(string correo)
        {
            var optometristas = await GetAllAsync();
            return optometristas.FirstOrDefault(o => o.Correo == correo);
        }

        public async Task<(bool Success, string Message)> CreateAsync(Optometrista optometrista)
        {
            return await _optometristaService.CreateAsync(optometrista);
        }

        public async Task<(bool Success, string Message)> UpdateAsync(string id, Optometrista optometrista)
        {
            return await _optometristaService.UpdateAsync(id, optometrista);
        }

        public async Task<(bool Success, string Message)> DeleteAsync(string id)
        {
            return await _optometristaService.DeleteAsync(id);
        }
    }
}
using NoSQL.Application.Interfaces;
using NoSQL.Application.Services;
using NoSQL.CLI.Services;
using NoSQL.Domain.Entities;

namespace NoSQL.CLI.Services.Adapters
{
    public class PacienteServiceAdapter : IPacienteService
    {
        private readonly PacienteService _pacienteService;

        public PacienteServiceAdapter(PacienteService pacienteService)
        {
            _pacienteService = pacienteService;
        }

        public async Task<IEnumerable<Paciente>> GetAllAsync()
        {
            return await _pacienteService.GetAllAsync();
        }

        public async Task<Paciente?> GetByIdAsync(string id)
        {
            return await _pacienteService.GetByIdAsync(id);
        }

        public async Task<Paciente?> GetByEmailAsync(string correo)
        {
            return await _pacienteService.GetByEmailAsync(correo);
        }

        public async Task<Paciente?> GetByDniAsync(string dni)
        {
            return await _pacienteService.GetByDniAsync(dni);
        }

        public async Task<(bool Success, string Message)> CreateAsync(Paciente paciente)
        {
            return await _pacienteService.CreateAsync(paciente);
        }

        public async Task<(bool Success, string Message)> UpdateAsync(string id, Paciente paciente)
        {
            return await _pacienteService.UpdateAsync(id, paciente);
        }

        public async Task<(bool Success, string Message)> DeleteAsync(string id)
        {
            return await _pacienteService.DeleteAsync(id);
        }
    }
} 
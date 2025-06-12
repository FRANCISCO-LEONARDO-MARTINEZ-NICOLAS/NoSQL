using NoSQL.Application.Interfaces;
using NoSQL.Application.Services;
using NoSQL.CLI.Services;
using NoSQL.Domain.Entities;

namespace NoSQL.CLI.Services.Adapters
{
    public class CitaServiceAdapter : ICitaService
    {
        private readonly CitaService _citaService;

        public CitaServiceAdapter(CitaService citaService)
        {
            _citaService = citaService;
        }

        public async Task<IEnumerable<Cita>> GetAllAsync()
        {
            return await _citaService.GetAllAsync();
        }

        public async Task<Cita?> GetByIdAsync(string id)
        {
            return await _citaService.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Cita>> GetByPacienteIdAsync(string pacienteId)
        {
            return await _citaService.GetByPacienteIdAsync(pacienteId);
        }

        public async Task<IEnumerable<Cita>> GetByOptometristaIdAsync(string optometristaId)
        {
            return await _citaService.GetByOptometristaIdAsync(optometristaId);
        }

        public async Task<(bool Success, string Message)> CreateAsync(Cita cita)
        {
            return await _citaService.CreateAsync(cita);
        }

        public async Task<(bool Success, string Message)> UpdateAsync(string id, Cita cita)
        {
            return await _citaService.UpdateAsync(id, cita);
        }

        public async Task<(bool Success, string Message)> DeleteAsync(string id)
        {
            return await _citaService.DeleteAsync(id);
        }

        // Métodos auxiliares que usan los métodos base
        public async Task<IEnumerable<Cita>> GetByPacientecorreoAsync(string correo)
        {
            var citas = await GetAllAsync();
            return citas.Where(c => c.Pacientecorreo == correo);
        }

        public async Task<IEnumerable<Cita>> GetByOptometristacorreoAsync(string correo)
        {
            var citas = await GetAllAsync();
            return citas.Where(c => c.Optometristacorreo == correo);
        }

        public async Task<IEnumerable<Cita>> GetByFechaAsync(DateTime fecha)
        {
            var citas = await GetAllAsync();
            return citas.Where(c => c.FechaHora.Date == fecha.Date);
        }

        public async Task<IEnumerable<Cita>> GetByEstadoAsync(string estado)
        {
            var citas = await GetAllAsync();
            return citas.Where(c => c.Estado == estado);
        }
    }
} 
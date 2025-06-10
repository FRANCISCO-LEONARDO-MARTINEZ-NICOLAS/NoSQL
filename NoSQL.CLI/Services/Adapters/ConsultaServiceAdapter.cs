using NoSQL.Application.Interfaces;
using NoSQL.Application.Services;
using NoSQL.CLI.Services;
using NoSQL.Domain.Entities;

namespace NoSQL.CLI.Services.Adapters
{
    public class ConsultaServiceAdapter : IConsultaService
    {
        private readonly ConsultaService _consultaService;

        public ConsultaServiceAdapter(ConsultaService consultaService)
        {
            _consultaService = consultaService;
        }

        public async Task<IEnumerable<Consulta>> GetAllAsync()
        {
            return await _consultaService.GetAllAsync();
        }

        public async Task<Consulta?> GetByIdAsync(string id)
        {
            return await _consultaService.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Consulta>> GetByPacienteIdAsync(string pacienteId)
        {
            return await _consultaService.GetByPacienteIdAsync(pacienteId);
        }

        public async Task<IEnumerable<Consulta>> GetByOptometristaIdAsync(string optometristaId)
        {
            return await _consultaService.GetByOptometristaIdAsync(optometristaId);
        }

        public async Task<IEnumerable<Consulta>> GetByPacienteEmailAsync(string email)
        {
            var consultas = await GetAllAsync();
            return consultas.Where(c => c.PacienteEmail == email);
        }

        public async Task<IEnumerable<Consulta>> GetByOptometristaEmailAsync(string email)
        {
            var consultas = await GetAllAsync();
            return consultas.Where(c => c.OptometristaEmail == email);
        }

        public async Task<IEnumerable<Consulta>> GetByFechaAsync(DateTime fecha)
        {
            var consultas = await GetAllAsync();
            return consultas.Where(c => c.Fecha.Date == fecha.Date);
        }

        public async Task<(bool Success, string Message)> CreateAsync(Consulta consulta)
        {
            return await _consultaService.CreateAsync(consulta);
        }

        public async Task<(bool Success, string Message)> UpdateAsync(string id, Consulta consulta)
        {
            return await _consultaService.UpdateAsync(id, consulta);
        }

        public async Task<(bool Success, string Message)> DeleteAsync(string id)
        {
            return await _consultaService.DeleteAsync(id);
        }

        public async Task<bool> AddAsync(Consulta consulta)
        {
            var (success, _) = await _consultaService.CreateAsync(consulta);
            return success;
        }
    }
}
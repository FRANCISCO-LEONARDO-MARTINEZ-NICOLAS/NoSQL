using NoSQL.Application.Interfaces;
using NoSQL.Domain.Entities;
using NoSQL.Domain.Interfaces;

namespace NoSQL.Application.Services
{
    public class ConsultaService : IConsultaService
    {
        private readonly IConsultaRepository _repository;

        public ConsultaService(IConsultaRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Consulta>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Consulta?> GetByIdAsync(string id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Consulta>> GetByPacientecorreoAsync(string correo)
        {
            return await _repository.GetByPacientecorreoAsync(correo);
        }

        public async Task<IEnumerable<Consulta>> GetByOptometristacorreoAsync(string correo)
        {
            return await _repository.GetByOptometristacorreoAsync(correo);
        }

        public async Task<IEnumerable<Consulta>> GetByPacienteIdAsync(string id)
        {
            return await _repository.GetByPacienteIdAsync(id);
        }

        public async Task<IEnumerable<Consulta>> GetByOptometristaIdAsync(string id)
        {
            return await _repository.GetByOptometristaIdAsync(id);
        }

        public async Task<IEnumerable<Consulta>> GetByFechaAsync(DateTime fecha)
        {
            var consultas = await GetAllAsync();
            return consultas.Where(c => c.Fecha.Date == fecha.Date);
        }

        public async Task<IEnumerable<Consulta>> GetByFechaAsync(DateTime fechaInicio, DateTime fechaFin)
        {
            var consultas = await GetAllAsync();
            return consultas.Where(c => c.Fecha >= fechaInicio && c.Fecha <= fechaFin);
        }

        public async Task<(bool Success, string Message)> CreateAsync(Consulta consulta)
        {
            try
            {
                consulta.Id = Guid.NewGuid().ToString();
                consulta.Fecha = DateTime.UtcNow;
                await _repository.AddAsync(consulta);
                return (true, "Consulta creada exitosamente");
            }
            catch (Exception ex)
            {
                return (false, $"Error al crear consulta: {ex.Message}");
            }
        }

        public async Task<(bool Success, string Message)> UpdateAsync(string id, Consulta consulta)
        {
            try
            {
                var existing = await _repository.GetByIdAsync(id);
                if (existing == null)
                    return (false, "Consulta no encontrada");

                consulta.Id = id;
                await _repository.UpdateAsync(id, consulta);
                return (true, "Consulta actualizada exitosamente");
            }
            catch (Exception ex)
            {
                return (false, $"Error al actualizar consulta: {ex.Message}");
            }
        }

        public async Task<(bool Success, string Message)> DeleteAsync(string id)
        {
            try
            {
                var existing = await _repository.GetByIdAsync(id);
                if (existing == null)
                    return (false, "Consulta no encontrada");

                await _repository.DeleteAsync(id);
                return (true, "Consulta eliminada exitosamente");
            }
            catch (Exception ex)
            {
                return (false, $"Error al eliminar consulta: {ex.Message}");
            }
        }
    }
}
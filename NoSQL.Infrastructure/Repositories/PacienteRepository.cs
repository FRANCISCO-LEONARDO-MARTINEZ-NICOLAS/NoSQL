using Couchbase;
using Couchbase.KeyValue;
using Couchbase.Core.Exceptions.KeyValue;
using Couchbase.Query;
using NoSQL.Domain.Entities;
using NoSQL.Domain.Interfaces;

namespace NoSQL.Infrastructure.Repositories
{
    public class PacienteRepository : IPacienteRepository
    {
        private readonly ICouchbaseCollection _collection;
        private readonly ICluster _cluster;

        public PacienteRepository(CouchbaseDbContext context)
        {
            _collection = context.Bucket.DefaultCollection();
            _cluster = context.Cluster;
        }

        public async Task<IEnumerable<Paciente>> GetAllAsync()
        {
            var query = "SELECT p.* FROM `OpticaNoSQL` p WHERE p.type = 'paciente'";
            var result = await _cluster.QueryAsync<Paciente>(query);
            return await result.Rows.ToListAsync();
        }

        public async Task<Paciente?> GetByIdAsync(string id)
        {
            try
            {
                var result = await _collection.GetAsync(id);
                return result.ContentAs<Paciente>();
            }
            catch (DocumentNotFoundException)
            {
                return null;
            }
        }

        public async Task<Paciente?> GetBycorreoAsync(string correo)
        {
            var query = "SELECT p.* FROM `OpticaNoSQL` p WHERE p.type = 'paciente' AND p.correo = $correo LIMIT 1";
            var options = new QueryOptions().Parameter("correo", correo);
            var result = await _cluster.QueryAsync<Paciente>(query, options);
            return await result.Rows.FirstOrDefaultAsync();
        }

        public async Task<Paciente?> GetByDniAsync(string dni)
        {
            var query = "SELECT p.* FROM `OpticaNoSQL` p WHERE p.type = 'paciente' AND p.dni = $dni LIMIT 1";
            var options = new QueryOptions().Parameter("dni", dni);
            var result = await _cluster.QueryAsync<Paciente>(query, options);
            return await result.Rows.FirstOrDefaultAsync();
        }

        public async Task AddAsync(Paciente paciente)
        {
            var pacienteWithType = new
            {
                paciente.Id,
                paciente.Nombre,
                paciente.Apellido,
                paciente.FechaNacimiento,
                paciente.Genero,
                paciente.Direccion,
                paciente.Telefono,
                paciente.correo,
                paciente.Dni,
                paciente.Ocupacion,
                paciente.SeguroMedico,
                paciente.HistorialClinico,
                type = "paciente"
            };
            
            await _collection.InsertAsync(paciente.Id, pacienteWithType);
        }

        public async Task UpdateAsync(string id, Paciente paciente)
        {
            var pacienteWithType = new
            {
                paciente.Id,
                paciente.Nombre,
                paciente.Apellido,
                paciente.FechaNacimiento,
                paciente.Genero,
                paciente.Direccion,
                paciente.Telefono,
                paciente.correo,
                paciente.Dni,
                paciente.Ocupacion,
                paciente.SeguroMedico,
                paciente.HistorialClinico,
                type = "paciente"
            };
            
            await _collection.ReplaceAsync(id, pacienteWithType);
        }

        public async Task DeleteAsync(string id)
        {
            await _collection.RemoveAsync(id);
        }
    }
}
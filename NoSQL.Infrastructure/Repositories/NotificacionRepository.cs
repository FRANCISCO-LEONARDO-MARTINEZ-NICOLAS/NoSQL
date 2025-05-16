using Couchbase;
using Couchbase.Core.Exceptions.KeyValue;
using Couchbase.Query;
using NoSQL.Domain.Entities;
using NoSQL.Domain.Interfaces;

namespace NoSQL.Infrastructure.Repositories
{
    public class NotificacionRepository : INotificacionRepository
    {
        private readonly CouchbaseDbContext _context;
        private const string CollectionName = "notificaciones";

        public NotificacionRepository(CouchbaseDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Notificacion>> GetAllAsync()
        {
            var query = $"SELECT * FROM {_context.BucketName}.{CollectionName}";
            var result = await _context.Bucket.Cluster.QueryAsync<Notificacion>(query);
            return await result.ToListAsync();
        }

        public async Task<Notificacion?> GetByIdAsync(Guid id)
        {
            try
            {
                var result = await _context.Bucket.DefaultCollection()
                    .GetAsync(id.ToString());
                return result.ContentAs<Notificacion>();
            }
            catch (DocumentNotFoundException)
            {
                return null;
            }
        }

        public async Task<IEnumerable<Notificacion>> GetByPacienteIdAsync(Guid pacienteId)
        {
            var query = $"SELECT * FROM {_context.BucketName}.{CollectionName} WHERE pacienteId = $pacienteId";
            var options = QueryOptions.Create().Parameter("$pacienteId", pacienteId);
            var result = await _context.Bucket.Cluster.QueryAsync<Notificacion>(query, options);
            return await result.ToListAsync();
        }

        public async Task<IEnumerable<Notificacion>> GetPendientesAsync()
        {
            var query = $"SELECT * FROM {_context.BucketName}.{CollectionName} WHERE estado = 'Pendiente'";
            var result = await _context.Bucket.Cluster.QueryAsync<Notificacion>(query);
            return await result.ToListAsync();
        }

        public async Task CreateAsync(Notificacion notificacion)
        {
            await _context.Bucket.DefaultCollection()
                .InsertAsync(notificacion.Id.ToString(), notificacion);
        }

        public async Task UpdateAsync(Notificacion notificacion)
        {
            await _context.Bucket.DefaultCollection()
                .ReplaceAsync(notificacion.Id.ToString(), notificacion);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _context.Bucket.DefaultCollection()
                .RemoveAsync(id.ToString());
        }
    }
} 
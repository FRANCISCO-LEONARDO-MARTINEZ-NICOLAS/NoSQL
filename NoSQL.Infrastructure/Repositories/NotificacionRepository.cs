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
            var query = $"SELECT n.* FROM `{_context.BucketName}` n WHERE n.type = 'notificacion'";
            var result = await _context.Bucket.Cluster.QueryAsync<Notificacion>(query);
            return await result.ToListAsync();
        }

        public async Task<Notificacion?> GetByIdAsync(string id)
        {
            try
            {
                var result = await _context.Bucket.DefaultCollection()
                    .GetAsync(id);
                return result.ContentAs<Notificacion>();
            }
            catch (DocumentNotFoundException)
            {
                return null;
            }
        }

        public async Task<IEnumerable<Notificacion>> GetByPacienteIdAsync(string pacienteId)
        {
            var query = $"SELECT n.* FROM `{_context.BucketName}` n WHERE n.pacienteId = $pacienteId";
            var options = QueryOptions.Create().Parameter("pacienteId", pacienteId);
            var result = await _context.Bucket.Cluster.QueryAsync<Notificacion>(query, options);
            return await result.ToListAsync();
        }

        public async Task<IEnumerable<Notificacion>> GetPendientesAsync()
        {
            var query = $"SELECT n.* FROM `{_context.BucketName}` n WHERE n.estado = 'Pendiente'";
            var result = await _context.Bucket.Cluster.QueryAsync<Notificacion>(query);
            return await result.ToListAsync();
        }

        public async Task CreateAsync(Notificacion notificacion)
        {
            await _context.Bucket.DefaultCollection()
                .InsertAsync(notificacion.Id, notificacion);
        }

        public async Task UpdateAsync(Notificacion notificacion)
        {
            await _context.Bucket.DefaultCollection()
                .ReplaceAsync(notificacion.Id, notificacion);
        }

        public async Task DeleteAsync(string id)
        {
            await _context.Bucket.DefaultCollection()
                .RemoveAsync(id);
        }
    }
}
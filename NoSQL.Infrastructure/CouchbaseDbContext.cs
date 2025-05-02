using Couchbase;
using Couchbase.KeyValue;
using Microsoft.Extensions.Configuration;

namespace NoSQL.Infrastructure
{
    public class CouchbaseDbContext
    {
        private readonly IBucket _bucket;

        public CouchbaseDbContext(IConfiguration configuration)
        {
            // Conectar al clúster de Couchbase
            var cluster = Cluster.ConnectAsync(
                configuration["Couchbase:ConnectionString"],
                configuration["Couchbase:Username"],
                configuration["Couchbase:Password"]
            ).Result;

            // Obtener el bucket
            _bucket = cluster.BucketAsync(configuration["Couchbase:BucketName"]).Result;
        }

        // Obtener una colección del bucket
        public ICouchbaseCollection GetCollection(string collectionName)
        {
            return _bucket.DefaultCollection();
        }
    }
}
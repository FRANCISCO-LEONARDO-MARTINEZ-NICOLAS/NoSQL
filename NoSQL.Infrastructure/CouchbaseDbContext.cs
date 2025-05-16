using Couchbase;
using Couchbase.KeyValue;
using Microsoft.Extensions.Configuration;

namespace NoSQL.Infrastructure
{
    public class CouchbaseDbContext
    {
        private readonly IConfiguration _configuration;
        private readonly ICluster _cluster;
        private readonly IBucket _bucket;

        public string BucketName { get; }
        public IBucket Bucket => _bucket;

        public CouchbaseDbContext(IConfiguration configuration)
        {
            _configuration = configuration;
            
            var settings = _configuration.GetSection("CouchbaseSettings");
            BucketName = settings["BucketName"] ?? "OpticaNoSQL";
            
            var connectionString = settings["ConnectionString"] ?? "couchbase://localhost";
            var username = settings["Username"] ?? "Administrator";
            var password = settings["Password"] ?? "password";

            // Conectar a Couchbase
            _cluster = Cluster.ConnectAsync(connectionString, username, password).Result;
            _bucket = _cluster.BucketAsync(BucketName).Result;
        }

        // Obtener una colección del bucket
        public ICouchbaseCollection GetCollection(string collectionName)
        {
            return _bucket.DefaultCollection();
        }
    }
}
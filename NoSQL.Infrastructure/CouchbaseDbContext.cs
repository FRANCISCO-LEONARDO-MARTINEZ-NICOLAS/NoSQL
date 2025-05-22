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
        public ICluster Cluster => _cluster;

        public CouchbaseDbContext(IConfiguration configuration)
        {
            _configuration = configuration;
            
            var settings = _configuration.GetSection("CouchbaseSettings");
            BucketName = settings["BucketName"] ?? "OpticaNoSQL";
            
            var connectionString = settings["ConnectionString"] ?? "couchbase://localhost";
            var username = settings["Username"] ?? "OpticaNoSQL";
            var password = settings["Password"] ?? "Leo000426";

            var options = new ClusterOptions
            {
                UserName = username,
                Password = password,
                ConnectionString = connectionString
            };
            
            _cluster = Task.Run(async () => await Couchbase.Cluster.ConnectAsync(connectionString, options)).Result;
            _bucket = Task.Run(async () => await _cluster.BucketAsync(BucketName)).Result;
        }

        // Obtener una colección del bucket
        public ICouchbaseCollection GetCollection(string collectionName)
        {
            return _bucket.DefaultCollection();
        }
    }
}
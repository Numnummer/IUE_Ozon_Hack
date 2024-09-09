using Cassandra;

namespace ApiResponse.Infrastructure
{
    public class CassandraDbContext
    {
        private readonly string _CassandraHost = "127.0.0.1";
        private readonly Cluster _cluster;
        private readonly Dictionary<string, Cassandra.ISession> _tables;

        public CassandraDbContext(IConfiguration configuration)
        {
            var cassandraConfig = configuration.GetSection("Cassandra");
            _CassandraHost = cassandraConfig["Host"];
            _cluster = Cluster.Builder()
                .AddContactPoint(_CassandraHost)
                .Build();

            var tables = cassandraConfig.GetSection("ResponsesTables").Get<List<string>>();

            _tables = new Dictionary<string, Cassandra.ISession>();

            foreach (var table in tables)
            {
                var session = _cluster.Connect();
                AddTable(table, session);
            }

            
        }
        /// <summary>
        /// Add table if it hasn't been added yet
        /// else do nothing
        /// </summary>
        /// <param name="tableName"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public void AddTable(string tableName, Cassandra.ISession session)
        {
            if (string.IsNullOrWhiteSpace(tableName))
                throw new ArgumentNullException($"Table name can't be null");
            if (session == null)
                throw new ArgumentNullException("Bad Cassandra session");

            if (!_tables.ContainsKey(tableName))
            {
                _tables.Add(tableName, session);
            }
        }

        public bool IsTableExist(string tableName)
        {
            if (string.IsNullOrWhiteSpace(tableName))
            {
                return false;
            }
            return _tables.ContainsKey(tableName);
        }

        public Cluster GetCluster()
        {
            return _cluster;
        }

        public Cassandra.ISession GetTableSession(string tableName)
        {
            if (!IsTableExist(tableName))
                throw new ArgumentNullException(
                    $"Wrong table name, or table with {tableName}" +
                    $"doesn't exist");

            return _tables[tableName];
        }
    }
}

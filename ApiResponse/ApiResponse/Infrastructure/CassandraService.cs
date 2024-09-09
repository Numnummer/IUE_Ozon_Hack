using ApiResponse.Core;
using ApiResponse.Core.Abstractions;

namespace ApiResponse.Infrastructure
{
    public class CassandraService : IResponsesDbService
    {
        private readonly Cassandra.ISession _session;
        private readonly CassandraDbContext _dbContext;

        public CassandraService(Cassandra.ISession session, CassandraDbContext dbContext)
        {
            _session = session;
            _dbContext = dbContext;
        }

        public async Task<List<Product>> GetByQueryId(string keyspace, string tableName, int queryId)
        {
            Console.WriteLine("--> Get query in Service");
            if (!_dbContext.IsTableExist(tableName))
            {
                throw new ArgumentException($"Table {tableName} doesn't exist");
            }

            var ps = _session.Prepare($"SELECT * FROM {keyspace}.{tableName} WHERE id = ?");
            var statement = ps.Bind(queryId);
            Console.WriteLine("--> Start ExecuteSync");

            var queryResult = await _session.ExecuteAsync(statement);
            var result = new List<Product>();

            Console.WriteLine("--> Got asnwer");
            foreach (var row in queryResult)
            {
                Console.WriteLine(row.GetValue<string>("name"));
                result.Add(new Product()
                {
                    Id = row.GetValue<int>("id"),
                    Name = row.GetValue<string>("name"),
                    Description = row.GetValue<string>("description")
                });
            }

            return result;
        }
    }
}

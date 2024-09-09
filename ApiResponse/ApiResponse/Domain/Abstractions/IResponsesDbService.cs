namespace ApiResponse.Core.Abstractions
{
    public interface IResponsesDbService
    {
        public Task<List<Product>> GetByQueryId(string keyspace, string tableName, int queryId);
    }
}

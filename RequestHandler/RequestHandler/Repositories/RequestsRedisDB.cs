using RequestHandler.Application;
using StackExchange.Redis;

namespace RequestHandler.Repositories
{
    public class RequestsRedisDB : IRequestsDb
    {
        private readonly IDatabase requestsDb;

        public RequestsRedisDB(ConnectionMultiplexer connectionMultiplexer)
        {
            requestsDb = connectionMultiplexer.GetDatabase();
        }

        public string FinishRequest(string key)
        {
            throw new NotImplementedException();
        }

        public bool IsRequestProcessing(string key)
        {
            return requestsDb.KeyExists(key);
        }

        public async Task<long> AddNewRequest(string key, string queryId)
        {
            return await requestsDb.ListRightPushAsync(key, queryId);
        }
    }
}

using Microsoft.Extensions.Primitives;
using StackExchange.Redis;

namespace AuthMicroservice.Configuration
{
    public class SecretConfigurationProvider : ConfigurationProvider
    {
        public override void Load()
        {
            var connectionString = "";
            var connection = ConnectionMultiplexer.Connect(connectionString);
            var database = connection.GetDatabase();
            var keys = connection.GetServer(connectionString).Keys().ToArray();
            var data = new Dictionary<string, string>();
            foreach (var key in keys)
            {
                data.Add(key.ToString(), database.StringGet(key).ToString());
            }
            Data = data;
        }
    }
}

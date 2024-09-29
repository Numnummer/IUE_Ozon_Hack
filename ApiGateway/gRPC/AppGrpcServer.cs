using ApiGateway.MessageBroker.Model;
using ApiGateway.Proto;
using Confluent.Kafka;
using Grpc.Core;
using Newtonsoft.Json;

namespace ApiGateway.gRPC
{
    public class AppGrpcServer(IProducer<string, string> producer,
        IConfiguration configuration) : RpcServer.RpcServerBase
    {
        public override async Task<DataResponse> GetData(DataRequest request, ServerCallContext context)
        {
            var searchModel = new SearchModel(request.RedirectUrl, request.SearchedString);
            var topic = configuration["Kafka:ProducerTopic"];
            var message = new Message<string, string>()
            {
                Value=JsonConvert.SerializeObject(searchModel)
            };
            var result = await producer.ProduceAsync(topic, message);
            if (result.Status == PersistenceStatus.NotPersisted)
            {
                return new DataResponse()
                {
                    Status="Not Accepted"
                };
            }
            return new DataResponse()
            {
                Status="Accepted"
            };
        }
    }
}

using ApiGateway.MessageBroker.Model;
using ApiGateway.Proto;
using Grpc.Core;
using MassTransit;

namespace ApiGateway.gRPC
{
    public class AppGrpcServer(ITopicProducer<SearchModel> bus) : RpcServer.RpcServerBase
    {
        public override async Task<DataResponse> GetData(DataRequest request, ServerCallContext context)
        {
            var searchModel = new SearchModel(request.RedirectUrl, request.SearchedString);
            await bus.Produce(searchModel);
            return new DataResponse()
            {
                Status="Accepted"
            };
        }
    }
}

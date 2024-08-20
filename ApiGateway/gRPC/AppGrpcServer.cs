using ApiGateway.Proto;
using Grpc.Core;

namespace ApiGateway.gRPC
{
    public class AppGrpcServer : RpcServer.RpcServerBase
    {
        public override Task<DataResponse> GetData(DataRequest request, ServerCallContext context)
        {
            return base.GetData(request, context);
        }
    }
}

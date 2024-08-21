using ApiGateway.Extentions;
using ApiGateway.gRPC;

var builder = WebApplication.CreateBuilder(args);
builder.AddMessageBroker();

var app = builder.Build();

app.MapGrpcService<AppGrpcServer>();

app.Run();

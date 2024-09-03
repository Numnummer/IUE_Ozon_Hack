using ApiGateway.Extentions;
using ApiGateway.gRPC;

var builder = WebApplication.CreateBuilder(args);
builder.AddMessageBroker();
builder.Services.AddGrpc();

var app = builder.Build();

app.MapGrpcService<AppGrpcServer>();

app.Run();

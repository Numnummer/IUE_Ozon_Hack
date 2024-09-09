using ApiGateway.Extentions;
using ApiGateway.gRPC;
using ApiGateway.MessageBroker.Consumer;
using Confluent.Kafka;

var builder = WebApplication.CreateBuilder(args);
builder.AddKafka();
builder.Services.AddHostedService<ProductConsumer>();
builder.Services.AddGrpc();

var app = builder.Build();

app.MapGrpcService<AppGrpcServer>();

app.Run();

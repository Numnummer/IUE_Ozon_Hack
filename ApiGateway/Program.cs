using ApiGateway.Extentions;
using ApiGateway.gRPC;
using ApiGateway.MessageBroker.Model;
using ApiGateway.MessageBroker.Publisher;
using Confluent.Kafka;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;


var builder = WebApplication.CreateBuilder(args);
builder.AddKafka();
builder.Services.AddScoped<IProductPublisher, ProductPublisher>();
//builder.Services.AddHostedService<ProductConsumer>();
builder.Services.AddGrpc();
builder.Services.BuildServiceProvider().GetRequiredService<IProducer<string,string>>();

var app = builder.Build();

app.MapGrpcService<AppGrpcServer>();

app.MapPost("/", async ([FromBody] Product product,
    [FromServices] IProductPublisher publisher) =>
{
    var message = new Message<string, string>()
    {
        Value = JsonConvert.SerializeObject(product)
    };
    publisher.ProduceAsync("requests", message);
    Console.WriteLine("--> Send Message to Kafka");
});
app.MapGet("/", () => "Hello");

app.Run();

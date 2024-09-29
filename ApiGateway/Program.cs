using ApiGateway.Extentions;
using ApiGateway.gRPC;
using ApiGateway.MessageBroker.Model;
using Confluent.Kafka;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;


var builder = WebApplication.CreateBuilder(args);
builder.AddKafka();
//builder.Services.AddHostedService<ProductConsumer>();
builder.Services.AddGrpc();
builder.Services.BuildServiceProvider().GetRequiredService<IProducer<string,string>>();

var app = builder.Build();

app.MapGrpcService<AppGrpcServer>();
app.MapGet("/asd",async ([FromServices] IProducer<string,string> producer)=>{    
    var searchModel = new SearchModel("","");
    var topic = "";
    var message = new Message<string, string>()
    {
        Value=JsonConvert.SerializeObject(searchModel)
    };
    var result = await producer.ProduceAsync(topic, message);
});

app.Run();

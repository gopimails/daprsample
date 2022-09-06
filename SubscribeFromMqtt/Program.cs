using Dapr;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// Dapr will send serialized event object vs. being raw CloudEvent
app.UseCloudEvents();

// needed for Dapr pub/sub routing
app.MapSubscribeHandler();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) { app.UseDeveloperExceptionPage(); }

// Dapr subscription in [Topic] routes orders topic to this route
app.MapPost("/orders", [Topic("mqttcomponent-pubsub", "orders")] (Order order) => {
    Console.WriteLine("Subscriber received : " + order);
    return Results.Ok(order);
});

await app.RunAsync();

public record Order([property: JsonPropertyName("orderId")] int OrderId);

/*Commands
 * dapr run --app-id SubscribeFromMqtt --app-port 6001 --dapr-http-port 3601 --dapr-grpc-port 60001 --components-path components dotnet run
 */
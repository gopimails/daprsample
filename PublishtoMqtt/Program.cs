using Dapr.Client;
using System.Text.Json.Serialization;

for (int i = 0; i < 1000; i++)
{
    var order = new Order(i);
    using var client = new DaprClientBuilder().Build();
    await client.PublishEventAsync("mqttcomponent-pubsub", "orders", order);
    Console.WriteLine("Published data: " + order);
    await Task.Delay(TimeSpan.FromSeconds(15));
}

public record Order([property: JsonPropertyName("orderId")] int OrderId);

/*Commands
 * dapr run --app-id PublishtoMqtt --app-port 6002 --dapr-http-port 3602 --dapr-grpc-port 60002 --app-ssl --components-path components dotnet run
 */

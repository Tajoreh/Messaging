// See https://aka.ms/new-console-template for more information

using System.Diagnostics;
using System.Net;
using System.Text.Json;
using Confluent.Kafka;
using Producer;

Console.Title = "Producer";
string bootstrapServers = "localhost:9092";
string topic = "test";
while (true)
{

    var request = new OrderRequest()
    {
        CustomerId = 1,
        OrderId = 2,
        ProductId = 3,
        Quantity = 4,
        Status = "Delivered"
    };
    var message = JsonSerializer.Serialize(request);

    Console.WriteLine(message);

    var config = new ProducerConfig
    {
        BootstrapServers = bootstrapServers,
        ClientId = Dns.GetHostName()
    };

    using var producer = new ProducerBuilder<Null, string>(config).Build();

    producer.Produce(topic, new Message<Null, string> {Value = message});

    Console.ReadLine();
}


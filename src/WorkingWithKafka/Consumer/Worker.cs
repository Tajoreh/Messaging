using System.Diagnostics;
using System.Text.Json;
using Confluent.Kafka;

namespace Consumer;

public class MyConsumer : IHostedService
{
    private readonly string topic = "test";
    private readonly string groupId = "test_group";
    private readonly string bootstrapServers = "localhost:9092";

    public Task StartAsync(CancellationToken cancellationToken)
    {
        var config = new ConsumerConfig
        {
            GroupId = groupId,
            BootstrapServers = bootstrapServers,
            AutoOffsetReset = AutoOffsetReset.Earliest
        };

        try
        {
            using (var consumerBuilder = new ConsumerBuilder<Ignore, string>(config).Build())
            {
                consumerBuilder.Subscribe(topic);

                var cancelToken = new CancellationTokenSource();

                try
                {
                    while (true)
                    {
                        var consumer = consumerBuilder.Consume(cancelToken.Token);

                        try
                        {
                            var orderRequest = JsonSerializer.Deserialize<OrderProcessingRequest>(consumer.Message.Value);

                            Console.WriteLine(consumer.Message.Value);
                        }
                        catch (Exception e)
                        {
                            
                        }
                    }
                }
                catch (OperationCanceledException)
                {
                    consumerBuilder.Close();
                }
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(ex.Message);
        }

        return Task.CompletedTask;
    }
    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}

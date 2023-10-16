using Confluent.Kafka;
using Framework.Kafka;
using Newtonsoft.Json;
using WorkingWithKafkaAndTests.Configurations;
using WorkingWithKafkaAndTests.Models;

namespace WorkingWithKafkaAndTests.Consumers;

public class PersonEnrolledConsumer : IPersonEnrolledConsumer
{
    private readonly IConsumer<string, string> _consumer;

    public PersonEnrolledConsumer(IConsumer<string, string> consumer)
    {
        _consumer = consumer;
    }

    public Task Consume(CancellationToken cancellationToken)
    {
        try
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("_____________________________________ Consume ______________________________");

                var consumeResult = _consumer.Consume(cancellationToken);

                var person = JsonConvert.DeserializeObject<Person>(consumeResult.Message.Value);
                Console.ForegroundColor = ConsoleColor.Green;

                Console.Write("Message Consumed:");
                Console.WriteLine($"Name:{person.Name} , Age:{person.Age}");
            }
        }
        catch (OperationCanceledException)
        {
            // This catch block will handle the cancellation and stop the loop.
            // We don't need to do anything here.

        }
        finally
        {
            //This line checks if cancellation has been requested and throws an OperationCanceledException if it has. This allows the consumer to exit the loop gracefully when cancellation is requested.
            cancellationToken.ThrowIfCancellationRequested();

            _consumer.Close();
        }

        return Task.CompletedTask;
    }
}
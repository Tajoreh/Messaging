using Confluent.Kafka;
using FluentAssertions;
using System.ComponentModel;
using Testcontainers.Kafka;

namespace WorkingWithKafkaAndTests.Test.Integration;
public class KafkaContainerFixture : ContainerFixture<KafkaTestContainer>
{
    public string GetBootstrapServers() => Container.Get();
    public IProducer<string, string> GetProducer() => Container.GetProducer();
}
public class PersonEnrolledConsumerIntegrationTest:IClassFixture<KafkaContainerFixture>
{
    private readonly KafkaContainer _kafkaContainer;

    public PersonEnrolledConsumerIntegrationTest(KafkaContainer kafkaContainer)
    {
        _kafkaContainer = kafkaContainer;
    }

    [Fact]
    public async Task Consume_Should_Consume_Messages_From_Kafka()
    {
        // Arrange
        var topic = "test-topic";
        var cancellationTokenSource = new CancellationTokenSource();
        var cancellationToken = cancellationTokenSource.Token;

        // Create the Kafka consumer configuration.
        var consumerConfig = new ConsumerConfig
        {
            BootstrapServers = _kafkaContainer.e(),
            GroupId = "test-group",
            AutoOffsetReset = AutoOffsetReset.Earliest,
            EnableAutoCommit = false,
            // Add any additional configuration settings you need.
        };

        // Create the Kafka consumer instance.
        using var consumer = new ConsumerBuilder<string, string>(consumerConfig).Build();

        // Subscribe to the test topic.
        consumer.Subscribe(topic);

        // Produce test messages to the test topic.
        using (var producer = _kafkaContainer..GetProducer())
        {
            var message1 = new Message<string, string> { Key = "1", Value = "{\"Name\":\"John\",\"Age\":30}" };
            var message2 = new Message<string, string> { Key = "2", Value = "{\"Name\":\"Jane\",\"Age\":25}" };

            producer.Produce(topic, message1);
            producer.Produce(topic, message2);
        }

        // Create the PersonEnrolledConsumer with the Kafka consumer.
        var consumerWrapper = new PersonEnrolledConsumer(consumer);

        // Act - Start the consumer on a separate thread using Task.Run.
        var consumeTask = Task.Run(() => consumerWrapper.Consume(cancellationToken));

        // Wait for a short period to allow the consumer to process messages.
        await Task.Delay(3000);

        // Request cancellation of the consumer.
        cancellationTokenSource.Cancel();

        // Assert
        await consumeTask.Should().NotThrowAsync();
    }
}

public class ControllerSetup : AutoDataAttribute
{

}
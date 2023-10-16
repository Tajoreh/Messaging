using Confluent.Kafka;
using Consul;
using FluentAssertions;
using Framework.Kafka;
using Newtonsoft.Json;
using NSubstitute;
using System.Threading;
using WorkingWithKafkaAndTests.Configurations;
using WorkingWithKafkaAndTests.Consumers;
using WorkingWithKafkaAndTests.Factories;
using WorkingWithKafkaAndTests.Models;
using WorkingWithKafkaAndTests.Test.Unit.Extensions;

namespace WorkingWithKafkaAndTests.Test.Unit;

public class KafkaConsumerTest
{
    [Fact]
    public async Task consumer_should_consumes()
    {
        // Arrange
        var mockConsumer = Substitute.For<IConsumer<string, string>>();
        var cancellationTokenSource = new CancellationTokenSource();
        var cancellationToken = cancellationTokenSource.Token;

        var consumeResult = new ConsumeResult<string, string>
        {
            Message = new Message<string, string>
            {
                Value = JsonConvert.SerializeObject(new Person {Name = "John", Age = 30})
            }
        };
        mockConsumer.Consume(Arg.Any<CancellationToken>()).Returns(consumeResult);

        var personEnrolledConsumer = new PersonEnrolledConsumer(mockConsumer);
        //Act
        var consumeTask = Task.Run(() => personEnrolledConsumer.Consume(cancellationToken), cancellationToken);

        await Task.Delay(TimeSpan.FromSeconds(2), cancellationToken);

        cancellationTokenSource.Cancel();

        await consumeTask;

        // Assert
        mockConsumer.Received().Consume(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Consume_Should_Handle_Exception_During_Message_Processing()
    {
        // Arrange
        var mockConsumer = Substitute.For<IConsumer<string, string>>();
        var cancellationTokenSource = new CancellationTokenSource();
        var cancellationToken = cancellationTokenSource.Token;

        // Mock a message with an invalid JSON format to simulate an exception during deserialization.
        var consumeResult = new ConsumeResult<string, string>
        {
            Message = new Message<string, string>
            {
                Value = "{InvalidJsonData}"
            }
        };
        mockConsumer.Consume(Arg.Any<CancellationToken>()).Returns(consumeResult);

        var personEnrolledConsumer = new PersonEnrolledConsumer(mockConsumer);

        // Act
        var action = async () => await personEnrolledConsumer.Consume(cancellationToken);

        // Assert
        // Here, you can add assertions based on how the consumer should handle the exception.
        // For example, you can check whether it logs the exception or continues processing other messages.
        // Depending on your logging mechanism, you might need to use a logging framework and verify its behavior during the test.
        await action.Should().ThrowAsync<Exception>();

    }

    [Fact]
    public async Task Consume_Should_Stop_On_Cancellation()
    {
        // Arrange
        var cancellationTokenSource = new CancellationTokenSource();
        var cancellationToken = cancellationTokenSource.Token;

        // Create the Kafka consumer instance using NSubstitute.
        var mockConsumer = Substitute.For<IConsumer<string, string>>();
        var consumeResult = new ConsumeResult<string, string>
        {
            Message = new Message<string, string>
            {
                Value = "{\"Name\":\"John Doe\",\"Age\":30}"
            }
        };
        mockConsumer.Consume(cancellationToken).Returns(consumeResult);

        // Create the PersonEnrolledConsumer with the mocked consumer.
        var consumer = new PersonEnrolledConsumer(mockConsumer);

        // Act - Start the consumer on a separate thread using Task.Run
        var consumeTask = Task.Run(() => consumer.Consume(cancellationToken), cancellationToken);

        // Wait for a short period (you can adjust this based on your specific scenario).
        // This simulates the consumer running for a brief time before cancellation is requested.
        await Task.Delay(1000, cancellationToken);

        // Request cancellation of the consumer.
        cancellationTokenSource.Cancel();

        // Assert
        consumeTask.Should().NotBeNull();
        consumeTask.IsCompleted.Should().BeTrue();
        consumeTask.IsCanceled.Should().BeTrue();
    }
}
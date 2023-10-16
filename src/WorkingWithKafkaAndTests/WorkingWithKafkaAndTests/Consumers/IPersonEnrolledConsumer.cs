namespace WorkingWithKafkaAndTests.Consumers;

public interface IPersonEnrolledConsumer
{
    Task Consume(CancellationToken cancellationToken);
}
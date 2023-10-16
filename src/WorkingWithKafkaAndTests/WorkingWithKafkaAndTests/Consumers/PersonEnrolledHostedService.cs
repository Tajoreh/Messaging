namespace WorkingWithKafkaAndTests.Consumers;

public class PersonEnrolledHostedService : BackgroundService
{
    private readonly IPersonEnrolledConsumer _personEnrolledConsumer;
    private readonly CancellationTokenSource _cancellationTokenSource;

    public PersonEnrolledHostedService(IPersonEnrolledConsumer personEnrolledConsumer)
    {
        _personEnrolledConsumer = personEnrolledConsumer;
        _cancellationTokenSource = new CancellationTokenSource();
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using (_cancellationTokenSource)
        {
            // Combine the provided stoppingToken and the custom CancellationTokenSource.
            // This allows stopping the background worker gracefully when the application shuts down.
            var combinedTokenSource = CancellationTokenSource.CreateLinkedTokenSource(stoppingToken, _cancellationTokenSource.Token);

            await _personEnrolledConsumer.Consume(combinedTokenSource.Token);
        }
    }

    public void Stop()
    {
        // Request the cancellation of the background worker.
        _cancellationTokenSource.Cancel();
    }
}
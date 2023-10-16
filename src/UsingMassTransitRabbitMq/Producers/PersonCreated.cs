namespace UsingMassTransitRabbitMq.Producers;

public record PersonCreated
{
    public Guid Id { get; init; }=Guid.NewGuid();
    public string FullName { get; init; } = Faker.Name.FullName();
}
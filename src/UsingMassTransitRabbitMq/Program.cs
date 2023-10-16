using MassTransit;
using Microsoft.Extensions.Options;
using UsingMassTransitRabbitMq.Configurations;
using UsingMassTransitRabbitMq.Consumers;
using UsingMassTransitRabbitMq.Producers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<RabbitMqConfigs>(builder.Configuration.GetSection(nameof(RabbitMqConfigs)));
builder.Services.AddSingleton(sp => sp.GetRequiredService<IOptions<RabbitMqConfigs>>().Value);

builder.Services.AddMassTransit(busConfigurations =>
{
    busConfigurations.SetKebabCaseEndpointNameFormatter();

    busConfigurations.UsingRabbitMq((context, configurator) =>
    {
        var settings = context.GetRequiredService<RabbitMqConfigs>();

        configurator.Host(new Uri(settings.Host), h =>
        {
            h.Username(settings.UserName);
            h.Password(settings.Password);
        });
        configurator.ReceiveEndpoint("producer", ep =>
        {
            ep.Consumer<PersonCreatedConsumer>();
        });
    });

    busConfigurations.AddConsumer<PersonCreatedConsumer>();

});

builder.Services.AddTransient<IEventBus, EventBus>();

var app = builder.Build();

app.UseHttpsRedirection();

app.MapGet("/produce", async (IEventBus eventBus, CancellationToken cancellationToken) =>
{
    var message = new PersonCreated();

    await eventBus.PublishAsync(message, cancellationToken);

    return Results.Ok();
});

app.Run();


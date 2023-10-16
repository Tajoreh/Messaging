using MassTransit;
using Microsoft.Extensions.Options;
using UsingMassTransitRabbitMq.Configurations;

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
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
});

app.Run();

internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}

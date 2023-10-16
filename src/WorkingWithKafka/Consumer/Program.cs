using Consumer;
Console.Title = "Consumer";

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddSingleton<MyConsumer>();
        services.AddHostedService<MyConsumer>();
    })
    .Build();

host.Run();

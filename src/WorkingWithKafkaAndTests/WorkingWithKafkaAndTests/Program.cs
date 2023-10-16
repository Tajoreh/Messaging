using Autofac.Extensions.DependencyInjection;
using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;
using WorkingWithKafkaAndTests;
using WorkingWithKafkaAndTests.Configurations;
using WorkingWithKafkaAndTests.Consumers;
using WorkingWithKafkaAndTests.Factories;
using WorkingWithKafkaAndTests.ServiceExtensions;

var host = Host.CreateDefaultBuilder(args)
    .UseServiceProviderFactory(new AutofacServiceProviderFactory())
    .ConfigureAppConfiguration(configs =>
    {
        //find suitable appSetting.json file
    })
    .ConfigureServices((hostContext, services) =>
    {
        services.RegisterKafkaConfigs(hostContext);
        services.AddHostedService<Worker>();
        //services.AddHostedService<PersonEnrolledConsumer>();


        services.AddScoped<IPersonEnrolledConsumer, PersonEnrolledConsumer>();

        // Register the custom hosted service as a singleton
        services.AddHostedService<PersonEnrolledHostedService>();
    })
    .Build();

await host.RunAsync();

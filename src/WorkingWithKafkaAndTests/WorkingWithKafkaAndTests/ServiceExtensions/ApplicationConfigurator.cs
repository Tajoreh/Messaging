using Autofac.Core;
using Confluent.Kafka;
using Framework.Kafka;
using WorkingWithKafkaAndTests.Configurations;
using WorkingWithKafkaAndTests.Factories;

namespace WorkingWithKafkaAndTests.ServiceExtensions;

public static class ApplicationConfigurator
{
    public static void RegisterKafkaConfigs(this IServiceCollection services, HostBuilderContext context)
    {
        var kafkaConfigurations = context.Configuration.GetSection("KafkaConfiguration").Get<KakaConfiguration>();

        services.AddSingleton(kafkaConfigurations.ConsumerConfiguration);
        services.AddSingleton(kafkaConfigurations.ProducerConfiguration);

        services.AddTransient<IConsumer<string, string>>(sp => KafkaFactories.CreateConsumer(sp.GetRequiredService<KafkaConsumerConfigs>()));
    }
}
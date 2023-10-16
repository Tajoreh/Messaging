using Confluent.Kafka;
using WorkingWithKafkaAndTests.Configurations;

namespace WorkingWithKafkaAndTests.Factories;

public class KafkaFactories
{
    public static IConsumer<string, string> CreateConsumer(KafkaConsumerConfigs consumerConfigs)
    {
        var config = new ConsumerConfig
        {
            GroupId = consumerConfigs.GroupId,
            BootstrapServers = consumerConfigs.BootstrapServer,
            EnableAutoOffsetStore = consumerConfigs.EnableAutoOffsetStore,
            AutoOffsetReset = consumerConfigs.AutoOffsetReset,
            EnableAutoCommit = consumerConfigs.EnableAutoCommit
        };

        var consumer = new ConsumerBuilder<string, string>(config).Build();
        consumer.Assign(new TopicPartition(consumerConfigs.TopicName, new Partition(consumerConfigs.PartitionNumber)));

        return consumer;
    }
}
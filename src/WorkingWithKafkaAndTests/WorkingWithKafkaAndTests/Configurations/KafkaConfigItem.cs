using Confluent.Kafka;

namespace WorkingWithKafkaAndTests.Configurations;

public abstract class KafkaConfigItem
{
    public string BootstrapServer { get; set; }

    public string TopicName { get; set; }

    public int PartitionNumber { get; set; }

    public int MessageTimeoutMs { get; set; }

    public Acks Acks { get; set; }

    public string GroupId { get; set; }

    public bool EnableAutoOffsetStore { get; set; }

    public bool EnableAutoCommit { get; set; }

    public AutoOffsetReset AutoOffsetReset { get; set; }

    public string OffsetPath { get; set; }

}
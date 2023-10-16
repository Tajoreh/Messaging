using Confluent.Kafka;
using Consul;
using Newtonsoft.Json;
using WorkingWithKafkaAndTests.Configurations;
using WorkingWithKafkaAndTests.Models;

namespace WorkingWithKafkaAndTests;

public class Worker : BackgroundService
{
    private IProducer<string, string> _producer;
    private readonly KafkaProducerConfigs _producerConfigs;
    public Worker(KafkaProducerConfigs producerConfigs)
    {
        _producerConfigs = producerConfigs;
        _producer = CreateProducer();
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("********************************* Produce **********************************");
            Console.Write("Message Produced:");

            var person = new Person()
            {
                Name = $"Afsaneh{Guid.NewGuid()}",
                Age = new Random().Next(15, 40),
            };
            Console.WriteLine($"Name:{person.Name} , Age:{person.Age}");
           
            var message = JsonConvert.SerializeObject(person);
            var topic = new TopicPartition(_producerConfigs.TopicName,
                new Partition(_producerConfigs.PartitionNumber));

            _producer.Produce(topic, new Message<string, string>()
            {
                Value = message
            });

            Console.WriteLine("End of producing ...");
            await Task.Delay(5000, stoppingToken);
        }
    }
    private IProducer<string, string> CreateProducer()
    {
        var config = new ProducerConfig
        {
            BootstrapServers = _producerConfigs.BootstrapServer,
            MessageTimeoutMs = _producerConfigs.MessageTimeoutMs,
            Acks = _producerConfigs.Acks,
        };

        var producer = new ProducerBuilder<string, string>(config).Build();

        return producer;
    }
}
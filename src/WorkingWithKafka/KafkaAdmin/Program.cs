// See https://aka.ms/new-console-template for more information

using Confluent.Kafka;
using Confluent.Kafka.Admin;
using System.Net;

static async Task CreateTopicAsync(string bootstrapServers, string topicName)
{
    using (var adminClient = new AdminClientBuilder(new AdminClientConfig { BootstrapServers = bootstrapServers }).Build())
    {
        try
        {
            await adminClient.CreateTopicsAsync(new TopicSpecification[] {
                new TopicSpecification { Name = topicName, ReplicationFactor = 1, NumPartitions = 1 } });
        }
        catch (CreateTopicsException e)
        {
            Console.WriteLine($"An error occured creating topic {e.Results[0].Topic}: {e.Results[0].Error.Reason}");
        }
    }
}

static async Task ProduceMessage(string bootstrapServer,string topicName)
{
   
    ProducerConfig config = new ProducerConfig { BootstrapServers = bootstrapServer,
              ClientId = Dns.GetHostName() , EnableIdempotence  = true};
    using (var producer = new ProducerBuilder<Null, string>(config).Build())
    {
        var topicPart = new TopicPartition(topicName, new Partition(0));
       

        Console.WriteLine("\n-----------------------------------------------------------------------");
        Console.WriteLine($"Producer {producer.Name} producing on topic {topicName}.");
        Console.WriteLine("-----------------------------------------------------------------------");
        Console.WriteLine("To create a kafka message with UTF-8 encoded key and value:");
        Console.WriteLine("> key value<Enter>");
        Console.WriteLine("To create a kafka message with a null key and UTF-8 encoded value:");
        Console.WriteLine("> value<enter>");
        Console.WriteLine("Ctrl-C to quit.\n");

        var cancelled = false;
        Console.CancelKeyPress += (_, e) =>
        {
            e.Cancel = true; // prevent the process from terminating.
            cancelled = true;
        };

        while (!cancelled)
        {
            Console.Write("> ");

            string text;
            try
            {
                text = Console.ReadLine();
            }
            catch (IOException)
            {
                // IO exception is thrown when ConsoleCancelEventArgs.Cancel == true.
                break;
            }

            if (text == null)
            {
                // Console returned null before 
                // the CancelKeyPress was treated
                break;
            }

            string key = null;
            string val = text;

            // split line if both key and value specified.
            int index = text.IndexOf(" ");
            if (index != -1)
            {
                key = text.Substring(0, index);
                val = text.Substring(index + 1);
            }

            try
            {
                // Notes: Awaiting the asynchronous produce request below prevents flow of execution
                // from proceeding until the acknowledgement from the broker is received (at the 
                // expense of low throughput).
                val = "{  \"instrumentId\": \"IRO1PIAZ0003\",  \"color\": \"#EFC9D2\",  \"instrumentName\": \"والبر\",  \"persianName\": \"والبر\",  \"englishName\": \"Valber\",  \"logoPath\": null,  \"logoBackgroundColor\": null,\r\n  \"instrumentType\": null,  \"companyCode\": null,  \"boardCode\": 0,  \"boardName\": null,  \"companyPersianName\": null,  \"companyEnglishName\": null,  \"groupCode\": null,  \"subGroupCode\": null,  \"industryCode\": \"43\",  \"industryTitle\": \"مواد و محصولات دارویی\",  \"industryColor\": \"#FFDBC8\",  \"subIndustryCode\": null,  \"subIndustryTitle\": null,  \"marketPlace\": null,  \"tickPrice\": 0.0,  \"assetClassTitle\": null,  \"assetClassColor\": null,  \"messageOffset\": 0,  \"highestAllowedVolume\": 0.0,  \"lowestAllowedVolume\": 0.0}";
                    

                var result = await producer.ProduceAsync(topicPart, new Message<Null, string> { Value = val });
            



                
                //var deliveryReport = producer.Produce("Key", "message", "topicName", "partitionNumber");

                Console.WriteLine($"delivered to: {result.TopicPartitionOffset}");
            }
            catch (ProduceException<string, string> e)
            {
                Console.WriteLine($"failed to deliver message: {e.Message} [{e.Error.Code}]");
            }
        }

    }
}

var bootstrapServer = "172.16.10.48:9094";
var topicName = "Symbols";

//await CreateTopicAsync(bootstrapServer,topicName);
await ProduceMessage(bootstrapServer, topicName);
Console.WriteLine("Hello, World!");

Console.ReadLine();
using MassTransit;
using Newtonsoft.Json;
using Producer.Event;

namespace Consumer;

public class UserRegisteredConsumer : IConsumer<Message>
{
    //public Task Consume(ConsumeContext<UserRegistered> context)
    //{
    //    Console.Out.WriteLineAsync($"Received:{context.Message.UserName} , {context.Message.UserId}");
    //    return Task.CompletedTask;
    //}

    //public Task Consume(ConsumeContext<string> context)
    //{
    //    var user = JsonConvert.DeserializeObject<UserRegistered>(context.Message);
    //    Console.Out.WriteLineAsync($"Received:{user} ");
    //    return Task.CompletedTask;
    //}

    public Task Consume(ConsumeContext<Message> context)
    {
         var user = JsonConvert.DeserializeObject<UserRegistered>(context.Message.Body);
        Console.Out.WriteLineAsync($"Received :{user}");
        return Task.CompletedTask;
    }
}
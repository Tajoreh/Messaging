using System.Runtime.CompilerServices;
using MassTransit;
using Newtonsoft.Json;
using Producer.Event;

var bus = Bus.Factory.CreateUsingRabbitMq(a =>
{
    a.Host("rabbitmq://localhost");

});
await bus.StartAsync();



Console.WriteLine("Bus started");
while (true)
{
    Console.WriteLine("Press any key to publish");
    Console.ReadKey();

    
    var user = new UserRegistered()
    {
        UserId = Faker.RandomNumber.Next(10000),
        UserName = Faker.Internet.UserName()
    };



    var message = new Message
    {
       // MessageType = typeof(UserRegistered),
        Body = JsonConvert.SerializeObject(user)
    };


   
    await bus.Publish(message);



}


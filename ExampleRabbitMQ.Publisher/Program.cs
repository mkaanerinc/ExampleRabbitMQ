using RabbitMQ.Client;
using System.Text;

var factory = new ConnectionFactory();
factory.Uri = new Uri("amqps://cqnbuort:IzFjEYcDD3l8h7ILqS9mgBC-cbXfMUPZ@cow.rmq2.cloudamqp.com/cqnbuort");

using var connection = factory.CreateConnection();

var channel = connection.CreateModel();

channel.ExchangeDeclare(exchange:"logs-direct",durable:true,type:ExchangeType.Direct);

Enum.GetNames(typeof(LogNames)).ToList().ForEach(x =>
{
    var routeKey = $"route-{x}";
    var queueName = $"direct-queue-{x}";
    channel.QueueDeclare(queueName,true,false,false);

    channel.QueueBind(queueName,"logs-direct",routeKey);
});

Enumerable.Range(1,50).ToList().ForEach(x =>
{
    LogNames log = (LogNames) new Random().Next(1,5);

    string message = $"Log-type: {log}";

    var messageBody = Encoding.UTF8.GetBytes(message);

    var routeKey = $"route-{log}";

    channel.BasicPublish(exchange:"logs-direct",routingKey:routeKey, null, messageBody);

    Console.WriteLine($"Log gönderilmiştir : {message}");
});

Console.ReadLine();

public enum LogNames
{
    Critical=1,
    Error=2,
    Warning=3,
    Info=4,
}
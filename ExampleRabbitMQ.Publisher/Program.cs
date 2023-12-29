using RabbitMQ.Client;
using System.Text;

var factory = new ConnectionFactory();
factory.Uri = new Uri("amqps://cqnbuort:IzFjEYcDD3l8h7ILqS9mgBC-cbXfMUPZ@cow.rmq2.cloudamqp.com/cqnbuort");

using var connection = factory.CreateConnection();

var channel = connection.CreateModel();

channel.ExchangeDeclare(exchange:"logs-topic",durable:true,type:ExchangeType.Topic);

Random rnd = new Random();

Enumerable.Range(1,50).ToList().ForEach(x =>
{
    LogNames log1 = (LogNames)rnd.Next(1, 5);
    LogNames log2 = (LogNames)rnd.Next(1, 5);
    LogNames log3 = (LogNames)rnd.Next(1, 5);

    var routeKey = $"{log1}.{log2}.{log3}";

    string message = $"Log-type: {log1}-{log2}-{log3}";
    var messageBody = Encoding.UTF8.GetBytes(message);

    channel.BasicPublish(exchange:"logs-topic",routingKey:routeKey, null, messageBody);

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
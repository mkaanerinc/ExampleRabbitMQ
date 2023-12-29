using RabbitMQ.Client;
using System.Text;

var factory = new ConnectionFactory();
factory.Uri = new Uri("amqps://cqnbuort:IzFjEYcDD3l8h7ILqS9mgBC-cbXfMUPZ@cow.rmq2.cloudamqp.com/cqnbuort");

using var connection = factory.CreateConnection();

var channel = connection.CreateModel();

channel.ExchangeDeclare(exchange:"logs-fanout",durable:true,type:ExchangeType.Fanout);

Enumerable.Range(1,50).ToList().ForEach(x =>
{
    string message = $"Log {x}";

    var messageBody = Encoding.UTF8.GetBytes(message);

    channel.BasicPublish(exchange:"logs-fanout",routingKey:"", null, messageBody);

    Console.WriteLine($"Mesaj gönderilmiştir : {message}");
});

Console.ReadLine();

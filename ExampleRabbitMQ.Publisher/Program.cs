using RabbitMQ.Client;
using System.Text;

var factory = new ConnectionFactory();
factory.Uri = new Uri("amqps://cqnbuort:IzFjEYcDD3l8h7ILqS9mgBC-cbXfMUPZ@cow.rmq2.cloudamqp.com/cqnbuort");

using var connection = factory.CreateConnection();

var channel = connection.CreateModel();

channel.ExchangeDeclare(exchange:"header-exchange",durable:true,type:ExchangeType.Headers);

Dictionary<string,object> headers = new Dictionary<string, object>();

headers.Add("format","pdf");
headers.Add("shape","a4");

var properties = channel.CreateBasicProperties();
properties.Headers = headers;

channel.BasicPublish("header-exchange",string.Empty,properties,
    Encoding.UTF8.GetBytes("Header message"));

Console.WriteLine("Mesaj gönderilmiştir...");

Console.ReadLine();

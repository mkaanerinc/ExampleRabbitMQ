using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

var factory = new ConnectionFactory();
factory.Uri = new Uri("amqps://cqnbuort:IzFjEYcDD3l8h7ILqS9mgBC-cbXfMUPZ@cow.rmq2.cloudamqp.com/cqnbuort");

using var connection = factory.CreateConnection();

var channel = connection.CreateModel();

var randomQueueName = channel.QueueDeclare().QueueName; // Random name for queue

channel.QueueBind(randomQueueName,"logs-fanout","",null); // Queue will be deleted when consumer stooped.

channel.BasicQos(0,1,false);

var consumer = new EventingBasicConsumer(channel);

channel.BasicConsume(randomQueueName,false,consumer);

Console.WriteLine("Loglar dinleniyor...");

consumer.Received += (object sender, BasicDeliverEventArgs e) =>
{
    var message = Encoding.UTF8.GetString(e.Body.ToArray());

    Thread.Sleep(1500);

    Console.WriteLine("Gelen Mesaj:" + message);

    channel.BasicAck(e.DeliveryTag,false);
};

Console.ReadLine();

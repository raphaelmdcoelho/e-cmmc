using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Threading.Channels;

public class RabbitMQConsumer : IHostedService
{
    private readonly ConnectionFactory _factory;
    private readonly string _queueName;
    private readonly IServiceProvider _serviceProvider;
    private IConnection _connection;
    private IModel _channel;

    public RabbitMQConsumer(ConnectionFactory factory, string queueName, IServiceProvider serviceProvider)
    {
        _factory = factory;
        _queueName = queueName;
        _serviceProvider = serviceProvider;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _connection = _factory.CreateConnection();
        _channel = _connection.CreateModel();
        _channel.QueueDeclare(_queueName,
                            durable: false,
                            exclusive: false,
                            autoDelete: false,
                            arguments: null);

        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += (model, ea) =>
        {



            var body = ea.Body.ToArray();
            var contentMessage = Encoding.UTF8.GetString(body);
            Console.WriteLine($"Received message: {contentMessage}");
            // Process the message as needed

            MessageStore.AddMessage(contentMessage); // Store the message



            // using (var scope = _serviceProvider.CreateScope())
            // {
            //     var context = scope.ServiceProvider.GetRequiredService<MessageDbContext>();

            //     var body = ea.Body.ToArray();
            //     var contentMessage = Encoding.UTF8.GetString(body);
            //     Console.WriteLine($"Received message: {contentMessage}");
            //     // Process the message as needed

            //     MessageStore.AddMessage(contentMessage); // Store the message

            //     var message = new MessageEntity
            //     {
            //         product_name = contentMessage
            //     };

            //     context.Messages.Add(message);
            //     context.SaveChanges();
            // }
        };

        _channel.BasicConsume(queue: _queueName,
                             autoAck: true,
                             consumer: consumer);

        Console.WriteLine("Consumer started");
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _channel?.Close();
        _connection?.Close();
        Console.WriteLine("RabbitMQ Consumer stopped");
        return Task.CompletedTask;
    }
}

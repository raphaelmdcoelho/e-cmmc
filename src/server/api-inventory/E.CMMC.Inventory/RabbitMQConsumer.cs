using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

public class RabbitMQConsumer
{
    private readonly ConnectionFactory _factory;
    private readonly string _queueName;

    public RabbitMQConsumer(ConnectionFactory factory, string queueName)
    {
        _factory = factory;
        _queueName = queueName;
    }

    public void Start()
    {
        using var connection = _factory.CreateConnection();
        using var channel = connection.CreateModel();
        channel.QueueDeclare(_queueName,
                            durable: true,
                            exclusive: false,
                            autoDelete: false,
                            arguments: null);

        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var contentMessage = Encoding.UTF8.GetString(body);
            Console.WriteLine($"Received message: {contentMessage}");
            // Process the message as needed

            MessageStore.AddMessage(contentMessage); // Store the message

            var message = new MessageEntity
            {
                product_name = contentMessage
            };

            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<MessageDbContext>();
                dbContext.Messages.Add(message);
                await dbContext.SaveChangesAsync();
            }
        };

        channel.BasicConsume(queue: _queueName,
                             autoAck: true,
                             consumer: consumer);

        Console.WriteLine("Consumer started");
        // Keep the consumer running
        while (true) { }
    }
}

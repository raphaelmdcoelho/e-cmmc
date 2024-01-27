using RabbitMQ.Client;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<MessageDbContext>(options =>
    options.UseNpgsql("Your_Connection_String_Here"));

// Configure RabbitMQ ConnectionFactory
var factory = new ConnectionFactory
{
    HostName = "e-cmmc-brk",
    UserName = "rabbitmq",
    Password = "rabbitmq"
};

// Register RabbitMQConsumer
builder.Services.AddSingleton(factory);
builder.Services.AddHostedService<RabbitMQConsumer>(sp => 
{
    var connectionFactory = sp.GetRequiredService<ConnectionFactory>();
    var maxAttempts = 5;
    var delay = TimeSpan.FromSeconds(5);

    for (int attempt = 1; attempt <= maxAttempts; attempt++)
    {
        try
        {
            var connection = connectionFactory.CreateConnection();
            connection.Dispose(); // Dispose the connection as we just want to test if we can connect
            Console.WriteLine("Successfully connected to RabbitMQ");
            break; // Exit the loop if connection is successful
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Attempt {attempt} failed: {ex.Message}");
            if (attempt == maxAttempts)
            {
                throw; // Re-throw the exception if all attempts fail
            }
            Thread.Sleep(delay); // Wait before the next attempt
        }
    }

    return new RabbitMQConsumer(connectionFactory, "inventory", sp);
});

var app = builder.Build();

//// Start RabbitMQ Consumer
//var consumer = app.Services.GetRequiredService<RabbitMQConsumer>();
//await Task.Run(() => consumer.StartAsync());

// Configure the HTTP request pipeline.
app.MapGet("/messages", () => MessageStore.GetAllMessages());
app.MapGet("/messages/latest", () => MessageStore.GetLatestMessage() ?? "No messages available");

app.Run();

Console.WriteLine("Started web host");

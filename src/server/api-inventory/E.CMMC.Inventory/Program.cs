using RabbitMQ.Client;

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
builder.Services.AddSingleton<RabbitMQConsumer>(sp => 
    new RabbitMQConsumer(sp.GetRequiredService<ConnectionFactory>(), "inventory"));

var app = builder.Build();

// Start RabbitMQ Consumer
var consumer = app.Services.GetRequiredService<RabbitMQConsumer>();
await Task.Run(() => consumer.Start());

// Configure the HTTP request pipeline.
app.MapGet("/messages", () => MessageStore.GetAllMessages());
app.MapGet("/messages/latest", () => MessageStore.GetLatestMessage() ?? "No messages available");

app.Run();

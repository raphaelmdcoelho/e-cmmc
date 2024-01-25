const express = require('express');
const amqp = require('amqplib');

const app = express();
const port = 5001;

const rabbitMqUrl = 'amqp://rabbitmq:rabbitmq@e-cmmc-brk'; // Replace with your RabbitMQ server URL
const queue = 'inventory'; // Replace with your desired queue name
const message = 'search for a item'; // Your fixed message

async function sendToRabbitMQ() {
    const connection = await amqp.connect(rabbitMqUrl);
    const channel = await connection.createChannel();
    await channel.assertQueue(queue, {
        durable: false
    });

    channel.sendToQueue(queue, Buffer.from(message));
    console.log(`Sent: ${message}`);

    setTimeout(() => {
        connection.close();
    }, 500); // Close connection after 500ms
}

app.get('/send', async (req, res) => {
    try {
        await sendToRabbitMQ();
        res.send('Message sent to RabbitMQ');
    } catch (error) {
        console.error(error);
        res.status(500).send('Error sending message to RabbitMQ');
    }
});

app.listen(port, () => {
    console.log(`Server running on http://localhost:${port}`);
});

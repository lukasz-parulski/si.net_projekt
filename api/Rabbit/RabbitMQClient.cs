using System;
using System.Threading;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;

namespace api.Rabbit
{
    public abstract class RabbitMqClient : IDisposable
    {
    protected IModel Channel { get; private set; }
    protected MongoClient DbClient;
    private IConnection _connection;
    private readonly ConnectionFactory _connectionFactory;

    protected RabbitMqClient(ConnectionFactory connectionFactory, MongoClient client)
    {
        _connectionFactory = connectionFactory;
        DbClient = client;
        ConnectToRabbitMq();
    }

    private void ConnectToRabbitMq()
    {
        if (_connection == null || _connection.IsOpen == false)
        {
            var tries = 10;
            while((tries--) > 0) {
                try {
                    _connection = _connectionFactory.CreateConnection();
                    Console.WriteLine("Connected to rabbitmq.");
                    break;
                } catch(BrokerUnreachableException ex)
                {
                    Thread.Sleep(10000);
                }
            }
        }

        if (Channel == null || Channel.IsOpen == false)
        {
            Channel = _connection.CreateModel();
            Channel.QueueDeclare(queue: "main_queue", durable: true, exclusive: false, autoDelete: false);
        }
    }

    public void Dispose()
    {
        try
        {
            Channel?.Close();
            Channel?.Dispose();
            Channel = null;

            _connection?.Close();
            _connection?.Dispose();
            _connection = null;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Cannot dispose RabbitMQ channel or connection");
        }
    }
    }
}
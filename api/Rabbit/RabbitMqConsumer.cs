using System;
using System.Threading;
using System.Threading.Tasks;
using api.Model;
using Microsoft.Extensions.Hosting;
using MongoDB.Driver;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using ConnectionFactory = RabbitMQ.Client.ConnectionFactory;

namespace api.Rabbit
{
    public class RabbitMqConsumer : ConsumingHandler, IHostedService
    {
        public RabbitMqConsumer(
            ConnectionFactory connectionFactory, MongoClient client) :
            base(connectionFactory, client)
        {
            try
            {
                var consumer = new AsyncEventingBasicConsumer(Channel);
                consumer.Received += OnReceive<SensorReading>;
                Channel.BasicConsume(queue: "main_queue", autoAck: false, consumer: consumer);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while consuming message");
            }
        }

        public virtual Task StartAsync(CancellationToken cancellationToken) => Task.CompletedTask;

        public virtual Task StopAsync(CancellationToken cancellationToken)
        {
            Dispose();
            return Task.CompletedTask;
        }
    }
}
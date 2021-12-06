using System;
using System.Text;
using System.Threading.Tasks;
using api.Model;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace api.Rabbit
{
    public abstract class ConsumingHandler : RabbitMqClient
    {

        public ConsumingHandler(ConnectionFactory connectionFactory, MongoClient client) : base(connectionFactory, client)
        {
        }

        protected virtual async Task OnReceive<T>(object sender, BasicDeliverEventArgs @event)
        {
            try
            {
                var body = Encoding.UTF8.GetString(@event.Body.ToArray());
                var messageArray = body.Split(";");
                Console.WriteLine("Received: [{0}]", string.Join(", ", messageArray));

                var sensorType = messageArray[0];
                var sensorId = int.Parse(messageArray[1]);
                var value = double.Parse(messageArray[2]);

                SensorReading reading = new SensorReading(
                    MongoDB.Bson.ObjectId.GenerateNewId(), 
                    sensorId, 
                    value, 
                    sensorType
                    );

                string collectionName = "";
                
                switch (reading.SensorType)
                {
                    case SensorTypes.TEMPERATURE_SENSOR:
                        collectionName = CollectionTypes.TEMPERATURE_COLLECTION;
                        break;
                    case SensorTypes.PRESSURE_SENSOR:
                        collectionName = CollectionTypes.PRESSURE_COLLECTION;
                        break;
                    case SensorTypes.WIND_SENSOR:
                        collectionName = CollectionTypes.WIND_COLLECTION;
                        break;
                    case SensorTypes.BEARS_SENSOR:
                        collectionName = CollectionTypes.BEARS_COLLECTION;
                        break;
                    default:
                        Console.WriteLine("Unexpected sensor type!");
                        throw new Exception("Unexpected sensor type!");
                }

                var database = DbClient.GetDatabase(Environment.GetEnvironmentVariable("DB_NAME"));
                var collection = database.GetCollection<SensorReading>(collectionName);

                await collection.InsertOneAsync(reading);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while processing message.");
                Console.WriteLine(ex);
            }
            finally
            {
                Channel.BasicAck(@event.DeliveryTag, false);
            }
        }
    }
}
using System;
using System.Collections.Generic;
using api.Model;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace api.Controllers
{
    [ApiController]
    [Route("/api")]
    public class SensorsController : ControllerBase
    {
        private MongoClient _dbClient;
        private IMongoDatabase database;

        public SensorsController(MongoClient dbClient)
        {
            _dbClient = dbClient;
            database = _dbClient.GetDatabase(Environment.GetEnvironmentVariable("DB_NAME"));
        }


        [Route("temperature")]
        [HttpGet]
        public List<SensorReading> GetTemperature()
        {
            var collection = database.GetCollection<SensorReading>(CollectionTypes.TEMPERATURE_COLLECTION);
            return collection.Find(_ => true).ToList();
        }
        
        [Route("pressure")]
        [HttpGet]
        public List<SensorReading> GetPressure()
        {
            var collection = database.GetCollection<SensorReading>(CollectionTypes.PRESSURE_COLLECTION);
            return collection.Find(_ => true).ToList();
        }
        
        [Route("wind")]
        [HttpGet]
        public List<SensorReading> GetWind()
        {
            var collection = database.GetCollection<SensorReading>(CollectionTypes.WIND_COLLECTION);
            return collection.Find(_ => true).ToList();
        }
        
        [Route("bears")]
        [HttpGet]
        public List<SensorReading> GetBears()
        {
            var collection = database.GetCollection<SensorReading>(CollectionTypes.BEARS_COLLECTION);
            return collection.Find(_ => true).ToList();
        }
    }
}
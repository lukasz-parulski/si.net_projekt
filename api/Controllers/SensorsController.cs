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
        public List<SensorReadingDto> GetTemperature()
        {
            var collection = database.GetCollection<SensorReading>(CollectionTypes.TEMPERATURE_COLLECTION);
            return ConvertToDto(collection.Find(_ => true).ToList());
        }
        
        [Route("pressure")]
        [HttpGet]
        public List<SensorReadingDto> GetPressure()
        {
            var collection = database.GetCollection<SensorReading>(CollectionTypes.PRESSURE_COLLECTION);
            return ConvertToDto(collection.Find(_ => true).ToList());
        }
        
        [Route("wind")]
        [HttpGet]
        public List<SensorReadingDto> GetWind()
        {
            var collection = database.GetCollection<SensorReading>(CollectionTypes.WIND_COLLECTION);
            return ConvertToDto(collection.Find(_ => true).ToList());
        }
        
        [Route("bears")]
        [HttpGet]
        public List<SensorReadingDto> GetBears()
        {
            var collection = database.GetCollection<SensorReading>(CollectionTypes.BEARS_COLLECTION);
            return ConvertToDto(collection.Find(_ => true).ToList());
        }
        
        [NonAction]
        private List<SensorReadingDto> ConvertToDto(List<SensorReading> sensorReadings)
        {
            List<SensorReadingDto> sensorReadingsDto = new List<SensorReadingDto>();
            sensorReadings.ForEach(x =>
            {
                sensorReadingsDto.Add(new SensorReadingDto(x));
            });
            return sensorReadingsDto;
        }
    }
}
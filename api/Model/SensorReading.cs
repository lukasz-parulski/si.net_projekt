using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace api.Model
{
    public  class SensorReading
    {
        public SensorReading(ObjectId id, int sensorId, double value, string type)
        {
            _id = id;
            SensorId = sensorId;
            Value = value;
            SensorType = type;
        }
        
        [BsonId]      
        public ObjectId _id { get; set; }

        public int SensorId { get; set; }
        
        public double Value { get; set; }

        public string SensorType { get; set; }
    }
}
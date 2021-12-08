namespace api.Model
{
    public class SensorReadingDto
    {
        public SensorReadingDto(SensorReading sensorReading)
        {
            SensorId = sensorReading.SensorId;
            Value = sensorReading.Value;
            SensorType = sensorReading.SensorType;
        }
        
        public int SensorId { get; set; }
                
        public double Value { get; set; }
        
        public string SensorType { get; set; }
    }
}
using System;

namespace front.Models
{
    public class SensorReading
    {
        public int sensorId { get; set; }
        
        public double value { get; set; }

        public string sensorType { get; set; }
    }
}
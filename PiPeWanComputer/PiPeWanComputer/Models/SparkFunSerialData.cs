using System;

namespace PiPeWanComputer.Models
{
    public class SparkFunSerialData
    {
        public double Temperature { get; }
        public double Flow { get; }
        public DateTime Time { get; }

        public SparkFunSerialData(double temperature, double flow)
        {
            Temperature = temperature;
            Flow = flow;
            Time = DateTime.Now;
        }
    }
}

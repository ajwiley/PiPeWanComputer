using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PiPeWanComputer {
    public class TemperatureDatum {
        public double Temperature { get; }
        public double Time { get; }

        public TemperatureDatum(double temperature, double time) {
            Temperature = temperature;
            Time = time;
        }
    }
}

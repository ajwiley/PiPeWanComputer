using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PiPeWanComputer {
    public class TemperatureDatum {
        public double Temperature { get; set; }
        public double Time { get; set; }

        public TemperatureDatum(double temperature, double time) {
            Temperature = temperature;
            Time = time;
        }
    }
}

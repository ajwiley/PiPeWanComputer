using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PiPeWanComputer {
    public class TemperatureGraph {
        public double Temp { get; set; }
        public double Time { get; set; }

        public TemperatureGraph(double Temperature, double Milliseconds) {
            Temp = Temperature;
            Time = Milliseconds;
        }
    }
}

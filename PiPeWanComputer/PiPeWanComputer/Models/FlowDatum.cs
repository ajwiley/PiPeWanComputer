using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PiPeWanComputer {
    public class FlowDatum {
        public double Flow { get; }
        public double Time { get; }

        public FlowDatum(double flow, double time) {
            Flow = flow;
            Time = time;
        }
    }
}

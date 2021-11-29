using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PiPeWanComputer {
    public class FlowGraph {
        public double Flow { get; set; }
        public double Time { get; set; }

        public FlowGraph(double Flow, double Milliseconds) {
            this.Flow = Flow;
            Time = Milliseconds;
        }
    }
}

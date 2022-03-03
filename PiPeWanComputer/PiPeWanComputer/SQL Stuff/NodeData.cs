using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PiPeWanComputer.SQL_Stuff {
    public class NodeData : BaseConnection{
        public float Battery;
        public float Temperature;
        public float Flow;
        public NodeStatus Status;

        public NodeData(float battery, float temperature, float flow, NodeStatus status) {
            Battery = battery;
            Temperature = temperature;
            Flow = flow;
            Status = status;
        }
    }
}

public enum NodeStatus {
    IDLE,
    RUNNING,
    PAUSED
}

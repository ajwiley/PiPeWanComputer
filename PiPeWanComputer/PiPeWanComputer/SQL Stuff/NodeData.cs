using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PiPeWanComputer.SQL_Stuff {
    public enum NodeStatus {
        DEFAULT = -1,
        DISCONNECTED,
        IDLE,
        RUNNING,
        PAUSED
    }

    public class NodeData : BaseConnection{
        public int NodeID { get; }
        public float Battery { get; }
        public float Temperature { get; }
        public float Flow { get; }
        public NodeStatus Status { get; }

        public NodeData(int nodeID, float battery, float temperature, float flow, NodeStatus status = NodeStatus.IDLE) {
            NodeID = nodeID;
            Battery = battery;
            Temperature = temperature;
            Flow = flow;
            Status = status;
        }
    }
}

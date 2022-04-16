using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace PiPeWanComputer.SQL_Stuff {
    public class Node : BaseConnection {
        public int NodeID { get; }
        public string NodeName { get; }
        public string IPAddress { get; }
        public string LocationName { get; }
        public DateTime? LastUpdated { get; }

        public Node(int nodeID, string ipAddress, string nodeName = "", string locationName = "", DateTime? lastUpdated = null) {
            NodeID = nodeID;
            NodeName = nodeName;
            IPAddress = ipAddress;
            LocationName = locationName;
            LastUpdated = lastUpdated;
        }
    }
}

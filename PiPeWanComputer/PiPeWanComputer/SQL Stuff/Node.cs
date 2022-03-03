using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PiPeWanComputer.SQL_Stuff {
    public class Node : BaseConnection {
        public string NodeName;
        public IPAddress IPAddress;
        public string LocationName;

        public Node(string nodeName, IPAddress ipAddress) : this(nodeName, ipAddress, "") { }

        public Node(string nodeName, IPAddress ipAddress, string locationName) {
            NodeName = nodeName;
            IPAddress = ipAddress;
            LocationName = locationName;
        }
    }
}

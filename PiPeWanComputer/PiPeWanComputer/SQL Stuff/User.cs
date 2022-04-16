using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PiPeWanComputer.SQL_Stuff {
    public class User : BaseConnection {
        public string UserName { get; }
        public byte[] PasswordHash { get; }
        public int AccessLevel { get; }

        public User(string userName, byte[] passwordHash, int accessLevel = 0) {
            UserName = userName;
            PasswordHash = passwordHash;
            AccessLevel = accessLevel;
        }
    }
}

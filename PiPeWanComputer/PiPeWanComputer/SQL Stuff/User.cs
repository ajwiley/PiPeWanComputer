using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PiPeWanComputer.SQL_Stuff {
    public class User : BaseConnection {
        public string UserName;
        public byte[] PasswordHash;

        public User(string userName, byte[] passwordHash) {
            UserName = userName;
            PasswordHash = passwordHash;
        }
    }
}

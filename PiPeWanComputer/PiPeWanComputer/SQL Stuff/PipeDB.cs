using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Transactions;

namespace PiPeWanComputer.SQL_Stuff {
    public static class PipeDB {
        private static string ConnectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=PipeWan;Integrated Security=True;Connect Timeout=30;";
        private static string ConnectionStringMaster = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=master;";
        private static string ProjectDirectory = ProjectSourcePath.Value;
        private static string PipeWanFile = ProjectDirectory + @"SQL stuff\PipeWan.mdf";
        private static string PipeWanLogFile = ProjectDirectory + @"SQL stuff\PipeWanLog.ldf";
        private static string Procedures = ProjectDirectory + @"SQL stuff\Procedures\";
        private static string Setup = ProjectDirectory + @"SQL stuff\Setup\";
        private static string Tables = ProjectDirectory + @"SQL stuff\Tables\";

        /// <summary>
        /// Creates the database, tables, and procedures. Fills the tables with mock data.
        /// </summary>
        public static void CreateDB() {
            string sql = String.Join(Environment.NewLine, new string[] {
                "DROP DATABASE IF EXISTS PipeWan",
                "CREATE DATABASE PipeWan ON PRIMARY (",
                    "NAME = PipeWanData,",
                    $"FILENAME = '{PipeWanFile}',",
                    "SIZE = 2MB,",
                    "MAXSIZE = 10MB,",
                    "FILEGROWTH = 10%",
                ")",
                "LOG ON (",
                    "NAME = PipeWanLog,",
                    $"FILENAME = '{PipeWanLogFile}',",
                    "SIZE = 1MB,",
                    "MAXSIZE = 5MB,",
                    "FILEGROWTH = 10%",
                ")"
            });

            IEnumerable<string> commandStrings = Regex.Split(sql, @"^\s*GO\s*$", RegexOptions.Multiline | RegexOptions.IgnoreCase);

            using (SqlConnection connect = new SqlConnection(ConnectionStringMaster)) {
                connect.Open();
                foreach (string commandString in commandStrings) {
                    if (commandString.Trim() != "") {
                        using (var command = new SqlCommand(commandString, connect)) {
                            try {
                                command.ExecuteNonQuery();
                            }
                            catch (SqlException ex) {
                                Console.WriteLine(ex.Message);
                            }
                            finally {
                                connect.Close();
                            }
                        }
                    }
                }
            }

            // Drop the Tables, and Constraints
            RunSqlScriptFile(Setup + "Drop.sql");

            // Create the Tables
            RunSqlScriptFile(Tables + "User.sql");
            RunSqlScriptFile(Tables + "Node.sql");
            RunSqlScriptFile(Tables + "NodeData.sql");

            // Insert the Mock Data into the Tables
            RunSqlScriptFile(Setup + "Data.sql");

            // Create the Procedures
            // DELETE
            RunSqlScriptFile(Procedures + @"\DELETE\DeleteUser.sql");
            RunSqlScriptFile(Procedures + @"\DELETE\DeleteNode.sql");
            RunSqlScriptFile(Procedures + @"\DELETE\DeleteNodeData.sql");

            // INSERT
            RunSqlScriptFile(Procedures + @"\INSERT\AddUser.sql");
            RunSqlScriptFile(Procedures + @"\INSERT\AddNode.sql");
            RunSqlScriptFile(Procedures + @"\INSERT\AddNodeData.sql");

            // UPDATE
            RunSqlScriptFile(Procedures + @"\UPDATE\UpdateUser.sql");
            RunSqlScriptFile(Procedures + @"\UPDATE\UpdateNode.sql");
            RunSqlScriptFile(Procedures + @"\UPDATE\UpdateNodeData.sql");

            // SELECT
            RunSqlScriptFile(Procedures + @"\SELECT\SelectUser.sql");
            RunSqlScriptFile(Procedures + @"\SELECT\SelectNode.sql");
            RunSqlScriptFile(Procedures + @"\SELECT\SelectNodeData.sql");
        }

        /// <summary>
        /// Runs the given sql script against the database. Can be used to create procedures, tables, etc.
        /// </summary>
        /// <param name="sqlFile">The full path to the Sql file to execute.</param>
        /// <returns>Success or fail</returns>
        private static bool RunSqlScriptFile(string sqlFile) {
            try {
                string script = File.ReadAllText(sqlFile);

                // Split script on GO command
                IEnumerable<string> commandStrings = Regex.Split(script, @"^\s*GO\s*$", RegexOptions.Multiline | RegexOptions.IgnoreCase);
                using (SqlConnection connect = new SqlConnection(ConnectionString)) {
                    connect.Open();
                    foreach (string commandString in commandStrings) {
                        if (commandString.Trim() != "") {
                            using (var command = new SqlCommand(commandString, connect)) {
                                try {
                                    command.ExecuteNonQuery();
                                    Console.WriteLine(string.Format("Successfully executed {0}", sqlFile));
                                }
                                catch (SqlException ex) {
                                    Console.WriteLine(ex.Message);
                                    return false;
                                }
                            }
                        }
                    }
                    connect.Close();
                }

                return true;
            }
            catch (Exception ex) {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public static void AddUser(User user) {
            string script = File.ReadAllText(Procedures + "AddUser.sql");

            using (TransactionScope scope = new TransactionScope()) {
                using (SqlConnection connect = new SqlConnection(ConnectionString)) {
                    using (SqlCommand command = new SqlCommand("[dbo].AddUser", connect)) {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.Add("UserName", SqlDbType.NVarChar).Value = user.UserName;
                        command.Parameters.Add("PasswordHash", SqlDbType.Binary, (64)).Value = user.PasswordHash;
                        command.Parameters.Add("AccessLevel", SqlDbType.Int).Value = user.AccessLevel;

                        connect.Open();

                        SqlDataReader reader = command.ExecuteReader();

                        scope.Complete();
                    }
                }
            }
        }
        public static void AddUser(string userName, byte[] passwordHash, int accessLevel = 0) {
            AddUser(new User(userName, passwordHash, accessLevel));
        }

        public static void AddNode(Node node) {
            string script = File.ReadAllText(Procedures + "AddUser.sql");

            using (TransactionScope scope = new TransactionScope()) {
                using (SqlConnection connect = new SqlConnection(ConnectionString)) {
                    using (SqlCommand command = new SqlCommand("[dbo].AddNode", connect)) {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.Add("NodeName", SqlDbType.NVarChar).Value = node.NodeName;
                        command.Parameters.Add("IPAddress", SqlDbType.NVarChar).Value = node.IPAddress;
                        command.Parameters.Add("LocationName", SqlDbType.NVarChar).Value = node.LocationName;

                        connect.Open();

                        SqlDataReader reader = command.ExecuteReader();

                        scope.Complete();
                    }
                }
            }
        }
        public static void AddNode(int nodeID, string nodeName, string ipAddress, string LocationName = "") {
            AddNode(new Node(nodeID, nodeName, ipAddress, LocationName));
        }

        public static void AddNodeData(NodeData nodeData) {
            string script = File.ReadAllText(Procedures + "AddUser.sql");

            using (TransactionScope scope = new TransactionScope()) {
                using (SqlConnection connect = new SqlConnection(ConnectionString)) {
                    using (SqlCommand command = new SqlCommand("[dbo].AddNode", connect)) {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.Add("Battery", SqlDbType.Float).Value = nodeData.Battery;
                        command.Parameters.Add("Temperature", SqlDbType.NVarChar).Value = nodeData.Temperature;
                        command.Parameters.Add("Flow", SqlDbType.NVarChar).Value = nodeData.Flow;
                        command.Parameters.Add("Status", SqlDbType.NVarChar).Value = nodeData.Status;

                        connect.Open();

                        SqlDataReader reader = command.ExecuteReader();

                        scope.Complete();
                    }
                }
            }
        }
        public static void AddNodeData(int nodeID, float battery = 0f, float temperature = 0f, float flow = 0f, NodeStatus status = NodeStatus.IDLE) {
            AddNodeData(new NodeData(nodeID, battery, temperature, flow, status));
        }

        public static void DeleteUser(User user) {
            using (TransactionScope scope = new TransactionScope()) {
                using (SqlConnection connect = new SqlConnection(ConnectionString)) {
                    using (SqlCommand command = new SqlCommand("[dbo].DeleteUser", connect)) {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.Add("UserName", SqlDbType.NVarChar).Value = user.UserName;

                        connect.Open();

                        command.ExecuteReader();

                        scope.Complete();
                    }
                }
            }
        }
        public static void DeleteUser(string userName) {
            DeleteUser(new User(userName));
        }

        public static void DeleteNode(Node node) {
            using (TransactionScope scope = new TransactionScope()) {
                using (SqlConnection connect = new SqlConnection(ConnectionString)) {
                    using (SqlCommand command = new SqlCommand("[dbo].DeleteNode", connect)) {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.Add("NodeID", SqlDbType.NVarChar).Value = node.NodeID;

                        connect.Open();

                        command.ExecuteReader();

                        scope.Complete();
                    }
                }
            }
        }
        public static void DeleteNode(int nodeID) {
            DeleteNode(new Node(nodeID));
        }

        public static void DeleteNodeData(NodeData nodeData) {
            using (TransactionScope scope = new TransactionScope()) {
                using (SqlConnection connect = new SqlConnection(ConnectionString)) {
                    using (SqlCommand command = new SqlCommand("[dbo].DeleteNodeData", connect)) {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.Add("NodeID", SqlDbType.NVarChar).Value = nodeData.NodeID;

                        connect.Open();

                        command.ExecuteReader();

                        scope.Complete();
                    }
                }
            }
        }
        public static void DeleteNodeData(int nodeID) {
            DeleteNodeData(new NodeData(nodeID));
        }

        public static List<User> SelectAllUsers() {
            var users = new List<User>();

            using (SqlConnection connect = new SqlConnection(ConnectionString)) {
                using (SqlCommand command = new SqlCommand("[dbo].SelectUsers", connect)) {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add("UserName", SqlDbType.Int).Value = "";

                    connect.Open();

                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read()) {
                        users.Add(new User(
                                (string)reader["UserName"],
                                (byte[])reader["PasswordHash"],
                                (int)reader["AccessLevel"]));
                    }

                    return users;
                }
            }
        }
        public static User? SelectUser(User user) {
            using (SqlConnection connect = new SqlConnection(ConnectionString)) {
                using (SqlCommand command = new SqlCommand("[dbo].SelectUsers", connect)) {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add("UserName", SqlDbType.Int).Value = user.UserName;

                    connect.Open();

                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read()) {
                        return new User(
                            (string)reader["UserName"],
                            (byte[])reader["PasswordHash"],
                            (int)reader["AccessLevel"]);
                    }

                    return null;
                }
            }
        }
        public static User? SelectUser(string userName) {
            return SelectUser(new User(userName));
        }

        public static List<Node> SelectAllNodes() {
            var nodes = new List<Node>();

            using (SqlConnection connect = new SqlConnection(ConnectionString)) {
                using (SqlCommand command = new SqlCommand("[dbo].SelectNodes", connect)) {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add("NodeID", SqlDbType.Int).Value = "";

                    connect.Open();

                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read()) {
                        nodes.Add(new Node(
                                (int)reader["NodeID"],
                                (string)reader["NodeName"],
                                (string)reader["IPAddress"],
                                (string)reader["LocationName"],
                                (DateTime)reader["LastUpdated"]));
                    }

                    return nodes;
                }
            }
        }
        public static Node? SelectNode(Node node) {
            using (SqlConnection connect = new SqlConnection(ConnectionString)) {
                using (SqlCommand command = new SqlCommand("[dbo].SelectNodes", connect)) {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add("NodeID", SqlDbType.Int).Value = node.NodeID;

                    connect.Open();

                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read()) {
                        return new Node(
                            (int)reader["NodeID"],
                            (string)reader["NodeName"],
                            (string)reader["IPAddress"],
                            (string)reader["LocationName"],
                            (DateTime)reader["LastUpdated"]);
                    }

                    return null;
                }
            }
        }
        public static Node? SelectNode(int nodeID) {
            return SelectNode(new Node(nodeID));
        }

        public static List<NodeData> SelectAllNodesData() {
            return SelectNodeData(-1);
        }
        public static List<NodeData> SelectNodeData(NodeData NodeData) {
            var NodeDatas = new List<NodeData>();

            using (SqlConnection connect = new SqlConnection(ConnectionString)) {
                using (SqlCommand command = new SqlCommand("[dbo].SelectNodeDatas", connect)) {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add("NodeDataID", SqlDbType.Int).Value = NodeData.NodeID;

                    connect.Open();

                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read()) {
                        NodeDatas.Add(new NodeData(
                                (int)reader["NodeID"],
                                (float)reader["Battery"],
                                (float)reader["Temperature"],
                                (float)reader["Flow"],
                                (NodeStatus)reader["Status"]));
                    }

                    return NodeDatas;
                }
            }
        }
        public static List<NodeData> SelectNodeData(int NodeID) {
            return SelectNodeData(new NodeData(NodeID));
        }

        public static void UpdateUser(User user) {
            using (TransactionScope scope = new TransactionScope()) {
                using (SqlConnection connect = new SqlConnection(ConnectionString)) {
                    using (SqlCommand command = new SqlCommand("[dbo].UpdateUser", connect)) {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.Add("UserName", SqlDbType.NVarChar, 15).Value = user.UserName;
                        command.Parameters.Add("PasswordHash", SqlDbType.Binary, 64).Value = user.PasswordHash;
                        command.Parameters.Add("AccessLevel", SqlDbType.Int).Value = user.AccessLevel;

                        connect.Open();

                        command.ExecuteReader();

                        scope.Complete();
                    }
                }
            }
        }
        public static void UpdateUser(string userName, byte[] passwordHash) {
            using (TransactionScope scope = new TransactionScope()) {
                using (SqlConnection connect = new SqlConnection(ConnectionString)) {
                    using (SqlCommand command = new SqlCommand("[dbo].UpdateUser", connect)) {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.Add("UserName", SqlDbType.NVarChar, 15).Value = userName;
                        command.Parameters.Add("PasswordHash", SqlDbType.Binary, 64).Value = passwordHash;

                        connect.Open();

                        command.ExecuteReader();

                        scope.Complete();
                    }
                }
            }
        }
        public static void UpdateUser(string userName, int accessLevel) {
            using (TransactionScope scope = new TransactionScope()) {
                using (SqlConnection connect = new SqlConnection(ConnectionString)) {
                    using (SqlCommand command = new SqlCommand("[dbo].UpdateUser", connect)) {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.Add("UserName", SqlDbType.NVarChar, 15).Value = userName;
                        command.Parameters.Add("AccessLevel", SqlDbType.Int).Value = accessLevel;

                        connect.Open();

                        command.ExecuteReader();

                        scope.Complete();
                    }
                }
            }
        }
        public static void UpdateUser(string userName, byte[] passwordHash, int accessLevel) {
            UpdateUser(new User(userName, passwordHash, accessLevel));
        }

        public static void UpdateNode(Node node) {
            using (TransactionScope scope = new TransactionScope()) {
                using (SqlConnection connect = new SqlConnection(ConnectionString)) {
                    using (SqlCommand command = new SqlCommand("[dbo].UpdateNode", connect)) {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.Add("NodeID", SqlDbType.Int).Value = node.NodeID;

                        if (node.NodeName != "")
                            command.Parameters.Add("NodeName", SqlDbType.NVarChar, 128).Value = node.NodeName;
                        if (node.IPAddress != "")
                            command.Parameters.Add("IPAddress", SqlDbType.NVarChar, 128).Value = node.IPAddress;
                        if (node.LocationName != "")
                            command.Parameters.Add("LocationName", SqlDbType.NVarChar, 128).Value = node.LocationName;

                        connect.Open();

                        command.ExecuteReader();

                        scope.Complete();
                    }
                }
            }
        }
        public static void UpdateNode(int nodeID, string nodeName = "", string ipAddress = "", string locationName = "") {
            if (string.IsNullOrEmpty(nodeName + ipAddress + locationName)) return;
            UpdateNode(new Node(nodeID, nodeName, ipAddress, locationName));
        }

        public static void UpdateNodeData(NodeData nodeData) {
            using (TransactionScope scope = new TransactionScope()) {
                using (SqlConnection connect = new SqlConnection(ConnectionString)) {
                    using (SqlCommand command = new SqlCommand("[dbo].UpdateNode", connect)) {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.Add("NodeID", SqlDbType.Int).Value = nodeData.NodeID;
                        if (nodeData.Battery != float.MinValue)
                            command.Parameters.Add("Battery", SqlDbType.Float).Value = nodeData.Battery;
                        if (nodeData.Temperature != float.MinValue)
                            command.Parameters.Add("Temperature", SqlDbType.Float).Value = nodeData.Temperature;
                        if (nodeData.Flow != float.MinValue)
                            command.Parameters.Add("Flow", SqlDbType.Float).Value = nodeData.Flow;
                        if (nodeData.Status != NodeStatus.DEFAULT)
                            command.Parameters.Add("Status", SqlDbType.NVarChar, 128).Value = nodeData.Status;

                        connect.Open();

                        command.ExecuteReader();

                        scope.Complete();
                    }
                }
            }
        }
        public static void UpdateNodeData(int nodeID, NodeStatus nodeStatus) {
            using (TransactionScope scope = new TransactionScope()) {
                using (SqlConnection connect = new SqlConnection(ConnectionString)) {
                    using (SqlCommand command = new SqlCommand("[dbo].UpdateNode", connect)) {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.Add("NodeID", SqlDbType.Int).Value = nodeID;
                        command.Parameters.Add("Status", SqlDbType.NVarChar, 128).Value = nodeStatus;

                        connect.Open();

                        command.ExecuteReader();

                        scope.Complete();
                    }
                }
            }
        }
        public static void UpdateNodeData(int nodeID, float battery = float.MinValue, float temperature = float.MinValue, float flow = float.MinValue, NodeStatus nodeStatus = NodeStatus.DEFAULT) {
            UpdateNodeData(new NodeData(nodeID, battery, temperature, flow, nodeStatus));
        }
    }
}

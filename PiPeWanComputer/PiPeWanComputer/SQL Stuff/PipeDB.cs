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
        private static readonly string ConnectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=PipeWan;Integrated Security=True;Connect Timeout=30;";
        private static readonly string ConnectionStringMaster = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=master;";
        private static readonly string ProjectDirectory = ProjectSourcePath.Value;
        private static readonly string PipeWanFile = ProjectDirectory + @"SQL stuff\PipeWan.mdf";
        private static readonly string PipeWanLogFile = ProjectDirectory + @"SQL stuff\PipeWanLog.ldf";
        private static readonly string Procedures = ProjectDirectory + @"SQL stuff\Procedures\";
        private static readonly string Setup = ProjectDirectory + @"SQL stuff\Setup\";
        private static readonly string Tables = ProjectDirectory + @"SQL stuff\Tables\";

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

            using (SqlConnection connect = new(ConnectionStringMaster)) {
                connect.Open();
                foreach (string commandString in commandStrings) {
                    if (commandString.Trim() != "") {
                        using var command = new SqlCommand(commandString, connect);
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
            RunSqlScriptFile(Procedures + @"DELETE\DeleteUser.sql");
            RunSqlScriptFile(Procedures + @"DELETE\DeleteNode.sql");
            RunSqlScriptFile(Procedures + @"DELETE\DeleteNodeData.sql");

            // INSERT
            RunSqlScriptFile(Procedures + @"INSERT\AddUser.sql");
            RunSqlScriptFile(Procedures + @"INSERT\AddNode.sql");
            RunSqlScriptFile(Procedures + @"INSERT\AddNodeData.sql");

            // UPDATE
            RunSqlScriptFile(Procedures + @"UPDATE\UpdateUser.sql");
            RunSqlScriptFile(Procedures + @"UPDATE\UpdateNode.sql");
            RunSqlScriptFile(Procedures + @"UPDATE\UpdateNodeData.sql");

            // SELECT
            RunSqlScriptFile(Procedures + @"SELECT\SelectUser.sql");
            RunSqlScriptFile(Procedures + @"SELECT\SelectNode.sql");
            RunSqlScriptFile(Procedures + @"SELECT\SelectNodeData.sql");


            // Testing Only
            /*{
                PipeDB.AddNodeData(1, temperature: 20f, flow: 40);
                PipeDB.AddNodeData(1, temperature: 21f, flow: 42);
                PipeDB.AddNodeData(1, temperature: 22f, flow: 40);
                PipeDB.AddNodeData(1, temperature: 23f, flow: 38);
                PipeDB.AddNodeData(1, temperature: 24f, flow: 36);
            }*/
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
            using var scope = new TransactionScope();
            using var connect = new SqlConnection(ConnectionString);
            using var command = new SqlCommand("[dbo].AddUser", connect);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.Add("UserName", SqlDbType.NVarChar).Value = user.UserName;
            command.Parameters.Add("PasswordHash", SqlDbType.Binary, 64).Value = user.PasswordHash;
            command.Parameters.Add("AccessLevel", SqlDbType.Int).Value = user.AccessLevel;

            connect.Open();

            SqlDataReader reader = command.ExecuteReader();

            scope.Complete();
        }
        public static void AddUser(string userName, byte[] passwordHash, int accessLevel = 0) {
            AddUser(new User(userName, passwordHash, accessLevel));
        }

        public static void AddNode(Node node) {
            using var scope = new TransactionScope();
            using var connect = new SqlConnection(ConnectionString);
            using var command = new SqlCommand("[dbo].AddNode", connect);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.Add("NodeName", SqlDbType.NVarChar).Value = node.NodeName;
            command.Parameters.Add("IPAddress", SqlDbType.NVarChar).Value = node.IPAddress;
            command.Parameters.Add("LocationName", SqlDbType.NVarChar).Value = node.LocationName;

            connect.Open();

            SqlDataReader reader = command.ExecuteReader();

            scope.Complete();
        }
        public static void AddNode(string ipAddress, string nodeName = "", string LocationName = "") {
            AddNode(new Node(-1, ipAddress, nodeName, LocationName));
        }

        public static void AddNodeData(NodeData nodeData) {
            using var scope = new TransactionScope();
            using var connect = new SqlConnection(ConnectionString);
            using var command = new SqlCommand("[dbo].AddNodeData", connect);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.Add("NodeID", SqlDbType.Int).Value = nodeData.NodeID;
            command.Parameters.Add("Battery", SqlDbType.Float).Value = nodeData.Battery;
            command.Parameters.Add("Temperature", SqlDbType.Float).Value = nodeData.Temperature;
            command.Parameters.Add("Flow", SqlDbType.Float).Value = nodeData.Flow;
            command.Parameters.Add("Status", SqlDbType.NVarChar, 128).Value = nodeData.Status.ToString();

            connect.Open();

            SqlDataReader reader = command.ExecuteReader();

            scope.Complete();
        }
        public static void AddNodeData(int nodeID, float battery = 0f, float temperature = 0f, float flow = 0f, NodeStatus status = NodeStatus.IDLE) {
            AddNodeData(new NodeData(nodeID, battery, temperature, flow, status));
        }

        public static void DeleteUser(string userName) {
            using var scope = new TransactionScope();
            using var connect = new SqlConnection(ConnectionString);
            using var command = new SqlCommand("[dbo].DeleteUser", connect);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.Add("UserName", SqlDbType.NVarChar).Value = userName;

            connect.Open();

            command.ExecuteReader();

            scope.Complete();
        }

        public static void DeleteNode(int nodeID) {
            using var scope = new TransactionScope();
            using var connect = new SqlConnection(ConnectionString);
            using var command = new SqlCommand("[dbo].DeleteNode", connect);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.Add("NodeID", SqlDbType.NVarChar).Value = nodeID;

            connect.Open();

            command.ExecuteReader();

            scope.Complete();
        }

        public static void DeleteNodeData(int nodeID) {
            using var scope = new TransactionScope();
            using var connect = new SqlConnection(ConnectionString);
            using var command = new SqlCommand("[dbo].DeleteNodeData", connect);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.Add("NodeID", SqlDbType.NVarChar).Value = nodeID;

            connect.Open();

            command.ExecuteReader();

            scope.Complete();
        }

        public static User? SelectUser(string userName) {
            using var connect = new SqlConnection(ConnectionString);
            using var command = new SqlCommand("[dbo].SelectUser", connect);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.Add("UserName", SqlDbType.VarChar).Value = userName;

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
        public static List<User> SelectAllUsers() {
            var users = new List<User>();

            using var connect = new SqlConnection(ConnectionString);
            using var command = new SqlCommand("[dbo].SelectUser", connect);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.Add("UserName", SqlDbType.VarChar).Value = "";

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

        public static Node? SelectNode(int nodeID) {
            using var connect = new SqlConnection(ConnectionString);
            using var command = new SqlCommand("[dbo].SelectNodes", connect);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.Add("NodeID", SqlDbType.Int).Value = nodeID;

            connect.Open();

            SqlDataReader reader = command.ExecuteReader();

            if (reader.Read()) {
                return new Node(
                    (int)reader["NodeID"],
                    (string)reader["NodeName"],
                    (string)reader["IPAddress"],
                    (string)reader["LocationName"]);
            }

            return null;
        }
        public static List<Node> SelectAllNodes() {
            var nodes = new List<Node>();

            using var connect = new SqlConnection(ConnectionString);
            using var command = new SqlCommand("[dbo].SelectNode", connect);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.Add("NodeID", SqlDbType.Int).Value = -1;

            connect.Open();

            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read()) {
                nodes.Add(new Node(
                        (int)reader["NodeID"],
                        (string)reader["NodeName"],
                        (string)reader["IPAddress"],
                        (string)reader["LocationName"]));
            }

            return nodes;
        }

        public static List<NodeData> SelectNodeData(int? nodeID, DateTime? StartDate = null, DateTime? EndTime = null) {
            var NodeDatas = new List<NodeData>();

            using var connect = new SqlConnection(ConnectionString);
            using var command = new SqlCommand("[dbo].SelectNodeData", connect);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.Add("NodeID", SqlDbType.Int).Value = nodeID;
            command.Parameters.Add("StartDate", SqlDbType.DateTime).Value = StartDate;
            command.Parameters.Add("EndDate", SqlDbType.DateTime).Value = EndTime;

            connect.Open();

            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read()) {
                int id = (int)reader["NodeID"];
                float battery = (float)(double)reader["Battery"];
                float temperature = (float)(double)reader["Temperature"];
                float flow = (float)(double)reader["Flow"];
                DateTime timeStamp = (DateTime)reader["TimeStamp"];

                NodeStatus status = NodeStatus.DEFAULT;
                if (Enum.TryParse(typeof(NodeStatus), reader["Status"].ToString(), out var ns) && ns is not null) {
                    status = (NodeStatus)ns;
                }

                NodeDatas.Add(new NodeData(id, battery, temperature, flow, timeStamp, status));
            }

            return NodeDatas;
        }
        public static List<NodeData> SelectAllNodesData() {
            return SelectNodeData(null);
        }

        public static void UpdateUser(string userName, byte[]? passwordHash = null, int? accessLevel = null) {
            using var scope = new TransactionScope();
            using var connect = new SqlConnection(ConnectionString);
            using var command = new SqlCommand("[dbo].UpdateUser", connect);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.Add("UserName", SqlDbType.NVarChar, 15).Value = userName;
            command.Parameters.Add("PasswordHash", SqlDbType.Binary, 64).Value = passwordHash;
            command.Parameters.Add("AccessLevel", SqlDbType.Int).Value = accessLevel;

            connect.Open();

            command.ExecuteReader();

            scope.Complete();
        }

        public static void UpdateNode(int nodeID, string? nodeName = null, string? ipAddress = null, string? locationName = null) {
            using var scope = new TransactionScope();
            using var connect = new SqlConnection(ConnectionString);
            using var command = new SqlCommand("[dbo].UpdateNode", connect);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.Add("NodeID", SqlDbType.Int).Value = nodeID;
            command.Parameters.Add("NodeName", SqlDbType.NVarChar, 128).Value = nodeName;
            command.Parameters.Add("IPAddress", SqlDbType.NVarChar, 128).Value = ipAddress;
            command.Parameters.Add("kLocationName", SqlDbType.NVarChar, 128).Value = locationName;

            connect.Open();

            command.ExecuteReader();

            scope.Complete();
        }

        public static void UpdateNodeData(int nodeID, float? battery = null, float? temperature = null, float? flow = null, NodeStatus? nodeStatus = null) {
            using var scope = new TransactionScope();
            using var connect = new SqlConnection(ConnectionString);
            using var command = new SqlCommand("[dbo].UpdateNode", connect);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.Add("NodeID", SqlDbType.Int).Value = nodeID;
            command.Parameters.Add("Battery", SqlDbType.Float).Value = battery;
            command.Parameters.Add("Temperature", SqlDbType.Float).Value = temperature;
            command.Parameters.Add("Flow", SqlDbType.Float).Value = flow;
            command.Parameters.Add("Status", SqlDbType.NVarChar, 128).Value = nodeStatus;

            connect.Open();

            command.ExecuteReader();

            scope.Complete();
        }
    }
}

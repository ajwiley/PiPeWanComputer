using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PiPeWanComputer.SQL_Stuff {
    public class SQLStart : BaseConnection {
        public string FailMessage { get; private set; }

        /// <summary>
        /// This will create the Database, Tables, Procedures, and Insert the Data
        /// </summary>
        public SQLStart() {
            // Initial setter
            FailMessage = "";

            // Use master for initially creating the Database
            string Master = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=master;";
            // The current directory of where the files are stored might have to change per user
            string currentDirectory = @"C:\Users\mrd\source\repos\PipeWanComputer";

            // Create the Location Folder If it doesn't exist
            System.IO.Directory.CreateDirectory("C:\\Smart Cart");

            // Create the DB, Drop the Tables, and Constraints
            RunSqlScriptFile(currentDirectory + "\\SQL Setup\\", "Setup.sql", Master);
            RunSqlScriptFile(currentDirectory + "\\SQL Setup\\", "Drop.sql", Master);

            // Create the Tables
            RunSqlScriptFile(currentDirectory + "\\SQL Tables\\", "User.sql", Connection);
            RunSqlScriptFile(currentDirectory + "\\SQL Tables\\", "Node.sql", Connection);
            RunSqlScriptFile(currentDirectory + "\\SQL Tables\\", "NodeData.sql", Connection);

            // Insert the Default Data into the Tables
            RunSqlScriptFile(currentDirectory + "\\SQL Setup\\", "Data.sql", Connection);

            // Create the Procedures
            // DELETE
            RunSqlScriptFile(currentDirectory + "\\SQL Procedures\\DELETE\\", "DeleteUser.sql", Connection);
            RunSqlScriptFile(currentDirectory + "\\SQL Procedures\\DELETE\\", "DeleteNode.sql", Connection);
            RunSqlScriptFile(currentDirectory + "\\SQL Procedures\\DELETE\\", "DeleteNodeData.sql", Connection);

            // INSERT
            RunSqlScriptFile(currentDirectory + "\\SQL Procedures\\INSERT\\", "AddUser.sql", Connection);
            RunSqlScriptFile(currentDirectory + "\\SQL Procedures\\INSERT\\", "AddNode.sql", Connection);
            RunSqlScriptFile(currentDirectory + "\\SQL Procedures\\INSERT\\", "AddNodeData.sql", Connection);

            // UPDATE
            RunSqlScriptFile(currentDirectory + "\\SQL Procedures\\UPDATE\\", "UpdateUser.sql", Connection);
            RunSqlScriptFile(currentDirectory + "\\SQL Procedures\\UPDATE\\", "UpdateNode.sql", Connection);
            RunSqlScriptFile(currentDirectory + "\\SQL Procedures\\UPDATE\\", "UpdateNodeData.sql", Connection);
            
            // SELECT
            RunSqlScriptFile(currentDirectory + "\\SQL Procedures\\SELECT\\", "SelectUsers.sql", Connection);
            RunSqlScriptFile(currentDirectory + "\\SQL Procedures\\SELECT\\", "SelectNodes.sql", Connection);
            RunSqlScriptFile(currentDirectory + "\\SQL Procedures\\SELECT\\", "SelectNodeData.sql", Connection);
        }

        /// <summary>
        /// This will run the individual sql scripts to auto create the DB, Tables, Prodecured, etc.
        /// </summary>
        /// <param name="path">The path to the sql file, not including the sql file</param>
        /// <param name="sqlFile">The Sql file to execute, should end in .sql</param>
        /// <param name="connectionString">the connection string to the database or master</param>
        /// <returns>Success or fail</returns>
        private bool RunSqlScriptFile(string path, string sqlFile, string connectionString) {
            try {
                string script = File.ReadAllText(path + sqlFile);

                // Split script on GO command
                IEnumerable<string> commandStrings = Regex.Split(script, @"^\s*GO\s*$", RegexOptions.Multiline | RegexOptions.IgnoreCase);
                using (SqlConnection connect = new SqlConnection(connectionString)) {
                    connect.Open();
                    foreach (string commandString in commandStrings) {
                        if (commandString.Trim() != "") {
                            using (var command = new SqlCommand(commandString, connect)) {
                                try {
                                    command.ExecuteNonQuery();
                                    Console.WriteLine(string.Format("Successfully executed {0}", sqlFile));
                                }
                                catch (SqlException ex) {
                                    Console.WriteLine(string.Format("{0}", ex.Message));
                                    FailMessage = ex.Message;
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
                Console.WriteLine(string.Format("{0}", ex.Message));
                FailMessage = ex.Message;
                return false;
            }
        }
    }
}

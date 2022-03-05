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
        public string NodeName;
        public IPAddress IPAddress;
        public string LocationName;

        public Node(string nodeName, IPAddress ipAddress) : this(nodeName, ipAddress, "") { }

        public Node(string nodeName, IPAddress ipAddress, string locationName) {
            NodeName = nodeName;
            IPAddress = ipAddress;
            LocationName = locationName;
        }

        /// <summary>
        /// This will add a new Cart, the CartID is automatic
        /// </summary>
        /// <param name="barcode">The barcode must be unique in the Database</param>
        /// <param name="name">The name must be unique in the Database</param>
        /// <returns>It will return the CartID of the newly created Cart if succesful</returns>
        public int AddCart(int barcode, int name) {
            using (TransactionScope scope = new TransactionScope()) {
                using (SqlConnection connect = new SqlConnection(Connection)) {
                    using (SqlCommand command = new SqlCommand("[dbo].AddCart", connect)) {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.Add("Barcode", SqlDbType.Int).Value = barcode;
                        command.Parameters.Add("CartName", SqlDbType.NVarChar, (128)).Value = name;

                        connect.Open();

                        SqlDataReader reader = command.ExecuteReader();

                        scope.Complete();

                        if (reader.Read()) {
                            return reader.GetInt32(reader.GetOrdinal("CartID"));
                        }
                        else
                            return 0;
                    }
                }
            }
        }

        /// <summary>
        /// This will delete the specific cart
        /// </summary>
        /// <param name="cartID"></param>
        public void DeleteCart(int cartID) {
            using (TransactionScope scope = new TransactionScope()) {
                using (SqlConnection connect = new SqlConnection(Connection)) {
                    using (SqlCommand command = new SqlCommand("[dbo].DeleteCart", connect)) {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.Add("CartID", SqlDbType.Int).Value = cartID;

                        connect.Open();

                        command.ExecuteReader();

                        scope.Complete();
                    }
                }
            }
        }

        /// <summary>
        /// This will update the specific Cart
        /// </summary>
        /// <param name="cartID"></param>
        /// <param name="barcode">The barcode must be unique in the Database</param>
        /// <param name="name">The name must be unique in the Database</param>
        public void UpdateCart(int cartID, int barcode, string name) {
            using (TransactionScope scope = new TransactionScope()) {
                using (SqlConnection connect = new SqlConnection(Connection)) {
                    using (SqlCommand command = new SqlCommand("[dbo].UpdateCart", connect)) {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.Add("CartID", SqlDbType.Int).Value = cartID;
                        command.Parameters.Add("Barcode", SqlDbType.Int).Value = barcode;
                        command.Parameters.Add("CartName", SqlDbType.NVarChar, (128)).Value = name;

                        connect.Open();

                        command.ExecuteReader();

                        scope.Complete();
                    }
                }
            }
        }

        ///// <summary>
        ///// This will return a list of the carts
        ///// </summary>
        ///// <param name="cartID">If you want to just return a specific cart, by default 0 to return all</param>
        ///// <returns></returns>
        //public List<Cart> ListOfCarts(int cartID = 0) {
        //    using (SqlConnection connect = new SqlConnection(Connection)) {
        //        using (SqlCommand command = new SqlCommand("[dbo].SelectCarts", connect)) {
        //            command.CommandType = CommandType.StoredProcedure;

        //            command.Parameters.Add("CartID", SqlDbType.Int).Value = cartID;

        //            connect.Open();

        //            SqlDataReader reader = command.ExecuteReader();

        //            List<Cart> carts = new List<Cart>();

        //            while (reader.Read()) {
        //                carts.Add(new Cart(
        //                    (int)reader["CartID"],
        //                    (int)reader["Barcode"],
        //                    (string)reader["CartName"]));
        //            }

        //            return carts;
        //        }
        //    }
        //}
    }
}

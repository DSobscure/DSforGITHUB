using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Security.Cryptography;
using System.Data.SqlClient;

namespace DSServer
{
    public class DSDatabase : IDisposable
    {
        private readonly string hostname;
        private readonly string username;
        private readonly string password;
        private readonly string database;
        private MySqlConnection connection;

        public DSDatabase(string _hostname, string _username, string _password, string _database)
        {
            hostname = _hostname;
            username = _username;
            password = _password;
            database = _database;
        }

        public void Dispose()
        {
            if(connection.State != System.Data.ConnectionState.Closed)
                connection.Close();
        }

        public bool Connect()
        {
            string connectString = "server=" + hostname + ";uid=" + username + ";pwd=" + password + ";database=" + database;
            connection = new MySqlConnection(connectString);
            connection.Open();

            return connection is MySqlConnection;
        }

        public bool LoginCheck(string _account, string _password, out string UniqueID)
        {
            try
            {
                if (connection.State == System.Data.ConnectionState.Closed)
                    connection.Open();
                using(SHA512 sha512 = new SHA512CryptoServiceProvider())
                {
                    string _passwordSHA512 = ToHexString(sha512.ComputeHash(Encoding.Default.GetBytes(_password)));


                    String sqlText = "SELECT * FROM user WHERE Account=@account and Password=@password";
                    using(MySqlCommand cmd = new MySqlCommand(sqlText, connection))
                    {
                        cmd.Parameters.AddWithValue("@account", _account);
                        cmd.Parameters.AddWithValue("@password", _passwordSHA512);
                        using(MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                UniqueID = reader.GetGuid(0).ToString();
                                return true;
                            }
                            else
                            {
                                UniqueID = "";
                                return false;
                            }
                        }
                    }
                }
            }
            catch (Exception EX)
            {
                throw EX;
            }
            finally
            {
                if (connection.State == System.Data.ConnectionState.Open)
                    connection.Close();
            }
        }

        public string[] GetDataByUniqueID(string uniqueID,string[] requestItem,string table)
        {
            try
            {
                if (connection.State == System.Data.ConnectionState.Closed)
                    connection.Open();
                StringBuilder sqlText = new StringBuilder();
                sqlText.Append("SELECT " + requestItem[0]);
                int requestNumber = requestItem.Length;
                for (int index1 = 1; index1 < requestNumber; index1++)
                {
                    sqlText.Append("," + requestItem[index1]);
                }
                sqlText.Append(" FROM " + table + " WHERE UniqueID=@uniqueID");
                using (MySqlCommand cmd = new MySqlCommand(sqlText.ToString(), connection))
                {
                    cmd.Parameters.AddWithValue("@uniqueID", uniqueID);
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string[] returnValue = new string[requestNumber];
                            for (int index1 = 0; index1 < requestNumber; index1++)
                            {
                                if (reader.IsDBNull(index1))
                                    returnValue[index1] = "";
                                else
                                    returnValue[index1] = reader.GetString(index1);
                            }
                            return returnValue;
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (connection.State == System.Data.ConnectionState.Open)
                    connection.Close();
            }
        }

        public object[] GetDataByUniqueID(string uniqueID,string[] requestItem,TypeCode[] requestType,string table)
        {
            try
            {
                if (connection.State == System.Data.ConnectionState.Closed)
                    connection.Open();
                StringBuilder sqlText = new StringBuilder();
                sqlText.Append("SELECT " + requestItem[0]);
                int requestNumber = requestItem.Length;
                for (int index1 = 1; index1 < requestNumber; index1++)
                {
                    sqlText.Append("," + requestItem[index1]);
                }
                sqlText.Append(" FROM " + table + " WHERE UniqueID=@uniqueID");
                using (MySqlCommand cmd = new MySqlCommand(sqlText.ToString(), connection))
                {
                    cmd.Parameters.AddWithValue("@uniqueID", uniqueID);
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            object[] returnValue = new object[requestNumber];
                            for (int index1 = 0; index1 < requestNumber; index1++)
                            {
                                if (reader.IsDBNull(index1))
                                    returnValue[index1] = null;
                                else
                                {
                                    switch (requestType[index1])
                                    {
                                        case TypeCode.Boolean:
                                            returnValue[index1] = reader.GetBoolean(index1);
                                            break;
                                        case TypeCode.String:
                                            returnValue[index1] = reader.GetString(index1);
                                            break;
                                        case TypeCode.Int32:
                                            returnValue[index1] = reader.GetInt32(index1);
                                            break;
                                        case TypeCode.Single:
                                            returnValue[index1] = reader.GetFloat(index1);
                                            break;
                                    }
                                }
                            }
                            return returnValue;
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (connection.State == System.Data.ConnectionState.Open)
                    connection.Close();
            }
        }
        
        public bool InsertData(string[] insertItem,object[] insertValue,string table)
        {
            try
            {
                if (connection.State == System.Data.ConnectionState.Closed)
                    connection.Open();
                StringBuilder sqlText = new StringBuilder();
                sqlText.Append("INSERT INTO " + table + " (" + insertItem[0]);
                int insertNumber = insertItem.Length;
                for (int index1 = 1; index1 < insertNumber; index1++)
                {
                    sqlText.Append("," + insertItem[index1]);
                }
                sqlText.Append(") values (@insertValue0");
                for (int index1 = 1; index1 < insertNumber; index1++)
                {
                    sqlText.Append(",@insertValue" + index1.ToString());
                }
                sqlText.Append(")");
                using (MySqlCommand cmd = new MySqlCommand(sqlText.ToString(), connection))
                {
                    for (int index1 = 0; index1 < insertNumber; index1++)
                    {
                        cmd.Parameters.AddWithValue("@insertValue" + index1.ToString(), insertValue[index1]);
                    }
                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        return true;
                    }
                }
                return false;
            }
            catch(Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (connection.State == System.Data.ConnectionState.Open)
                    connection.Close();
            }
        }

        public bool UpdateDataByUniqueID(string uniqueID,string[] updateItem,object[] updateValue,string table)
        {
            try
            {
                if (connection.State == System.Data.ConnectionState.Closed)
                    connection.Open();
                StringBuilder sqlText = new StringBuilder();
                sqlText.Append("UPDATE " + table + " SET " + updateItem[0] + "=@updateValue0");
                int updateNumber = updateItem.Length;
                for (int index1 = 1; index1 < updateNumber; index1++)
                {
                    sqlText.Append("," + updateItem[index1] + "=@updateValue" + index1.ToString());
                }
                sqlText.Append(" where UniqueID=@uniqueID");
                using (MySqlCommand cmd = new MySqlCommand(sqlText.ToString(), connection))
                {
                    for (int index1 = 0; index1 < updateNumber; index1++)
                    {
                        cmd.Parameters.AddWithValue("@updateValue" + index1.ToString(), updateValue[index1]);
                    }
                    cmd.Parameters.AddWithValue("@uniqueID",uniqueID);
                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (connection.State == System.Data.ConnectionState.Open)
                    connection.Close();
            }
        }

        public string ToHexString(byte[] bytes)
        {
            string hexString = string.Empty;
            if (bytes != null)
            {
                StringBuilder strB = new StringBuilder();

                for (int i = 0; i < bytes.Length; i++)
                {
                    strB.Append(bytes[i].ToString("X2").ToLower());
                }
                hexString = strB.ToString();
            }
            return hexString;
        }
    }
}

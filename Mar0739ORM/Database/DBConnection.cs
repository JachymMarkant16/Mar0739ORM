using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mar0739ORM.Database
{
    class DBConnection
    {
        public SqlConnection connection { get; set; }

        public static string connectionstring { get; set; }
        private SqlTransaction SqlTransaction { get; set; }




        public DBConnection()
        {
            connection = new SqlConnection();

        }

        public bool ConnectStr()
        {

            if (connection.State != System.Data.ConnectionState.Open)
            {
                connection.ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionStringMsSql"].ConnectionString;
                connection.Open();
            }

            return true;
        }

        public bool Connect()
        {

            bool ret = true;

            if (connection.State != System.Data.ConnectionState.Open)
            {
                // connection string is stored in file App.config or Web.config
                ret = ConnectStr();
            }
            return ret;
        }

        public void Close()
        {
            connection.Close();
        }

        public void BeginTransaction()
        {
            SqlTransaction = connection.BeginTransaction(IsolationLevel.Serializable);
        }

        public void EndTransaction()
        {
            SqlTransaction.Commit();
            Close();
        }
        public void Rollback()
        {
            SqlTransaction.Rollback();
        }

        public int ExecuteNonQuery(SqlCommand command)
        {
            int rowNumber = 0;
            try
            {
                rowNumber = command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                throw e;
            }
            return rowNumber;
        }

        public SqlCommand CreateCommand(string strCommand)
        {
            SqlCommand command = new SqlCommand(strCommand, connection);

            if (SqlTransaction != null)
            {
                command.Transaction = SqlTransaction;
            }
            return command;
        }

        public SqlDataReader Select(SqlCommand command)
        {
            SqlDataReader sqlReader = command.ExecuteReader();
            return sqlReader;
        }
    }
}

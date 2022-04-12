using Mar0739ORM.Database;
using Mar0739ORM.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ProjektDB.ORM
{
    public class StockRepository
    {
        public static List<Stock> FindAllStock()
        {
            List<Stock> result = new List<Stock>();
            DBConnection db = new DBConnection();

            db.Connect();
            SqlCommand cmd = new SqlCommand("SELECT * FROM [dbo].[stock]", db.connection); //
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                int i = -1;
                Stock tmpStock = new Stock();
                tmpStock.Id = reader.GetInt32(++i);
                tmpStock.Name = reader.GetString(++i);
                tmpStock.Description = reader.GetString(++i);
                tmpStock.Count = reader.GetInt32(++i);
                tmpStock.Price = reader.GetInt32(++i);

                result.Add(tmpStock);
            }

            db.Close();
            return result;
        }
        public static int CreateStock(Stock stock)
        {

            DBConnection db = new DBConnection();

            db.Connect();

            SqlCommand cmdGame = new SqlCommand("INSERT INTO [dbo].[stock] VALUES ( " +
                "@name, @description," +
                " @count, @price)",
                db.connection);

            cmdGame.Parameters.AddWithValue("@name", stock.Name);
            cmdGame.Parameters.AddWithValue("@description", stock.Description);
            cmdGame.Parameters.AddWithValue("@count", stock.Count);
            cmdGame.Parameters.AddWithValue("@price", stock.Price);

            int newGame = (Int32)cmdGame.ExecuteScalar();

            db.Close();
            return newGame;
        }
        public static void UpdateStock(List<Stock> stocks)
        {

            DBConnection db = new DBConnection();

            db.Connect();
            foreach(var stock in stocks)
            {
                SqlCommand cmdGame = new SqlCommand("UPDATE [dbo].[stock]  " +
                "count =  @count where id = @id",
                db.connection);

                cmdGame.Parameters.AddWithValue("@count", stock.Count);
                cmdGame.Parameters.AddWithValue("@id", stock.Id);
                cmdGame.ExecuteNonQuery();
            }
            
            db.Close();
        }
        public static void DeleteStockById(int id)
        {
            DBConnection db = new DBConnection();

            db.Connect();
            SqlCommand cmdUser = new SqlCommand("DELETE FROM stock WHERE id = @id");
            cmdUser.Parameters.AddWithValue("@id", id);
            cmdUser.ExecuteNonQuery();

            db.Close();
        }

        public static List<int> GetEmailToSendForLowStock(int recAmount, string adrese)
        {
            List<int> emails = new List<int>();


            DBConnection db = new DBConnection();
            db.Connect();
            SqlCommand countCmd = new SqlCommand("Select count(id) from stock where stock.count < "+
"@rec_amount", db.connection);
            countCmd.Parameters.AddWithValue("@rec_amount", recAmount);
            int resCount = 0;
            SqlDataReader reader = countCmd.ExecuteReader();
            while (reader.Read())
            {
                resCount = reader.GetInt32(0);
            }
            reader.Close();
            if (resCount == 0)
            {
                return emails;
            }
            else
            {
                SqlTransaction transaction;

                SqlCommand cmd = new SqlCommand();

                transaction = db.connection.BeginTransaction();

                cmd.Connection = db.connection;
                cmd.Transaction = transaction;

                try
                {
                    cmd.CommandText = "execute CreateEmailsForLowStock @recAmount = @rec_amount, @adrese = @adresee";
                    cmd.Parameters.AddWithValue("@rec_amount", recAmount);
                    cmd.Parameters.AddWithValue("@adresee", adrese);
                    cmd.ExecuteNonQuery();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Commit Exception Type: {0}", ex.GetType());
                    Console.WriteLine("  Message: {0}", ex.Message);

                    // Attempt to roll back the transaction.
                    try
                    {
                        transaction.Rollback();
                    }
                    catch (Exception ex2)
                    {
                        // This catch block will handle any errors that may have occurred
                        // on the server that would cause the rollback to fail, such as
                        // a closed connection.
                        Console.WriteLine("Rollback Exception Type: {0}", ex2.GetType());
                        Console.WriteLine("  Message: {0}", ex2.Message);
                    }
                }
            }
            SqlCommand phonesCmd = new SqlCommand("Select id from email " +
"where date = '1.1.1970'" , db.connection);
            SqlDataReader phonesReader = phonesCmd.ExecuteReader();
            while (phonesReader.Read())
            {
                int i = -1;
                emails.Add(phonesReader.GetInt32(++i));
            }
            db.Close();


            return emails;
        }
    }
}

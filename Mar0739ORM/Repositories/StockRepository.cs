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
            SqlCommand cmd = new SqlCommand("SELECT * FROM [dbo].[user]", db.connection); //
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
    }
}

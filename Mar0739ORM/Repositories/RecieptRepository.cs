using Mar0739ORM.Database;
using Mar0739ORM.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ProjektDB.ORM
{
    public class RecieptRepository
    {
        public static List<Reciept> GetRecieptsForUser(User user)
        {
            List<Reciept> result = new List<Reciept>();
            DBConnection db = new DBConnection();

            db.Connect();
            SqlCommand cmd = new SqlCommand("SELECT * FROM [dbo].[reciept] where user_id = @user_id", db.connection); //
            cmd.Parameters.AddWithValue("@user_id", user.Id);
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                int i = -1;
                Reciept tmpReciept = new Reciept();
                tmpReciept.Id = reader.GetInt32(++i);
                tmpReciept.Date = reader.GetDateTime(++i);
                tmpReciept.Description = reader.GetString(++i);
                tmpReciept.Price = reader.GetInt32(++i);
                tmpReciept.State = reader.GetString(++i);
                tmpReciept.User = UserRepository.FindUserById(reader.GetInt32(++i));
                tmpReciept.Stock = StockRepository.FindAllStock().Select(x => x).Where(x => x.Id == reader.GetInt32(++i)).FirstOrDefault();

                result.Add(tmpReciept);
            }

            db.Close();
            return result;
        }

        public static List<Reciept> GetRecieptsByFilter(string firstName, string lastName, string email)
        {
            List<Reciept> result = new List<Reciept>();
            DBConnection db = new DBConnection();

            db.Connect();
            SqlCommand cmd = new SqlCommand("Select u.first_name, u.last_name, u.email,"+
                        "(select count(id) from reciept"+
                        "where state = „K zaplacení“ and user_id = u.id) as RecCount"+
                        "from user u"+
                        "where RecCount > 0 and"+
                        "(case"+
                        "when @firstName not null"+
                        "then u.first_name like @firstName"+
                        "when @lastName not null "+
                        "then u.last_name like @lastName"+
                        "when @email not null"+
                        "then u.email like @email)"+
                        "order by RecCount" 
                        , db.connection); //
            cmd.Parameters.AddWithValue("@firstName", firstName);
            cmd.Parameters.AddWithValue("@lastName", lastName);
            cmd.Parameters.AddWithValue("@email", email);
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                int i = -1;
                Reciept tmpReciept = new Reciept();
                tmpReciept.Id = reader.GetInt32(++i);
                tmpReciept.Date = reader.GetDateTime(++i);
                tmpReciept.Description = reader.GetString(++i);
                tmpReciept.Price = reader.GetInt32(++i);
                tmpReciept.State = reader.GetString(++i);
                tmpReciept.User = UserRepository.FindUserById(reader.GetInt32(++i));
                tmpReciept.Stock = StockRepository.FindAllStock().Select(x => x).Where(x => x.Id == reader.GetInt32(++i)).FirstOrDefault();

                result.Add(tmpReciept);
            }

            db.Close();
            return result;
        }
        public static void CreateReciept(Reciept reciept)
        {
            DBConnection db = new DBConnection();

            db.Connect();

            SqlCommand cmdUser = new SqlCommand("INSERT INTO [dbo].[reciept] VALUES ( " +
                "@date, @description," +
                " @price, @state, @user_id, @stock_id)",
                db.connection);

            cmdUser.Parameters.AddWithValue("@date", reciept.Date);
            cmdUser.Parameters.AddWithValue("@description", reciept.Description);
            cmdUser.Parameters.AddWithValue("@price", reciept.Price);
            cmdUser.Parameters.AddWithValue("@state", reciept.State);
            cmdUser.Parameters.AddWithValue("@user_id", reciept.User.Id);
            cmdUser.Parameters.AddWithValue("@stock_id", reciept.Stock.Id);

            cmdUser.ExecuteNonQuery();

            db.Close();
        }
        public static void PayReciept(int id)
        {
            DBConnection db = new DBConnection();

            db.Connect();

            SqlCommand cmdUser = new SqlCommand("UPDATE [dbo].[reciept] set state = \'Zaplaceno\' where id = @id",
                db.connection);

            cmdUser.Parameters.AddWithValue("@id", id);

            cmdUser.ExecuteNonQuery();

            db.Close();
        }
    }
}

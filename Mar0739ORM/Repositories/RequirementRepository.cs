using Mar0739ORM.Database;
using Mar0739ORM.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ProjektDB.ORM
{
    public class RequirementRepository
    {
        public static List<Requirement> FindAllRequirements()
        {
            List<Requirement> result = new List<Requirement>();
            DBConnection db = new DBConnection();

            db.Connect();
            SqlCommand cmd = new SqlCommand("SELECT * FROM [dbo].[requirement]", db.connection); //
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                int i = -1;
                Requirement tmpReq = new Requirement();
                tmpReq.Id = reader.GetInt32(++i);
                tmpReq.Description = reader.GetString(++i);
                tmpReq.State = reader.GetString(++i);
                tmpReq.User = UserRepository.FindUserById(reader.GetInt32(++i));
                tmpReq.Game = GameRepository.FindGame(reader.GetInt32(++i));
                tmpReq.Reservation = ReservationRepository.FindAllReservations().Select(x=>x).Where(x => x.Id ==reader.GetInt32(++i)).FirstOrDefault();
                tmpReq.Stock = StockRepository.FindAllStock().Select(x => x).Where(x => x.Id == reader.GetInt32(++i)).FirstOrDefault();

                result.Add(tmpReq);
            }

            db.Close();
            return result;
        }
        public static int CreateRequirement(Requirement requirement)
        {
            DBConnection db = new DBConnection();

            db.Connect();

            SqlCommand cmdGame = new SqlCommand("INSERT INTO [dbo].[game] VALUES ( " +
                "@description, @state," +
                " @user_id, @game_id, @reservation_id, @stock_id)",
                db.connection);

            cmdGame.Parameters.AddWithValue("@description", requirement.Description);
            cmdGame.Parameters.AddWithValue("@state", requirement.State);
            cmdGame.Parameters.AddWithValue("@user_id", requirement.User.Id);
            cmdGame.Parameters.AddWithValue("@game_id", requirement.Game.Id);
            cmdGame.Parameters.AddWithValue("@reservation_id", requirement.Reservation.Id);
            cmdGame.Parameters.AddWithValue("@stock_id", requirement.Stock.Id);

            int newGame = (Int32)cmdGame.ExecuteScalar();

            db.Close();
            return newGame;
        }
        public static void CompleteReq(int id)
        {
            DBConnection db = new DBConnection();

            db.Connect();
            SqlCommand cmdUser = new SqlCommand("UPDATE [dbo].[requirement] " +
                "state= \"Zpracováno\" where id = @id",
                db.connection);

            cmdUser.Parameters.AddWithValue("@id", id);

            cmdUser.ExecuteNonQuery();

            db.Close();
        }
    }
}

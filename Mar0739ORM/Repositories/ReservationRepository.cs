using Mar0739ORM.Database;
using Mar0739ORM.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ProjektDB.ORM
{
    public class ReservationRepository
    {
        public static void CreateReservation(Reservation reservation)
        {
            DBConnection db = new DBConnection();

            db.Connect();

            SqlCommand cmdGame = new SqlCommand("INSERT INTO [dbo].[reservation] VALUES ( " +
                "@date, @Length," +
                " @add_info, @state, @game_id, @user_id)",
                db.connection);

            cmdGame.Parameters.AddWithValue("@date", reservation.Date);
            cmdGame.Parameters.AddWithValue("@Length", reservation.Length);
            cmdGame.Parameters.AddWithValue("@add_info", reservation.AddInfo);
            cmdGame.Parameters.AddWithValue("@state", reservation.State);
            cmdGame.Parameters.AddWithValue("@game_id", reservation.Game.Id);
            cmdGame.Parameters.AddWithValue("@user_id", reservation.User.Id);

            cmdGame.ExecuteNonQuery();

            db.Close();
        }
        public static void UpdateReservation(Reservation reservation)
        {
            DBConnection db = new DBConnection();

            db.Connect();
                SqlCommand cmdGame = new SqlCommand("UPDATE [dbo].[reservation] SET " +
                    "date = @date, length = @Length," +
                    "add_info = @add_info,state = @state,game_id = @game_id,user_id =  @user_id where id = @id",
                    db.connection);

            cmdGame.Parameters.AddWithValue("@date", reservation.Date);
            cmdGame.Parameters.AddWithValue("@Length", reservation.Length);
            cmdGame.Parameters.AddWithValue("@add_info", reservation.AddInfo);
            cmdGame.Parameters.AddWithValue("@state", reservation.State);
            cmdGame.Parameters.AddWithValue("@game_id", reservation.Game.Id);
            cmdGame.Parameters.AddWithValue("@user_id", reservation.User.Id);
            cmdGame.Parameters.AddWithValue("@id", reservation.Id);

            cmdGame.ExecuteNonQuery();

            db.Close();
        }
        public static int CreateRecieptForReservation(Reservation reservation)
        {
            DBConnection db = new DBConnection();

            db.Connect();
            Game game = GameRepository.FindGame(reservation.Game.Id);
            SqlCommand cmdGame = new SqlCommand("INSERT INTO [dbo].[reciept] VALUES ( " +
                "@date, @description," +
                " @price, @state, @user_id)",
                db.connection);

            cmdGame.Parameters.AddWithValue("@date", DateTime.Now);
            cmdGame.Parameters.AddWithValue("@description", "Účet za rezervaci");
            cmdGame.Parameters.AddWithValue("@price", game.Price);
            cmdGame.Parameters.AddWithValue("@state", "K zaplacení");
            cmdGame.Parameters.AddWithValue("@user_id", reservation.User.Id);

            int newGame = (Int32)cmdGame.ExecuteScalar();

            db.Close();
            return newGame;
        }

        public static List<Reservation> FindAllReservations()
        {
            List<Reservation> result = new List<Reservation>();
            DBConnection db = new DBConnection();

            db.Connect();
            SqlCommand cmd = new SqlCommand("SELECT * FROM [dbo].[reservation]", db.connection); //
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                int i = -1;
                Reservation tmpGame = new Reservation();
                tmpGame.Id = reader.GetInt32(++i);
                tmpGame.Date = reader.GetDateTime(++i);
                tmpGame.Length = reader.GetInt32(++i);
                tmpGame.AddInfo = reader.GetString(++i);
                tmpGame.State = reader.GetString(++i);
                tmpGame.Game = GameRepository.FindGame(reader.GetInt32(++i));
                tmpGame.User = UserRepository.FindUserById(reader.GetInt32(++i));

                result.Add(tmpGame);
            }

            db.Close();
            return result;
        }
        public static List<Reservation> FindAllReservationsByUserId(int id)
        {
            List<Reservation> result = new List<Reservation>();
            DBConnection db = new DBConnection();

            db.Connect();
            SqlCommand cmd = new SqlCommand("SELECT * FROM [dbo].[reservation] where id = @id", db.connection); //
            cmd.Parameters.AddWithValue("@id", id);
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                int i = -1;
                Reservation tmpGame = new Reservation();
                tmpGame.Id = reader.GetInt32(++i);
                tmpGame.Date = reader.GetDateTime(++i);
                tmpGame.Length = reader.GetInt32(++i);
                tmpGame.AddInfo = reader.GetString(++i);
                tmpGame.Game = GameRepository.FindGame(reader.GetInt32(++i));
                tmpGame.User = UserRepository.FindUserById(reader.GetInt32(++i));

                result.Add(tmpGame);
            }

            db.Close();
            return result;
        }
        public static void CancelReservation(Reservation reservation)
        {
            List<Reservation> result = new List<Reservation>();
            DBConnection db = new DBConnection();

            db.Connect();
            SqlCommand cmd = new SqlCommand("UPDATE reservation set state = \'Zrušeno\' where id = @id", db.connection); //
            cmd.Parameters.AddWithValue("@id", reservation.Id);
            cmd.ExecuteNonQuery();

            db.Close();
        }
        public static void AcceptReservation(Reservation reservation)
        {
            List<Reservation> result = new List<Reservation>();
            DBConnection db = new DBConnection();

            db.Connect();
            SqlCommand cmd = new SqlCommand("UPDATE reservation set state = \'Potvrzeno\' where id = @id", db.connection); //
            cmd.Parameters.AddWithValue("@id", reservation.Id);
            cmd.ExecuteNonQuery();
            db.Close();
        }
        public static void SetNotPaidReservation(Reservation reservation)
        {
            List<Reservation> result = new List<Reservation>();
            DBConnection db = new DBConnection();

            db.Connect();
            SqlCommand cmd = new SqlCommand("UPDATE reservation set state = \'Propadlá\' where id = @id", db.connection); //
            cmd.Parameters.AddWithValue("@id", reservation.Id);
            cmd.ExecuteNonQuery();
            db.Close();
        }
        public static List<int> CheckNotPaidReservations()
        {
            List<int> phones = new List<int>();
            DBConnection db = new DBConnection();
            db.Connect();
            SqlCommand countCmd = new SqlCommand("Select count(id) from reservation " +
"where datum < Date(CURRENT_TIMESTAMP)" +
"and state != \'Hotová\' and state != \'Propadlá\'", db.connection);
            int resCount;
            SqlDataReader reader = countCmd.ExecuteReader();
            while (reader.Read())
            {
                resCount  = reader.GetInt32(0);
            }

                db.Close();
            return phones;
        }

    }
}

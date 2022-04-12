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

            SqlTransaction transaction;
            db.Connect();
            transaction = db.connection.BeginTransaction();

            SqlCommand checkCmd = new SqlCommand("Select count(*) " +
"from reservation " +
"where user_id = @user_id  " +
"and @date >= date and @date <= dateadd(minute, reservation.length, reservation.date)", db.connection);
            checkCmd.Transaction = transaction;
            checkCmd.Parameters.AddWithValue("@date", reservation.Date);
            checkCmd.Parameters.AddWithValue("@user_id", reservation.User.Id);
            SqlDataReader checkReader = checkCmd.ExecuteReader();
            int checkCount = 0;
            while (checkReader.Read())
            {
                int i = -1;
                checkCount = checkReader.GetInt32(++i);
            }
            checkReader.Close();
            if (checkCount == 0)
            {
                SqlCommand cmdGame = new SqlCommand("INSERT INTO [dbo].[reservation] VALUES ( " +
                    "@date, @Length," +
                    " @add_info, @state, @game_id, @user_id)",
                    db.connection);
                cmdGame.Transaction = transaction;
                cmdGame.Parameters.AddWithValue("@date", reservation.Date);
                cmdGame.Parameters.AddWithValue("@Length", reservation.Length);
                cmdGame.Parameters.AddWithValue("@add_info", reservation.AddInfo);
                cmdGame.Parameters.AddWithValue("@state", reservation.State);
                cmdGame.Parameters.AddWithValue("@game_id", reservation.Game.Id);
                cmdGame.Parameters.AddWithValue("@user_id", reservation.User.Id);

                cmdGame.ExecuteNonQuery();
                transaction.Commit();
            }
            else
            {
                transaction.Rollback();
                db.Close();
                throw new Exception("Uz existuje rezervace");
            }
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
        public static List<string> CheckNotPaidReservations()
        {
            List<string> phones = new List<string>();
            DBConnection db = new DBConnection();
            db.Connect();
            SqlCommand countCmd = new SqlCommand("Select count(id) from reservation " +
"where [dbo].Reservation.date < CURRENT_TIMESTAMP " +
"and [dbo].Reservation.state != 'Hotová' and [dbo].Reservation.state != 'Propadlá'", db.connection);
            int resCount = 0;
            SqlDataReader reader = countCmd.ExecuteReader();
            while (reader.Read())
            {
                resCount = reader.GetInt32(0);
            }
            reader.Close();
            if (resCount == 0)
            {
                return phones;
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
                    cmd.CommandText = "EXEC CreateRecieptsFromReservations";
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
            SqlCommand phonesCmd = new SqlCommand("Select [dbo].Game.price*0.75 as price, phone " +
"from reservation " +
"inner join [dbo].[User] on [dbo].[User].id = reservation.user_id " +
"inner join [dbo].Game on [dbo].[Game].id = reservation.game_id " +
"where reservation.date < CURRENT_TIMESTAMP " +
"and reservation.state != \'Hotová\' and reservation.state != \'Propadlá\'", db.connection);
            SqlDataReader phonesReader = phonesCmd.ExecuteReader();
            while (phonesReader.Read())
            {
                int i = -1;
                phonesReader.GetDecimal(++i);
                phones.Add(phonesReader.GetString(++i));
            }
            db.Close();
            return phones;
        }

    }
}

using Mar0739ORM.Database;
using Mar0739ORM.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ProjektDB.ORM
{
    public class GameRepository
    {
        public static List<Game> FindAllGames()
        {
            List<Game> result = new List<Game>();
            DBConnection db = new DBConnection();

            db.Connect();
            SqlCommand cmd = new SqlCommand("SELECT * FROM [dbo].[game]", db.connection); //
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                int i = -1;
                Game tmpGame = new Game();
                tmpGame.Id = reader.GetInt32(++i);
                tmpGame.Name = reader.GetString(++i);
                tmpGame.Price = reader.GetInt32(++i);
                tmpGame.Length = reader.GetInt32(++i);
                tmpGame.State = reader.GetString(++i);

                result.Add(tmpGame);
            }

            db.Close();
            return result;
        }
        public static Game FindGame(int id)
        {
            List<Game> result = new List<Game>();
            DBConnection db = new DBConnection();

            db.Connect();
            SqlCommand cmd = new SqlCommand("SELECT * FROM [dbo].[game] WHERE id = @id", db.connection); //
            cmd.Parameters.AddWithValue("@id", id);
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                int i = -1;
                Game tmpGame = new Game();
                tmpGame.Id = reader.GetInt32(++i);
                tmpGame.Name = reader.GetString(++i);
                tmpGame.Price = reader.GetInt32(++i);
                tmpGame.Length = reader.GetInt32(++i);
                tmpGame.State = reader.GetString(++i);

                result.Add(tmpGame);
            }

            db.Close();
            return result[0];
        }

        public static void CreateGame(Game game)
        {
            DBConnection db = new DBConnection();

            db.Connect();

            SqlCommand cmdGame = new SqlCommand("INSERT INTO [dbo].[game] VALUES ( " +
                "@name, @price," +
                " @length, @state)",
                db.connection);

            cmdGame.Parameters.AddWithValue("@name", game.Name);
            cmdGame.Parameters.AddWithValue("@price", game.Price);
            cmdGame.Parameters.AddWithValue("@length", game.Length);
            cmdGame.Parameters.AddWithValue("@state", game.State);

            cmdGame.ExecuteNonQuery();

            db.Close();
        }

        public static void UpdateGame(Game game)
        {
            DBConnection db = new DBConnection();

            db.Connect();
            SqlCommand cmdUser = new SqlCommand("UPDATE [dbo].[game] set " +
                "name = @name,price =  @price," +
                " length= @length,state= @state where id = @id",
                db.connection);

            cmdUser.Parameters.AddWithValue("@id", game.Id);
            cmdUser.Parameters.AddWithValue("@name", game.Name);
            cmdUser.Parameters.AddWithValue("@length", game.Length);
            cmdUser.Parameters.AddWithValue("@state", game.State);
            cmdUser.Parameters.AddWithValue("@price", game.Price);

            cmdUser.ExecuteNonQuery();

            db.Close();
        }
        public static void UpdateGameState(string state, int id)
        {
            DBConnection db = new DBConnection();

            db.Connect();
            SqlCommand cmdUser = new SqlCommand("UPDATE [dbo].[game] set " +
                "state= @state where id = @id",
                db.connection);

            cmdUser.Parameters.AddWithValue("@id", id);
            cmdUser.Parameters.AddWithValue("@state", state);

            cmdUser.ExecuteNonQuery();

            db.Close();
        }
        public static void DeleteGameById(int id)
        {
            DBConnection db = new DBConnection();

            db.Connect();
            SqlCommand cmdUser = new SqlCommand("DELETE FROM game WHERE id = @id",
                db.connection);
            cmdUser.Parameters.AddWithValue("@id", id);
            cmdUser.ExecuteNonQuery();

            db.Close();
        }
    }
}

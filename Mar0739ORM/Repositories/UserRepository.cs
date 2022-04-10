using Mar0739ORM.Database;
using Mar0739ORM.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ProjektDB.ORM
{
    public class UserRepository
    {
        public static void CreateUser(User user) {
            DBConnection db = new DBConnection();

            db.Connect();

            SqlCommand cmdUser = new SqlCommand("INSERT INTO [dbo].[user] VALUES ( " +
                "@first_name, @last_name," +
                " @street, @city, @country, @postal_code, @email, @phone,@role)",
                db.connection);

            cmdUser.Parameters.AddWithValue("@first_name", user.FirstName);
            cmdUser.Parameters.AddWithValue("@last_name", user.LastName);
            cmdUser.Parameters.AddWithValue("@street", user.Street);
            cmdUser.Parameters.AddWithValue("@city", user.City);
            cmdUser.Parameters.AddWithValue("@country", user.Country);
            cmdUser.Parameters.AddWithValue("@postal_code", user.PostalCode);
            cmdUser.Parameters.AddWithValue("@email", user.Email);
            cmdUser.Parameters.AddWithValue("@phone", user.Phone);
            cmdUser.Parameters.AddWithValue("@role", user.Role);

            cmdUser.ExecuteNonQuery();

            db.Close();
        }
        public static User FindUserById(int id)
        {
            List<User> result = new List<User>();
            DBConnection db = new DBConnection();

            db.Connect();
            SqlCommand cmd = new SqlCommand("SELECT * FROM [dbo].[user] WHERE id = @id;", db.connection); //
            cmd.Parameters.AddWithValue("@id", id);
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                int i = -1;
                User tmpUser = new User();
                tmpUser.Id = reader.GetInt32(++i);
                tmpUser.FirstName = reader.GetString(++i);
                tmpUser.LastName = reader.GetString(++i);
                tmpUser.Street = reader.GetString(++i);
                tmpUser.City = reader.GetString(++i);
                tmpUser.Country = reader.GetString(++i);
                tmpUser.PostalCode = reader.GetString(++i);
                tmpUser.Email = reader.GetString(++i);
                tmpUser.Phone = reader.GetString(++i);
                tmpUser.Role = reader.GetString(++i);

                result.Add(tmpUser);
            }

            db.Close();
            return result[0];
        }
        public static void UpdateUser(User user)
        {
            DBConnection db = new DBConnection();

            db.Connect();
            SqlCommand cmdUser = new SqlCommand("UPDATE [dbo].[user] SET " +
                "first_name = @first_name,last_name = @last_name," +
                "street = @street, city = @city, country = @country, postal_code = @postal_code, email = @email, phone = @phone, role = @role Where id = @id",
                db.connection);

            cmdUser.Parameters.AddWithValue("@id", user.Id);
            cmdUser.Parameters.AddWithValue("@first_name", user.FirstName);
            cmdUser.Parameters.AddWithValue("@last_name", user.LastName);
            cmdUser.Parameters.AddWithValue("@street", user.Street);
            cmdUser.Parameters.AddWithValue("@city", user.City);
            cmdUser.Parameters.AddWithValue("@country", user.Country);
            cmdUser.Parameters.AddWithValue("@postal_code", user.PostalCode);
            cmdUser.Parameters.AddWithValue("@email", user.Email);
            cmdUser.Parameters.AddWithValue("@phone", user.Phone);
            cmdUser.Parameters.AddWithValue("@role", user.Role);

            cmdUser.ExecuteNonQuery();

            db.Close();
        }
        public static void DeleteUserById(int id)
        {
            DBConnection db = new DBConnection();

            db.Connect();
            SqlCommand cmdUser = new SqlCommand("DELETE FROM [dbo].[user] WHERE id = @id", db.connection);
            cmdUser.Parameters.AddWithValue("@id", id);
            cmdUser.ExecuteNonQuery();

           db.Close();
        }
    }
}

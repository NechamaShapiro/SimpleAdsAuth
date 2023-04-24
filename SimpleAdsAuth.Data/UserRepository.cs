using System.Data.SqlClient;

namespace SimpleAdsAuth.Data
{
    public class UserRepository
    {
        private string _connectionString;

        public UserRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
        public void AddUser(User user, string password)
        {
            using var connection = new SqlConnection(_connectionString);
            using var cmd = connection.CreateCommand();
            cmd.CommandText = "INSERT INTO Users (FirstName, LastName, Email, PasswordHash) " +
                "VALUES (@firstName, @lastName, @email, @passwordHash)";
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(password);
            cmd.Parameters.AddWithValue("@firstName", user.FirstName);
            cmd.Parameters.AddWithValue("@lastName", user.LastName);
            cmd.Parameters.AddWithValue("@email", user.Email);
            cmd.Parameters.AddWithValue("@passwordHash", passwordHash);
            connection.Open();
            cmd.ExecuteNonQuery();
        }
        public User Login(string email, string password)
        {
            var user = GetByEmail(email);
            if (user == null)
            {
                return null;
            }

            var isValid = BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
            if (!isValid)
            {
                return null;
            }

            return user;

        }
        public User GetByEmail(string email)
        {
            using var connection = new SqlConnection(_connectionString);
            using var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM Users WHERE Email = @email";
            cmd.Parameters.AddWithValue("@email", email);
            connection.Open();
            var reader = cmd.ExecuteReader();
            if (!reader.Read())
            {
                return null;
            }

            return new User
            {
                Id = (int)reader["Id"],
                FirstName = (string)reader["FirstName"],
                LastName = (string)reader["LastName"],
                Email = (string)reader["Email"],
                PasswordHash = (string)reader["PasswordHash"]
            };
        }
        public User GetUserById(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            using var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM Users WHERE Id = @id";
            cmd.Parameters.AddWithValue("@id", id);
            connection.Open();
            var reader = cmd.ExecuteReader();
            if (!reader.Read())
            {
                return null;
            }

            return new User
            {
                Id = (int)reader["Id"],
                FirstName = (string)reader["FirstName"],
                LastName = (string)reader["LastName"],
                Email = (string)reader["Email"],
                PasswordHash = (string)reader["PasswordHash"]
            };
        }
        public List<Ad> GetByUserId(int userId)
        {
            using var connection = new SqlConnection(_connectionString);
            using var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM Ads WHERE UserId = @userId";
            cmd.Parameters.AddWithValue("@userId", userId);
            connection.Open();
            var reader = cmd.ExecuteReader();
            if (!reader.Read())
            {
                return null;
            }
            List<Ad> myAds = new List<Ad>();
            while (reader.Read())
            {
                myAds.Add(new Ad
                {
                    Id = (int)reader["Id"],
                    Date = (DateTime)reader["Date"],
                    PhoneNumber = (string)reader["PhoneNumber"],
                    Details = (string)reader["Details"]
                });
            }
            return myAds;
        }
        public List<Ad> GetAllAds()
        {
            using var connection = new SqlConnection(_connectionString);
            using var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM Ads ORDER BY Date DESC ";
            List<Ad> ads = new List<Ad>();
            connection.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                ads.Add(new Ad
                {
                    Id = (int)reader["Id"],
                    UserId = (int)reader["UserId"],
                    Date = (DateTime)reader["Date"],
                    PhoneNumber = (string)reader["PhoneNumber"],
                    Details = (string)reader["Details"]
                });
            }
            connection.Close();
            return ads;
        }
        public void AddNewAd(int userId, string phoneNumber, string details)
        {
            using var connection = new SqlConnection(_connectionString);
            using var cmd = connection.CreateCommand();
            cmd.CommandText = "INSERT INTO Ads " +
                "VALUES (@userId, GETDATE(), @phoneNumber, @details)";
            cmd.Parameters.AddWithValue("@userId", userId);
            cmd.Parameters.AddWithValue("@phoneNumber", phoneNumber);
            cmd.Parameters.AddWithValue("@details", details);
            connection.Open();
            cmd.ExecuteNonQuery();
        }
        public void DeleteAd(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            using var cmd = connection.CreateCommand();
            cmd.CommandText = "DELETE FROM Ads WHERE Id = @id";
            cmd.Parameters.AddWithValue("@id", id);
            connection.Open();
            cmd.ExecuteNonQuery();
        }
    }
}
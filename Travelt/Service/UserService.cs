using System;
using System.Collections.Generic;
using System.IO.Packaging;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using MySqlConnector;
using TravelT;

namespace Travelt.Service
{
    public class UserService
    {





        public class DatabaseConnection
        {
            private readonly string connection_info = "server=localhost;port=3306;user=root;password=;database=travelt";

            public MySqlConnection GetConnection()
            {
                return new MySqlConnection(connection_info);
            }
        }





        private readonly DatabaseConnection database_connection = new DatabaseConnection();

        public static User CurrentUser { get; set; }





        public User Login(string email, string password)
        {

            using var connection = database_connection.GetConnection();
            connection.Open();

            string db_Query = "SELECT user_id, username, email, first_name, last_name, gender, date_of_birth, bio, role FROM user WHERE email = @email AND password_hash = @password_hash";

            using var db_SqlCommand = new MySqlCommand(db_Query, connection);
            db_SqlCommand.Parameters.AddWithValue("@email", email);
            db_SqlCommand.Parameters.AddWithValue("@password_hash", password);

            using var reader = db_SqlCommand.ExecuteReader();

            if (reader.Read())
            {
                return new User
                {
                    UserId = Convert.ToInt32(reader["user_id"]),
                    Username = reader["username"].ToString(),
                    Email = reader["email"].ToString(),
                    FirstName = reader["first_name"].ToString(),
                    LastName = reader["last_name"].ToString(),
                    Gender = reader["gender"].ToString(),
                    DateOfBirth = Convert.ToDateTime(reader["date_of_birth"]),
                    Bio = reader["bio"].ToString(),
                    Role = reader["role"].ToString()
                };

            }
            return null;
        }





        public User Register(string firstName, string lastName, string username, string gender, DateTime dateOfBirth, string email, string password)
        {

            using var connection = database_connection.GetConnection();
            connection.Open();

            string userExists = "SELECT COUNT(*) FROM user WHERE email = @email OR username = @username";
            using var check_db = new MySqlCommand(userExists, connection);
            check_db.Parameters.AddWithValue("@email", email);
            check_db.Parameters.AddWithValue("@username", username);

            int result_count = Convert.ToInt32(check_db.ExecuteScalar());

            if (result_count > 0)
            {
                return null;
            }

            string insert_to_db = @"INSERT INTO user (username, email, password_hash, first_name, last_name, date_of_birth, gender)
                                    VALUES (@username, @email, @password_hash, @first_name, @last_name, @date_of_birth, @gender)";


            using var insert_to_db_data = new MySqlCommand(insert_to_db, connection);

            insert_to_db_data.Parameters.AddWithValue("@username", username);
            insert_to_db_data.Parameters.AddWithValue("@email", email);
            insert_to_db_data.Parameters.AddWithValue("@password_hash", password);
            insert_to_db_data.Parameters.AddWithValue("@first_name", firstName);
            insert_to_db_data.Parameters.AddWithValue("@last_name", lastName);
            insert_to_db_data.Parameters.AddWithValue("@date_of_birth", dateOfBirth);
            insert_to_db_data.Parameters.AddWithValue("@gender", gender);


            int count_result = insert_to_db_data.ExecuteNonQuery();

            if (count_result > 0)
            {
                User new_user = new User
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Username = username,
                    Gender = gender,
                    DateOfBirth = dateOfBirth,
                    Email = email
                };

                CurrentUser = new_user;

                return new_user; ;
            }

            return null;




        }





        public bool ChangeBio(int user_id, string bio)
        {
            using var connection = database_connection.GetConnection();
            connection.Open();


            string bioUpdate = "UPDATE user SET bio = @bio WHERE user_id = @user_id";

            using var insert_to_db_data = new MySqlCommand(bioUpdate, connection);

            insert_to_db_data.Parameters.AddWithValue("@user_id", user_id);
            insert_to_db_data.Parameters.AddWithValue("@bio", bio);

            int count_result = insert_to_db_data.ExecuteNonQuery();

            return count_result > 0;

        }





        public bool ChangeUsername(int user_id, string username)
        {
            using var connection = database_connection.GetConnection();
            connection.Open();


            string usernameUpdate = "UPDATE user SET username = @username WHERE user_id = @user_id";

            using var insert_to_db_data = new MySqlCommand(usernameUpdate, connection);

            insert_to_db_data.Parameters.AddWithValue("@user_id", user_id);
            insert_to_db_data.Parameters.AddWithValue("@username", username);

            int count_result = insert_to_db_data.ExecuteNonQuery();

            return count_result > 0;

        }





        public bool ChangePassword(int user_id, string old_password, string new_password)
        {

            using var connection = database_connection.GetConnection();
            connection.Open();


            string changePassword = "UPDATE user SET password_hash = @new_password WHERE user_id = @user_id AND password_hash = @old_password";

            using var insert_to_db_data = new MySqlCommand(changePassword, connection);

            insert_to_db_data.Parameters.AddWithValue("@user_id", user_id);
            insert_to_db_data.Parameters.AddWithValue("@old_password", old_password);
            insert_to_db_data.Parameters.AddWithValue("@new_password", new_password);

            int count_result = insert_to_db_data.ExecuteNonQuery();

            return count_result > 0;

        }




        public bool DeleteUser(int user_id, string password) 
        {
            using var connection = database_connection.GetConnection();
            connection.Open();

            string deleteUser = "DELETE FROM user WHERE user_id = @user_id AND password_hash = @password";

            using var insert_to_db_data = new MySqlCommand(deleteUser, connection);

            insert_to_db_data.Parameters.AddWithValue("@user_id", user_id);
            insert_to_db_data.Parameters.AddWithValue("@password", password);

            int count_result = insert_to_db_data.ExecuteNonQuery();

            return count_result > 0;
        }

    }
}

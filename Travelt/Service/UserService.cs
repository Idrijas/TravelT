using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Windows;
using TravelT;

namespace Travelt.Service
{
    public class UserService
    {
        public class DatabaseConnection
        {
            private readonly string connection_info = "server=localhost;port=3306;user=root;password=;database=travelt;ConvertZeroDateTime=True";

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

            string db_Query = "SELECT user_id, username, email, first_name, last_name, gender, date_of_birth, bio, profile_picture, role FROM user WHERE email = @email AND password_hash = @password_hash";

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
                    ProfilePicture = reader["profile_picture"] == DBNull.Value ? "" : reader["profile_picture"].ToString(),
                    Role = reader["role"].ToString()
                };
            }
            return null;
        }





        public User Register(string firstName, string lastName, string username, string gender, DateTime dateOfBirth, string email, string password, int nationalityCountryId)
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

            string insert_to_db = @"INSERT INTO user (username, email, password_hash, first_name, last_name, date_of_birth, gender, nationality_country_id)
                                    VALUES (@username, @email, @password_hash, @first_name, @last_name, @date_of_birth, @gender, @nationality_country_id)";

            using var insert_to_db_data = new MySqlCommand(insert_to_db, connection);

            insert_to_db_data.Parameters.AddWithValue("@username", username);
            insert_to_db_data.Parameters.AddWithValue("@email", email);
            insert_to_db_data.Parameters.AddWithValue("@password_hash", password);
            insert_to_db_data.Parameters.AddWithValue("@first_name", firstName);
            insert_to_db_data.Parameters.AddWithValue("@last_name", lastName);
            insert_to_db_data.Parameters.AddWithValue("@date_of_birth", dateOfBirth);
            insert_to_db_data.Parameters.AddWithValue("@gender", gender);
            insert_to_db_data.Parameters.AddWithValue("@nationality_country_id", nationalityCountryId);

            int count_result = insert_to_db_data.ExecuteNonQuery();

            if (count_result > 0)
            {

                int newUserId = (int)insert_to_db_data.LastInsertedId;

                User new_user = new User
                {
                    UserId = newUserId,
                    FirstName = firstName,
                    LastName = lastName,
                    Username = username,
                    Gender = gender,
                    DateOfBirth = dateOfBirth,
                    Email = email,
                    NationalityCountryId = nationalityCountryId
                };

                CurrentUser = new_user;


                string get_rank = @" INSERT INTO user_rank (user_id, rank_id, assigned_at)
                                    VALUES (@userId, 1, NOW());";

                using var get_rank_from_db = new MySqlCommand(get_rank, connection);

                get_rank_from_db.Parameters.AddWithValue("@userId", newUserId);

                get_rank_from_db.ExecuteNonQuery();

                GiveAchievementToUser(newUserId, 1);

                return new_user;
            }

            return null;
        }





        public bool ChangeBio(int user_id, string bio)
        {
            using var connection = database_connection.GetConnection();
            connection.Open();

            

            string bioUpdate = "UPDATE user SET bio = @bio WHERE user_id = @user_id";

            using var command = new MySqlCommand(bioUpdate, connection);

            command.Parameters.AddWithValue("@user_id", user_id);
            command.Parameters.AddWithValue("@bio", bio);

            int count_result = command.ExecuteNonQuery();

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





        public List<User> GetAllUsers()
        {
            List<User> users_list = new List<User>();

            using var connection = database_connection.GetConnection();
            connection.Open();

            string select_users = "SELECT user_id, first_name, last_name, username, email, role FROM user";

            using var select_from_db_data = new MySqlCommand(select_users, connection);

            using var reader = select_from_db_data.ExecuteReader();

            while (reader.Read())
            {
                User user = new User
                {
                    UserId = reader.GetInt32("user_id"),
                    FirstName = reader.GetString("first_name"),
                    LastName = reader.GetString("last_name"),
                    Username = reader.GetString("username"),
                    Email = reader.GetString("email"),
                    Role = reader.GetString("role"),
                };
                users_list.Add(user);
            }
            return users_list;
        }





        public bool AdminDeleteUser(int userid)
        {
            using var connection = database_connection.GetConnection();
            connection.Open();

            string delete_posts = "DELETE FROM posts WHERE user_id = @user_id";
            using var delete_posts_db = new MySqlCommand(delete_posts, connection);
            delete_posts_db.Parameters.AddWithValue("@user_id", userid);
            delete_posts_db.ExecuteNonQuery();

            string delete_achievements = "DELETE FROM user_achievement WHERE user_id = @user_id";
            using var delete_achievements_db = new MySqlCommand(delete_achievements, connection);
            delete_achievements_db.Parameters.AddWithValue("@user_id", userid);
            delete_achievements_db.ExecuteNonQuery();

            string delete_rank = "DELETE FROM user_rank WHERE user_id = @user_id";
            using var delete_rank_db = new MySqlCommand(delete_rank, connection);
            delete_rank_db.Parameters.AddWithValue("@user_id", userid);
            delete_rank_db.ExecuteNonQuery();

            string delete_trips = "DELETE FROM user_trip WHERE user_id = @user_id";
            using var delete_trips_db = new MySqlCommand(delete_trips, connection);
            delete_trips_db.Parameters.AddWithValue("@user_id", userid);
            delete_trips_db.ExecuteNonQuery();

            string delete_visited_countries = "DELETE FROM user_visited_country WHERE user_id = @user_id";
            using var delete_visited_countries_db = new MySqlCommand(delete_visited_countries, connection);
            delete_visited_countries_db.Parameters.AddWithValue("@user_id", userid);
            delete_visited_countries_db.ExecuteNonQuery();

            string delete_user = "DELETE FROM user WHERE user_id = @user_id";
            using var delete_user_db = new MySqlCommand(delete_user, connection);
            delete_user_db.Parameters.AddWithValue("@user_id", userid);

            int count_result = delete_user_db.ExecuteNonQuery();

            return count_result > 0;
        }





        public bool AdminUpdateUsername(int userId, string new_username)
        {
            using var connection = database_connection.GetConnection();
            connection.Open();

            string admin_updateUsername = "UPDATE user SET username = @username WHERE user_id = @user_id";

            using var update_new_db_data = new MySqlCommand(admin_updateUsername, connection);

            update_new_db_data.Parameters.AddWithValue("@username", new_username);
            update_new_db_data.Parameters.AddWithValue("@user_id", userId);

            int count_result = update_new_db_data.ExecuteNonQuery();

            return count_result > 0;
        }





        public bool AdminDeleteBio(int userId)
        {
            using var connection = database_connection.GetConnection();
            connection.Open();

            string admin_deleteBio = "UPDATE user SET bio = '' WHERE user_id = @user_id";

            using var update_delete_bio_db = new MySqlCommand(admin_deleteBio, connection);
            update_delete_bio_db.Parameters.AddWithValue("@user_id", userId);

            int count_result = update_delete_bio_db.ExecuteNonQuery();

            return count_result > 0;
        }





        public bool AdminDeleteProfilePicture (int userId)
        {

            using var connection = database_connection.GetConnection();
            connection.Open();

            string admin_deleteProfilePicture = "UPDATE user SET profile_picture = NULL WHERE user_id = @user_id";

            using var update_delete_profile_pic_db = new MySqlCommand(admin_deleteProfilePicture, connection);

            update_delete_profile_pic_db.Parameters.AddWithValue("@user_id", userId);

            int count_result = update_delete_profile_pic_db.ExecuteNonQuery();

            return count_result > 0;
        }





        public User GetUserById(int userId)
        {
            try
            {
                using var connection = database_connection.GetConnection();
                connection.Open();

                string db_Query = "SELECT user_id, username, email, first_name, last_name, gender, date_of_birth, bio, profile_picture, role FROM user WHERE user_id = @user_id";

                using var db_SqlCommand = new MySqlCommand(db_Query, connection);
                db_SqlCommand.Parameters.AddWithValue("@user_id", userId);

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
                        ProfilePicture = reader["profile_picture"] == DBNull.Value ? "" : reader["profile_picture"].ToString(),
                        Role = reader["role"].ToString()
                    };
                }
                return null;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Database Error in GetUserById: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }





        public bool ChangeProfilePicture(int user_id,  string profilePicturePath)
        {
            using var connection = database_connection.GetConnection();
            connection.Open();


            string change_pic_query = "UPDATE user SET profile_picture = @profile_picture WHERE user_id = @user_id";

            using var update_picture_db = new MySqlCommand(change_pic_query, connection);

            update_picture_db.Parameters.AddWithValue("@profile_picture", profilePicturePath);
            update_picture_db.Parameters.AddWithValue("@user_id", user_id);

            int result_count = update_picture_db.ExecuteNonQuery();

            return result_count > 0;

        }





        public List<Achievements> GetAllAchievements()
        {
            List<Achievements> achievements = new List<Achievements>();

            using var connection = database_connection.GetConnection();
            connection.Open();

            string get_achievement = "SELECT * FROM achievement";

            using var get_from_db = new MySqlCommand(get_achievement, connection);

            using var reader = get_from_db.ExecuteReader();

            while (reader.Read()) 
            {
                Achievements achievement = new Achievements()
                {
                    AchievementId = Convert.ToInt32(reader["achievement_id"]),
                    Title = reader["title"].ToString(),
                    Description = reader["description"].ToString(),
                    IconUrl = reader["icon_url"].ToString()
                };

                achievements.Add(achievement);
            }
            return achievements;
        }





        public List<AchievementsDisplay> GetUserAchievements(int userId)
        {
            List<AchievementsDisplay> achievementsDisplays = new List<AchievementsDisplay>();

            using var connection = database_connection.GetConnection();
            connection.Open();

            string get_user_achievement = @"SELECT 
                                            a.achievement_id,
                                            a.title,
                                            a.description,
                                            a.icon_url,
                                           CASE 
                                           WHEN ua.user_id IS NOT NULL THEN 1
                                           ELSE 0
                                           END AS is_unlocked
                                           FROM achievement a
                                           LEFT JOIN user_achievement ua 
                                           ON a.achievement_id = ua.achievement_id
                                           AND ua.user_id = @user_id;";

            using var get_achievements_from_db = new MySqlCommand(get_user_achievement, connection);
            get_achievements_from_db.Parameters.AddWithValue("user_id", userId);

            using var reader = get_achievements_from_db.ExecuteReader();

            while (reader.Read()) 
            {
                AchievementsDisplay achievements_display = new AchievementsDisplay()
                {
                    AchievementId = Convert.ToInt32(reader["achievement_id"]),
                    Title = reader["title"].ToString(),
                    Description = reader["description"].ToString(),
                    IconUrl = reader["icon_url"].ToString(),
                    AchievementUnlocked = Convert.ToInt32(reader["is_unlocked"]) == 1
                };
                achievementsDisplays.Add(achievements_display);

            }
            return achievementsDisplays;


        }





        public bool GiveAchievementToUser(int userId, int achievementId)
        {
            using var connection = database_connection.GetConnection();
            connection.Open();

            string give_achievement = @"
                            INSERT INTO user_achievement (user_id, achievement_id, date_earned)
                            SELECT @user_id, @achievement_id, NOW()
                            WHERE NOT EXISTS (
                                                SELECT 1 
                                                FROM user_achievement 
                                                WHERE user_id = @user_id 
                                                AND achievement_id = @achievement_id);";

            using var assing_to_db = new MySqlCommand(give_achievement, connection);
            assing_to_db.Parameters.AddWithValue("@user_id", userId);
            assing_to_db.Parameters.AddWithValue("@achievement_id", achievementId);

            int result = assing_to_db.ExecuteNonQuery();

            return result > 0;
        }





        public string GetUserRank(int userId)
        {
            using var connection = database_connection.GetConnection();
            connection.Open();

            string get_rank = @"SELECT r.name
                                FROM user_rank ur
                                JOIN `rank` r ON ur.rank_id = r.rank_id
                                WHERE ur.user_id = @userId
                                ORDER BY ur.assigned_at DESC
                                LIMIT 1;";

            using var get_from_db = new MySqlCommand(get_rank, connection);

            get_from_db.Parameters.AddWithValue("@userId", userId);

            string? result = get_from_db.ExecuteScalar()?.ToString();

            return result ?? "Wanderer";

        }





        public List<Country> GetAllCountrie()
        {
            List<Country> countries = new List<Country>();

            using var connection = database_connection.GetConnection();
            connection.Open();

            string get_country = @"SELECT country_id, country_name, country_code FROM country ORDER BY country_name;";

            using var get_from_db = new MySqlCommand(get_country, connection);

            using var reader = get_from_db.ExecuteReader();

            while (reader.Read())
            {
                countries.Add(new Country{
                    CountryId = Convert.ToInt32(reader["country_id"]),
                    CountryName = reader["country_name"].ToString(),
                    CountryCode = reader["country_code"].ToString()

                });
            }
            return countries;

        }





        public string GetUserNationality(int userId)
        {
            using var connection = database_connection.GetConnection();
            connection.Open();

            string get_from_db = @"SELECT c.country_name
                                   FROM `user` u
                                   JOIN country c ON u.nationality_country_id = c.country_id
                                   WHERE u.user_id = @userId;";

            using var get_nat_from_db = new MySqlCommand(get_from_db, connection);
            get_nat_from_db.Parameters.AddWithValue("@userId", userId);

            string? result = get_nat_from_db.ExecuteScalar()?.ToString();

            return result ?? "Not Selected";
        }





        public int CountVisitedCountries(int userId)
        {
            using var connection = database_connection.GetConnection();
            connection.Open();

            string count_db = @"SELECT COUNT(*) FROM user_visited_country WHERE user_id = @userId;";

            using var count_countries_db = new MySqlCommand(count_db, connection);
            count_countries_db.Parameters.AddWithValue("@userId", userId);

            return Convert.ToInt32(count_countries_db.ExecuteScalar());

        }





        public List<Country>GetVisitedCountries(int userId)
        {
            List<Country> countries_list = new List<Country>();

            using var connection = database_connection.GetConnection();
            connection.Open();

            string get_country = @"SELECT c.country_id, c.country_name, c.country_code
                             FROM user_visited_country uvc
                             JOIN country c ON uvc.country_id = c.country_id
                             WHERE uvc.user_id = @userId
                             ORDER BY c.country_name;";

            using var get_country_from_db = new MySqlCommand(get_country, connection);

            get_country_from_db.Parameters.AddWithValue("@userId", userId);

            using var reader = get_country_from_db.ExecuteReader();

            while (reader.Read()) 
            {
                countries_list.Add(new Country
                {
                    CountryId = Convert.ToInt32(reader["country_id"]),
                    CountryName = reader["country_name"].ToString(),
                    CountryCode = reader["country_code"].ToString()
                });
            }
            return countries_list;
        }





        public void AddVisitedCountry(int userId, int countryId) 
        {
            using var connection = database_connection.GetConnection();
            connection.Open();

            string add_country = @"INSERT IGNORE INTO user_visited_country (user_id, country_id, visited_at)
                                 VALUES (@userId, @countryId, NOW());";

            using var add_country_db = new MySqlCommand(add_country, connection);

            add_country_db.Parameters.AddWithValue("@userId", userId);
            add_country_db.Parameters.AddWithValue("@countryId", countryId);

            add_country_db.ExecuteNonQuery();

        }




        public void DeleteVisitedCountry(int userId, int countryId)
        {
            using var connection = database_connection.GetConnection();
            connection.Open();

            string delete_from = @"DELETE FROM user_visited_country WHERE user_id = @userId AND country_id = @countryId;";
            
            using var delete_visited_country = new MySqlCommand(delete_from, connection);

            delete_visited_country.Parameters.AddWithValue("@userId", userId);
            delete_visited_country.Parameters.AddWithValue("@countryId", countryId);

            delete_visited_country.ExecuteNonQuery();
        }

        public List<User> SearchUsers(string query)
        {
            List<User> users_list = new List<User>();
            using var connection = database_connection.GetConnection();
            connection.Open();

            string search_users = @"SELECT user_id, first_name, last_name, username, profile_picture 
                                    FROM user 
                                    WHERE username LIKE @q OR first_name LIKE @q OR last_name LIKE @q 
                                    ORDER BY user_id DESC LIMIT 50";

            using var cmd = new MySqlCommand(search_users, connection);
            cmd.Parameters.AddWithValue("@q", "%" + query + "%");
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                users_list.Add(new User
                {
                    UserId = reader.GetInt32("user_id"),
                    FirstName = reader.GetString("first_name"),
                    LastName = reader.GetString("last_name"),
                    Username = reader.GetString("username"),
                    ProfilePicture = reader["profile_picture"] == DBNull.Value ? "" : reader.GetString("profile_picture")
                });
            }
            return users_list;
        }
        public List<User> GetNewestUsers()
        {
            List<User> users_list = new List<User>();
            using var connection = database_connection.GetConnection();
            connection.Open();

            string query = @"SELECT user_id, first_name, last_name, username, profile_picture 
                             FROM user 
                             ORDER BY user_id DESC LIMIT 50";

            using var cmd = new MySqlCommand(query, connection);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                users_list.Add(new User
                {
                    UserId = reader.GetInt32("user_id"),
                    FirstName = reader.GetString("first_name"),
                    LastName = reader.GetString("last_name"),
                    Username = reader.GetString("username"),
                    ProfilePicture = reader["profile_picture"] == DBNull.Value ? "" : reader.GetString("profile_picture")
                });
            }
            return users_list;
        }
    }

}
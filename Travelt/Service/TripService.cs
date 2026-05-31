using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Windows;

namespace Travelt.Service
{
    // Model for editing existing trips
    public class FullTripModel
    {
        public int TripId { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public bool IsFlexible { get; set; }
        public string FlexibleMonths { get; set; }
        public int MaxPeople { get; set; }
        public string TripType { get; set; }
        public string Description { get; set; }
        public bool IsPublic { get; set; }
        public List<string> Countries { get; set; } = new List<string>();
        public List<string> Places { get; set; } = new List<string>();
    }

    public class TripService
    {
        private readonly string connection_info = "server=localhost;port=3306;user=root;password=;database=travelt;ConvertZeroDateTime=True";

        public FullTripModel GetFullTripById(int tripId)
        {
            using var connection = new MySqlConnection(connection_info);
            connection.Open();
            string sql = "SELECT * FROM trip WHERE trip_id = @id";
            using var cmd = new MySqlCommand(sql, connection);
            cmd.Parameters.AddWithValue("@id", tripId);

            using var reader = cmd.ExecuteReader();
            if (!reader.Read()) return null;

            var trip = new FullTripModel
            {
                TripId = tripId,
                DateFrom = reader["date_from"] != DBNull.Value ? (DateTime?)reader["date_from"] : null,
                DateTo = reader["date_to"] != DBNull.Value ? (DateTime?)reader["date_to"] : null,
                IsFlexible = Convert.ToBoolean(reader["is_flexible_date"]),
                FlexibleMonths = reader["flexible_months"]?.ToString(),
                MaxPeople = Convert.ToInt32(reader["max_people"]),
                TripType = reader["trip_type"].ToString(),
                Description = reader["description"].ToString(),
                IsPublic = Convert.ToBoolean(reader["is_public"])
            };
            reader.Close();

            string countrySql = "SELECT c.country_name FROM country c JOIN trip_country tc ON c.country_id = tc.country_id WHERE tc.trip_id = @id";
            using var cCmd = new MySqlCommand(countrySql, connection);
            cCmd.Parameters.AddWithValue("@id", tripId);
            using var cReader = cCmd.ExecuteReader();
            while (cReader.Read()) trip.Countries.Add(cReader.GetString(0));
            cReader.Close();

            string placeSql = "SELECT place_name FROM trip_place WHERE trip_id = @id";
            using var pCmd = new MySqlCommand(placeSql, connection);
            pCmd.Parameters.AddWithValue("@id", tripId);
            using var pReader = pCmd.ExecuteReader();
            while (pReader.Read()) trip.Places.Add(pReader.GetString(0));

            return trip;
        }

        public bool UpdateTrip(FullTripModel trip)
        {
            using var connection = new MySqlConnection(connection_info);
            connection.Open();
            using var transaction = connection.BeginTransaction();

            try
            {
                string sql = @"UPDATE trip SET date_from = @df, date_to = @dt, is_flexible_date = @if, 
                               flexible_months = @fm, max_people = @mp, trip_type = @tt, 
                               description = @desc, is_public = @pub WHERE trip_id = @id";

                using var cmd = new MySqlCommand(sql, connection, transaction);
                cmd.Parameters.AddWithValue("@df", (object)trip.DateFrom ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@dt", (object)trip.DateTo ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@if", trip.IsFlexible ? 1 : 0);
                cmd.Parameters.AddWithValue("@fm", (object)trip.FlexibleMonths ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@mp", trip.MaxPeople);
                cmd.Parameters.AddWithValue("@tt", trip.TripType);
                cmd.Parameters.AddWithValue("@desc", trip.Description);
                cmd.Parameters.AddWithValue("@pub", trip.IsPublic ? 1 : 0);
                cmd.Parameters.AddWithValue("@id", trip.TripId);
                cmd.ExecuteNonQuery();

                new MySqlCommand("DELETE FROM trip_country WHERE trip_id = @id", connection, transaction) { Parameters = { new MySqlParameter("@id", trip.TripId) } }.ExecuteNonQuery();
                new MySqlCommand("DELETE FROM trip_place WHERE trip_id = @id", connection, transaction) { Parameters = { new MySqlParameter("@id", trip.TripId) } }.ExecuteNonQuery();

                foreach (string countryName in trip.Countries)
                {
                    var cCmd = new MySqlCommand("SELECT country_id FROM country WHERE country_name = @n", connection, transaction);
                    cCmd.Parameters.AddWithValue("@n", countryName);
                    var cIdObj = cCmd.ExecuteScalar();
                    int cId;
                    if (cIdObj == null)
                    {
                        var insCmd = new MySqlCommand("INSERT INTO country (country_name, country_code) VALUES (@n, ''); SELECT LAST_INSERT_ID();", connection, transaction);
                        insCmd.Parameters.AddWithValue("@n", countryName);
                        cId = Convert.ToInt32(insCmd.ExecuteScalar());
                    }
                    else cId = Convert.ToInt32(cIdObj);

                    var linkCmd = new MySqlCommand("INSERT INTO trip_country (trip_id, country_id) VALUES (@tid, @cid)", connection, transaction);
                    linkCmd.Parameters.AddWithValue("@tid", trip.TripId);
                    linkCmd.Parameters.AddWithValue("@cid", cId);
                    linkCmd.ExecuteNonQuery();
                }

                foreach (string place in trip.Places)
                {
                    var pCmd = new MySqlCommand("INSERT INTO trip_place (trip_id, place_name) VALUES (@tid, @pn)", connection, transaction);
                    pCmd.Parameters.AddWithValue("@tid", trip.TripId);
                    pCmd.Parameters.AddWithValue("@pn", place);
                    pCmd.ExecuteNonQuery();
                }

                transaction.Commit();
                return true;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                MessageBox.Show("Error updating trip: " + ex.Message);
                return false;
            }
        }

        public bool DeleteTrip(int tripId)
        {
            try
            {
                using var connection = new MySqlConnection(connection_info);
                connection.Open();
                string sql = "DELETE FROM trip WHERE trip_id = @id";
                using var cmd = new MySqlCommand(sql, connection);
                cmd.Parameters.AddWithValue("@id", tripId);
                return cmd.ExecuteNonQuery() > 0;
            }
            catch { return false; }
        }

        public bool SaveNewTrip(int userId, DateTime? dateFrom, DateTime? dateTo, bool isFlexible, string flexibleMonths, int maxPeople, string tripType, string description, bool isPublic, ICollection<string> countries, ICollection<string> places)
        {
            using var connection = new MySqlConnection(connection_info);
            connection.Open();
            using var transaction = connection.BeginTransaction();
            try
            {
                string tripQuery = @"INSERT INTO trip (date_from, date_to, is_flexible_date, flexible_months, max_people, trip_type, description, is_public, status) 
                                     VALUES (@date_from, @date_to, @is_flexible_date, @flexible_months, @max_people, @trip_type, @description, @is_public, 'started');
                                     SELECT LAST_INSERT_ID();";

                using var tripCommand = new MySqlCommand(tripQuery, connection, transaction);
                tripCommand.Parameters.AddWithValue("@date_from", (object)dateFrom ?? DBNull.Value);
                tripCommand.Parameters.AddWithValue("@date_to", (object)dateTo ?? DBNull.Value);
                tripCommand.Parameters.AddWithValue("@is_flexible_date", isFlexible ? 1 : 0);
                tripCommand.Parameters.AddWithValue("@flexible_months", string.IsNullOrWhiteSpace(flexibleMonths) ? DBNull.Value : flexibleMonths);
                tripCommand.Parameters.AddWithValue("@max_people", maxPeople);
                tripCommand.Parameters.AddWithValue("@trip_type", tripType);
                tripCommand.Parameters.AddWithValue("@description", description);
                tripCommand.Parameters.AddWithValue("@is_public", isPublic ? 1 : 0);
                int newTripId = Convert.ToInt32(tripCommand.ExecuteScalar());

                string userTripQuery = "INSERT INTO user_trip (trip_id, user_id, role) VALUES (@trip_id, @user_id, 'admin')";
                using var userTripCommand = new MySqlCommand(userTripQuery, connection, transaction);
                userTripCommand.Parameters.AddWithValue("@trip_id", newTripId);
                userTripCommand.Parameters.AddWithValue("@user_id", userId);
                userTripCommand.ExecuteNonQuery();

                foreach (string countryName in countries)
                {
                    string getCountryQuery = "SELECT country_id FROM country WHERE country_name = @name";
                    using var checkCmd = new MySqlCommand(getCountryQuery, connection, transaction);
                    checkCmd.Parameters.AddWithValue("@name", countryName);
                    object countryIdResult = checkCmd.ExecuteScalar();
                    int countryId;
                    if (countryIdResult != null) countryId = Convert.ToInt32(countryIdResult);
                    else
                    {
                        string insertCountry = "INSERT INTO country (country_name, country_code) VALUES (@name, '') ; SELECT LAST_INSERT_ID();";
                        using var insCmd = new MySqlCommand(insertCountry, connection, transaction);
                        insCmd.Parameters.AddWithValue("@name", countryName);
                        countryId = Convert.ToInt32(insCmd.ExecuteScalar());
                    }
                    string linkCountryQuery = "INSERT INTO trip_country (trip_id, country_id) VALUES (@trip_id, @country_id)";
                    using var linkCountryCmd = new MySqlCommand(linkCountryQuery, connection, transaction);
                    linkCountryCmd.Parameters.AddWithValue("@trip_id", newTripId);
                    linkCountryCmd.Parameters.AddWithValue("@country_id", countryId);
                    linkCountryCmd.ExecuteNonQuery();
                }

                foreach (string placeName in places)
                {
                    string linkPlaceQuery = "INSERT INTO trip_place (trip_id, place_name) VALUES (@trip_id, @place_name)";
                    using var linkPlaceCmd = new MySqlCommand(linkPlaceQuery, connection, transaction);
                    linkPlaceCmd.Parameters.AddWithValue("@trip_id", newTripId);
                    linkPlaceCmd.Parameters.AddWithValue("@place_name", placeName);
                    linkPlaceCmd.ExecuteNonQuery();
                }
                transaction.Commit();
                return true;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                MessageBox.Show("Error saving trip data to database: " + ex.Message, "Database Error");
                return false;
            }
        }

        public bool JoinTrip(int tripId, int userId)
        {
            try
            {
                using var connection = new MySqlConnection(connection_info);
                connection.Open();
                string query = "INSERT IGNORE INTO user_trip (trip_id, user_id, role) VALUES (@trip_id, @user_id, 'member')";
                using var cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@trip_id", tripId);
                cmd.Parameters.AddWithValue("@user_id", userId);
                return cmd.ExecuteNonQuery() > 0;
            }
            catch { return false; }
        }

        public List<TripDisplayModel> GetUserTrips(int userId)
        {
            List<TripDisplayModel> userTrips = new List<TripDisplayModel>();
            try
            {
                using var connection = new MySqlConnection(connection_info);
                connection.Open();
                string sql = @"SELECT t.trip_id, t.trip_type, t.date_from, t.date_to, t.is_flexible_date, t.flexible_months, t.description, t.max_people,
                        (SELECT GROUP_CONCAT(c.country_name SEPARATOR ', ') FROM trip_country tc JOIN country c ON tc.country_id = c.country_id WHERE tc.trip_id = t.trip_id) AS countries,
                        (SELECT GROUP_CONCAT(tp.place_name SEPARATOR ', ') FROM trip_place tp WHERE tp.trip_id = t.trip_id) AS places,
                        (SELECT COUNT(*) FROM user_trip ut2 WHERE ut2.trip_id = t.trip_id) AS joined_count,
                        (SELECT COUNT(*) FROM user_trip ut3 WHERE ut3.trip_id = t.trip_id AND ut3.user_id = @currentUserId) AS is_joined,
                        u.user_id AS admin_id, u.username AS admin_username, u.profile_picture AS admin_profile_picture
                        FROM trip t
                        JOIN user_trip ut ON t.trip_id = ut.trip_id
                        JOIN user_trip ut_admin ON t.trip_id = ut_admin.trip_id AND ut_admin.role = 'admin'
                        JOIN user u ON ut_admin.user_id = u.user_id
                        WHERE ut.user_id = @user_id ORDER BY t.trip_id DESC";
                using var command = new MySqlCommand(sql, connection);
                command.Parameters.AddWithValue("@user_id", userId);
                command.Parameters.AddWithValue("@currentUserId", userId); // Adjusted for current user
                using var reader = command.ExecuteReader();
                while (reader.Read()) userTrips.Add(MapTripDisplayModel(reader));
                reader.Close();
                foreach (var trip in userTrips) trip.JoinedMembers = GetTripMembers(trip.TripId);
            }
            catch (Exception ex) { MessageBox.Show("Error loading user trips: " + ex.Message); }
            return userTrips;
        }

        public List<TripDisplayModel> SearchPublicTrips(string query, int currentUserId)
        {
            List<TripDisplayModel> publicTrips = new List<TripDisplayModel>();
            try
            {
                using var connection = new MySqlConnection(connection_info);
                connection.Open();
                string sql = @"SELECT DISTINCT t.trip_id, t.trip_type, t.date_from, t.date_to, t.is_flexible_date, t.flexible_months, t.description, t.max_people,
                        (SELECT GROUP_CONCAT(c2.country_name SEPARATOR ', ') FROM trip_country tc2 JOIN country c2 ON tc2.country_id = c2.country_id WHERE tc2.trip_id = t.trip_id) AS countries,
                        (SELECT GROUP_CONCAT(tp2.place_name SEPARATOR ', ') FROM trip_place tp2 WHERE tp2.trip_id = t.trip_id) AS places,
                        (SELECT COUNT(*) FROM user_trip ut2 WHERE ut2.trip_id = t.trip_id) AS joined_count,
                        (SELECT COUNT(*) FROM user_trip ut3 WHERE ut3.trip_id = t.trip_id AND ut3.user_id = @currentUserId) AS is_joined,
                        u.user_id AS admin_id, u.username AS admin_username, u.profile_picture AS admin_profile_picture
                        FROM trip t LEFT JOIN trip_country tc ON t.trip_id = tc.trip_id LEFT JOIN country c ON tc.country_id = c.country_id LEFT JOIN trip_place tp ON t.trip_id = tp.trip_id JOIN user_trip ut_admin ON t.trip_id = ut_admin.trip_id AND ut_admin.role = 'admin' JOIN user u ON ut_admin.user_id = u.user_id WHERE t.is_public = 1 AND (c.country_name LIKE @q OR tp.place_name LIKE @q OR t.description LIKE @q) ORDER BY t.trip_id DESC LIMIT 50";
                using var command = new MySqlCommand(sql, connection);
                command.Parameters.AddWithValue("@q", "%" + query + "%");
                command.Parameters.AddWithValue("@currentUserId", currentUserId);
                using var reader = command.ExecuteReader();
                while (reader.Read()) publicTrips.Add(MapTripDisplayModel(reader));
                reader.Close();
                foreach (var trip in publicTrips) trip.JoinedMembers = GetTripMembers(trip.TripId);
            }
            catch (Exception ex) { MessageBox.Show("Error searching trips: " + ex.Message); }
            return publicTrips;
        }

        public List<TripDisplayModel> GetNewestPublicTrips(int currentUserId)
        {
            List<TripDisplayModel> publicTrips = new List<TripDisplayModel>();
            try
            {
                using var connection = new MySqlConnection(connection_info);
                connection.Open();
                string sql = @"SELECT t.trip_id, t.trip_type, t.date_from, t.date_to, t.is_flexible_date, t.flexible_months, t.description, t.max_people,
                        (SELECT GROUP_CONCAT(c.country_name SEPARATOR ', ') FROM trip_country tc JOIN country c ON tc.country_id = c.country_id WHERE tc.trip_id = t.trip_id) AS countries,
                        (SELECT GROUP_CONCAT(tp.place_name SEPARATOR ', ') FROM trip_place tp WHERE tp.trip_id = t.trip_id) AS places,
                        (SELECT COUNT(*) FROM user_trip ut2 WHERE ut2.trip_id = t.trip_id) AS joined_count,
                        (SELECT COUNT(*) FROM user_trip ut3 WHERE ut3.trip_id = t.trip_id AND ut3.user_id = @currentUserId) AS is_joined,
                        u.user_id AS admin_id, u.username AS admin_username, u.profile_picture AS admin_profile_picture
                        FROM trip t JOIN user_trip ut_admin ON t.trip_id = ut_admin.trip_id AND ut_admin.role = 'admin' JOIN user u ON ut_admin.user_id = u.user_id WHERE t.is_public = 1 ORDER BY t.trip_id DESC LIMIT 50";
                using var command = new MySqlCommand(sql, connection);
                command.Parameters.AddWithValue("@currentUserId", currentUserId);
                using var reader = command.ExecuteReader();
                while (reader.Read()) publicTrips.Add(MapTripDisplayModel(reader));
                reader.Close();
                foreach (var trip in publicTrips) trip.JoinedMembers = GetTripMembers(trip.TripId);
            }
            catch (Exception ex) { MessageBox.Show("Error loading newest trips: " + ex.Message); }
            return publicTrips;
        }

        public List<User> GetTripMembers(int tripId)
        {
            List<User> members = new List<User>();
            try
            {
                using var connection = new MySqlConnection(connection_info);
                connection.Open();
                string sql = "SELECT u.user_id, u.first_name, u.last_name, u.username, u.profile_picture FROM user u JOIN user_trip ut ON u.user_id = ut.user_id WHERE ut.trip_id = @trip_id ORDER BY u.username ASC";
                using var command = new MySqlCommand(sql, connection);
                command.Parameters.AddWithValue("@trip_id", tripId);
                using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    members.Add(new User { UserId = reader.GetInt32("user_id"), FirstName = reader.GetString("first_name"), LastName = reader.GetString("last_name"), Username = reader.GetString("username"), ProfilePicture = reader["profile_picture"] == DBNull.Value ? "" : reader.GetString("profile_picture") });
                }
            }
            catch { }
            return members;
        }

        private TripDisplayModel MapTripDisplayModel(MySqlDataReader reader)
        {
            string rawTripType = reader["trip_type"].ToString().Replace("_", " ");
            string cleanTripType = char.ToUpper(rawTripType[0]) + rawTripType.Substring(1);
            string countries = reader["countries"]?.ToString() ?? "Unknown Location";
            string places = reader["places"]?.ToString();
            string dateDisplay = "";
            if (Convert.ToBoolean(reader["is_flexible_date"]))
            {
                string flexMonths = reader["flexible_months"]?.ToString();
                dateDisplay = (string.IsNullOrWhiteSpace(flexMonths) || flexMonths.Equals("Anytime", StringComparison.OrdinalIgnoreCase)) ? "Flexible" : flexMonths;
            }
            else
            {
                string dFrom = reader["date_from"] != DBNull.Value ? Convert.ToDateTime(reader["date_from"]).ToString("dd/MM/yyyy") : "?";
                string dTo = reader["date_to"] != DBNull.Value ? Convert.ToDateTime(reader["date_to"]).ToString("dd/MM/yyyy") : "?";
                dateDisplay = $"{dFrom} - {dTo}";
            }
            return new TripDisplayModel
            {
                TripId = Convert.ToInt32(reader["trip_id"]),
                Title = $"{cleanTripType} in {countries}",
                Subtitle = string.IsNullOrWhiteSpace(places) ? "No specific places added" : places,
                Description = reader["description"]?.ToString(),
                DateDisplay = dateDisplay,
                MaxPeople = Convert.ToInt32(reader["max_people"]),
                JoinedCount = Convert.ToInt32(reader["joined_count"]),
                IsJoined = Convert.ToInt32(reader["is_joined"]) > 0,
                AdminUserId = Convert.ToInt32(reader["admin_id"]),
                AdminName = reader["admin_username"].ToString(),
                AdminProfilePicture = reader["admin_profile_picture"] == DBNull.Value ? "" : reader["admin_profile_picture"].ToString()
            };
        }

        public class TripDisplayModel : System.ComponentModel.INotifyPropertyChanged
        {
            public int TripId { get; set; }
            public string Title { get; set; }
            public string Subtitle { get; set; }
            public string Description { get; set; }
            public string DateDisplay { get; set; }
            public int MaxPeople { get; set; }
            private int joinedCount;
            public int JoinedCount { get => joinedCount; set { joinedCount = value; OnPropertyChanged(nameof(JoinedCount)); OnPropertyChanged(nameof(PeopleCount)); } }
            private bool isJoined;
            public bool IsJoined { get => isJoined; set { isJoined = value; OnPropertyChanged(nameof(IsJoined)); OnPropertyChanged(nameof(JoinButtonText)); OnPropertyChanged(nameof(CanJoin)); } }
            public string PeopleCount => $"{JoinedCount}/{MaxPeople}";
            public string JoinButtonText => IsJoined ? "Joined" : "Join";
            public bool CanJoin => !IsJoined && JoinedCount < MaxPeople;
            public int AdminUserId { get; set; }
            public string AdminName { get; set; }
            public string AdminProfilePicture { get; set; }
            public string AdminProfilePictureFullPath => string.IsNullOrEmpty(AdminProfilePicture) ? null : System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, AdminProfilePicture.Replace("/", "\\"));
            public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
            protected void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            public List<User> JoinedMembers { get; set; } = new List<User>();
        }
    }
}
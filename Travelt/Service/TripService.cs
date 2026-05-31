using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Windows;

namespace Travelt.Service
{
    public class TripService
    {
        private readonly string connection_info = "server=localhost;port=3306;user=root;password=;database=travelt;ConvertZeroDateTime=True";

        public bool SaveNewTrip(
            int userId,
            DateTime? dateFrom,
            DateTime? dateTo,
            bool isFlexible,
            string flexibleMonths,
            int maxPeople,
            string tripType,
            string description,
            bool isPublic,
            ICollection<string> countries,
            ICollection<string> places)
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
                    if (countryIdResult != null)
                    {
                        countryId = Convert.ToInt32(countryIdResult);
                    }
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
                MessageBox.Show("Error saving trip data to database: " + ex.Message, "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }
        public class TripDisplayModel
        {
            public string Title { get; set; }
            public string Subtitle { get; set; }
            public string Description { get; set; }
            public string PeopleCount { get; set; }
            public string DateDisplay { get; set; }
        }
        public List<TripDisplayModel> GetUserTrips(int userId)
        {
            List<TripDisplayModel> userTrips = new List<TripDisplayModel>();

            try
            {
                using var connection = new MySqlConnection(connection_info);
                connection.Open();

                string query = @"
                    SELECT 
                        t.trip_id,
                        t.trip_type, 
                        t.date_from, 
                        t.date_to,
                        t.is_flexible_date, 
                        t.flexible_months,
                        t.description,
                        t.max_people,
                        (SELECT GROUP_CONCAT(c.country_name SEPARATOR ', ') FROM trip_country tc JOIN country c ON tc.country_id = c.country_id WHERE tc.trip_id = t.trip_id) AS countries,
                        (SELECT GROUP_CONCAT(tp.place_name SEPARATOR ', ') FROM trip_place tp WHERE tp.trip_id = t.trip_id) AS places,
                        (SELECT COUNT(*) FROM user_trip ut2 WHERE ut2.trip_id = t.trip_id) AS joined_count
                    FROM trip t
                    JOIN user_trip ut ON t.trip_id = ut.trip_id
                    WHERE ut.user_id = @user_id
                    ORDER BY t.trip_id DESC";

                using var command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@user_id", userId);

                using var reader = command.ExecuteReader();

                while (reader.Read())
                {

                    string rawTripType = reader["trip_type"].ToString().Replace("_", " ");
                    string cleanTripType = char.ToUpper(rawTripType[0]) + rawTripType.Substring(1);

                    string countries = reader["countries"]?.ToString();
                    if (string.IsNullOrEmpty(countries)) countries = "Unknown Location";

                    string formattedTitle = $"{cleanTripType} in {countries}";

                    string places = reader["places"]?.ToString();
                    string formattedSubtitle = string.IsNullOrWhiteSpace(places) ? "No specific places added" : places;
                    string formattedDescription = reader["description"]?.ToString();

                    int joinedCount = Convert.ToInt32(reader["joined_count"]);
                    int maxPeople = Convert.ToInt32(reader["max_people"]);
                    string formattedPeople = $"{joinedCount}/{maxPeople}";

                    string dateDisplay = "";
                    bool isFlexible = Convert.ToBoolean(reader["is_flexible_date"]);

                    if (isFlexible)
                    {
                        string flexMonths = reader["flexible_months"]?.ToString();
                        if (string.IsNullOrWhiteSpace(flexMonths) || flexMonths.Equals("Anytime", StringComparison.OrdinalIgnoreCase))
                        {
                            dateDisplay = "Flexible";
                        }
                        else
                        {
                            dateDisplay = flexMonths;
                        }
                    }
                    else
                    {
                        string dFrom = reader["date_from"] != DBNull.Value ? Convert.ToDateTime(reader["date_from"]).ToString("dd/MM/yyyy") : "?";
                        string dTo = reader["date_to"] != DBNull.Value ? Convert.ToDateTime(reader["date_to"]).ToString("dd/MM/yyyy") : "?";
                        dateDisplay = $"{dFrom} - {dTo}";
                    }

                    // Add to list
                    userTrips.Add(new TripDisplayModel
                    {
                        Title = formattedTitle,
                        Subtitle = formattedSubtitle,
                        Description = formattedDescription,
                        PeopleCount = formattedPeople,
                        DateDisplay = dateDisplay
                    });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading trips: " + ex.Message);
            }

            return userTrips;
        }
    }
}
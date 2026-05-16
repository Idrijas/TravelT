using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TravelT;
using static Travelt.Service.UserService;

namespace Travelt.Service
{
    public class ReportService
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





        public bool CreateReport(int reporterUserId, int reportedUserId, string reason, string description)
        {

            using var connection = database_connection.GetConnection();
            connection.Open();


            using var transaction = connection.BeginTransaction();

            try
            {
                string report_query = @"INSERT INTO report (reason, description, report_date) VALUES (@reason , @description , @report_date); SELECT LAST_INSERT_ID();";

                using var insert_report_to_db = new MySqlCommand(report_query, connection, transaction);

                insert_report_to_db.Parameters.AddWithValue("@reason", reason);
                insert_report_to_db.Parameters.AddWithValue("@description", description);
                insert_report_to_db.Parameters.AddWithValue("@report_date", DateTime.Now);

                int reportId = Convert.ToInt32(insert_report_to_db.ExecuteScalar());


                string report_participant_query = @"INSERT INTO report_participant (report_id, reporter_id, reported_user_id) VALUES (@report_id , @reporter_id , @reported_user_id);";

                using var insert_participant_to_db = new MySqlCommand(report_participant_query, connection, transaction);

                insert_participant_to_db.Parameters.AddWithValue("@report_id", reportId);
                insert_participant_to_db.Parameters.AddWithValue("@reporter_id", reporterUserId);
                insert_participant_to_db.Parameters.AddWithValue("@reported_user_id", reportedUserId);

                insert_participant_to_db.ExecuteNonQuery();

                transaction.Commit();
                return true;


            }
            catch (Exception ex)
            {
                transaction.Rollback();

                MessageBox.Show(
                    "Database error while creating report:\n" + ex.Message,
                    "Report Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );

                return false;
            }


        }



    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MySqlConnector;

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

        public bool Login(string email, string password)
        {

            using var connection = database_connection.GetConnection();
            connection.Open();

            string db_Query = "SELECT COUNT(*) FROM user WHERE email = @email AND password_hash = @password_hash";

            using var db_SqlCommand = new MySqlCommand(db_Query, connection);
            db_SqlCommand.Parameters.AddWithValue("@email", email);
            db_SqlCommand.Parameters.AddWithValue("@password_hash", password);

            int result_count = Convert.ToInt32(db_SqlCommand.ExecuteScalar());
            return result_count > 0;
        }

        public bool Register(string firstName, string lastName, string userName, DateTime dateOfBirth, string email, string password)
        {
            // TEMPORARY: Mock sign up logic for testing UI functionality.
            // TODO: Replace this with a database query (SQL) once backend is connected.

            if (email == "test@test.com" || userName == "admin")
            {
                return false;
            }

            return true;
        }



    }
}

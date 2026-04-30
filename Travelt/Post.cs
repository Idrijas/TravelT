using Dapper; 
using MySqlConnector;
using System;
using System.Collections.Generic; 
using System.Linq;
using System.Windows;
using System.Windows.Shapes;

namespace Travelt.Service
{
    public class PostService
    {
        private readonly string connectstring = "server=localhost;port=3306;database=travelt;uid=root;pwd=;Allow User Variables=true;";

        public List<Post> getallposts()
        {
            using (var connection = new MySqlConnection(connectstring))
            {
                string sql = @"
                    SELECT p.*, u.username, u.profile_picture
                    FROM posts p 
                    JOIN user u ON p.user_id = u.user_id 
                    ORDER BY p.timestamp DESC";

                return connection.Query<Post>(sql).ToList();
            }
        }
        public List<Post> getsearchresults(string search, string choice)
        {
            using (var connection = new MySqlConnection(connectstring))
            {
                string sql = @"
            SELECT p.*, u.username, u.profile_picture 
            FROM posts p 
            JOIN user u ON p.user_id = u.user_id 
            WHERE ";

                if (choice == "People")
                {
                    sql += "u.username LIKE @searchTerm";
                }
                else if (choice == "Trips")
                {
                    sql += "p.trip_id IS NOT NULL AND p.description LIKE @searchTerm";
                }
                else
                {
                    sql += "p.description LIKE @searchTerm";
                }

                sql += " ORDER BY p.timestamp DESC";

                return connection.Query<Post>(sql, new { searchTerm = "%" + search + "%" }).ToList();
            }
        }
    }

    public class Post
    {
        public int PostId { get; set; }
        public int UserId { get; set; }
        public string Username { get; set; }
        public int? TripId { get; set; }
        public string Description { get; set; }
        public DateTime Timestamp { get; set; }
        public string GeneratedTimestamp
        {
            get
            {
                return $"Posted on {Timestamp.ToString("yyyy-MM-dd HH:mm")}";
            }
        }
        public string ImagePath { get; set; }
        public string Profile_Picture { get; set; }
        public string ImageFullPath
        {
            get
            {
                if (string.IsNullOrEmpty(ImagePath)) return null;
                else return pathconverter(ImagePath);
            }
        }
        public string Profile_PictureFullPath
        {
            get
            {
                if (string.IsNullOrEmpty(Profile_Picture)) return null;
                else return pathconverter(Profile_Picture);
            }
        }
        public string pathconverter(string path)
        {
            
            string basedir = System.AppDomain.CurrentDomain.BaseDirectory;

            string convert = path.Replace("/", "\\");

            return System.IO.Path.Combine(basedir, convert);
        }

    }
}

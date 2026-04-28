/*
using MySqlConnector;
using Dapper; 
using System;
using System.Collections.Generic; 
using System.Linq;

namespace Travelt.Service
{
    public class PostService
    {
        private readonly string connectstring = "server=localhost;port=3308;database=travelt;uid=root;pwd=;Allow User Variables=true;";

        public List<Post> getallposts()
        {
            using (var connection = new MySqlConnection(connectstring))
            {
                string sql = @"
                    SELECT p.*, u.username 
                    FROM posts p 
                    JOIN user u ON p.user_id = u.user_id 
                    WHERE p.description LIKE @search OR u.username LIKE @search
                    ORDER BY p.timestamp DESC";

                return connection.Query<Post>(sql).ToList();
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
        public string ImagePath { get; set; }
    }
}
*/
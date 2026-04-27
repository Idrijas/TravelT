using MySqlConnector;
using System;

public class Post
{
    public int PostId { get; set; }
    public int UserId { get; set; }
    public int TripId { get; set; }
    public string Description { get; set; }
    public DateTime Timestamp { get; set; }
    public string ImagePath { get; set; }
}
public List<Post> GetAllPosts()
{
    using (var connection = new MySqlConnection(yourConnectionString))
    {
        return connection.Query<Post>("SELECT * FROM Posts").ToList();
    }
}

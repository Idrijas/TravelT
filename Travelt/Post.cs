using Dapper; 
using MySqlConnector;
using System;
using System.Collections.Generic; 
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Shapes;
using System.Xml.Linq;
using static Travelt.Service.UserService;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Travelt.Service
{
    public class PostService
    {
        private readonly string connectstring = "server=localhost;port=3306;database=travelt;uid=root;pwd=;Allow User Variables=true;";
        public PostService()
        {
            Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
        }
        public List<Post> getallposts(int currentUserId)
        {
            using (var connection = new MySqlConnection(connectstring))
            {
                string sql = @"
            SELECT p.*, u.username, u.profile_picture,
                (SELECT COUNT(*) FROM post_likes WHERE post_id = p.post_id) AS LikeCount,
                (SELECT COUNT(*) FROM post_comments WHERE post_id = p.post_id) AS CommentCount,
                EXISTS(SELECT 1 FROM post_likes WHERE post_id = p.post_id AND user_id = @userId) AS IsLikedByMe
            FROM posts p 
            JOIN user u ON p.user_id = u.user_id 
            ORDER BY p.timestamp DESC";

                return connection.Query<Post>(sql, new { userId = currentUserId }).ToList();
            }
        }
        public List<Post> getsearchresults(string search, int currentUserId)
        {
            using (var connection = new MySqlConnection(connectstring))
            {
                string sql = @"
            SELECT p.*, u.username, u.profile_picture,
                (SELECT COUNT(*) FROM post_likes WHERE post_id = p.post_id) AS LikeCount,
                (SELECT COUNT(*) FROM post_comments WHERE post_id = p.post_id) AS CommentCount,
                EXISTS(SELECT 1 FROM post_likes WHERE post_id = p.post_id AND user_id = @userId) AS IsLikedByMe
            FROM posts p 
            JOIN user u ON p.user_id = u.user_id 
            WHERE p.description LIKE @searchTerm
            ORDER BY p.timestamp DESC";

                return connection.Query<Post>(sql, new
                {
                    searchTerm = "%" + search + "%",
                    userId = currentUserId
                }).ToList();
            }
        }
        public void ToggleLike(int postId, int userId)
        {
            using (var connection = new MySqlConnection(connectstring))
            {
                string sql = @"
            INSERT IGNORE INTO post_likes (post_id, user_id) VALUES (@pId, @uId);
            IF ROW_COUNT() = 0 THEN
                DELETE FROM post_likes WHERE post_id = @pId AND user_id = @uId;
            END IF;";
                connection.Execute(sql, new { pId = postId, uId = userId });
            }
        }
        public void AddComment(int postId, int userId, string text)
        {
            using (var connection = new MySqlConnection(connectstring))
            {
                string sql = "INSERT INTO post_comments (post_id, user_id, comment_text, created_at) VALUES (@pId, @uId, @txt, NOW())";
                connection.Execute(sql, new { pId = postId, uId = userId, txt = text });
            }
        }

        public string DeletePost(int postId, int userId)
        {
            try
            {
                using (var connection = new MySqlConnection(connectstring))
                {
                    connection.Open();
                    using (var transaction = connection.BeginTransaction())
                    {
                        var postRecord = connection.QueryFirstOrDefault(
                            "SELECT post_id, imagepath FROM posts WHERE post_id = @pId AND user_id = @uId",
                            new { pId = postId, uId = userId },
                            transaction);

                        if (postRecord == null)
                        {
                            return "Database Error: Post not found or you are not the owner.";
                        }

                        string imagePath = postRecord.imagepath?.ToString();

                        connection.Execute("DELETE FROM post_likes WHERE post_id = @pId", new { pId = postId }, transaction);
                        connection.Execute("DELETE FROM post_comments WHERE post_id = @pId", new { pId = postId }, transaction);

                        connection.Execute("DELETE FROM posts WHERE post_id = @pId", new { pId = postId }, transaction);

                        transaction.Commit();

                        if (!string.IsNullOrEmpty(imagePath))
                        {
                            try
                            {
                                string fullPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, imagePath.Replace("/", "\\"));
                                if (System.IO.File.Exists(fullPath))
                                {
                                    System.IO.File.Delete(fullPath);
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("Warning: Could not delete physical file: " + ex.Message);
                            }
                        }

                        return "Success";
                    }
                }
            }
            catch (MySqlException sqlEx)
            {
                return "MySQL Error: " + sqlEx.Message;
            }
            catch (Exception ex)
            {
                return "System Error: " + ex.Message;
            }
        }
        public List<Comment> GetCommentsForPost(int postId)
        {
            using (var connection = new MySqlConnection(connectstring))
            {
                string sql = @"
            SELECT c.comment_text, u.username, u.profile_picture 
            FROM post_comments c 
            JOIN user u ON c.user_id = u.user_id 
            WHERE c.post_id = @pId 
            ORDER BY c.created_at ASC";
                return connection.Query<Comment>(sql, new { pId = postId }).ToList();
            }
        }
        public bool CreateNewPost(int userId, string imagePath, string description)
        {
            try
            {
                string fileName = System.IO.Path.GetFileName(imagePath);

                string imagesFolder = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"Images");

                if (!System.IO.Directory.Exists(imagesFolder))
                {
                    System.IO.Directory.CreateDirectory(imagesFolder);
                }

                string destinationPath = System.IO.Path.Combine(imagesFolder, fileName);

                System.IO.File.Copy(imagePath, destinationPath, true);

                string extractedImagePath = "Images/" + fileName;

                using (var connection = new MySqlConnection(connectstring))
                {
                    string sql = @"INSERT INTO posts (user_id, description, timestamp, imagepath)
                           VALUES (@uId, @desc, NOW(), @img)";

                    int rowsAffected = connection.Execute(sql, new
                    {
                        uId = userId,
                        desc = description,
                        img = extractedImagePath
                    });

                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error creating post: " + ex.Message);
                return false;
            }
        }
    }

    public class Post : INotifyPropertyChanged
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
        public ImageSource ImageFullPath
        {
            get
            {
                if (string.IsNullOrEmpty(ImagePath)) return null;
                return LoadImageWithoutLocking(pathconverter(ImagePath));
            }
        }

        public ImageSource Profile_PictureFullPath
        {
            get
            {
                if (string.IsNullOrEmpty(Profile_Picture)) return null;
                return LoadImageWithoutLocking(pathconverter(Profile_Picture));
            }
        }

        private ImageSource LoadImageWithoutLocking(string path)
        {
            if (!System.IO.File.Exists(path)) return null;

            try
            {
                var bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.UriSource = new Uri(path);
                bitmap.EndInit();
                bitmap.Freeze();
                return bitmap;
            }
            catch
            {
                return null;
            }
        }
        private int _likeCount;
        public int LikeCount
        {
            get => _likeCount;
            set { _likeCount = value; OnPropertyChanged(); }
        }
        private bool _isLikedByMe;
        public bool IsLikedByMe
        {
            get => _isLikedByMe;
            set { _isLikedByMe = value; OnPropertyChanged(); OnPropertyChanged(nameof(LikeIconColor)); }
        }
        public string LikeIconColor => IsLikedByMe ? "#FF2D8C" : "Gray";
        private int _commentCount;
        public int CommentCount
        {
            get => _commentCount;
            set { _commentCount = value; OnPropertyChanged(); }
        }
        private bool _isCommentsExpanded;
        public bool IsCommentsExpanded
        {
            get => _isCommentsExpanded;
            set { _isCommentsExpanded = value; OnPropertyChanged(); OnPropertyChanged(nameof(CommentsVisibility)); }
        }
        public Visibility CommentsVisibility => IsCommentsExpanded ? Visibility.Visible : Visibility.Collapsed;
        public Post()
        {
            Comments = new ObservableCollection<Comment>();
        }
        public ObservableCollection<Comment> Comments { get; set; } = new ObservableCollection<Comment>();
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        public string pathconverter(string path)
        {
            
            string basedir = System.AppDomain.CurrentDomain.BaseDirectory;

            string convert = path.Replace("/", "\\");

            return System.IO.Path.Combine(basedir, convert);
        }
    }
    public class Comment
    {
        public string Username { get; set; }
        public string Comment_Text { get; set; }
        public string Profile_Picture { get; set; }
    }
}

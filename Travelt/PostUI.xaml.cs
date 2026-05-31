using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Travelt.Service;
using TravelT;

namespace Travelt
{
    public partial class PostUI : UserControl
    {
        private readonly PostService _postService = new PostService();

        public PostUI()
        {
            InitializeComponent();
        }

        private void CommentToggle_Click(object sender, RoutedEventArgs e)
        {
            if (this.DataContext is Post post)
            {
                post.IsCommentsExpanded = !post.IsCommentsExpanded;

                if (post.IsCommentsExpanded && post.Comments.Count == 0)
                {
                    var comments = _postService.GetCommentsForPost(post.PostId);
                    foreach (var c in comments)
                    {
                        post.Comments.Add(c);
                    }
                }
            }
        }

        private void Like_Click(object sender, RoutedEventArgs e)
        {
            if (this.DataContext is Post post)
            {
                int userId = 1;

                _postService.ToggleLike(post.PostId, userId);

                if (post.IsLikedByMe)
                {
                    post.LikeCount--;
                    post.IsLikedByMe = false;
                }
                else
                {
                    post.LikeCount++;
                    post.IsLikedByMe = true;
                }
            }
        }

        private void AddComment_Click(object sender, RoutedEventArgs e)
        {
            if (this.DataContext is Post post)
            {
                string text = CommentInput.Text;
                if (string.IsNullOrWhiteSpace(text)) return;

                try
                {
                    _postService.AddComment(post.PostId, 1, text);
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        post.Comments.Add(new Comment
                        {
                            Username = "fatpeterrealistic",
                            Comment_Text = text,
                            Profile_Picture = post.pathconverter("Images/peter_profilepic.jpg")
                        });

                        post.CommentCount++;
                        CommentInput.Clear();
                    });
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Database Error: {ex.Message}");
                }
            }
        }
        private void CommentInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                AddComment_Click(this, new RoutedEventArgs());
            }
        }
        private void Username_Click(object sender, RoutedEventArgs e)
        {
            if (this.DataContext is Post clickedPost)
            {
                ViewUserWindow viewuserwindow = new ViewUserWindow(
                    clickedPost.UserId,
                    false,
                    UserService.CurrentUser.UserId
                );

                viewuserwindow.Show();
            }
        
        }
        public event EventHandler<Post> PostDeleted;
        private void DeletePost_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is Post post)
            {
                if (post.UserId != UserService.CurrentUser.UserId)
                {
                    MessageBox.Show("You can only delete your own posts.");
                    return;
                }

                var result = MessageBox.Show("Are you sure you want to delete this post?", "Confirm Delete", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    PostService service = new PostService();

                    string resultMessage = service.DeletePost(post.PostId, UserService.CurrentUser.UserId);

                    if (resultMessage == "Success")
                    {
                        MessageBox.Show("Post deleted successfully. Refresh to confirm.");
                        PostDeleted?.Invoke(this, post);
                    }
                    else
                    {
                        MessageBox.Show(resultMessage, "Delete Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is Post post)
            {
                if (post.UserId == UserService.CurrentUser.UserId)
                {
                    DeleteButton.Visibility = Visibility.Visible;
                }
                else
                {
                    DeleteButton.Visibility = Visibility.Collapsed;
                }
            }
        }
    }
}
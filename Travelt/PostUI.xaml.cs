using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Travelt.Service;

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
    }
}
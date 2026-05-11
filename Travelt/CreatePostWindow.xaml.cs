using Microsoft.Win32;
using System;
using System.Windows;
using System.Windows.Media.Imaging;
using Travelt.Service;

namespace Travelt
{
    public partial class CreatePostWindow : Window
    {
        private readonly PostService postService = new PostService();
        private string selectedImagePath = "";

        public CreatePostWindow()
        {
            InitializeComponent();
        }

        private void SelectImage_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;";

            if (openFileDialog.ShowDialog() == true)
            {
                selectedImagePath = openFileDialog.FileName;

                ImagePreview.Source = new BitmapImage(new Uri(selectedImagePath));
                PlaceholderUI.Visibility = Visibility.Collapsed;
            }
        }

        private void Post_Click(object sender, RoutedEventArgs e)
        {
            string caption = CaptionInput.Text;

            if (string.IsNullOrEmpty(selectedImagePath))
            {
                MessageBox.Show("Please select an image for your post.", "Missing Image", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (UserService.CurrentUser == null)
            {
                MessageBox.Show("Developer Error: CurrentUser is null. Please log in first!", "Not Logged In", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            int currentUserId = UserService.CurrentUser.UserId;

            bool success = postService.CreateNewPost(currentUserId, selectedImagePath, caption);

            if (success)
            {
                MessageBox.Show("Post created successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                try
                {
                    DiscoveryPageWindow discoveryPage = new DiscoveryPageWindow();
                    discoveryPage.Show();
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Crash prevented! Error loading the feed: " + ex.Message, "Fatal Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Something went wrong while creating the post. Check your database column names!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DiscoveryPageWindow discoveryPage = new DiscoveryPageWindow();
            discoveryPage.Show();
            this.Close();
        }
        private void CaptionInput_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            int currentLength = CaptionInput.Text.Length;
            CharCountText.Text = $"{currentLength}/255";

            if (currentLength >= 255)
            {
                CharCountText.Foreground = System.Windows.Media.Brushes.Red;
            }
            else
            {
                CharCountText.Foreground = System.Windows.Media.Brushes.DarkGray;
            }
        }
    }
}
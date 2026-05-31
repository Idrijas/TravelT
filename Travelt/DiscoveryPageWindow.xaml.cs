using System;
using System.Collections.Generic;
using System.Runtime;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Travelt.Service;
using TravelT;

namespace Travelt
{
    public partial class DiscoveryPageWindow : Window
    {
        private readonly PostService postservice = new PostService();

        int currentUserId = UserService.CurrentUser.UserId;

        public DiscoveryPageWindow()
        {
            InitializeComponent();
            loadposts();
        }

        private void loadposts()
        {
            // Only try to update the UI if the UI has actually finished loading
            if (PostsFeed != null)
            {
                List<Post> posts = postservice.getallposts(currentUserId);
                PostsFeed.ItemsSource = posts;
            }
        }

        private void HideAllFeeds()
        {
            if (PostsFeed != null) PostsFeed.Visibility = Visibility.Collapsed;
            if (PeopleFeed != null) PeopleFeed.Visibility = Visibility.Collapsed;
            if (TripsFeed != null) TripsFeed.Visibility = Visibility.Collapsed;
            if (noresultslabel != null) noresultslabel.Visibility = Visibility.Collapsed;
        }

        private void PostsToggle_Checked(object sender, RoutedEventArgs e)
        {
            if (SearchBox != null) SearchBox.Text = "";
            HideAllFeeds();
            if (PostsFeed != null) PostsFeed.Visibility = Visibility.Visible;

            loadposts();
        }

        private void PeopleToggle_Checked(object sender, RoutedEventArgs e)
        {
            if (SearchBox != null) SearchBox.Text = "";
            HideAllFeeds();
            if (PeopleFeed != null) PeopleFeed.Visibility = Visibility.Visible;

            UserService userService = new UserService();
            PeopleFeed.ItemsSource = userService.GetNewestUsers();
        }

        private void TripsToggle_Checked(object sender, RoutedEventArgs e)
        {
            if (SearchBox != null) SearchBox.Text = "";
            HideAllFeeds();
            if (TripsFeed != null) TripsFeed.Visibility = Visibility.Visible;

            TripService tripService = new TripService();
            TripsFeed.ItemsSource = tripService.GetNewestPublicTrips();
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            string query = SearchBox.Text.Trim();

            if (string.IsNullOrWhiteSpace(query))
            {
                if (PostsToggle.IsChecked == true) PostsToggle_Checked(null, null);
                if (PeopleToggle.IsChecked == true) PeopleToggle_Checked(null, null);
                if (TripsToggle.IsChecked == true) TripsToggle_Checked(null, null);
                return;
            }

            if (PostsToggle.IsChecked == true)
            {
                var results = postservice.getsearchresults(query, currentUserId);
                PostsFeed.ItemsSource = results;
                noresultslabel.Visibility = results.Count == 0 ? Visibility.Visible : Visibility.Collapsed;
            }
            else if (PeopleToggle.IsChecked == true)
            {
                UserService userService = new UserService();
                var results = userService.SearchUsers(query);
                PeopleFeed.ItemsSource = results;
                noresultslabel.Visibility = results.Count == 0 ? Visibility.Visible : Visibility.Collapsed;
            }
            else if (TripsToggle.IsChecked == true)
            {
                TripService tripService = new TripService();
                var results = tripService.SearchPublicTrips(query);
                TripsFeed.ItemsSource = results;
                noresultslabel.Visibility = results.Count == 0 ? Visibility.Visible : Visibility.Collapsed;
            }
        }
        private void SearchBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SearchButton_Click(this, new RoutedEventArgs());
            }
        }
        private void ToHomePage_Button(object sender, RoutedEventArgs e)
        {
            HomePageWindow homePage = new HomePageWindow();
            homePage.Show();
            this.Close();
        }

        private void ToProfile_Button(object sender, RoutedEventArgs e)
        {

            ProfilePageWindow profilepagewindow = new ProfilePageWindow(currentUserId);
            profilepagewindow.Show();

            this.Close();


        } 

        private void OpenCreatePost_Click(object sender, RoutedEventArgs e)
        {
            CreatePostWindow createPostWin = new CreatePostWindow();
            createPostWin.Show();
            this.Close();
        }
        private void PersonCard_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.DataContext is User clickedUser)
            {
                bool isAdmin = UserService.CurrentUser.Role?.Equals("admin", StringComparison.OrdinalIgnoreCase) ?? false;

                ViewUserWindow viewUserWin = new ViewUserWindow(clickedUser.UserId, isAdmin, currentUserId);

                viewUserWin.Show();
            }
        }

    }
}

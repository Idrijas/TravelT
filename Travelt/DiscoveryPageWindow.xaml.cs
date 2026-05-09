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
            List<Post> posts = postservice.getallposts(currentUserId);
            DiscoverFeed.ItemsSource = posts;
        }

        private void search_click(object sender, RoutedEventArgs e)
        {
            noresultslabel.Visibility = Visibility.Collapsed;

            string search = SearchBox.Text;

            if (searchvalues.SelectedItem is ComboBoxItem selecteditem)
            {
                string choicestring = selecteditem.Content.ToString();

                var results = postservice.getsearchresults(search, choicestring, currentUserId);

                if (results == null || results.Count == 0)
                {
                    noresultslabel.Visibility = Visibility.Visible;
                    DiscoverFeed.ItemsSource = null;
                }
                else
                {
                    DiscoverFeed.ItemsSource = results;
                }
            }
        }
        private void SearchBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                search_click(this, new RoutedEventArgs());
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
            ProfilePageWindow profilePage = new ProfilePageWindow();
            profilePage.Show();
            this.Close();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Travelt.Service;

namespace Travelt
{
    public partial class DiscoveryPageWindow : Window
    {
        private readonly PostService postservice = new PostService();

        // Temporary hardcoded ID for the logged-in user
        // In the future, this will come from Login
        private int currentUserId = 1;

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
    }
}

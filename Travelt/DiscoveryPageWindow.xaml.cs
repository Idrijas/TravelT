using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Travelt.Service;

namespace Travelt
{
    public partial class DiscoveryPageWindow : Window
    {
        private readonly PostService postservice = new PostService();

        public DiscoveryPageWindow()
        {
            InitializeComponent();
            loadposts();
        }

        private void loadposts()
        {
            List<Post> posts = postservice.getallposts();
            DiscoverFeed.ItemsSource = posts;
        }

        private void search_click(object sender, RoutedEventArgs e)
        {
            string search = SearchBox.Text;

            ComboBoxItem selecteditem = (ComboBoxItem)searchvalues.SelectedItem;

            var choice = selecteditem.Content;
            string choicestring = choice.ToString();

            var results = postservice.getsearchresults(search, choicestring);

            if (results == null || results.Count == 0)
            {
                noresultslabel.Visibility = Visibility.Visible;
                DiscoverFeed.ItemsSource = null;
            }
            else
            {
                noresultslabel.Visibility = Visibility.Collapsed;
                DiscoverFeed.ItemsSource = results;
            }

                
        }
    }
}

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

    using Travelt.Service; 

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
            // Fetch all posts from XAMPP
            List<Post> posts = postservice.getallposts();

            // Push the list into the ItemsControl named "DiscoverFeed"
            DiscoverFeed.ItemsSource = posts;
        }
    }
}

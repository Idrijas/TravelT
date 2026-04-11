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
using System.Windows.Threading;

namespace Travelt
{
    /// <summary>
    /// Interaction logic for HomeWindow.xaml
    /// </summary>
    public partial class HomePageWindow : Window
    {

        private DispatcherTimer timer;

        private void StartClock()
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Ticking;
            timer.Start();


            DateTimeTextBlock.Text = DateTime.Now.ToString("dd.MM.yyyy  |  HH:mm:ss");

        }

        private void Ticking (object sender, EventArgs e)
        {
            DateTimeTextBlock.Text = DateTime.Now.ToString("dd.MM.yyyy  |  HH:mm:ss");
        }
        
        public HomePageWindow()
        {

            InitializeComponent();
            StartClock();
        }

        public HomePageWindow(string username)
        {
            InitializeComponent();
            WelcomeTextBlock.Text = $"Welcome, {username}";
            StartClock();
        }

        private void DiscoverButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Discover page will be added later :)");
        }

        private void PlanningButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Planning page will be added later :)");
        }

        private void ProfileButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Profile page will be added when Ondrej decides :}");
        }




      


    }
}

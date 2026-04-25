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
using TravelT;

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


            DateTimeTextBlock.Text = DateTime.Now.ToString("TODAY IS:  dd.MM.yyyy  |  IT IS:  HH:mm:ss");

        }

        private void Ticking (object sender, EventArgs e)
        {
            DateTimeTextBlock.Text = DateTime.Now.ToString("TODAY IS:  dd.MM.yyyy  |  IT IS:  HH:mm:ss");
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

        private void ToDiscoverPage(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Discover page will be added later :)");
        }

        private void ToPlanningPage(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Planning page will be added later :)");
        }

        private void ToProfilePage(object sender, RoutedEventArgs e)
        {

            ProfilePageWindow profilepagewindow = new ProfilePageWindow();
            profilepagewindow.Show();

            this.Close();

            MessageBox.Show("Ondrej decided to finally add Profile Page");
        }


        private void LogOut(object sender, RoutedEventArgs e)
        {
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show();

            this.Close();
        }







    }
}

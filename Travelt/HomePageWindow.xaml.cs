using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
using Travelt.Service;
using System.Net.Http;
using System.Text.Json;
using TravelT;
using static Travelt.Service.UserService;

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
            LoadQuote();

            if (UserService.CurrentUser != null)
            {
                WelcomeTextBlock.Text = $"Welcome, {UserService.CurrentUser.Username}";
            }

            if (CurrentUser.Role == "admin")
            {
                AdminButtonName.Visibility = Visibility.Visible;
            }
            else
            {
                AdminButtonName.Visibility = Visibility.Collapsed;
            }


            StartClock();
        }




        private async void LoadQuote()
        {
            try
            {
                using HttpClient client = new HttpClient();

                string json = await client.GetStringAsync("https://dummyjson.com/quotes/random");

                QuoteResult quote = JsonSerializer.Deserialize<QuoteResult>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true});

                Quote.Text = $"\"{quote.Quote}\"\n- {quote.Author}";


            }
            catch
            {
                Quote.Text = "“Stay consistent. It will pay off.”";
            }
        }

        public class QuoteResult
        {
            public string Quote { get; set; }
            public string Author { get; set; }
        }


        

        private void ToDiscoverPage(object sender, RoutedEventArgs e)
        {
            DiscoveryPageWindow discoverypagewindow = new DiscoveryPageWindow();
            discoverypagewindow.Show();

            this.Close();
        }

        private void ToPlanningPage(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Planning page will be added later :)");
        }

        private void ToProfilePage(object sender, RoutedEventArgs e)
        {
            ProfilePageWindow profilepagewindow = new ProfilePageWindow(UserService.CurrentUser.UserId);
            profilepagewindow.Show();

            this.Close();
        }


        private void LogOut(object sender, RoutedEventArgs e)
        {
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show();

            this.Close();
        }

        private void AdminButton(object sender, RoutedEventArgs e)
        {
            AdminToolboxWindow admintoolboxwindow = new AdminToolboxWindow();
            admintoolboxwindow.Show();

            this.Close();
        }






    }
}

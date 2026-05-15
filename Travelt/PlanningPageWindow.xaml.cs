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

namespace Travelt
{
    public partial class PlanningPageWindow : Window
    {
        public PlanningPageWindow()
        {
            InitializeComponent();
            LoadUserTrips();
        }

        private void LoadUserTrips()
        {
            // TODO: In the future, we will call TripService.GetUserTrips(UserService.CurrentUser.UserId)
            // and set TripsList.ItemsSource to those results!
        }

        private void AddTrip_Click(object sender, RoutedEventArgs e)
        {
            // TODO: Open the "Add a Trip" window from your wireframe
            MessageBox.Show("This will open the 'Where to, user?' window!");
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            // Navigate back to the Home Page
            HomePageWindow homePage = new HomePageWindow();
            homePage.Show();
            this.Close();
        }
    }
}

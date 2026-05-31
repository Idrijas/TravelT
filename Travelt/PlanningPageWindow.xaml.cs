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
    public partial class PlanningPageWindow : Window
    {
        public PlanningPageWindow()
        {
            InitializeComponent();
            LoadUserTrips();
        }

        private void LoadUserTrips()
        {
            if (UserService.CurrentUser != null)
            {
                TripService tripService = new TripService();

                var myTrips = tripService.GetUserTrips(UserService.CurrentUser.UserId);

                TripsList.ItemsSource = myTrips;
            }
        }

        private void AddTrip_Click(object sender, RoutedEventArgs e)
        {
            CreatePlanWindow createPlan = new CreatePlanWindow();
            createPlan.Show();
            this.Close();
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            HomePageWindow homePage = new HomePageWindow();
            homePage.Show();
            this.Close();
        }
        private void MemberAvatar_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.DataContext is User clickedMember)
            {
                bool isAdmin = UserService.CurrentUser.Role?.Equals("admin", StringComparison.OrdinalIgnoreCase) ?? false;

                ViewUserWindow viewUserWin = new ViewUserWindow(clickedMember.UserId, isAdmin, UserService.CurrentUser.UserId);
                viewUserWin.Show();
            }
        }

        private void EditTrip_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.DataContext is TripService.TripDisplayModel trip)
            {
                CreatePlanWindow editWin = new CreatePlanWindow(trip.TripId);
                editWin.Show();
                this.Close();
            }
        }

        private void DeleteTrip_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.DataContext is TripService.TripDisplayModel trip)
            {
                var result = MessageBox.Show("Are you sure?", "Delete", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    TripService ts = new TripService();
                    if (ts.DeleteTrip(trip.TripId)) LoadUserTrips();
                }
            }
        }
    }
}

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
using TravelT;
using static Travelt.Service.ReportService;
using static Travelt.Service.TripService;
using static Travelt.Service.UserService;
namespace Travelt
{
    /// <summary>
    /// Interaction logic for AdminToolboxWindow.xaml
    /// </summary>
    public partial class AdminToolboxWindow : Window
    {
        public AdminToolboxWindow()
        {
            InitializeComponent();
        }

        public void ViewUsersButton(object sender, RoutedEventArgs e)
        {
            AdminDataGrid.Columns.Clear();
            AdminDataGrid.AutoGenerateColumns = false;

            AdminDataGrid.Columns.Add(new DataGridTextColumn { Header = "ID", Binding = new Binding("UserId") });
            AdminDataGrid.Columns.Add(new DataGridTextColumn { Header = "Username", Binding = new Binding("Username") });
            AdminDataGrid.Columns.Add(new DataGridTextColumn { Header = "Email", Binding = new Binding("Email") });
            AdminDataGrid.Columns.Add(new DataGridTextColumn { Header = "First Name", Binding = new Binding("FirstName") });
            AdminDataGrid.Columns.Add(new DataGridTextColumn { Header = "Last Name", Binding = new Binding("LastName") });
            AdminDataGrid.Columns.Add(new DataGridTextColumn { Header = "Role", Binding = new Binding("Role") });

            UserService userservice = new UserService();
            AdminDataGrid.ItemsSource = userservice.GetAllUsers();
        }
        





        public void EditUsersButton(object sender, RoutedEventArgs e) 
        {
            User selectedUser = AdminDataGrid.SelectedItem as User;

            if (selectedUser == null)
            {
                MessageBox.Show("Select a user to continue");
                return;
            }

            EditUserWindow edituserwindow = new EditUserWindow(selectedUser);
            edituserwindow.ShowDialog();

            AdminDataGrid.ItemsSource = new UserService().GetAllUsers();



        }





        public void ShowUserButton(object sender, RoutedEventArgs e)
        {
            User selectedUser = AdminDataGrid.SelectedItem as User;

            if (selectedUser == null)
            {
                MessageBox.Show("Select user to see his profile");
                return;
            }

            ViewUserWindow viewuserwindow = new ViewUserWindow(
                selectedUser.UserId,
                true,
                UserService.CurrentUser.UserId
            );

            viewuserwindow.ShowDialog();

            AdminDataGrid.ItemsSource = new UserService().GetAllUsers();

        }



        public void ViewUsersReportsButton(object sender, RoutedEventArgs e)
        {

            ReportService reportService = new ReportService();
            AdminDataGrid.Columns.Clear();
            AdminDataGrid.AutoGenerateColumns = true;
            AdminDataGrid.ItemsSource = reportService.GetAllReports();

        }

        public void ShowReportButton(object sender, RoutedEventArgs e)
        {

            Report selectedReport = AdminDataGrid.SelectedItem as Report;

            if (selectedReport == null)
            {
                MessageBox.Show("Select report to see the details");
                return;
            }

            ViewReportWindow viewreportwindow = new ViewReportWindow(selectedReport);
            viewreportwindow.ShowDialog();
        }





        public void DeleteUserButton(object sender, RoutedEventArgs e)
        {

            User selectedUser = AdminDataGrid.SelectedItem as User;

            if (selectedUser == null) 
            {
                MessageBox.Show("Select user");
                return;
            }

            if (selectedUser.Role == "admin")
            {
                MessageBox.Show("You cannot delete Admin");
                return;
            }

            if (selectedUser.UserId == UserService.CurrentUser.UserId)
            {
                MessageBox.Show("You cannot delete yourself :)");
                return;
            }

            MessageBoxResult choice_confirm = MessageBox.Show($"Are you sure you want to delete user {selectedUser.Username}?", "Confirm this action", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (choice_confirm == MessageBoxResult.Yes) 
            {
                UserService userservice = new UserService();

                bool deleted_user = userservice.AdminDeleteUser(selectedUser.UserId);

                if (deleted_user) 
                {
                    MessageBox.Show("Almighty Admin deleted user!");
                    AdminDataGrid.ItemsSource = userservice.GetAllUsers();

                }
                else
                {
                    MessageBox.Show("Not even admin has the power to remove this account");
                }
            }
        }





        public void ViewTripsButton(object sender, RoutedEventArgs e)
        {
            AdminDataGrid.Columns.Clear();
            AdminDataGrid.AutoGenerateColumns = false;
            AdminDataGrid.Columns.Add(new DataGridTextColumn
            {
                Header = "Id",
                Binding = new Binding("TripId")
            });
            AdminDataGrid.Columns.Add(new DataGridTextColumn
            {
                Header = "Type",
                Binding = new Binding("TripType")
            });
            AdminDataGrid.Columns.Add(new DataGridTextColumn
            {
                Header = "Description",
                Binding = new Binding("Description")
            });
            AdminDataGrid.Columns.Add(new DataGridTextColumn
            {
                Header = "People",
                Binding = new Binding("MaxPeople")
            });
            AdminDataGrid.Columns.Add(new DataGridTextColumn
            {
                Header = "Status",
                Binding = new Binding("Status")
            });
            TripService tripsservice = new TripService();
            AdminDataGrid.ItemsSource = tripsservice.GetAllTrips();
        }



        public void DeleteTripButton(object sender, RoutedEventArgs e)
        {
            AdminTripModel selectedtrip = AdminDataGrid.SelectedItem as AdminTripModel;

            if (selectedtrip == null)
            {
                MessageBox.Show("Select trip you wish to delete:");
                return;
            }
            MessageBoxResult choice_confirm = MessageBox.Show(
                $"Are you REALLY 100% sure you want to delete trip {selectedtrip.TripId}?",
                  "Confirm this action",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);
            if (choice_confirm == MessageBoxResult.Yes)
            {
                TripService tripservice = new TripService();
                bool deleted_trip = tripservice.AdminDeleteTrip(selectedtrip.TripId);

                if (deleted_trip)
                {
                    MessageBox.Show("Trip deleted successfully");
                    AdminDataGrid.ItemsSource = tripservice.GetAllTrips();
                }
                else
                {
                    MessageBox.Show("Ooops, trip cannot be deleted");
                }

            }


        }


        private void ExitButton(object sender, RoutedEventArgs e)
        {
            HomePageWindow homepagewindow = new HomePageWindow();
            homepagewindow.Show();

            this.Close();
        }


        private void ClearButton(object sender, RoutedEventArgs e)
        {
            AdminDataGrid.ItemsSource = null;
        }
    }
}

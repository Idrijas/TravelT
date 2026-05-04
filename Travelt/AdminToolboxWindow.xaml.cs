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
using TravelT;
using Travelt.Service;
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





        public void ViewUsersReportsButton(object sender, RoutedEventArgs e)
        {

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

        }


        public void EditTripsButton(object sender, RoutedEventArgs e)
        {

        }


        public void ViewTripsReportsButton(object sender, RoutedEventArgs e)
        {

        }


        public void DeleteTripButton(object sender, RoutedEventArgs e)
        {

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

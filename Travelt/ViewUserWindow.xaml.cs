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
    /// Interaction logic for ViewUserWindow.xaml
    /// </summary>
    public partial class ViewUserWindow : Window
    {

        private int selectedUserId;
        private bool isAdminView;
        public ViewUserWindow(int userId, bool isAdmin)
        {
            InitializeComponent();

            selectedUserId = userId;
            isAdminView = isAdmin;

            LoadUserData();

            EditUserButton.Visibility= isAdminView ? Visibility.Visible : Visibility.Collapsed;

        }

        private void LoadUserData()
        {
            UserService userService = new UserService();
            User selectedUser = userService.GetUserById(selectedUserId);

            if (selectedUser == null)
            {
                MessageBox.Show("Something went wrong and user cannot be loaded");
                return;
            }



            PageHeader.Text = selectedUser.Username;
            Username_Text.Text = selectedUser.Username;
            Age_Text.Text = $"Age: {UserAge_Calc(selectedUser.DateOfBirth)}";
            Rank_Text.Text = "To be added";
            Rating_Text.Text = "To be added";
            BioBlock.Text = string.IsNullOrWhiteSpace(selectedUser.Bio) ? "No Bio Yet" : selectedUser.Bio;
                    

        }





        private int UserAge_Calc(DateTime birthDate)
        {
            int userAge = DateTime.Now.Year - birthDate.Year;

            if(DateTime.Now.Date < birthDate.AddYears(userAge))
            {
                userAge--;
            }
            return userAge;
        }



        private void Back_Button(object sender, RoutedEventArgs e)
        {
            this.Close();
        }





        private void ReportUser_Button(object sender, RoutedEventArgs e)
        {
            
        }





        private void EditUser_Button(object sender, RoutedEventArgs e)
        {
            UserService userService = new UserService();
            User selectedUser = userService.GetUserById(selectedUserId);

            if (selectedUser == null)
            {
                MessageBox.Show("User cannot be edited");
                return;
            }

            EditUserWindow edituserwindow = new EditUserWindow(selectedUser);
            edituserwindow.ShowDialog();

            LoadUserData();
        }



    }
}

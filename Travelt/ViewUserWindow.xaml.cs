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
using System.IO;
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
        private int currentUserId;
        private bool isAdminView;
        public ViewUserWindow(int userId, bool isAdmin, int currentUserId)
        {
            InitializeComponent();

            selectedUserId = userId;
            isAdminView = isAdmin;
            this.currentUserId = currentUserId;

            LoadUserData();

            EditUserButton.Visibility= isAdminView ? Visibility.Visible : Visibility.Collapsed;

        }





        private void Load_Picture(string profilepicturepath)
        {
            if (!string.IsNullOrWhiteSpace(profilepicturepath) && File.Exists(profilepicturepath))
            {
                    ProfilePicturePlaceholder.Source = new BitmapImage(
                    new Uri(profilepicturepath, UriKind.Absolute)
                );
            }
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
            Load_Picture(selectedUser.ProfilePicture);  
                    

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
            ReportUserWindow reportuserwindow = new ReportUserWindow(currentUserId, selectedUserId);
            reportuserwindow.ShowDialog();
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

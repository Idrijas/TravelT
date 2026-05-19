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

            Load_Achievements();

            LoadUserData();

            Load_Rank();

            EditUserButton.Visibility= isAdminView ? Visibility.Visible : Visibility.Collapsed;

        }





        private void Load_Picture(string profilepicturepath)
        {
            if (string.IsNullOrWhiteSpace(profilepicturepath))
                return;

            string fullPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, profilepicturepath);

            if (File.Exists(fullPath))
            {
                ProfilePicturePlaceholder.Source = new BitmapImage(
                    new Uri(fullPath, UriKind.Absolute)
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





        private void Load_Achievements()
        {
            UserService userService = new UserService();

            List<AchievementsDisplay> achievementDisplays =
                userService.GetUserAchievements(selectedUserId);

            ViewUserAchievementsBlock.ItemsSource = achievementDisplays;
        }





        private void Load_Rank()
        {
            UserService userservice = new UserService();

            string rank = userservice.GetUserRank(selectedUserId);

            Rank_Text.Text = $"Rank: {rank}";
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





        private void Achievement_Show(object sender, RoutedEventArgs e)
        {
            Button show_button = sender as Button;

            if(show_button?.Tag is AchievementsDisplay achievement)
            {
                AchievementDetailsWindow achievementdetailwindow = new AchievementDetailsWindow(achievement);

                achievementdetailwindow.ShowDialog();


            }


        }



    }
}

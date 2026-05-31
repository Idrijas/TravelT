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
using Travelt;
using Travelt.Service;
using static Travelt.Service.UserService;
using System.Diagnostics;

namespace TravelT
{
    public partial class ProfilePageWindow : Window
    {
        private int profileUserId;
        private readonly UserService _userService = new UserService();
        


        private void Load_Picture(string profilepicturepath)
        {
            if (!string.IsNullOrWhiteSpace(profilepicturepath))
            {
                string full_Path = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, profilepicturepath);

                if (File.Exists(full_Path)) 
                {
                    ProfilePicturePlace.Source = new BitmapImage(new Uri(full_Path, UriKind.Absolute));
                }
            }
                
        }





        private void Load_Achievements()
        {
            UserService userservice = new UserService();
            List<AchievementsDisplay> achievementsDisplays = userservice.GetUserAchievements(CurrentUser.UserId);

            AchievementsBlock.ItemsSource = achievementsDisplays;
        }





        private void Load_Rank()
        {
            UserService userservice = new UserService();

            string rank = userservice.GetUserRank(UserService.CurrentUser.UserId);

            RankButton.Content = $"Rank: {rank}";
        }





        private void Load_Visited_Countries()
        {
            UserService userservice = new UserService();

            int countries_count = userservice.CountVisitedCountries(UserService.CurrentUser.UserId);

            VisitedCountriesButton.Content = $"Countries visited: {countries_count}";
        }





        private void ToHomePage_Button(object sender, RoutedEventArgs e)
        {
            HomePageWindow homePageWindow = new HomePageWindow();
            homePageWindow.Show();
            this.Close();
        }





        private void ToSettings_Button(object sender, RoutedEventArgs e)
        {
            SettingsPageWindow settingsPageWindow = new SettingsPageWindow();
            settingsPageWindow.Show();
            this.Close();
        }





        private void ShowRankInfo(object sender, RoutedEventArgs e)
        {
            RankInfoWindow rankinfowindow = new RankInfoWindow();

            rankinfowindow.ShowDialog();
        }





        private void ShowCountriesInfo(object sender, RoutedEventArgs e)
        {
            VisitedCountriesWindow visitedcountrieswindow = new VisitedCountriesWindow();

            visitedcountrieswindow.ShowDialog();

            Load_Visited_Countries();
        }





        public ProfilePageWindow(int userIdToLoad)
        {
            InitializeComponent();


            profileUserId = userIdToLoad;

            Load_Achievements();

            Load_Rank();

            Load_Visited_Countries();

            if (profileUserId == UserService.CurrentUser.UserId)
            {
                PageTitleText.Text = "Your Profile";
                UsernameBlock.Text = UserService.CurrentUser.Username;
                BioExpander.Text = UserService.CurrentUser.Bio;
                Load_Picture(UserService.CurrentUser.ProfilePicture);   

                int myAge = DateTime.Today.Year - UserService.CurrentUser.DateOfBirth.Year;
                if (UserService.CurrentUser.DateOfBirth.Date > DateTime.Today.AddYears(-myAge)) myAge--;
                AgeBlock.Text = $"Age: {myAge}";

                UserService userService = new UserService();

                string nationality = userService.GetUserNationality(UserService.CurrentUser.UserId);

                NationalityBlock.Text = $"Nationality: {nationality}";
            }
            else
            {
                SettingsButton.Visibility = Visibility.Collapsed;

                User profileUser = _userService.GetUserById(profileUserId);

                if (profileUser != null)
                {
                    PageTitleText.Text = $"{profileUser.Username}'s Profile";
                    UsernameBlock.Text = profileUser.Username;
                    BioExpander.Text = profileUser.Bio;

                    int theirAge = DateTime.Today.Year - profileUser.DateOfBirth.Year;
                    if (profileUser.DateOfBirth.Date > DateTime.Today.AddYears(-theirAge)) theirAge--;
                    AgeBlock.Text = $"Age: {theirAge}";
                }
                else
                {
                    PageTitleText.Text = "User Not Found";
                    UsernameBlock.Text = "Unknown";
                    BioExpander.Text = "No bio available.";
                    AgeBlock.Text = "Age: Unknown";
                }
            }
        }





        private void Achievement_Show(object sender, RoutedEventArgs e)
        {
            Button show_button = sender as Button;

            if (show_button?.Tag is AchievementsDisplay achievement)
            {
                AchievementDetailsWindow achievementdetailwindow = new AchievementDetailsWindow(achievement);

                achievementdetailwindow.ShowDialog();


            }


        }

        private void OpenWorldMap(object sender, RoutedEventArgs e)
        {
            Process.Start(new ProcessStartInfo { FileName = "https://www.openstreetmap.org", UseShellExecute = true });
        }
    }
}

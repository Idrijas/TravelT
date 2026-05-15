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
using Travelt;
using Travelt.Service;
using static Travelt.Service.UserService;

namespace TravelT
{
    public partial class ProfilePageWindow : Window
    {
        private int profileUserId;
        private readonly UserService _userService = new UserService();

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

        private void ChangeProfilePicButton(object sender, RoutedEventArgs e)
        {

        }

        public ProfilePageWindow(int userIdToLoad)
        {
            InitializeComponent();
            profileUserId = userIdToLoad;

            if (profileUserId == UserService.CurrentUser.UserId)
            {
                PageTitleText.Text = "Your Profile";
                UsernameBlock.Text = UserService.CurrentUser.Username;
                BioExpander.Text = UserService.CurrentUser.Bio;

                int myAge = DateTime.Today.Year - UserService.CurrentUser.DateOfBirth.Year;
                if (UserService.CurrentUser.DateOfBirth.Date > DateTime.Today.AddYears(-myAge)) myAge--;
                AgeBlock.Text = $"Age: {myAge}";
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
    }
}

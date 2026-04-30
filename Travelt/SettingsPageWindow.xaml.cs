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
    /// Interaction logic for SettingsPageWindow.xaml
    /// </summary>
    public partial class SettingsPageWindow : Window
    {
        public SettingsPageWindow()
        {
            InitializeComponent();
        }

        private void ChangeUsernameButton(object sender, RoutedEventArgs e)
        {

            string changeUsername = NewUsername.Text;

            UserService userService = new UserService();

            bool successful_edit = userService.ChangeUsername(UserService.CurrentUser.UserId, changeUsername);

            if (string.IsNullOrWhiteSpace(changeUsername))
            {
                MessageBox.Show("Please enter a username");
                return;
            }

            if (successful_edit) 
            {
                UserService.CurrentUser.Username = changeUsername; 
                MessageBox.Show("Username changed successfully");
            }
            else
            {
                MessageBox.Show("Something went wrong");
            }

        }


        private void ChangePasswordButton(object sender, RoutedEventArgs e)
        {

        }

        private void EditBioButton(object sender, RoutedEventArgs e)
        {

            string editBio = EditBio.Text;

            UserService userService = new UserService();

            bool successful_edit = userService.ChangeBio(UserService.CurrentUser.UserId, editBio);

            if (successful_edit)
            {
                UserService.CurrentUser.Bio = editBio; 
                MessageBox.Show("Bio changed successfully");
            }
            else
            {
                MessageBox.Show("Change failed");
                return;
            }
        }


        private void BackToHomePageButton(object sender, RoutedEventArgs e)
        {
            ProfilePageWindow profilePageWindow = new ProfilePageWindow();
            profilePageWindow.Show();

            this.Close();
        }

        private void ChooseProfilePictureButton(object sender, RoutedEventArgs e)
        {

        }


        private void ChangeProfilePictureButton(object sender, RoutedEventArgs e)
        {

        }

        private void DeleteProfileButton(object sender, RoutedEventArgs e)
        {

        }

      
    }
}

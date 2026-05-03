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

            

            string oldpassword = OldPasswordBox.Password;
            string new_password = NewPasswordBox.Password;
            string repeat_password = RepeatNewPasswordBox.Password;

            if (string.IsNullOrWhiteSpace(oldpassword) || string.IsNullOrWhiteSpace(new_password) || string.IsNullOrWhiteSpace(repeat_password))
            {
                MessageBox.Show("Please fill all boxes");
                return;
            }

            if (new_password != repeat_password)
            {
                MessageBox.Show("Passwords don't match");
                return;
            }

            if (new_password == oldpassword)
            {
                MessageBox.Show("New password must be different");
                return;
            }

            UserService userService = new UserService();

            bool successfull_change = userService.ChangePassword(CurrentUser.UserId, oldpassword, new_password);

            if (successfull_change)
            {
                MessageBox.Show("Password changed");

                OldPasswordBox.Clear();
                NewPasswordBox.Clear();
                RepeatNewPasswordBox.Clear();

            }
            else
            {
                MessageBox.Show("Something went wrong");
            }

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
            var delete_confirmation = MessageBox.Show("Do you really want to delete your account? All your data will be lost", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (delete_confirmation != MessageBoxResult.Yes)
            {
                return;
            }

            string password = DeleteUserPasswordBox.Password;

            if (string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Please enter your password to delete profile");
                return;
            }

            UserService userService = new UserService();

            bool successful_delete = userService.DeleteUser(UserService.CurrentUser.UserId, password);

            if (successful_delete)
            {

                MessageBox.Show("Account Deleted :)");

                LoginWindow loginwindow = new LoginWindow();
                loginwindow.Show();

                this.Close();
            }
            else
            {
                MessageBox.Show("Incorrect Password");
                return;
            }

        }

    }
}

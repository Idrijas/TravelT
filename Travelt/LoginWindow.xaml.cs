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
using static Travelt.Service.UserService;

namespace Travelt
{
    /// <summary>
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void LoginButton(object sender, RoutedEventArgs e)
        {
            UserService user = new UserService();

            string email = EmailTextBox.Text;
            string password = ShowPasswordCheckBox.IsChecked == true
                                                ? PasswordTextBox.Text
                                                : PasswordBox.Password;

            User current_logged_user = user.Login(email, password);

            if (current_logged_user != null)
            {
                UserService.CurrentUser = current_logged_user;
                MessageBox.Show("Successful Login");

                HomePageWindow homepagewindow = new HomePageWindow();
                homepagewindow.Show();

                this.Close();
            }
            else
            {
                MessageBox.Show("Invalid email or password");
            }
        }

        private void ShowPasswordCheck(object sender, RoutedEventArgs e)
        {
            PasswordTextBox.Text = PasswordBox.Password;

            PasswordHiddenBorder.Visibility = Visibility.Collapsed;
            PasswordVisibleBorder.Visibility = Visibility.Visible;
        }

        private void ShowPasswordUnCheck(object sender, RoutedEventArgs e)
        {
            PasswordBox.Password = PasswordTextBox.Text;

            PasswordHiddenBorder.Visibility = Visibility.Visible;
            PasswordVisibleBorder.Visibility = Visibility.Collapsed;
        }

        private void ToSignUp(object sender, RoutedEventArgs e)
        {
            SignUpPageWindow signUp = new SignUpPageWindow();
            signUp.Show();

            this.Close();
        }
    }
}
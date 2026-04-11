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


namespace Travelt
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string email = EmailTextBox.Text;
            string password = PasswordTextBox.Password;

            Service.UserService userService = new Service.UserService();

            bool success = userService.Login(email, password);

            if (success)
            {
                MessageBox.Show("Login successful");
            }
            else
            {
                MessageBox.Show("Invalid credentials");
            }
        }


        private void SignUpButton_Click(object sender, RoutedEventArgs e)
        {
            SignUpPageWindow signUp = new SignUpPageWindow();
            signUp.Show();

            this.Close();
        }

       
    }
}

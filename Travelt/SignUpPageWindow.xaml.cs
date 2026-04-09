using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Travelt
{

    public partial class SignUpPageWindow : Window
    {
        public SignUpPageWindow()
        {
            InitializeComponent();
        }


        // Dries, this are the notes:

        // Here will be the SQL query code, this is just for testing
        // Also, the profile picture and Gender Enum will be added

        // Also functionality from Login page -> Sign Up page will be added



        //                       VERY    IMPORTANT

        //           if you want to see login page, you need to go to
        //           App.xaml, and look for: 
        //           StartupUri="SignUpPageWindow.xaml">
        //           And change it regarding if you want to see login page change it to -> StartupUri="LoginWindow.xaml">
        //           If you want Sign Up Window, then it's this: StartupUri="SignUpPageWindow.xaml">

        private void SignUpButton_Click(object sender, RoutedEventArgs e)
        {
            string firstName = FirstNameTextBox.Text;
            string lastName = LastNameTextBox.Text;
            string userName = UsernameTextBox.Text;
            string email = EmailTextBox.Text;
            string password = PasswordBox.Password;
            string repeatPassword = RepeatPasswordBox.Password;

            if (string.IsNullOrWhiteSpace(firstName) ||
                string.IsNullOrWhiteSpace(lastName) ||
                string.IsNullOrWhiteSpace(userName) ||
                string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(password) ||
                string.IsNullOrWhiteSpace(repeatPassword))
            {
                MessageBox.Show("Please fill in all fields.");
                return;
            }

            if (DobDatePicker.SelectedDate == null)
            {
                MessageBox.Show("Please select your date of birth.");
                return;
            }

            if (password != repeatPassword)
            {
                MessageBox.Show("Passwords do not match.");
                return;
            }

            Service.UserService userService = new Service.UserService();

            bool success = userService.Register(
                firstName,
                lastName,
                userName,
                DobDatePicker.SelectedDate.Value,
                email,
                password
            );

            if (success)
            {
                MessageBox.Show("Sign up successful!");
            }
            else
            {
                MessageBox.Show("Username or email already exists.");
            }
        }
    }
}
    

   
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
using MySqlConnector;
using System.Text.RegularExpressions;

namespace Travelt
{

    public partial class SignUpPageWindow : Window
    {
        public SignUpPageWindow()
        {
            InitializeComponent();
        }


        // Dries, this are the notes:

        

        // The profile picture and Gender Enum will be adde
        // Also functionality from Login page -> Sign Up page will be added (idk where or when tho xd)



        //                       VERY    IMPORTANT

        //           if you want to see login page, you need to go to
        //           App.xaml, and look for: 
        //           StartupUri="SignUpPageWindow.xaml">
        //           And change it regarding if you want to see login page change it to -> StartupUri="LoginWindow.xaml">
        //           If you want Sign Up Window, then it's this: StartupUri="SignUpPageWindow.xaml">

        private void SignUp_Button(object sender, RoutedEventArgs e)
        {


            string firstName = FirstNameTextBox.Text;
            string lastName = LastNameTextBox.Text;
            string userName = UsernameTextBox.Text;
            string gender = "";
            string email = EmailTextBox.Text;
            string password = PasswordBox.Password;
            string repeatPassword = RepeatPasswordBox.Password;



            if (!Regex.IsMatch(firstName, @"^[A-Za-z]+$"))
            {
                MessageBox.Show("First name can only cotain valid letters");
                return;
            }
            if(!Regex.IsMatch(lastName, @"^[A-Za-z]+$"))
            {
                MessageBox.Show("Last name can only contain valid letters");
                return;
            }


            // checks if gender is selected (if not null takes the selected item, else returning the function for user to pick a gendr)

            if (GenderSelection.SelectedItem != null)
            {
                gender = (GenderSelection.SelectedItem as ComboBoxItem).Content.ToString();
            }
            else
            {
                MessageBox.Show("Please select your gender");
                return;
            }


            // checks if all the fields that must be filled are filled

            if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName) || string.IsNullOrWhiteSpace(userName) ||
                string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(repeatPassword))
            {
                MessageBox.Show("Please fill in all fields.");
                return;
            }

            if (DateOfBirth.SelectedDate == null)
            {
                MessageBox.Show("Please select date of birth");
                return;
            }
            DateTime dateOfBirth = DateOfBirth.SelectedDate.Value;



            // checks if emaik contains "@" and "." and if it is in correct format

            if (!email.Contains("@") && !email.Contains("."))
            {
                MessageBox.Show("Invalid email");
                return;
            }
            if (email.IndexOf("@") > email.LastIndexOf("."))
            {
                MessageBox.Show("Incorrect format");
                return;
            }



            // checks if the user is older than 16 ( it is the age to use airplanes without supervision ( I think)
            /*
             
             I will try to explain:
            
            first it calculates the age TODAY against the age That was selected in DatePicker (only years)

            When we have calculated years, then it substracts to the picked date

            Then it compares months and days -> todays date with picked date 
              
             And lastly it decides if it is more or less than 16 years of age

            */


            DateTime DoB = DateOfBirth.SelectedDate.Value;
            int age_now = DateTime.Today.Year - DoB.Year;

            if (DoB.Date > DateTime.Today.AddYears(-age_now))
            {
                age_now--;    // if the age is not 16, then lower the actual age by 1
            }
            if (age_now < 16)
            {
                MessageBox.Show("You must be 16 years old to use TravelT (sorry)");
                return;
            }




            Service.UserService userService = new Service.UserService();

            bool success = userService.Register(
                    firstName,
                    lastName,
                    userName,
                    gender,
                    dateOfBirth,
                    email,
                    password
            );

            if (success)
            {
                MessageBox.Show("Sign up successful!");

                // navigation to Home Page

                HomePageWindow homepage = new HomePageWindow(userName);
                homepage.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Username or email already exists.");
            }



           


        }


       
        // transfering to login page

        private void BackToLogin(object sender, RoutedEventArgs e)
        {
            LoginWindow logIn = new LoginWindow();
            logIn.Show();

            this.Close();
        }


       
    }
}
    

   
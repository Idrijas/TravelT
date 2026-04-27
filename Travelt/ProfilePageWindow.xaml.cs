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
        public ProfilePageWindow()
        {
            InitializeComponent();

            if (UserService.CurrentUser != null)
            {
                UsernameBlock.Text = UserService.CurrentUser.Username;
            } 
        }

        private void ToHomePage_Button(object sender, RoutedEventArgs e)
        {
            HomePageWindow homePageWindow = new HomePageWindow();
            homePageWindow.Show();

            this.Close();
        }

        private void ToSettings_Button(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Ondrej will add settings shortly :)");
        }
    }
}

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

namespace Travelt
{
    /// <summary>
    /// Interaction logic for VisitedUserCountriesWindow.xaml
    /// </summary>
    public partial class VisitedUserCountriesWindow : Window
    {

        private int selectedUserId;
        public VisitedUserCountriesWindow(int userId)
        {
            InitializeComponent();

            selectedUserId = userId;

            Load_Visited();

        }





        private void Load_Visited() 
        {
            UserService userservice = new UserService();

            VisitedCountriesListBox_User.ItemsSource = userservice.GetVisitedCountries(selectedUserId);
        }





        private void Back_Button_User(object sender, RoutedEventArgs e) 
        {
            this.Close();
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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
    /// Interaction logic for VisitedCountriesWindow.xaml
    /// </summary>
    public partial class VisitedCountriesWindow : Window
    {
        public VisitedCountriesWindow()
        {
            InitializeComponent();

            Load_Countries();
            Load_Visited();
        }





        private void Load_Countries()
        {
            UserService userservice = new UserService();

            CountrySelection.ItemsSource = userservice.GetAllCountrie();
        }





        private void Load_Visited()
        {
            UserService userservice = new UserService();

            var visited_countries =
                userservice.GetVisitedCountries(UserService.CurrentUser.UserId);

            VisitedCountriesListBox.ItemsSource = visited_countries;
        }





        private void AddCountry_Button(object sender, RoutedEventArgs e)
        {
            if (CountrySelection.SelectedValue == null)
            {
                MessageBox.Show("Select a country to add it.");
                return;
            }

            int id_country = Convert.ToInt32(CountrySelection.SelectedValue);

            UserService userservice = new UserService();

            userservice.AddVisitedCountry(UserService.CurrentUser.UserId, id_country);

            Load_Visited();
        }





        private void DeleteCountry_Button(Object sender, RoutedEventArgs e)
        {
            if (VisitedCountriesListBox.SelectedItem == null)
            {
                MessageBox.Show("Select a country to delete it.");
                return;
            }

            Country selected_country = (Country)VisitedCountriesListBox.SelectedItem;

            UserService userservice = new UserService();

            userservice.DeleteVisitedCountry(UserService.CurrentUser.UserId, selected_country.CountryId);

            Load_Visited();
        }





        private void Back_Button(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}

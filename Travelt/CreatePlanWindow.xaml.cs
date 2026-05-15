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
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using Travelt.Service;

namespace Travelt
{
    public partial class CreatePlanWindow : Window
    {
        public ObservableCollection<string> SelectedCountries { get; set; } = new ObservableCollection<string>();
        public ObservableCollection<string> SelectedPlaces { get; set; } = new ObservableCollection<string>();
        public ObservableCollection<string> SelectedMonths { get; set; } = new ObservableCollection<string>(); // New list for Months!

        public CreatePlanWindow()
        {
            InitializeComponent();

            if (UserService.CurrentUser != null)
            {
                TitleText.Text = $"WHERE TO, {UserService.CurrentUser.Username.ToUpper()}?";
            }

            // Bind all UI lists
            SelectedCountriesList.ItemsSource = SelectedCountries;
            SelectedPlacesList.ItemsSource = SelectedPlaces;
            SelectedMonthsList.ItemsSource = SelectedMonths;

            LoadAllCountries();
            LoadUpcomingMonths();
        }

        // ==========================================
        // DATES LOGIC
        // ==========================================
        private void FlexibleDatesCheck_Click(object sender, RoutedEventArgs e)
        {
            if (FlexibleDatesCheck.IsChecked == true)
            {
                ExactDatesPanel.Visibility = Visibility.Collapsed;
                FlexibleDatesPanel.Visibility = Visibility.Visible;
            }
            else
            {
                ExactDatesPanel.Visibility = Visibility.Visible;
                FlexibleDatesPanel.Visibility = Visibility.Collapsed;
            }
        }

        private void AnytimeCheck_Click(object sender, RoutedEventArgs e)
        {
            // If they can go anytime, hide the specific month selector!
            if (AnytimeCheck.IsChecked == true)
            {
                MonthSelectionPanel.Visibility = Visibility.Collapsed;
                SelectedMonths.Clear(); // Clear any months they accidentally picked
            }
            else
            {
                MonthSelectionPanel.Visibility = Visibility.Visible;
            }
        }

        private void LoadUpcomingMonths()
        {
            List<string> months = new List<string>();
            DateTime currentMonth = DateTime.Now;

            // Generate the next 24 months
            for (int i = 0; i < 24; i++)
            {
                months.Add(currentMonth.ToString("MMMM yyyy"));
                currentMonth = currentMonth.AddMonths(1);
            }
            MonthSearchBox.ItemsSource = months;
        }

        private void MonthSearchBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                string chosenMonth = MonthSearchBox.Text.Trim();
                if (!string.IsNullOrWhiteSpace(chosenMonth) && !SelectedMonths.Contains(chosenMonth))
                {
                    SelectedMonths.Add(chosenMonth);
                    MonthSearchBox.Text = "";
                }
            }
        }

        private void RemoveMonth_Click(object sender, RoutedEventArgs e)
        {
            Button clickedButton = sender as Button;
            if (clickedButton?.DataContext is string monthToRemove)
            {
                SelectedMonths.Remove(monthToRemove);
            }
        }

        // ==========================================
        // COUNTRIES LOGIC
        // ==========================================
        private void LoadAllCountries()
        {
            List<string> countryList = new List<string>();
            foreach (CultureInfo cul in CultureInfo.GetCultures(CultureTypes.SpecificCultures))
            {
                RegionInfo region = new RegionInfo(cul.Name);
                if (!countryList.Contains(region.EnglishName))
                {
                    countryList.Add(region.EnglishName);
                }
            }
            countryList.Sort();
            CountrySearchBox.ItemsSource = countryList;
        }

        private void CountrySearchBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                string chosenCountry = CountrySearchBox.Text.Trim();
                if (!string.IsNullOrWhiteSpace(chosenCountry) && !SelectedCountries.Contains(chosenCountry))
                {
                    SelectedCountries.Add(chosenCountry);
                    CountrySearchBox.Text = "";
                }
            }
        }

        private void RemoveCountry_Click(object sender, RoutedEventArgs e)
        {
            Button clickedButton = sender as Button;
            if (clickedButton?.DataContext is string countryToRemove)
            {
                SelectedCountries.Remove(countryToRemove);
            }
        }

        // ==========================================
        // PLACES LOGIC
        // ==========================================
        private void PlaceInputBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                string typedPlace = PlaceInputBox.Text.Trim();
                if (!string.IsNullOrWhiteSpace(typedPlace) && !SelectedPlaces.Contains(typedPlace))
                {
                    SelectedPlaces.Add(typedPlace);
                    PlaceInputBox.Text = "";
                }
            }
        }

        private void RemovePlace_Click(object sender, RoutedEventArgs e)
        {
            Button clickedButton = sender as Button;
            if (clickedButton?.DataContext is string placeToRemove)
            {
                SelectedPlaces.Remove(placeToRemove);
            }
        }

        // ==========================================
        // ACTIONS
        // ==========================================
        private void AddTrip_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("This will save the trip to the database!");

            PlanningPageWindow planningPage = new PlanningPageWindow();
            planningPage.Show();
            this.Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            PlanningPageWindow planningPage = new PlanningPageWindow();
            planningPage.Show();
            this.Close();
        }
    }
}

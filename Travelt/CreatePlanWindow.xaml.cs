using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Travelt.Service;

namespace Travelt
{
    public partial class CreatePlanWindow : Window
    {
        public ObservableCollection<string> SelectedCountries { get; set; } = new ObservableCollection<string>();
        public ObservableCollection<string> SelectedPlaces { get; set; } = new ObservableCollection<string>();
        public ObservableCollection<string> SelectedMonths { get; set; } = new ObservableCollection<string>();

        private readonly TripService _tripService = new TripService();

        public CreatePlanWindow()
        {
            InitializeComponent();

            if (UserService.CurrentUser != null)
            {
                TitleText.Text = $"WHERE TO, {UserService.CurrentUser.Username.ToUpper()}?";
            }

            SelectedCountriesList.ItemsSource = SelectedCountries;
            SelectedPlacesList.ItemsSource = SelectedPlaces;
            SelectedMonthsList.ItemsSource = SelectedMonths;

            LoadAllCountries();
            LoadUpcomingMonths();
        }

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
            if (AnytimeCheck.IsChecked == true)
            {
                MonthSelectionPanel.Visibility = Visibility.Collapsed;
                SelectedMonths.Clear();
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

            for (int i = 0; i < 48; i++)
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

        private void AddTrip_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedCountries.Count == 0)
            {
                MessageBox.Show("Please specify at least one target destination country.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (SelectedPlaces.Count == 0)
            {
                MessageBox.Show("Please add at least one specific place (city, town, or national park).", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string description = DescriptionInput.Text.Trim();
            if (string.IsNullOrWhiteSpace(description))
            {
                MessageBox.Show("Please write a short description or note for this trip plan.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(MaxPeopleInput.Text))
            {
                MessageBox.Show("Please enter the maximum number of people for this trip.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!int.TryParse(MaxPeopleInput.Text, out int maxPeople) || maxPeople <= 0)
            {
                MessageBox.Show("Please enter a valid number of people greater than 0.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (TripTypeCombo.SelectedItem == null)
            {
                MessageBox.Show("Please select a trip style category classification.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            DateTime? dateFrom = null;
            DateTime? dateTo = null;
            bool isFlexible = FlexibleDatesCheck.IsChecked ?? false;
            string flexibleMonthsString = "";

            if (!isFlexible)
            {
                if (DateFromPicker.SelectedDate == null || DateToPicker.SelectedDate == null)
                {
                    MessageBox.Show("Please fill out complete fields for exact dates or opt into Flexible Settings.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (DateToPicker.SelectedDate < DateFromPicker.SelectedDate)
                {
                    MessageBox.Show("Your 'Date To' cannot be earlier than your 'Date From'.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                dateFrom = DateFromPicker.SelectedDate;
                dateTo = DateToPicker.SelectedDate;
            }
            else
            {
                if (AnytimeCheck.IsChecked == true)
                {
                    flexibleMonthsString = "Anytime";
                }
                else
                {
                    if (SelectedMonths.Count == 0)
                    {
                        MessageBox.Show("Please choose at least one flexible target month range option or check 'I can go ANYTIME!'.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                    flexibleMonthsString = string.Join(", ", SelectedMonths);
                }
            }

            bool isPublic = !(MakePrivateCheck.IsChecked ?? false);

            string tripType = "vacation";
            if (TripTypeCombo.SelectedItem is ComboBoxItem selectedType && selectedType.Tag != null)
            {
                tripType = selectedType.Tag.ToString();
            }

            bool success = _tripService.SaveNewTrip(
                UserService.CurrentUser.UserId,
                dateFrom,
                dateTo,
                isFlexible,
                flexibleMonthsString,
                maxPeople,
                tripType,
                description,
                isPublic,
                SelectedCountries,
                SelectedPlaces
            );

            if (success)
            {
                MessageBox.Show("Your brand new travel itinerary has been updated!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                PlanningPageWindow planningPage = new PlanningPageWindow();
                planningPage.Show();
                this.Close();
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            PlanningPageWindow planningPage = new PlanningPageWindow();
            planningPage.Show();
            this.Close();
        }
    }
}
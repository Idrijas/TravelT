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
        private int? _editingTripId;

        public CreatePlanWindow(int? tripId = null)
        {
            InitializeComponent();
            _editingTripId = tripId;

            if (_editingTripId.HasValue)
            {
                LoadTripData(_editingTripId.Value);
            }

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

        private void LoadTripData(int tripId)
        {
            var trip = _tripService.GetFullTripById(tripId);
            if (trip == null) return;

            DescriptionInput.Text = trip.Description;
            MaxPeopleInput.Text = trip.MaxPeople.ToString();
            FlexibleDatesCheck.IsChecked = trip.IsFlexible;
            MakePrivateCheck.IsChecked = !trip.IsPublic;

            foreach (var c in trip.Countries) SelectedCountries.Add(c);
            foreach (var p in trip.Places) SelectedPlaces.Add(p);

            if (trip.IsFlexible)
            {
                ExactDatesPanel.Visibility = Visibility.Collapsed;
                FlexibleDatesPanel.Visibility = Visibility.Visible;
            }
            else
            {
                DateFromPicker.SelectedDate = trip.DateFrom;
                DateToPicker.SelectedDate = trip.DateTo;
            }

            foreach (ComboBoxItem item in TripTypeCombo.Items)
            {
                if (item.Tag?.ToString() == trip.TripType)
                {
                    TripTypeCombo.SelectedItem = item;
                    break;
                }
            }
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
            if ((sender as Button)?.DataContext is string monthToRemove) SelectedMonths.Remove(monthToRemove);
        }

        private void LoadAllCountries()
        {
            List<string> countryList = new List<string>();
            foreach (CultureInfo cul in CultureInfo.GetCultures(CultureTypes.SpecificCultures))
            {
                RegionInfo region = new RegionInfo(cul.Name);
                if (!countryList.Contains(region.EnglishName)) countryList.Add(region.EnglishName);
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
            if ((sender as Button)?.DataContext is string countryToRemove) SelectedCountries.Remove(countryToRemove);
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
            if ((sender as Button)?.DataContext is string placeToRemove) SelectedPlaces.Remove(placeToRemove);
        }

        private void AddTrip_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedCountries.Count == 0) { MessageBox.Show("Please specify at least one target destination country."); return; }
            if (SelectedPlaces.Count == 0) { MessageBox.Show("Please add at least one specific place."); return; }
            if (string.IsNullOrWhiteSpace(DescriptionInput.Text)) { MessageBox.Show("Please write a description."); return; }
            if (!int.TryParse(MaxPeopleInput.Text, out int maxPeople) || maxPeople <= 0) { MessageBox.Show("Enter a valid number of people."); return; }
            if (TripTypeCombo.SelectedItem == null) { MessageBox.Show("Please select a trip style."); return; }

            bool isFlexible = FlexibleDatesCheck.IsChecked ?? false;
            string flexibleMonthsString = (AnytimeCheck.IsChecked == true) ? "Anytime" : string.Join(", ", SelectedMonths);
            bool isPublic = !(MakePrivateCheck.IsChecked ?? false);
            string tripType = (TripTypeCombo.SelectedItem as ComboBoxItem)?.Tag.ToString() ?? "vacation";

            if (_editingTripId.HasValue)
            {
                var model = new FullTripModel
                {
                    TripId = _editingTripId.Value,
                    Description = DescriptionInput.Text.Trim(),
                    MaxPeople = maxPeople,
                    IsFlexible = isFlexible,
                    FlexibleMonths = flexibleMonthsString,
                    IsPublic = isPublic,
                    TripType = tripType,
                    DateFrom = isFlexible ? null : DateFromPicker.SelectedDate,
                    DateTo = isFlexible ? null : DateToPicker.SelectedDate,
                    Countries = new List<string>(SelectedCountries),
                    Places = new List<string>(SelectedPlaces)
                };

                if (_tripService.UpdateTrip(model))
                {
                    MessageBox.Show("Trip updated successfully!");
                    OpenPlanningPage();
                }
            }
            else
            {
                bool success = _tripService.SaveNewTrip(
                    UserService.CurrentUser.UserId,
                    isFlexible ? null : DateFromPicker.SelectedDate,
                    isFlexible ? null : DateToPicker.SelectedDate,
                    isFlexible,
                    flexibleMonthsString,
                    maxPeople,
                    tripType,
                    DescriptionInput.Text.Trim(),
                    isPublic,
                    SelectedCountries,
                    SelectedPlaces
                );

                if (success)
                {
                    MessageBox.Show("Your brand new travel itinerary has been updated!");
                    OpenPlanningPage();
                }
            }
        }

        private void OpenPlanningPage()
        {
            PlanningPageWindow p = new PlanningPageWindow();
            p.Show();
            this.Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            OpenPlanningPage();
        }
    }
}
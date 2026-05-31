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
using System.IO;
using System.Windows.Shapes;
using TravelT;
using Travelt.Service;
using static Travelt.Service.UserService;

namespace Travelt
{
    /// <summary>
    /// Interaction logic for AchievementDetailsWindow.xaml
    /// </summary>
    public partial class AchievementDetailsWindow : Window
    {
        public AchievementDetailsWindow(AchievementsDisplay achievement)
        {
            InitializeComponent();

            AchievementTitle.Text = achievement.Title;
            AchievementDescription.Text = achievement.Description;
            Load_Icon(achievement.IconUrl);



        }





        private void Load_Icon(string iconPath)
        {
            if (string.IsNullOrWhiteSpace(iconPath))
            {
                return;

            }

            string full_path = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, iconPath);

            if (File.Exists(full_path))
            {
                AchievementIcon.Source = new BitmapImage(new Uri(full_path, UriKind.Absolute));
            }
        }





        private void Close_Button(object sender, RoutedEventArgs e) 
        {
            this.Close();
        }
    }
}

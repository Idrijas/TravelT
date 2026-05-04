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
using TravelT;
using Travelt.Service;
using static Travelt.Service.UserService;

namespace Travelt
{
    /// <summary>
    /// Interaction logic for EditUserWindow.xaml
    /// </summary>
    public partial class EditUserWindow : Window
    {
        private int selectedUserId;

        public EditUserWindow(User user)
        {
            InitializeComponent();

            selectedUserId = user.UserId;
            UsernameText.Text = user.Username;

        }

        private void SaveButton(object sender, RoutedEventArgs e)
        {
            string new_username = UsernameText.Text.Trim();

            if (string.IsNullOrWhiteSpace(new_username))
            {
                MessageBox.Show("You have to give a new username");
                return;
            }

            UserService userservice = new UserService();

            bool new_name = userservice.AdminUpdateUsername(selectedUserId, new_username);

            if (new_name)
            {
                MessageBox.Show("Username changed successfully");
                
            }
            else
            {
                MessageBox.Show("Ooops, Almight admin cannot change username");
            }

          
        }





        private void DeleteBioButton(object sender, RoutedEventArgs e)
        {

            MessageBoxResult choice_confirm = MessageBox.Show("Are you sure you want to delete this user's bio?", "Absolutely sure?",MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (choice_confirm != MessageBoxResult.Yes)
            {
                return;
            }

            UserService userservice = new UserService();

            bool bio_deleted = userservice.AdminDeleteBio(selectedUserId);

            if (bio_deleted)
            {
                MessageBox.Show("Bio deleted");

            }
            else
            {
                MessageBox.Show("Almighty Admin cannot delete this user's bio");
            }

        }


        private void DeleteProfilePictureButton(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Ondrej will implement profile picture soon, I promise!");
        }

        








        private void CancelButton(object sender, RoutedEventArgs e)
        {
            AdminToolboxWindow admintoolboxwindow = new AdminToolboxWindow();
            admintoolboxwindow.Show();

            this.Close();
        }





       

        
    }
}

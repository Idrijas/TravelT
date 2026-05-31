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
using static Travelt.Service.ReportService;

namespace Travelt
{
    /// <summary>
    /// Interaction logic for ReportUserWindow.xaml
    /// </summary>
    public partial class ReportUserWindow : Window
    {

        private int reporterUserId;
        private int reportedUserId;





        public ReportUserWindow(int reporterUserId, int reportedUserId)
        {
            InitializeComponent();

            this.reporterUserId= reporterUserId;
            this.reportedUserId= reportedUserId;

        }





        private void Back_Button(object sender, RoutedEventArgs e) 
        {
            this.Close();
        }




        private void SubmitReport_Button(object sender, RoutedEventArgs e)
        {
            ComboBoxItem selectReason = ReasonComboBox.SelectedItem as ComboBoxItem;

            if (selectReason == null)
            {
                MessageBox.Show("Select reason for report");
                return;
            }

            string selectedReason = selectReason.Content.ToString();
            string description = DescriptionTextBox.Text.Trim();

            if (string.IsNullOrWhiteSpace(description))
            {
                MessageBox.Show("Please give us a description");
                return;

            }

            ReportService reportservice = new ReportService();

            bool report_successfull = reportservice.CreateReport(reporterUserId, reportedUserId, selectedReason, description);

            if (report_successfull)
            {
                MessageBox.Show("Successfully Reported!");

                this.Close();
            }
            else
            {
                MessageBox.Show("Something went wrong with report");
            }
        }
    }
}

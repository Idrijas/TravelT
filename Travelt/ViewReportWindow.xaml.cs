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
    /// Interaction logic for ViewReportWindow.xaml
    /// </summary>
    public partial class ViewReportWindow : Window
    {


            private Report currentReport;

            public ViewReportWindow(Report selectedReport)
            {
                InitializeComponent();

                currentReport = selectedReport;

                LoadReportData();
            }

            private void LoadReportData()
            {
                ReportId_Text.Text = $"Report ID: {currentReport.ReportId}";
                Reporter_Text.Text = $"Reporter: {currentReport.ReporterUsername}";
                ReportedUser_Text.Text = $"Reported User: {currentReport.ReportedUsername}";
                Reason_Text.Text = $"Reason: {currentReport.Reason}";
                Date_Text.Text = $"Date: {currentReport.ReportDate}";
                Description_Text.Text = currentReport.Description;
            }


        private void Close_Button(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
        
    







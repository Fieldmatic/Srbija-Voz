using SrbijaVoz.database;
using SrbijaVoz.dataGridRecord;
using SrbijaVoz.managerPages;
using SrbijaVoz.managerWindows;
using SrbijaVoz.model;
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
using Line = SrbijaVoz.model.Line;

namespace SrbijaVoz
{
    /// <summary>
    /// Interaction logic for ManagerWindow.xaml
    /// </summary>
    public partial class ManagerWindow : Window
    {
        private Database Database;
        private Manager Manager;
        public ManagerWindow(Database database, Manager manager)
        {
            InitializeComponent();
            Database = database;
            Manager = manager;
            
        }

        private void LogoutEvent(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow(Database);
            mainWindow.Show();
            this.Close();

        }

        private void SwitchToLineNetworkView(object sender, RoutedEventArgs e)
        {
            DataFrame.Content = new LineNetwork(Database.Lines);

        }

        private void SwitchToTrainData(object sender, RoutedEventArgs e)
        {
            DataFrame.Content = new TrainPage(Database);

        }

        private void SwitchToLineData(object sender, RoutedEventArgs e)
        {
            DataFrame.Content = new LinePage(getLineGridData());

        }

        private void SwitchToLineScheduleData(object sender, RoutedEventArgs e)
        {
            DataFrame.Content = new LineSchedulePage(getLineScheduleGridData());

        }

        private void SwitchToStationsData(object sender, RoutedEventArgs e)
        {
            DataFrame.Content = new StationsPage(Database.Stations);

        }


        private List<LineRecord> getLineGridData()
        {
            List<LineRecord> lineRecordData = new List<LineRecord>();
            foreach (Line line in Database.Lines) lineRecordData.Add(new LineRecord(line));
            return lineRecordData;
        }

        private List<LineScheduleRecord> getLineScheduleGridData()
        {
            List<LineScheduleRecord> lineScheduleRecordData = new List<LineScheduleRecord>();
            foreach (LineSchedule schedule in Database.LineSchedules) lineScheduleRecordData.Add(new LineScheduleRecord(schedule));
            return lineScheduleRecordData;
        }
    }
}

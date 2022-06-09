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

            InitializeManagerShortcuts();
            
        }

        public void InitializeManagerShortcuts()
        {
            RoutedCommand showTrains = new RoutedCommand();
            showTrains.InputGestures.Add(new KeyGesture(Key.D1, ModifierKeys.Control));
            this.CommandBindings.Add(new CommandBinding(showTrains, SwitchToTrainData));

            RoutedCommand showLines = new RoutedCommand();
            showLines.InputGestures.Add(new KeyGesture(Key.D2, ModifierKeys.Control));
            this.CommandBindings.Add(new CommandBinding(showLines, SwitchToLineData));

            RoutedCommand showLineSchedules = new RoutedCommand();
            showLineSchedules.InputGestures.Add(new KeyGesture(Key.D3, ModifierKeys.Control));
            this.CommandBindings.Add(new CommandBinding(showLineSchedules, SwitchToLineScheduleData));

            RoutedCommand showNetwork = new RoutedCommand();
            showNetwork.InputGestures.Add(new KeyGesture(Key.D4, ModifierKeys.Control));
            this.CommandBindings.Add(new CommandBinding(showNetwork, SwitchToLineNetworkView));

            //RoutedCommand showTickets = new RoutedCommand();
            //showTickets.InputGestures.Add(new KeyGesture(Key.D5, ModifierKeys.Control));
            //this.CommandBindings.Add(new CommandBinding(showTickets, SwitchToLineNetworkView));
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
            DataFrame.Content = new LinePage(Database, this);

        }

        private void SwitchToLineScheduleData(object sender, RoutedEventArgs e)
        {
            DataFrame.Content = new LineSchedulePage(Database, this);

        }

        private void SwitchToSoldTicketsRide(object sender, RoutedEventArgs e)
        {
            DataFrame.Content = new SoldTicketsRide(Database);
        }

        private void SwitchToSoldTicketsMonth(object sender, RoutedEventArgs e)
        {
            DataFrame.Content = new SoldTicketsMonth(Database);
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

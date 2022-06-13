using SrbijaVoz.database;
using SrbijaVoz.dataGridRecord;
using SrbijaVoz.help;
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
            CenterWindowOnScreen();
            InitializeManagerShortcuts();

            RoutedCommand help = new();
            help.InputGestures.Add(new KeyGesture(Key.F1));
            this.CommandBindings.Add(new CommandBinding(help, OpenHelp));
        }

        public void InitializeManagerShortcuts()
        {
            RoutedCommand showTrains = new();
            showTrains.InputGestures.Add(new KeyGesture(Key.D1, ModifierKeys.Control));
            this.CommandBindings.Add(new CommandBinding(showTrains, SwitchToTrainData));

            RoutedCommand showStations = new();
            showStations.InputGestures.Add(new KeyGesture(Key.D2, ModifierKeys.Control));
            this.CommandBindings.Add(new CommandBinding(showStations, SwitchToStationsData));

            RoutedCommand showLines = new();
            showLines.InputGestures.Add(new KeyGesture(Key.D3, ModifierKeys.Control));
            this.CommandBindings.Add(new CommandBinding(showLines, SwitchToLineData));

            RoutedCommand showLineSchedules = new();
            showLineSchedules.InputGestures.Add(new KeyGesture(Key.D4, ModifierKeys.Control));
            this.CommandBindings.Add(new CommandBinding(showLineSchedules, SwitchToLineScheduleData));

            RoutedCommand showTickets = new();
            showTickets.InputGestures.Add(new KeyGesture(Key.D5, ModifierKeys.Control));
            this.CommandBindings.Add(new CommandBinding(showTickets, SwitchToSoldTicketsRide));

            RoutedCommand showMonthReport = new();
            showMonthReport.InputGestures.Add(new KeyGesture(Key.D6, ModifierKeys.Control));
            this.CommandBindings.Add(new CommandBinding(showMonthReport, SwitchToSoldTicketsMonth));

            RoutedCommand logout = new();
            logout.InputGestures.Add(new KeyGesture(Key.Back, ModifierKeys.Control));
            this.CommandBindings.Add(new CommandBinding(logout, LogoutEvent));
        }

        private void OpenHelp(object sender, ExecutedRoutedEventArgs e)
        {
            var helpViewer = new HelpViewer("homeHelp");
            helpViewer.Show();
        }

        private void LogoutEvent(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow(Database);
            mainWindow.Show();
            this.Close();

        }

        private void SwitchToTrainData(object sender, RoutedEventArgs e)
        {
            DataFrame.Content = new TrainPage(Database, this);

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
            DataFrame.Content = new SoldTicketsRide(Database, this);
        }

        private void SwitchToSoldTicketsMonth(object sender, RoutedEventArgs e)
        {
            DataFrame.Content = new SoldTicketsMonth(Database, this);
        }

        private void SwitchToStationsData(object sender, RoutedEventArgs e)
        {
            DataFrame.Content = new StationsPage(Database.Stations, this);

        }

        private void CenterWindowOnScreen()
        {
            double screenWidth = System.Windows.SystemParameters.PrimaryScreenWidth;
            double screenHeight = System.Windows.SystemParameters.PrimaryScreenHeight;
            double windowWidth = this.Width;
            double windowHeight = this.Height;
            this.Left = (screenWidth / 2) - (windowWidth / 2);
            this.Top = (screenHeight / 2) - (windowHeight / 2);
        }

    }
}

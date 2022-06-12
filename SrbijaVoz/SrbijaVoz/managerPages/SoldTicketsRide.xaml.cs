using SrbijaVoz.database;
using SrbijaVoz.dataGridRecord;
using SrbijaVoz.managerWindows.help;
using SrbijaVoz.model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SrbijaVoz.managerPages
{
    /// <summary>
    /// Interaction logic for SoldTickets.xaml
    /// </summary>
    public partial class SoldTicketsRide : Page
    {
        private Database Database;
        public SoldTicketsRide(Database db, ManagerWindow managerWindow)
        {
            InitializeComponent();
            Database = db;
            StartStationCombo.ItemsSource = db.GetStationNames();
            EndStationCombo.ItemsSource = db.GetStationNames();

            managerWindow.CommandBindings.Clear();
            managerWindow.InitializeManagerShortcuts();
            InitializeSoldTicketsRidePageShortcuts(managerWindow);
        }

        private void InitializeSoldTicketsRidePageShortcuts(ManagerWindow managerWindow)
        {
            RoutedCommand openDemo = new();
            openDemo.InputGestures.Add(new KeyGesture(Key.D, ModifierKeys.Control));
            managerWindow.CommandBindings.Add(new CommandBinding(openDemo, playDemo));

            RoutedCommand openHelp = new();
            openHelp.InputGestures.Add(new KeyGesture(Key.H, ModifierKeys.Control));
            managerWindow.CommandBindings.Add(new CommandBinding(openHelp, HelpBtn_Click));
        }

        private void playDemo(object sender, RoutedEventArgs e)
        {
            DemoVideo m = new DemoVideo(@"../../../demo/IzvestajVoznja.mp4");
            m.ShowDialog();
        }

        private void SearchTickets(object sender, RoutedEventArgs e)
        {
            try
            {            
                String startStation = StartStationCombo.SelectedItem.ToString();
                String endStation = EndStationCombo.SelectedItem.ToString();
                ObservableCollection<TicketRecord> soldTickets = new ObservableCollection<TicketRecord>();
                double revenue = 0;
                int ticketSold = 0;
                foreach (Ticket ticket in Database.Tickets)
                {
                    if (ticket.StartStation.Name.Equals(startStation) && ticket.ExitStation.Name.Equals(endStation))
                    {
                        soldTickets.Add(new TicketRecord(ticket.Client.Username, ticket.StartStation.Name + "-" + ticket.ExitStation.Name, ticket.Date.ToShortDateString(), ticket.Departure.ToString(), ticket.Arrival.ToString(), ticket.LineSchedule.Train.Name, ticket.Price + " RSD"));
                        revenue += ticket.Price;
                        ticketSold++;
                    }
                }
                if (soldTickets.Count == 0) MessageBox.Show("Nemamo takvih prodatih voznji!", "Obavestenje", MessageBoxButton.OK, MessageBoxImage.Information);
                TicketsDataGrid.ItemsSource = soldTickets;
                RevenueLabel.Content = "Ukupno je zaradjeno " + revenue.ToString() + " RSD";
                TicketSoldLabel.Content = "Ukupno je prodato " + ticketSold.ToString() + " karata";
            }
            catch (Exception ex)
            {
                if (ex is NullReferenceException || ex is InvalidOperationException)
                    MessageBox.Show("Odaberite sve potrebne parametre.", "Greska", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void HelpBtn_Click(object sender, RoutedEventArgs e)
        {
            var path = Environment.CurrentDirectory;
            string filePath = path + "/../../../help/soldTicketsRideHelp.html";
            Process.Start(new ProcessStartInfo(filePath) { UseShellExecute = true });
        }
    }
}

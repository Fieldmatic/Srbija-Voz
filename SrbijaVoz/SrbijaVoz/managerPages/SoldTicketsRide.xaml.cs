using SrbijaVoz.database;
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
        public SoldTicketsRide(Database db)
        {
            InitializeComponent();
            Database = db;
            StartStationCombo.ItemsSource = db.GetStationNames();
            EndStationCombo.ItemsSource = db.GetStationNames();
        }

        private void SearchTickets(object sender, RoutedEventArgs e)
        {
            try
            {
                TicketStackPanel.Children.Clear();
                String startStation = StartStationCombo.SelectedItem.ToString();
                String endStation = EndStationCombo.SelectedItem.ToString();
                List<Ticket> soldTickets = new List<Ticket>();
                foreach (Ticket ticket in Database.Tickets)
                {
                    if (ticket.StartStation.Name.Equals(startStation) && ticket.ExitStation.Name.Equals(endStation)) soldTickets.Add(ticket);

                }
                if (soldTickets.Count == 0) MessageBox.Show("Nemamo takvih prodatih voznji!", "Obavestenje", MessageBoxButton.OK, MessageBoxImage.Information);
                foreach (Ticket ticket in soldTickets)
                {
                    TicketStackPanel.Children.Add(new TicketCard(ticket));

                }
            }
            catch (Exception ex)
            {
                if (ex is NullReferenceException || ex is InvalidOperationException)
                    MessageBox.Show("Odaberite sve potrebne parametre.", "Greska", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}

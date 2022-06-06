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
using static SrbijaVoz.clientPages.DeparturesPage;

namespace SrbijaVoz.clientPages
{
    public partial class ReservedTicketCard : UserControl
    {
        private Ticket Ticket;
        private Database database;
        private Client Client;
        public ReservedTicketCard(Ticket ticket,Database db, Client client)
        {
            InitializeComponent();
            database = db;
            Ticket = ticket;
            Client = client;
            Line.Content = Ticket.StartStation.Name + "-" + Ticket.ExitStation.Name;
            Date.Content = Ticket.Date.ToShortDateString();
            Departure.Content = Ticket.Departure.ToString();
            Arrival.Content = Ticket.Arrival.ToString();
            Train.Content = Ticket.LineSchedule.Line.Train.Name;
            Seat.Content = Ticket.Seat.Number.ToString();
            Price.Content = Ticket.Price + "RSD";

        }

        private void TicketRemoveReservation(object sender, RoutedEventArgs e)
        {
            database.Tickets.Remove(Ticket);
            Client.Tickets.Remove(Ticket);
            Ticket.LineSchedule.TakenSeats[Ticket.Date].Remove(Ticket.Seat.Number);

        }
    }
}

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
    public partial class TicketCard : UserControl
    {
        private Ticket Ticket;
        private Database database;
        private Client Client;
        public TicketCard(Ticket ticket,Database db, Client client)
        {
            InitializeComponent();
            database = db;
            Ticket = ticket;
            Client = client;
            Line.Content = Ticket.StartStation.Name + "-" + Ticket.ExitStation.Name;
            Date.Content = Ticket.Date.ToShortDateString();
            Departure.Content = Ticket.Departure.ToString();
            Arrival.Content = Ticket.Arrival.ToString();
            Train.Content = Ticket.LineSchedule.Train.Name;
            Seat.Content = Ticket.Seat.Number.ToString();
            Price.Content = Ticket.Price + " RSD";

        }

        private void TicketConfirmReservation(object sender, RoutedEventArgs e)
        {
            string sMessageBoxText = "Jeste li sigurni da zelite da potvrdite rezervaciju za ovu kartu?";

            MessageBoxButton btnMessageBox = MessageBoxButton.YesNoCancel;
            MessageBoxImage icnMessageBox = MessageBoxImage.Question;
            string sCaption = "Potvrda rezervacije";

            MessageBoxResult rsltMessageBox = MessageBox.Show(sMessageBoxText, sCaption, btnMessageBox, icnMessageBox);

            switch (rsltMessageBox)
            {
                case MessageBoxResult.Yes:
                    Ticket.TicketStatus = TicketStatus.BOUGHT;
                    (this.Parent as StackPanel).Children.Remove(this);
                    break;

                case MessageBoxResult.No:
                    break;

                case MessageBoxResult.Cancel:
                    break;
            }


        }

        private void TicketRemoveReservation(object sender, RoutedEventArgs e)
        {
            string sMessageBoxText = "Jeste li sigurni da zelite da otkazete rezervaciju za ovu kartu?";

            MessageBoxButton btnMessageBox = MessageBoxButton.YesNoCancel;
            MessageBoxImage icnMessageBox = MessageBoxImage.Question;
            string sCaption = "Otkazivanje rezervacije";

            MessageBoxResult rsltMessageBox = MessageBox.Show(sMessageBoxText, sCaption, btnMessageBox, icnMessageBox);

            switch (rsltMessageBox)
            {
                case MessageBoxResult.Yes:
                    database.Tickets.Remove(Ticket);
                    Client.Tickets.Remove(Ticket);
                    Ticket.LineSchedule.TakenSeats[Ticket.Date].Remove(Ticket.Seat.Number);
                    (this.Parent as StackPanel).Children.Remove(this);
                    break;

                case MessageBoxResult.No:
                    break;

                case MessageBoxResult.Cancel:
                    break;
            }
          

        }
    }
}

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
    /// <summary>
    /// Interaction logic for OfferCard.xaml
    /// </summary>
    public partial class OfferCard : UserControl
    {
        private Offer cardOffer;
        private Database database;
        private Client Client;
        public OfferCard(Offer Offer,Database db, Client client)
        {
            InitializeComponent();
            database = db;
            cardOffer = Offer;
            Client = client;
            Line.Content = Offer.LineSchedule.Line.Name;
            Date.Content = Offer.Date.ToShortDateString();
            Departure.Content = Offer.StartTime.ToString();
            Arrival.Content = Offer.EndTime.ToString();
            Train.Content = Offer.LineSchedule.Line.Train.Name;
            Price.Content = "Od " + Offer.SecondClassPrice.ToString() + " RSD";

        }

        private void TicketResBuy_Click(object sender, RoutedEventArgs e)
        {
            TicketReservationAndBuy ticketReservationAndBuy = new TicketReservationAndBuy(database,cardOffer, Client);
            ticketReservationAndBuy.Show();
        }
    }
}

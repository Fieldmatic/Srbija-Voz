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
    public partial class TicketCard : UserControl
    {
        private Ticket Ticket;
        public TicketCard(Ticket ticket)
        {
            InitializeComponent();
            Ticket = ticket;
            Client.Content = ticket.Client.Username;
            Line.Content = Ticket.StartStation.Name + "-" + Ticket.ExitStation.Name;
            Date.Content = Ticket.Date.ToShortDateString();
            Departure.Content = Ticket.Departure.ToString();
            Arrival.Content = Ticket.Arrival.ToString();
            Train.Content = Ticket.LineSchedule.Line.Train.Name;
            Price.Content = Ticket.Price + " RSD";

        }

    
          

        
    }
}

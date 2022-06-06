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

namespace SrbijaVoz.clientPages
{
    /// <summary>
    /// Interaction logic for BoughtTicketsPage.xaml
    /// </summary>
    public partial class BoughtTicketsPage : Page
    {
        private Database Database;
        private Client Client;
        public BoughtTicketsPage(Database db, Client client)
        {
            InitializeComponent();
            Client = client;
            Database = db;
            foreach (Ticket t in client.Tickets)
            {
                if (t.TicketStatus.Equals(TicketStatus.BOUGHT)) BoughtTicketsStackPanel.Children.Add(new BoughtTicketCard(t, Database, Client));
            }
        }
    }
}

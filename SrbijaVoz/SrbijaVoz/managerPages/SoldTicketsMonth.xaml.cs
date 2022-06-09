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
    /// Interaction logic for SoldTicketsMonth.xaml
    /// </summary>
    public partial class SoldTicketsMonth : Page
    {
        private Database Database;
        public SoldTicketsMonth(Database db)
        {
            Database = db;
            InitializeComponent();


        }

        private void SearchTickets(object sender, RoutedEventArgs e)
        {
            try
            {
                DateTime month = MonthPicker.DisplayDate;
                TicketStackPanel.Children.Clear();
                MonthPicker.DisplayMode = CalendarMode.Year;
                double revenue = 0;
                int ticketSold = 0;
                List<Ticket> soldTickets = new List<Ticket>();
                foreach (Ticket ticket in Database.Tickets)
                {
                    if (ticket.Date.Month == (month.Month))
                    {
                        soldTickets.Add(ticket);
                        revenue += ticket.Price;
                        ticketSold++;
                    }
                }
                    if (soldTickets.Count == 0) MessageBox.Show("Nemamo prodate voznje u tom mesecu!", "Obavestenje", MessageBoxButton.OK, MessageBoxImage.Information);
                foreach (Ticket ticket in soldTickets)
                {
                    TicketStackPanel.Children.Add(new TicketCard(ticket));

                }
                RevenueLabel.Content = "Ukupno je zaradjeno " + revenue.ToString() + " RSD";
                TicketSoldLabel.Content = "Ukupno je prodato " + ticketSold.ToString() + " karata";
            }
            catch (Exception ex)
            {
                if (ex is NullReferenceException || ex is InvalidOperationException)
                    MessageBox.Show("Odaberite sve potrebne parametre.", "Greska", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        protected override void OnPreviewMouseUp(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseUp(e);
            if (Mouse.Captured is Calendar || Mouse.Captured is System.Windows.Controls.Primitives.CalendarItem)
            {
                Mouse.Capture(null);
            }
        }

        private void MonthPicker_DisplayModeChanged(object sender, CalendarModeChangedEventArgs e)
        {
            MonthPicker.DisplayMode = CalendarMode.Year;
        }
    }
}

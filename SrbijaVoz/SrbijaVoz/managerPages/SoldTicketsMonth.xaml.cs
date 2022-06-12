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
    /// Interaction logic for SoldTicketsMonth.xaml
    /// </summary>
    public partial class SoldTicketsMonth : Page
    {
        private Database Database;
        public SoldTicketsMonth(Database db, ManagerWindow managerWindow)
        {
            Database = db;
            InitializeComponent();

            managerWindow.CommandBindings.Clear();
            managerWindow.InitializeManagerShortcuts();
            InitializeSoldTicketsMonthPageShortcuts(managerWindow);
        }

        private void InitializeSoldTicketsMonthPageShortcuts(ManagerWindow managerWindow)
        {
            RoutedCommand openDemo = new();
            openDemo.InputGestures.Add(new KeyGesture(Key.D, ModifierKeys.Control));
            managerWindow.CommandBindings.Add(new CommandBinding(openDemo, playDemo));

            RoutedCommand openHelp = new();
            openHelp.InputGestures.Add(new KeyGesture(Key.F1));
            managerWindow.CommandBindings.Add(new CommandBinding(openHelp, HelpBtn_Click));
        }

        private void SearchTickets(object sender, RoutedEventArgs e)
        {
            try
            {
                DateTime month = MonthPicker.DisplayDate;
                MonthPicker.DisplayMode = CalendarMode.Year;
                double revenue = 0;
                int ticketSold = 0;
                ObservableCollection<TicketRecord> soldTickets = new ObservableCollection<TicketRecord>();
                foreach (Ticket ticket in Database.Tickets)
                {
                    if (ticket.Date.Month == (month.Month))
                    {
                        soldTickets.Add(new TicketRecord(ticket.Client.Username, ticket.StartStation.Name + "-" + ticket.ExitStation.Name, ticket.Date.ToShortDateString(), ticket.Departure.ToString(), ticket.Arrival.ToString(), ticket.LineSchedule.Train.Name,ticket.Price + " RSD"));
                        revenue += ticket.Price;
                        ticketSold++;
                    }
                }
                if (soldTickets.Count == 0) MessageBox.Show("Nemamo prodate voznje u tom mesecu!", "Obavestenje", MessageBoxButton.OK, MessageBoxImage.Information);
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

        private void playDemo(object sender, RoutedEventArgs e)
        {
            DemoVideo m = new DemoVideo(@"../../../demo/MesecniIzvestaj.mp4");
            m.ShowDialog();
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

        private void HelpBtn_Click(object sender, RoutedEventArgs e)
        {
            var path = Environment.CurrentDirectory;
            string filePath = path + "/../../../help/soldTicketsMonthHelp.html";
            Process.Start(new ProcessStartInfo(filePath) { UseShellExecute = true });
        }
    }
}

using SrbijaVoz.database;
using SrbijaVoz.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static SrbijaVoz.clientPages.DeparturesPage;

namespace SrbijaVoz.clientPages
{
    /// <summary>
    /// Interaction logic for TicketReservationAndBuy.xaml
    /// </summary>
    public partial class TicketReservationAndBuy : Window

    {
        private Offer currentOffer;
        private Seat Seat;
        private Database database;
        private double TicketPrice;
        private Client Client;
        public TicketReservationAndBuy(Database db, Offer offer, Client client)
        {
            InitializeComponent();
            currentOffer = offer;
            database = db;
            Client = client;
            DrawSeats();
            StartStation.Content = offer.StartStation.Name;
            EndStation.Content = offer.EndStation.Name;
            StartTime.Content = offer.StartTime.ToString();
            EndTime.Content = offer.EndTime.ToString();
            Date.Content = offer.Date.ToString();
            CenterWindowOnScreen();
        }

        private void DrawSeats()
        {
            List<Seat> Seats = currentOffer.LineSchedule.Train.Seats;
            int numberOfRows;
            if (Seats.Count() % 4 == 0) numberOfRows = Seats.Count() / 4;
            else numberOfRows = Seats.Count() / 4 + 1;

            for (int i = 0; i < numberOfRows; i++) SeatsGrid.RowDefinitions.Add(new RowDefinition());
            int columnTracker = 0;
            int rowTracker = 0;
            foreach (Seat seat in Seats)
            {
                RadioButton seatRB = new RadioButton();
                seatRB.Content = seat.Number.ToString();
                seatRB.GroupName = "Seats";             
                Grid.SetColumn(seatRB, columnTracker);
                Grid.SetRow(seatRB, rowTracker);
                if (columnTracker == 3)
                {
                    columnTracker = 0;
                    rowTracker++;
                }
                else columnTracker++;
                try
                {
                    if (!currentOffer.LineSchedule.TakenSeats[currentOffer.Date].Contains(seat.Number))
                    {
                        seatRB.Click += new RoutedEventHandler(ChangeSeat);
                    }
                    else seatRB.IsEnabled = false;
                    SeatsGrid.Children.Add(seatRB);
                }catch(KeyNotFoundException)
                {
                    seatRB.Click += new RoutedEventHandler(ChangeSeat);
                    SeatsGrid.Children.Add(seatRB);
                }
                }
        }

        void ChangeSeat(object sender, RoutedEventArgs e)
        {
            var radioButton = sender as RadioButton;
            if (radioButton == null)
                return;
            int seatNumber = Convert.ToInt32(radioButton.Content.ToString());
            Seat = database.getSeatScheduledLine(currentOffer.LineSchedule, seatNumber);
            if (Seat.SeatClass == SeatClass.I)
            {
                Price.Content = "Cena: " + currentOffer.FirstClassPrice.ToString();
                TicketPrice = currentOffer.FirstClassPrice;
            }
            else
            {
                Price.Content = "Cena: " + currentOffer.SecondClassPrice.ToString();
                TicketPrice = currentOffer.SecondClassPrice;
            }
            SeatNumber.Content = "Sediste: " + seatNumber.ToString();



        }

        private void BuyTicket(object sender, RoutedEventArgs e)
        {
            string sMessageBoxText = "Jeste li sigurni da zelite da kupite ovu kartu?";

            MessageBoxButton btnMessageBox = MessageBoxButton.YesNoCancel;
            MessageBoxImage icnMessageBox = MessageBoxImage.Question;
            string sCaption = "Potvrda kupovine";

            MessageBoxResult rsltMessageBox = MessageBox.Show(sMessageBoxText, sCaption, btnMessageBox, icnMessageBox);

            switch (rsltMessageBox)
            {
                case MessageBoxResult.Yes:
                    try
                    {
                        Ticket ticket = new Ticket(currentOffer.LineSchedule, TicketStatus.BOUGHT, Seat, Client, TicketPrice, currentOffer.StartStation, currentOffer.EndStation, currentOffer.StartTime, currentOffer.EndTime, currentOffer.Date);
                        Client.Tickets.Add(ticket);
                        database.Tickets.Add(ticket);
                        if (currentOffer.LineSchedule.TakenSeats.ContainsKey(currentOffer.Date)) currentOffer.LineSchedule.TakenSeats[currentOffer.Date].Add(Seat.Number);
                        else
                        {
                            List<int> newTakenSeats = new List<int>();
                            newTakenSeats.Add(Seat.Number);
                            currentOffer.LineSchedule.TakenSeats[currentOffer.Date] = newTakenSeats;
                        }
                        this.Close();
                        break;
                    }
                    catch (Exception ex)
                    {
                        if (ex is NullReferenceException || ex is InvalidOperationException)
                        {
                            MessageBox.Show("Morate odabrati sediste pre kupovine karte!", "Greska", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        break;
                    }
                   
                case MessageBoxResult.No:
                    break;

                case MessageBoxResult.Cancel:
                    break;
            }


        }

        private void ReserveTicket(object sender, RoutedEventArgs e)
        {
            string sMessageBoxText = "Jeste li sigurni da zelite da rezervisete ovu kartu?";

            MessageBoxButton btnMessageBox = MessageBoxButton.YesNoCancel;
            MessageBoxImage icnMessageBox = MessageBoxImage.Question;
            string sCaption = "Potvrda rezervacije";

            MessageBoxResult rsltMessageBox = MessageBox.Show(sMessageBoxText, sCaption, btnMessageBox, icnMessageBox);

            switch (rsltMessageBox)
            {
                case MessageBoxResult.Yes:
                    try
                    {
                        Ticket ticket = new Ticket(currentOffer.LineSchedule, TicketStatus.RESERVED, Seat, Client, TicketPrice, currentOffer.StartStation, currentOffer.EndStation, currentOffer.StartTime, currentOffer.EndTime, currentOffer.Date);
                        Client.Tickets.Add(ticket);
                        database.Tickets.Add(ticket);
                        if (currentOffer.LineSchedule.TakenSeats.ContainsKey(currentOffer.Date)) currentOffer.LineSchedule.TakenSeats[currentOffer.Date].Add(Seat.Number);
                        else
                        {
                            List<int> newTakenSeats = new List<int>();
                            newTakenSeats.Add(Seat.Number);
                            currentOffer.LineSchedule.TakenSeats[currentOffer.Date] = newTakenSeats;
                        }
                        this.Close();
                        break;
                    }
                    catch (Exception ex)
                    {
                        if (ex is NullReferenceException || ex is InvalidOperationException)
                        {
                            MessageBox.Show("Morate odabrati sediste pre rezervacije karte!", "Greska", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        break;
                    }

                case MessageBoxResult.No:
                    break;

                case MessageBoxResult.Cancel:
                    break;
            }
            
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

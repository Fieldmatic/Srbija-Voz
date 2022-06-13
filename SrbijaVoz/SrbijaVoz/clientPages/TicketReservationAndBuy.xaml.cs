using SrbijaVoz.database;
using SrbijaVoz.model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        private int lastRow;
        private Wagon SelectedWagon;
        private Button lastSelectedButton = new Button();
        public TicketReservationAndBuy(Database db, Offer offer, Client client)
        {
            InitializeComponent();
            currentOffer = offer;
            WagonCombo.ItemsSource = currentOffer.LineSchedule.Train.Wagons.Select(item => item.Number);
            database = db;
            Client = client;
            StartStation.Content = offer.StartStation.Name;
            EndStation.Content = offer.EndStation.Name;
            StartTime.Content = offer.StartTime;
            EndTime.Content = offer.EndTime;
            Date.Content = offer.Date.ToString("dd.M.yyyy.");
            CenterWindowOnScreen();
        }

        private void WagonCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SeatNumber.Content = "";
            Price.Content = "";
            int selectedWagonNumber = Int32.Parse(WagonCombo.SelectedItem.ToString());
            SelectedWagon = currentOffer.LineSchedule.Train.Wagons.Find(item => item.Number == selectedWagonNumber);
            List<Seat> seatt = SelectedWagon.Seats;
            SeatsGrid.Children.Clear();
            DrawSeats();
        }

        private void DrawSeats()
        {
            
            Grid leftRowGrid = new Grid();
            Grid.SetColumn(leftRowGrid, 0);
            Thickness margin = leftRowGrid.Margin;
            margin.Right = 30;
            leftRowGrid.Margin = margin;

            Grid rightRowGrid = new Grid();
            Grid.SetColumn(rightRowGrid, 1);

            DrawWagonSide(leftRowGrid, 0);
            DrawWagonSide(rightRowGrid, 2);

            SeatsGrid.Children.Add(leftRowGrid);
            SeatsGrid.Children.Add(rightRowGrid);

            if (SelectedWagon.Seats.Count % 4 != 0)
            {
                DrawLeftSeats(SelectedWagon.Seats.Count % 4, leftRowGrid, rightRowGrid);

            }
        }

        private void DrawWagonSide(Grid rowGrid, int startSeat)
        {
            //kolona do prozora i do prolaza
            ColumnDefinition col1 = new ColumnDefinition();
            ColumnDefinition col2 = new ColumnDefinition();
            col1.Width = new GridLength(1, GridUnitType.Star);
            col2.Width = new GridLength(1, GridUnitType.Star);
            rowGrid.ColumnDefinitions.Add(col1);
            rowGrid.ColumnDefinitions.Add(col2);

            int seatsNum = SelectedWagon.Seats.Count;
            int endSeatOfFullRow = seatsNum;
            if (seatsNum % 4 != 0)
            {
                endSeatOfFullRow = seatsNum - seatsNum % 4;
               
            }
            lastRow = 0;
            for (int i = startSeat; i < endSeatOfFullRow;  i += 4)
            {
                Seat leftSeatObj = SelectedWagon.Seats[i];
                Seat rightSeatObj = SelectedWagon.Seats[i + 1];

                RowDefinition row = new RowDefinition();
                row.Height = new GridLength(1, GridUnitType.Star);
                rowGrid.RowDefinitions.Add(row);

                //levo sediste
                Button leftSeat = new Button();
                Grid.SetRow(leftSeat, lastRow);
                Grid.SetColumn(leftSeat, 0);
                DrawSeat(leftSeat, leftSeatObj);

                //desno sediste
                Button rightSeat = new Button();
                Grid.SetRow(rightSeat, lastRow);
                Grid.SetColumn(rightSeat, 1);
                DrawSeat(rightSeat, rightSeatObj);



                rowGrid.Children.Add(leftSeat);
                rowGrid.Children.Add(rightSeat);

                //povecaj red za 1
                lastRow++;

            }
        }

        void DrawLeftSeats(int leftSeats, Grid leftRowGrid, Grid rightRowGrid)
        {
            RowDefinition row = new RowDefinition();
            row.Height = new GridLength(1, GridUnitType.Star);
         
            leftRowGrid.RowDefinitions.Add(row);
            RowDefinition rightRow = new RowDefinition();
            rightRow.Height = new GridLength(1, GridUnitType.Star);
            rightRowGrid.RowDefinitions.Add(rightRow);
            
            for (int i = 0; i < leftSeats; i++)
            {
                int seatsNum = SelectedWagon.Seats.Count;
                Seat seatObj = SelectedWagon.Seats[seatsNum - seatsNum % 4 + i];
                Button seat = new Button();
                Grid.SetRow(seat, lastRow);
                DrawSeat(seat, seatObj);

                if (i < 2)
                {
                    leftRowGrid.Children.Add(seat);
                    Grid.SetColumn(seat, i);
                } else
                {
                    rightRowGrid.Children.Add(seat);
                    Grid.SetColumn(seat, 0);
                }
            }
        }

        void DrawSeat(Button seatButton, Seat seatObj)
        {
            HandleButton(seatButton, seatObj);
            seatButton.Background = new SolidColorBrush(Colors.Transparent);
            seatButton.Width = double.NaN;
            seatButton.Height = 70;
            seatButton.BorderThickness = new Thickness(1);
            seatButton.BorderBrush = Brushes.Gray;

            StackPanel stackPanel = new StackPanel();
            stackPanel.Orientation = Orientation.Horizontal;

            TextBlock seatNumberTextBlock = new TextBlock();
            seatNumberTextBlock.Text = seatObj.Number.ToString();
            
            var seatIcon = new MaterialDesignThemes.Wpf.PackIcon();
            seatIcon.Kind = MaterialDesignThemes.Wpf.PackIconKind.Seat;
            seatIcon.Width = 30;
            seatIcon.Height = 30;

            stackPanel.Children.Add(seatIcon);
            stackPanel.Children.Add(seatNumberTextBlock);

            seatButton.Content = stackPanel;
        }

        void HandleButton(Button seatButton, Seat seat)
        {
            try {
                if (!currentOffer.LineSchedule.TakenSeats[currentOffer.Date].Contains(seat.Number))
                {
                    ColorButtonByClass(seatButton, seat.SeatClass);
                    seatButton.PreviewMouseLeftButtonDown += SelectSeat_MouseDown;
                } else
                {
                    seatButton.Foreground = Brushes.Red;
                }
            }
            catch (KeyNotFoundException)
            {
                ColorButtonByClass(seatButton, seat.SeatClass);
                seatButton.PreviewMouseLeftButtonDown += SelectSeat_MouseDown;
            }
        }

        void ColorButtonByClass(Button button, SeatClass seatClass)
        {
            if (seatClass == SeatClass.I) button.Foreground = Brushes.Goldenrod;
            else button.Foreground = (Brush) new BrushConverter().ConvertFrom("#0F3256");
        }

        private void BuyTicket(object sender, RoutedEventArgs e)
        {
            try
            {
                int SeatNum = Seat.Number;
            }
            catch (Exception ex)
            {
                if (ex is NullReferenceException)
                {
                    MessageBox.Show("Morate odabrati sedište pre kupovine karte. Molim Vas, pokušajte ponovo.", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                return;
            }
            string sMessageBoxText = "Da li ste sigurni da želite da kupite kartu?";

            MessageBoxButton btnMessageBox = MessageBoxButton.YesNoCancel;
            MessageBoxImage icnMessageBox = MessageBoxImage.Question;
            string sCaption = "Potvrda kupovine";

            MessageBoxResult rsltMessageBox = MessageBox.Show(sMessageBoxText, sCaption, btnMessageBox, icnMessageBox);

            switch (rsltMessageBox)
            {
                case MessageBoxResult.Yes:
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
                case MessageBoxResult.No:
                    break;

                case MessageBoxResult.Cancel:
                    break;
            }


        }

        private void ReserveTicket(object sender, RoutedEventArgs e)
        {
            try
            {
                int SeatNum = Seat.Number;
            }
            catch (Exception ex)
            {
                if (ex is NullReferenceException)
                {
                    MessageBox.Show("Morate odabrati sedište pre rezervacije karte!", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                return;
            }
            string sMessageBoxText = "Da li ste sigurni da želite da rezervišete kartu?";

            MessageBoxButton btnMessageBox = MessageBoxButton.YesNoCancel;
            MessageBoxImage icnMessageBox = MessageBoxImage.Question;
            string sCaption = "Potvrda rezervacije";

            MessageBoxResult rsltMessageBox = MessageBox.Show(sMessageBoxText, sCaption, btnMessageBox, icnMessageBox);

            switch (rsltMessageBox)
            {
                case MessageBoxResult.Yes:

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

        private void SelectSeat_MouseDown(object sender, MouseButtonEventArgs e)
        {
            lastSelectedButton.BorderThickness = new Thickness(1);
            lastSelectedButton.BorderBrush = Brushes.Gray;
            Button selectedButton = sender as Button;
            lastSelectedButton = selectedButton;
            selectedButton.BorderThickness = new Thickness(2);
            selectedButton.BorderBrush = (Brush)new BrushConverter().ConvertFrom("#0F3256");
            StackPanel stackPanel = (StackPanel) selectedButton.Content;
            TextBlock textBlock = (TextBlock) stackPanel.Children[1];

            int seatNumber = Int32.Parse(textBlock.Text);
            Seat = SelectedWagon.Seats.Where(seat => seat.Number == seatNumber).First();
            SeatNumber.Content = Seat.Number;

            if (Seat.SeatClass == SeatClass.I)
            {
                Price.Content = currentOffer.FirstClassPrice.ToString();
                TicketPrice = currentOffer.FirstClassPrice;
            }
            else
            {
                Price.Content = currentOffer.SecondClassPrice.ToString();
                TicketPrice = currentOffer.SecondClassPrice;
            }
        }
    }
}

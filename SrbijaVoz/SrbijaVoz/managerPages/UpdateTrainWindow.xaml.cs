using SrbijaVoz.database;
using SrbijaVoz.dataGridRecord;
using SrbijaVoz.model;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace SrbijaVoz.managerWindows
{
    /// <summary>
    /// Interaction logic for AddTrainWindow.xaml
    /// </summary>
    public partial class UpdateTrainWindow : Window
    {
        private List<Train> Trains;
        private TrainRecord selectedTrain;
        private readonly String operationType;

        public Delegate UpdateTrain;

        public UpdateTrainWindow(TrainRecord Train, List<Train> trains, String operationType)
        {
            InitializeComponent();
            this.Trains = trains;
            this.selectedTrain = Train;
            this.operationType = operationType;
            SetVariableWindowInfos();
            Name.Text = Train.Name;
            seatsNumIClass.Text = Train.FirstClassSeatsNumber.ToString();
            seatsPriceIClass.Text = Train.FirstClassPrice.ToString();
            seatsNumIIClass.Text = Train.SecondClassSeatsNumber.ToString();
            seatsPriceIIClass.Text = Train.SecondClassPrice.ToString();

        }

        public UpdateTrainWindow(List<Train> trains, String operationType)
        {
            //za dodavanje voza
            InitializeComponent();
            this.Trains = trains;
            this.operationType = operationType;
            SetVariableWindowInfos();
            ClearAllFields();

        }


        private void UpdateOrAddTrain_Click(object sender, RoutedEventArgs e)
        {
            if (this.operationType == "add")
            {
                AddTrain_Click(sender, e);
            } else
            {
                UpdateTrain_Click(sender, e);
            }
        }

        private void UpdateTrain_Click(object sender, RoutedEventArgs e)
        {
            Train trainForUpdate = Trains.Find(item => item.Id == this.selectedTrain.Id);
            List<Seat> seats = GetTrainSeats();
            Dictionary<SeatClass, int> seatsPrice = GetTrainSeatsPrice();
            trainForUpdate.Name = Name.Text;
            //trainForUpdate.Seats = seats;
            trainForUpdate.PricesPerMinute = seatsPrice;

            UpdateTrain.DynamicInvoke();
            this.Close();
        }

    private void AddTrain_Click(object sender, RoutedEventArgs e)
        {
            int newId = this.Trains.Last().Id + 1;
            List<Seat> seats = GetTrainSeats();
            Dictionary<SeatClass, int> seatsPrice = GetTrainSeatsPrice();
            //this.Trains.Add(new Train(newId, Name.Text, seats, seatsPrice));
            UpdateTrain.DynamicInvoke();
            this.Close();
        }

        private List<Seat> GetTrainSeats()
        {
            List<Seat> seats = new List<Seat>();
            for (int i = 1; i <= Int32.Parse(seatsNumIClass.Text); i++)
            {
                seats.Add(new Seat(i, SeatClass.I));
            }

            for (int i = 1; i <= Int32.Parse(seatsNumIIClass.Text); i++)
            {
                seats.Add(new Seat(i, SeatClass.II));
            }
            return seats;

        }

        private Dictionary<SeatClass, int> GetTrainSeatsPrice()
        {
            Dictionary<SeatClass, int> seatsPrice = new Dictionary<SeatClass, int>();
            seatsPrice.Add(SeatClass.I, Int32.Parse(seatsPriceIClass.Text));
            seatsPrice.Add(SeatClass.II, Int32.Parse(seatsPriceIIClass.Text));
            return seatsPrice;
        }

        private void SetVariableWindowInfos()
        {
            if (this.operationType == "add")
            {
                updateButton.Content = "Dodaj voz";
                Title = "Dodavanje voza";
            }
            else if (this.operationType == "update")
            {
                updateButton.Content = "Izmeni voz";
                Title = "Izmena voza";
            }
        }

        private void ClearAllFields()
        {
            Name.Text = "";
            seatsNumIClass.Text = "";
            seatsPriceIClass.Text = "";
            seatsNumIIClass.Text = "";
            seatsPriceIIClass.Text = "";
        }

        private void AddWagon_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }
    }

}

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
    public partial class AddTrainWindow : Window
    {
        private List<Train> Trains;

        public Delegate AddTrain;

        public AddTrainWindow(TrainRecord Train, List<Train> trains)
        {
            InitializeComponent();
            this.Trains = trains;
            Name.Text = Train.Name;
            seatsNumIClass.Text = Train.FirstClassSeatsNumber.ToString();
            seatsPriceIClass.Text = Train.FirstClassPrice.ToString();
            seatsNumIIClass.Text = Train.SecondClassSeatsNumber.ToString();
            seatsPriceIIClass.Text = Train.SecondClassPrice.ToString();

        }

        public AddTrainWindow(List<Train> trains)
        {
            InitializeComponent();
            this.Trains = trains;

        }

        private void AddTrain_Click(object sender, RoutedEventArgs e)
        {
            
            int newId = this.Trains.Last().Id + 1;

            List<Seat> seats = new List<Seat>();
            for (int i = 1; i <= Int32.Parse(seatsNumIClass.Text); i++)
            {
                seats.Add(new Seat(i, SeatClass.I));
            }

            for (int i = 1; i <= Int32.Parse(seatsNumIIClass.Text); i++)
            {
                seats.Add(new Seat(i, SeatClass.II));
            }

            Dictionary<SeatClass, int> seatsPrice = new Dictionary<SeatClass, int>();
            seatsPrice.Add(SeatClass.I, Int32.Parse(seatsPriceIClass.Text));
            seatsPrice.Add(SeatClass.II, Int32.Parse(seatsPriceIIClass.Text));
            this.Trains.Add(new Train(newId, Name.Text, seats, seatsPrice));


            AddTrain.DynamicInvoke();
            this.Close();
        }
    }
}

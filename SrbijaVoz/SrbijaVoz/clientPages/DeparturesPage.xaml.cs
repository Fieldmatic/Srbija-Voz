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
    /// Interaction logic for DeparturesPage.xaml
    /// </summary>
    public partial class DeparturesPage : Page
    {
        private Database Database;
        private Client Client;
        public DeparturesPage(Database db, Client client)
        {
            InitializeComponent();
            Client = client;
            Database = db;
            StartStationCombo.ItemsSource = db.GetStationNames();
            EndStationCombo.ItemsSource=db.GetStationNames();
        }

        private void SearchDepartures(object sender, RoutedEventArgs e)
        {
            try
            {
                OfferStackPanel.Children.Clear();
                String startStation = StartStationCombo.SelectedItem.ToString();
                String endStation = EndStationCombo.SelectedItem.ToString();
                DateTime selectedDate = (DateTime)DepartureDatePicker.SelectedDate;
                List<Offer> offers = new List<Offer>();
                foreach (LineSchedule lineSchedule in Database.LineSchedules)
                {
                    bool containsStartStation = false;
                    bool containsEndStation = false;
                    TimeSpan startTime = new TimeSpan();
                    TimeSpan endTime = new TimeSpan();
                    if (lineSchedule.Day == selectedDate.DayOfWeek)
                    {
                        foreach (TrainStop trainStop in lineSchedule.Line.TrainStops)
                        {
                            if (trainStop.StartStation.Name.Equals(startStation))
                            {
                                containsStartStation = true;
                                startTime = trainStop.DepartureTime;
                            }
                            if (trainStop.EndStation.Name.Equals(endStation)) 
                            {
                                containsEndStation = true;
                                endTime = trainStop.ArrivalTime;
                            }
                            if (containsStartStation && containsEndStation)
                            {
                                Station start = Database.GetStationByName(startStation);
                                Station end = Database.GetStationByName(endStation);
                                Dictionary<SeatClass, int> prices = Database.getPriceByDistance(lineSchedule.Line, start, end);
                                offers.Add(new Offer(lineSchedule, start, end, prices[SeatClass.I], prices[SeatClass.II], selectedDate, startTime, endTime));
                                break;
                            }

                        }
                    }
                }
                foreach(Offer offer in offers)
                {
                    OfferStackPanel.Children.Add(new OfferCard(offer, Database, Client));

                }
            }
            catch (Exception ex)
            {
                if (ex is NullReferenceException || ex is InvalidOperationException)
                 MessageBox.Show("Odaberite sve potrebne parametre.", "Greska", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
           

        public class Offer
        {
            public Offer(LineSchedule lineSchedule, Station startStation, Station endStation, double firstClassPrice, double secondClassPrice, DateTime date, TimeSpan startTime, TimeSpan endTime)
            {
                LineSchedule = lineSchedule;
                StartStation = startStation;
                EndStation = endStation;
                FirstClassPrice = firstClassPrice;
                SecondClassPrice = secondClassPrice;
                Date = date;
                StartTime = startTime;
                EndTime = endTime;
            }

            public LineSchedule LineSchedule { get; set; }
            public Station StartStation { get; set; }
            public Station EndStation { get; set; }

            public TimeSpan StartTime { get; set; }
            public TimeSpan EndTime { get; set; }
            public DateTime Date { get; set; }

            public double FirstClassPrice { get; set; }
            public double SecondClassPrice { get; set; }

        }

    }
}

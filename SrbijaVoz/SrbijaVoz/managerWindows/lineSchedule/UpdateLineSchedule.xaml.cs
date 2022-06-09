using MaterialDesignThemes.Wpf;
using SrbijaVoz.database;
using SrbijaVoz.dataGridRecord;
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
using System.Windows.Shapes;
using Line = SrbijaVoz.model.Line;

namespace SrbijaVoz.managerWindows
{
    /// <summary>
    /// Interaction logic for UpdateLineSchedule.xaml
    /// </summary>
    public partial class UpdateLineSchedule : Window
    {
        public Database Database { get; set; }

        public LineSchedule CurrentLineSchedule { get; set; }

        public Delegate Update;

        public List<TimePicker> TimePickers { get; set; }

        public List<TextBlock> ChosenStations { get; set; }

        public UpdateLineSchedule(Database database, LineScheduleRecord lineScheduleRecord)
        {
            InitializeComponent();
            this.DataContext = this;
            Database = database;
            CurrentLineSchedule = LineScheduleRecordToLineSchedule(lineScheduleRecord);
            LineDataGrid.ItemsSource = SetCurrentLine();
            TimePickers = new List<TimePicker>();
            ChosenStations = new List<TextBlock>();
            PopulateTrainNames();
            DisplayStationsAndTimes();
            DisplaySettedDays();
        }

        private List<LineRecord> SetCurrentLine()
        {
            List<LineRecord> lineRecords = new();
            LineRecord lineRecord = new(CurrentLineSchedule.Line);
            lineRecords.Add(lineRecord);
            return lineRecords;
        }

        private LineSchedule? LineScheduleRecordToLineSchedule(LineScheduleRecord lineScheduleRecord)
        {
            LineSchedule lineSchedule = Database.LineSchedules.Find(l => l.Line.Id.Equals(lineScheduleRecord.Id));
            return lineSchedule;
        }

        private List<int> GetCheckBoxValues()
        {
            List<int> values = new();
            if (Cb1.IsChecked == true) values.Add(1);
            if (Cb2.IsChecked == true) values.Add(2);
            if (Cb3.IsChecked == true) values.Add(3);
            if (Cb4.IsChecked == true) values.Add(4);
            if (Cb5.IsChecked == true) values.Add(5);
            if (Cb6.IsChecked == true) values.Add(6);
            if (Cb7.IsChecked == true) values.Add(0);
            return values;
        }

        private void DisplaySettedDays()
        {
            Cb1.IsChecked = CurrentLineSchedule.Days.Contains(DayOfWeek.Monday);
            Cb2.IsChecked = CurrentLineSchedule.Days.Contains(DayOfWeek.Tuesday);
            Cb3.IsChecked = CurrentLineSchedule.Days.Contains(DayOfWeek.Wednesday);
            Cb4.IsChecked = CurrentLineSchedule.Days.Contains(DayOfWeek.Thursday);
            Cb5.IsChecked = CurrentLineSchedule.Days.Contains(DayOfWeek.Friday);
            Cb6.IsChecked = CurrentLineSchedule.Days.Contains(DayOfWeek.Saturday);
            Cb7.IsChecked = CurrentLineSchedule.Days.Contains(DayOfWeek.Sunday);
        }

        private void PopulateTrainNames()
        {
            foreach (Train train in Database.Trains)
            {
                string name = train.Name + " (" + train.Id + ")";
                Trains.Items.Add(name);
            }

            Trains.SelectedIndex = CurrentLineSchedule.Train.Id - 1;
        }

        private void SubmitBtn_Click(object sender, RoutedEventArgs e)
        {
            List<TrainStop>? trainStops = GetTrainStops();
            if (trainStops == null)
            {
                MessageBox.Show("Niste popunili sva vremena!",
                                "Greška",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
                return;
            }

            if (!CkeckTimesOrder())
            {
                MessageBox.Show("Vremena nisu u pravilnom redosledu!",
                                "Greška",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
                return;
            }

            List<int> days = GetCheckBoxValues();
            if (days.Count == 0)
            {
                MessageBox.Show("Niste izabrali nijedan dan!",
                                "Greška",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
                return;
            }

            Line line = CurrentLineSchedule.Line;
            string trainIdStr = Trains.SelectedItem.ToString().Split(" (")[1];
            int trainId = int.Parse(trainIdStr.Substring(0, trainIdStr.Length - 1));
            Train train = Database.Trains.Find(t => t.Id.Equals(trainId));

            CurrentLineSchedule.Train = train;
            CurrentLineSchedule.TrainStops = trainStops;
            // days
            MessageBox.Show("Red vožnje uspešno ažuriran.",
                                "Ažuriranje reda vožnje",
                                MessageBoxButton.OK,
                                MessageBoxImage.Information);
            Update.DynamicInvoke();
            this.Close();
        }

        private bool CkeckTimesOrder()
        {
            for (int i = 0; i < TimePickers.Count - 1; i++)
            {
                if (TimePickers[i].SelectedTime.Value >= TimePickers[i + 1].SelectedTime.Value)
                    return false;
            }
            return true;
        }

        private void DisplayStationsAndTimes()
        {
            Line line = CurrentLineSchedule.Line;
            int rowNum = 0;
            foreach (Station station in line.Stations)
            {
                TimeStationPanel.RowDefinitions.Add(new RowDefinition());
                DateTime? time;
                if (rowNum == 0)
                    time = Convert.ToDateTime(CurrentLineSchedule.TrainStops[rowNum].DepartureTime.ToString());
                else
                    time = Convert.ToDateTime(CurrentLineSchedule.TrainStops[rowNum - 1].ArrivalTime.ToString());
                DisplayStation(station, rowNum);
                MakeTimePicker(time, rowNum);
                rowNum++;
            }

        }

        private void DisplayStation(Station station, int rowNum)
        {
            TextBlock textBlock = new TextBlock();
            textBlock.Text = station.Name;
            textBlock.FontSize = 16;
            Thickness margin = textBlock.Margin;
            margin.Top = 11;
            textBlock.Margin = margin;
            ChosenStations.Add(textBlock);

            TimeStationPanel.Children.Add(textBlock);
            Grid.SetColumn(textBlock, 0);
            Grid.SetRow(textBlock, rowNum);

        }

        private void MakeTimePicker(DateTime? time, int rowNum)
        {
            TimePicker timePicker = new()
            {
                Width = 100,
                Is24Hours = true,
                Style = (Style)Resources["MaterialDesignFloatingHintTimePicker"],
                SelectedTime = time
            };
            Thickness margin = timePicker.Margin;
            margin.Bottom = 7;
            timePicker.Margin = margin;

            TimeStationPanel.Children.Add(timePicker);
            TimePickers.Add(timePicker);
            Grid.SetColumn(timePicker, 1);
            Grid.SetRow(timePicker, rowNum);
        }

        private List<TrainStop>? GetTrainStops()
        {
            List<TrainStop> trainStops = new();
            for (int i = 0; i < TimePickers.Count - 1; i++)
            {
                Station station1 = Database.Stations.Find(s => s.Name.Equals(ChosenStations[i].Text));
                Station station2 = Database.Stations.Find(s => s.Name.Equals(ChosenStations[i + 1].Text));
                try
                {
                    TimeSpan departureTime = TimePickers[i].SelectedTime.Value.TimeOfDay;
                    TimeSpan arrivalTime = TimePickers[i + 1].SelectedTime.Value.TimeOfDay;
                    trainStops.Add(new TrainStop(station1, station2, departureTime, arrivalTime));
                }
                catch (InvalidOperationException)
                {
                    return null;
                }
            }
            return trainStops;
        }
    }
}

using MaterialDesignThemes.Wpf;
using SrbijaVoz.database;
using SrbijaVoz.dataGridRecord;
using SrbijaVoz.model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for CreateLineSchedule.xaml
    /// </summary>
    public partial class CreateLineSchedule : Window
    {
        public Database Database { get; set; }

        public Delegate Create;

        public List<TimePicker> TimePickers { get; set; }

        public List<TextBlock> ChosenStations { get; set; }

        public List<RowDefinition> GridRows { get; set; }

        public CreateLineSchedule(Database database, List<LineRecord> lineRecords)
        {
            InitializeComponent();
            this.DataContext = this;
            Database = database;
            LineDataGrid.ItemsSource = lineRecords;
            PopulateTrainNames();
            TimePickers = new List<TimePicker>();
            ChosenStations = new List<TextBlock>();
            GridRows = new List<RowDefinition>();
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

        private void PopulateTrainNames()
        {
            foreach (Train train in Database.Trains)
            {
                string name = train.Name + " (" + train.Id + ")";
                Trains.Items.Add(name);
            }

            Trains.SelectedIndex = 0;
        }

        private void SubmitBtn_Click(object sender, RoutedEventArgs e)
        {
            LineRecord lineRecord = (LineRecord)LineDataGrid.SelectedItem;
            if (lineRecord == null)
            {
                MessageBox.Show("Niste izabrali nijednu liniju!",
                                "Greška",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
                return;
            }
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

            Line line = Database.Lines.Find(l => l.Id.Equals(lineRecord.Id));
            string trainIdStr = Trains.SelectedItem.ToString().Split(" (")[1];
            int trainId = int.Parse(trainIdStr.Substring(0, trainIdStr.Length - 1));
            Train train = Database.Trains.Find(t => t.Id.Equals(trainId));
            Database.LineSchedules.Add(new LineSchedule(line, days, new Dictionary<DateTime, List<int>>(), trainStops, train));
            MessageBox.Show("Novi red vožnje uspešno dodat.",
                                "Dodavanje novog reda vožnje",
                                MessageBoxButton.OK,
                                MessageBoxImage.Information);
            Create.DynamicInvoke();
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

        private void LineDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LineRecord lineRecord = (LineRecord)LineDataGrid.SelectedItem;
            Line line = Database.Lines.Find(l => l.Id.Equals(lineRecord.Id));
            ClearTimePickersAndStations();
            int rowNum = 0;
            foreach (Station station in line.Stations)
            {
                RowDefinition row = new();
                TimeStationPanel.RowDefinitions.Add(row);
                GridRows.Add(row);
                DisplayStation(station, rowNum);
                MakeTimePicker(rowNum);
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

        private void ClearTimePickersAndStations()
        {            
            for (int i = 0; i < TimePickers.Count; i++)
            {
                TimePicker elementToRemove = TimePickers.ElementAt(i);
                TimeStationPanel.Children.Remove(elementToRemove);

                TextBlock textBlockToRemove = ChosenStations.ElementAt(i);
                TimeStationPanel.Children.Remove(textBlockToRemove);

                RowDefinition rowToRemove = GridRows.ElementAt(i);
                TimeStationPanel.RowDefinitions.Remove(rowToRemove);
            }
            TimePickers.Clear();
            ChosenStations.Clear();
            GridRows.Clear();
        }

        private void MakeTimePicker(int rowNum)
        {
            TimePicker timePicker = new()
            {
                Width = 100,
                Is24Hours = true,
                Style = (Style)Resources["MaterialDesignFloatingHintTimePicker"]
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

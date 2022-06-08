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
    /// Interaction logic for UpdateLine.xaml
    /// </summary>
    public partial class UpdateLine : Window
    {

        Point startPoint = new Point();

        public ObservableCollection<Station> AvailableStations { get; set; }

        public ObservableCollection<Station> SettedStations { get; set; }

        public Database Database { get; set; }

        public List<TimePicker> TimePickers { get; set; }

        public Line CurrentLine { get; set; }

        public Delegate Update;

        public UpdateLine(Database database, LineRecord lineRecord)
        {
            InitializeComponent();
            this.DataContext = this;
            Database = database;
            CurrentLine = LineRecordToLine(lineRecord);
            SetLineName();
            PopulateTrainNames();
            SettedStations = SetSettedStations();
            AvailableStations = SetAvailableStations();          
            TimePickers = SetTimePickers();
        }

        private List<TimePicker> SetTimePickers()
        {
            List<TimePicker> timePickers = new();
            int i = 0;
            foreach (TrainStop trainStop in CurrentLine.TrainStops)
            {
                timePickers.Add(new TimePicker());
                timePickers[i].SelectedTime = Convert.ToDateTime(trainStop.DepartureTime.ToString());
                timePickers[i].Width = 100;
                timePickers[i].Is24Hours = true;
                timePickers[i].Style = (Style)Resources["MaterialDesignFloatingHintTimePicker"];
                Thickness margin = timePickers[i].Margin;
                margin.Bottom = 8;
                timePickers[i].Margin = margin;
                TimePanel.Children.Add(timePickers[i]);
                if (trainStop == CurrentLine.TrainStops.Last())
                {
                    timePickers.Add(new TimePicker());
                    timePickers[i + 1].SelectedTime = Convert.ToDateTime(trainStop.ArrivalTime.ToString());
                    timePickers[i + 1].Width = 100;
                    timePickers[i + 1].Is24Hours = true;
                    timePickers[i + 1].Style = (Style)Resources["MaterialDesignFloatingHintTimePicker"];
                    timePickers[i + 1].Margin = margin;
                    TimePanel.Children.Add(timePickers[i + 1]);
                }
                
                i++;
            }
            return timePickers;
        }

        private ObservableCollection<Station> SetAvailableStations()
        {
            ObservableCollection<Station> stations = new();
            foreach (Station station in Database.Stations)
            {
                if (SettedStations.Contains(station))   // OVO NEMA SMISLA ALI RADI
                    stations.Add(station);
            }
            return stations;
        }

        private ObservableCollection<Station> SetSettedStations()
        {
            ObservableCollection<Station> stations = new();
            foreach (TrainStop trainStop in CurrentLine.TrainStops)
            {
                stations.Add(trainStop.StartStation);
                if (trainStop == CurrentLine.TrainStops.Last())
                    stations.Add(trainStop.EndStation);
            }
            return stations;
        }

        private Line LineRecordToLine(LineRecord lineRecord)
        {
            Line line = Database.Lines.Where(l => l.Id.Equals(lineRecord.Id)).First();
            return line;
        }

        private void SetLineName()
        {
            StartStation.Text = CurrentLine.Name.Split(" - ")[0];
            EndStation.Text = CurrentLine.Name.Split(" - ")[1];
        }

        private void ListView_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            startPoint = e.GetPosition(null);
        }

        private void ListView_MouseMove(object sender, MouseEventArgs e)
        {
            Point mousePos = e.GetPosition(null);
            Vector diff = startPoint - mousePos;

            if (e.LeftButton == MouseButtonState.Pressed &&
                (Math.Abs(diff.X) > SystemParameters.MinimumHorizontalDragDistance ||
                Math.Abs(diff.Y) > SystemParameters.MinimumVerticalDragDistance))
            {
                // Get the dragged ListViewItem
                ListView listView = sender as ListView;
                ListViewItem listViewItem =
                    FindAncestor<ListViewItem>((DependencyObject)e.OriginalSource);

                // Find the data behind the ListViewItem
                Station station = (Station)listView.ItemContainerGenerator.
                    ItemFromContainer(listViewItem);

                // Initialize the drag & drop operation
                DataObject dragData = new DataObject("myFormat", station);
                DragDrop.DoDragDrop(listViewItem, dragData, DragDropEffects.Move);
            }
        }

        private static T FindAncestor<T>(DependencyObject current) where T : DependencyObject
        {
            do
            {
                if (current is T)
                {
                    return (T)current;
                }
                current = VisualTreeHelper.GetParent(current);
            }
            while (current != null);
            return null;
        }

        private void ListView_DragEnter(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent("myFormat") || sender == e.Source)
            {
                e.Effects = DragDropEffects.None;
            }
        }

        private void ListView_DropSelected(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("myFormat"))
            {
                Station station = e.Data.GetData("myFormat") as Station;
                AvailableStations.Remove(station);
                SettedStations.Add(station);

                TimePicker timePicker = new TimePicker();
                timePicker.Width = 100;
                timePicker.Is24Hours = true;
                timePicker.Style = (Style)Resources["MaterialDesignFloatingHintTimePicker"];
                Thickness margin = timePicker.Margin;
                margin.Bottom = 8;
                timePicker.Margin = margin;

                TimePanel.Children.Add(timePicker);
                TimePickers.Add(timePicker);
            }
        }

        private void ListView_DropGiven(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("myFormat"))
            {
                Station station = e.Data.GetData("myFormat") as Station;
                int index = SettedStations.IndexOf(station);
                SettedStations.Remove(station);
                AvailableStations.Add(station);
                TimePicker elementToRemove = TimePickers.ElementAt(index);
                TimePickers.Remove(elementToRemove);
                TimePanel.Children.Remove(elementToRemove);
            }
        }

        private void PopulateTrainNames()
        {
            foreach (Train train in Database.Trains)
            {
                string name = train.Name + " (" + train.Id + ")";
                Trains.Items.Add(name);
            }

            Trains.SelectedIndex = CurrentLine.Train.Id - 1;
        }

        private void SubmitBtn_Click(object sender, RoutedEventArgs e)
        {
            if (StartStation.Text.Length == 0 || EndStation.Text.Length == 0)
            {
                MessageBox.Show("Morate popuniti nazive za početnu i krajnju stanicu!",
                                "Greška",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
                return;
            }
            CurrentLine.Name = StartStation.Text + " - " + EndStation.Text;
            string selectedTrainId = Trains.SelectedValue.ToString().Split(" (")[1];
            selectedTrainId = selectedTrainId.Substring(0, selectedTrainId.Length - 1);
            CurrentLine.Train = Database.Trains.Where(t => t.Id.Equals(int.Parse(selectedTrainId))).First();
            CurrentLine.TrainStops = GetTrainStops();

            if (CurrentLine.TrainStops == null)
            {
                MessageBox.Show("Morate uneti bar 2 stanice!",
                                "Greška",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
                return;
            }
            MessageBox.Show("Linija uspešno izmenjena.",
                                "Ažuriranje linije",
                                MessageBoxButton.OK,
                                MessageBoxImage.Information);

            Update?.DynamicInvoke();
            this.Close();
        }

        private List<TrainStop>? GetTrainStops()
        {
            if (SettedStations.Count < 2)
                return null;

            List<TrainStop> trainStops = new();
            for (int i = 0; i < SettedStations.Count - 1; i++)
            {
                Station station1 = SettedStations[i];
                Station station2 = SettedStations[i + 1];
                TimeSpan departureTime = TimePickers[i].SelectedTime.Value.TimeOfDay;
                TimeSpan arrivalTime = TimePickers[i + 1].SelectedTime.Value.TimeOfDay;
                trainStops.Add(new TrainStop(station1, station2, departureTime, arrivalTime));
            }
            return trainStops;
        }
    }
}

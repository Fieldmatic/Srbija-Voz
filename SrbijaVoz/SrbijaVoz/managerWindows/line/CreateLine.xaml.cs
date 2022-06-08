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

namespace SrbijaVoz.managerWindows
{
    /// <summary>
    /// Interaction logic for AddLineForm.xaml
    /// </summary>
    public partial class CreateLine : Window
    {
        Point startPoint = new Point();

        public ObservableCollection<Station> AvailableStations { get; set; }

        public ObservableCollection<Station> SettedStations { get; set; }

        public Database Database { get; set; }

        public List<TimePicker> TimePickers { get; set; }

        public Delegate Create;

        public CreateLine(Database database)
        {
            InitializeComponent();
            this.DataContext = this;
            Database = database;
            AvailableStations = new ObservableCollection<Station>(Database.Stations);
            SettedStations = new ObservableCollection<Station>();
            PopulateTrainNames();
            TimePickers = new List<TimePicker>();
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
                SettedStations.Remove(station);
                AvailableStations.Add(station);
                TimePicker lastElement = TimePickers.Last();
                TimePickers.Remove(lastElement);
                TimePanel.Children.Remove(lastElement);
            }
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
            int id = Database.Lines.Count() + 1;
            if (StartStation.Text.Length == 0 || EndStation.Text.Length == 0)
            {
                MessageBox.Show("Morate popuniti nazive za početnu i krajnju stanicu!",
                                "Greška",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
                return;
            }
            string name = StartStation.Text + " - " + EndStation.Text;
            string selectedTrainId = Trains.SelectedValue.ToString().Split(" (")[1];
            selectedTrainId = selectedTrainId.Substring(0, selectedTrainId.Length - 1);
            Train train = Database.Trains.Where(t => t.Id.Equals(int.Parse(selectedTrainId))).First();

            List<TrainStop>? trainStops = GetTrainStops();
            if (trainStops == null)
            {
                MessageBox.Show("Morate uneti bar 2 stanice!",
                                "Greška",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
                return;
            }
            Database.Lines.Add(new model.Line(id, name, trainStops, train));
            MessageBox.Show("Nova linija uspešno dodata.",
                                "Dodavanje nove linije",
                                MessageBoxButton.OK,
                                MessageBoxImage.Information);
            Create.DynamicInvoke();
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

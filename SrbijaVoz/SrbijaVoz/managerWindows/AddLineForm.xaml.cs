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
    public partial class AddLineForm : Window
    {
        Point startPoint = new Point();

        public ObservableCollection<Station> Stations { get; set; }

        public ObservableCollection<Station> StationsSetted { get; set; }

        public Database Database { get; set; }

        public List<TimePicker> TimePickers { get; set; }

        public AddLineForm(List<Station> stations, Database database)
        {
            InitializeComponent();
            this.DataContext = this;
            Stations = new ObservableCollection<Station>(stations);
            StationsSetted = new ObservableCollection<Station>();
            Database = database;
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

        private void ListView_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("myFormat"))
            {
                Station station = e.Data.GetData("myFormat") as Station;
                Stations.Remove(station);
                StationsSetted.Add(station);
                
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
            string name = StartStation.Text + " - " + EndStation.Text;
            string selectedTrainId = Trains.SelectedValue.ToString().Split(" (")[1];
            selectedTrainId = selectedTrainId.Substring(0, selectedTrainId.Length - 1);
            Train train = Database.Trains.Where(t => t.Id.Equals(int.Parse(selectedTrainId))).First();

            List<TrainStop> trainStops = getTrainStops();
            Database.Lines.Add(new model.Line(id, name, trainStops, train));

            MessageBox.Show("Nova linija uspešno dodata.");
            this.Close();
        }

        private List<TrainStop> getTrainStops()
        {
            List<TrainStop> trainStops = new();
            for (int i = 0; i < StationsSetted.Count - 1; i++)
            {
                Station station1 = StationsSetted[i];
                Station station2 = StationsSetted[i + 1];
                TimeSpan departureTime = TimePickers[i].SelectedTime.Value.TimeOfDay;
                TimeSpan arrivalTime = TimePickers[i + 1].SelectedTime.Value.TimeOfDay;
                trainStops.Add(new TrainStop(station1, station2, departureTime, arrivalTime));
            }
            return trainStops;
        }
    }
}

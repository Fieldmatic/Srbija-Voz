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

        public Line CurrentLine { get; set; }

        public Delegate Update;

        public UpdateLine(Database database, LineRecord lineRecord)
        {
            InitializeComponent();
            this.DataContext = this;
            Database = database;
            CurrentLine = LineRecordToLine(lineRecord);
            SettedStations = SetSettedStations();
            AvailableStations = SetAvailableStations();
        }

        private ObservableCollection<Station> SetAvailableStations()
        {
            ObservableCollection<Station> stations = new();
            ObservableCollection<string> stationNames = new();
            foreach (Station station in SettedStations)
            {
                stationNames.Add(station.Name);
            }
            foreach (Station station in Database.Stations)
            {
                if (!stationNames.Contains(station.Name))
                    stations.Add(station);
            }
            return stations;
        }

        private ObservableCollection<Station> SetSettedStations()
        {
            ObservableCollection<Station> stations = new();
            foreach (Station station in CurrentLine.Stations)
                stations.Add(station);         
            return stations;
        }

        private Line LineRecordToLine(LineRecord lineRecord)
        {
            Line line = Database.Lines.Where(l => l.Id.Equals(lineRecord.Id)).First();
            return line;
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
            }
        }

        private void ListView_DropGiven(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("myFormat"))
            {
                Station station = e.Data.GetData("myFormat") as Station;
                SettedStations.Remove(station);
                AvailableStations.Add(station);
            }
        }

        private void SubmitBtn_Click(object sender, RoutedEventArgs e)
        {
            if (SettedStations.Count < 2)
            {
                MessageBox.Show("Morate uneti bar 2 stanice!",
                                "Greška",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
                return;
            }
            if (LineAlreadyExist())
            {
                MessageBox.Show("Ovakva linija već postoji!",
                                   "Greška",
                                   MessageBoxButton.OK,
                                   MessageBoxImage.Error);
                return;
            }
            CurrentLine.Name = SettedStations.First().Name + " - " + SettedStations.Last().Name;
            CurrentLine.Stations = SettedStations.ToList();
            MessageBox.Show("Linija uspešno ažurirana!",
                                   "Ažuriranje linije",
                                   MessageBoxButton.OK,
                                   MessageBoxImage.Information);
            Update.DynamicInvoke();
            this.Close();
        }

        private bool LineAlreadyExist()
        {
            bool alreadyExist = false;
            foreach (Line line in Database.Lines)
            {
                if (line.Stations.Count != SettedStations.Count)
                    continue;
                for (int i = 0; i < SettedStations.Count; i++)
                {
                    if (SettedStations[i].Name == line.Stations[i].Name)
                    {
                        alreadyExist = true;
                        continue;
                    }
                    alreadyExist = false;
                    break;
                }
                if (alreadyExist)
                    return true;
            }
            return alreadyExist;
        }

    }
}

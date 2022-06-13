using Hangfire.Annotations;
using MaterialDesignThemes.Wpf;
using Microsoft.Maps.MapControl.WPF;
using SrbijaVoz.managerWindows.help;
using SrbijaVoz.model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
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

namespace SrbijaVoz.managerPages
{
    public partial class StationsPage : Page
    {
        public List<Station> Stations;

        public StationsPage(List<Station> stations, ManagerWindow managerWindow)
        {
            Stations = stations;
            InitializeComponent();
            StationsNetworkMap.Focus();
            DrawStationsOnMap();
            StationsListBox.ItemsSource = Stations;

            managerWindow.CommandBindings.Clear();
            managerWindow.InitializeManagerShortcuts();
            InitializeStationsPageShortcuts(managerWindow);
        }

        private void InitializeStationsPageShortcuts(ManagerWindow managerWindow)
        {
            RoutedCommand openDemo = new();
            openDemo.InputGestures.Add(new KeyGesture(Key.D, ModifierKeys.Control));
            managerWindow.CommandBindings.Add(new CommandBinding(openDemo, playDemo));
        }

        private void DrawStationsOnMap()
        {
            foreach(Station station in Stations)
            {
                AddStationOnMap(station);
            }
        }

        private void AddStationOnMap(Station station)
        {
            Pushpin pin = CreateStationPin(station);
            StationsNetworkMap.Children.Add(pin);
        }

        private static Pushpin CreateStationPin(Station station)
        {
            Pushpin pin = new Pushpin();
            ToolTip toolTip = new ToolTip();
            pin.Content = station.Id;
            toolTip.Content = "Stanica " + station.Name;
            toolTip.Cursor = Cursors.Arrow;
            pin.ToolTip = toolTip;
            pin.Location = new Location(station.Location);
            return pin;
        }


        public void AddNewStation_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragDrop.DoDragDrop(PinIcon, "add", DragDropEffects.Move);
        }

        private void StationsNetworkMap_Drop(object sender, DragEventArgs e)
        {
            Point dropPosition = e.GetPosition(StationsNetworkMap);
            if (e.Data.GetData(DataFormats.StringFormat).ToString() == "add")
            {
                _ = AddStation(dropPosition);
            } else
            {
                DeleteStation(dropPosition);
            }           
        }

        private void DeleteStation(Point dropPosition)
        {
            Location location = StationsNetworkMap.ViewportPointToLocation(dropPosition);
            Station station = FindStationByLocation(location);
            if (station != null)
            {
                Pushpin pin = CreateStationPin(station);
                Stations.Remove(station);
                StationsListBox.Items.Refresh();
                StationsNetworkMap.Children.Clear();
                DrawStationsOnMap();
            }
        }

        private Station? FindStationByLocation(Location location)
        {
            foreach(Station station in Stations)
            {
                if ((Math.Abs(station.Location.Latitude - location.Latitude) <= 0.002) && (Math.Abs(station.Location.Longitude - location.Longitude) <= 0.002)) return station;
            }
            return null;
        }

        private async Task<bool> AddStation(Point dropPosition)
        {
            var viewModel = new MessageBoxModel
            {
                MessageBoxAction = "Unesite ime nove stanice:"
            };

            var result = await DialogHost.Show(viewModel);
            if (result is bool b && b)
            {
                Location pinLocation = StationsNetworkMap.ViewportPointToLocation(dropPosition);
                int newId;
                if (Stations.Count == 0) newId = 1;
                else newId = Stations.Last().Id + 1;
                Station newStation = new(newId, viewModel.MessageBoxInput, pinLocation);
                Stations.Add(newStation);
                StationsListBox.Items.Refresh();
                AddStationOnMap(newStation);
                return true;
            }
            return false;
        }

        private void DeleteStation_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragDrop.DoDragDrop(EraseIcon, "delete", DragDropEffects.Move);
        }

        public Station GetStationByName(String name)
        {
            foreach (Station station in Stations)
                if (station.Name.Equals(name)) return station;
            return null;
        }

        private void StationsListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Station selectedStation = StationsListBox.SelectedItem as Station;
            StationsNetworkMap.Center = selectedStation.Location;
            StationsNetworkMap.Focus();
            StationsNetworkMap.ZoomLevel = 15;
        }


        private void playDemo(object sender, RoutedEventArgs e)
        {
            DemoVideo m = new DemoVideo(@"../../../demo/Stanice.mp4");
            m.ShowDialog();
         }
    }

    public class MessageBoxModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string _messageBoxInput;
        private string _messageBoxAction;

        public string MessageBoxAction
        {
            get => _messageBoxAction;
            set
            {
                if (_messageBoxAction != value)
                {
                    _messageBoxAction = value;
                    OnPropertyChanged();
                }
            }
        }

        public string MessageBoxInput
        {
            get => _messageBoxInput;
            set
            {
                if (_messageBoxInput != value)
                {
                    _messageBoxInput = value;
                    OnPropertyChanged();
                }
            }
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

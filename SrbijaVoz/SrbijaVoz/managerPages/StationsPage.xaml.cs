using Hangfire.Annotations;
using MaterialDesignThemes.Wpf;
using Microsoft.Maps.MapControl.WPF;
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
        private List<Station> Stations;

        public StationsPage(List<Station> stations)
        {
            InitializeComponent();
            Stations = stations;
            StationsNetworkMap.Focus();
            ShowStationsOnMap();
        }

        private void ShowStationsOnMap()
        {
            foreach(Station station in Stations)
            {
                AddStationOnMap(station);
            }
        }

        private void AddStationOnMap(Station station)
        {
            Pushpin pin = new Pushpin();
            ToolTip toolTip = new ToolTip();
            pin.Content = station.Id;
            toolTip.Content = "Stanica " + station.Name;
            toolTip.Cursor = Cursors.Arrow;
            pin.ToolTip = toolTip;
            
            pin.Location = new Location(station.Location);
            StationsNetworkMap.Children.Add(pin);
        }

        public void AddNewStation_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragDrop.DoDragDrop(PinIcon, PinIcon, DragDropEffects.Move);
        }

        private void StationsNetworkMap_Drop(object sender, DragEventArgs e)
        {
            Point dropPosition = e.GetPosition(StationsNetworkMap);
            _ = AddStation(dropPosition);
           
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
                int newId = Stations.Last().Id + 1;
                Station newStation = new(newId, viewModel.MessageBoxInput, pinLocation);
                Stations.Add(newStation);
                AddStationOnMap(newStation);
                return true;
            }
            return false;
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

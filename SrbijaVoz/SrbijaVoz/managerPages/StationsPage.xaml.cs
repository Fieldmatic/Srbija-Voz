﻿using Hangfire.Annotations;
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
        public List<Station> Stations;

        public bool IsSelected = true;

        public StationsPage(List<Station> stations)
        {
            Stations = stations;
            InitializeComponent();
            StationsNetworkMap.Focus();
            DrawStationsOnMap();
            StationsList.ItemsSource = Stations;

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
            /*Point dropPosition = e.GetPosition(StationsNetworkMap);
            Canvas.SetLeft(PinIcon, dropPosition.X);
            Canvas.SetTop(PinIcon, dropPosition.X);*/
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
                StationsList.Items.Refresh();
                StationsNetworkMap.Children.Clear();
                DrawStationsOnMap();

            }
        }

        private Station? FindStationByLocation(Location location)
        {
            foreach(Station station in Stations)
            {
                Trace.WriteLine(station.Name);
                Trace.WriteLine("Latitude: " + (location.Latitude - station.Location.Latitude).ToString());
                Trace.WriteLine("Longitude: " + (location.Longitude - station.Location.Longitude).ToString());
                if ((Math.Abs(station.Location.Latitude - location.Latitude) <= 0.001) && (Math.Abs(station.Location.Longitude - location.Longitude) <= 0.001)) return station;
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
                int newId = Stations.Last().Id + 1;
                Station newStation = new(newId, viewModel.MessageBoxInput, pinLocation);
                Stations.Add(newStation);
                StationsList.Items.Refresh();
                AddStationOnMap(newStation);
                return true;
            }
            return false;
        }

        private void DeleteStation_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragDrop.DoDragDrop(EraseIcon, "delete", DragDropEffects.Move);
        }

        private void ToggleButton_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //StationsNetworkMap.SetView();
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Grid grid = sender as Grid;
            StackPanel panel = grid.Children[1] as StackPanel;
            TextBlock textBlock = panel.Children[0] as TextBlock;

            Station station = GetStationByName(textBlock.Text);
            StationsNetworkMap.Center = station.Location;
            StationsNetworkMap.Focus();
            StationsNetworkMap.ZoomLevel = 15;
        }

        public Station GetStationByName(String name)
        {
            foreach (Station station in Stations)
                if (station.Name.Equals(name)) return station;
            return null;
        }

        private void Grid_MouseEnter(object sender, MouseEventArgs e)
        {
            Grid grid = sender as Grid;
            StackPanel panel = grid.Children[1] as StackPanel;
            TextBlock textBlock = panel.Children[0] as TextBlock;

            Station station = GetStationByName(textBlock.Text);
            StationsNetworkMap.Center = station.Location;
            StationsNetworkMap.Focus();
            StationsNetworkMap.ZoomLevel = 15;
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

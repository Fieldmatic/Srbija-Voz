using Microsoft.Maps.MapControl.WPF;
using SrbijaVoz.model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace SrbijaVoz.managerPages
{
    /// <summary>
    /// Interaction logic for Stations.xaml
    /// </summary>
    public partial class StationsPage : Page
    {
        private List<Station> Stations;
        public StationsPage(List<Station> stations)
        {
            InitializeComponent();
            Stations = stations;
            StationsNetworkMap.Focus();
            ShowStations();
        }

        private void ShowStations()
        {
            foreach(Station station in Stations)
            {
                Pushpin pin = new Pushpin();
                pin.Content = station.Id;
                pin.Location = new Location(station.Location);
                StationsNetworkMap.Children.Add(pin);
            }
        }

    }
}

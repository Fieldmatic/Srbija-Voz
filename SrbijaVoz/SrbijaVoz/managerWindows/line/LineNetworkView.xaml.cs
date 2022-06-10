using Microsoft.Maps.MapControl.WPF;
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

namespace SrbijaVoz.managerWindows.line
{
    /// <summary>
    /// Interaction logic for LineNetworkView.xaml
    /// </summary>
    public partial class LineNetworkView : Window
    {
        private Line Line;
        public LineNetworkView(Line line)
        {
            InitializeComponent();
            Line = line;
            DrawLineNetwork();
            CenterWindowOnScreen();

        }

        private void DrawLineNetwork()
        {
            MapPolyline polyline = new MapPolyline();
            polyline.Stroke = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.MidnightBlue);
            polyline.StrokeThickness = 5;
            polyline.Opacity = 0.7;
            polyline.Locations = new LocationCollection();
            int counter = 0;
            Location startLocation = Line.Stations[0].Location;
            LineNetworkMap.Center = startLocation;
            LineNetworkMap.Focus();
            polyline.Locations.Add(startLocation);
            Pushpin startPin = new Pushpin();
            startPin.Content = counter.ToString();
            counter++;
            startPin.Location = new Location(startLocation);
            LineNetworkMap.Children.Add(startPin);
            foreach (Station station in Line.Stations)
            {
                polyline.Locations.Add(station.Location);
                Pushpin endStation = new Pushpin();
                endStation.Content = counter.ToString();
                counter++;
                endStation.Location = station.Location;
                LineNetworkMap.Children.Add(endStation);
            }
            LineNetworkMap.Children.Add(polyline);
        }


        private void CenterWindowOnScreen()
        {
            double screenWidth = System.Windows.SystemParameters.PrimaryScreenWidth;
            double screenHeight = System.Windows.SystemParameters.PrimaryScreenHeight;
            double windowWidth = this.Width;
            double windowHeight = this.Height;
            this.Left = (screenWidth / 2) - (windowWidth / 2);
            this.Top = (screenHeight / 2) - (windowHeight / 2);
        }



    }
}

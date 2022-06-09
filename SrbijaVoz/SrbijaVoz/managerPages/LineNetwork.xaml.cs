using System;
using System.Collections.Generic;
using System.Net;
using System.Windows.Controls;
using Microsoft.Maps.MapControl.WPF;
using Newtonsoft.Json.Linq;
using SrbijaVoz.model;
using Line = SrbijaVoz.model.Line;

namespace SrbijaVoz.managerPages
{
    /// <summary>
    /// Interaction logic for LineNetwork.xaml
    /// </summary>
    public partial class LineNetwork : Page
    {
        private List<Line> Lines;
        public LineNetwork(List<Line> lines)
        {
            InitializeComponent();
            Lines = lines;
            LineNetworkMap.Focus();
            List<String> lineNames = new List<String>();
            foreach (Line line in lines)
            {
                if (!lineNames.Contains(line.Name))
                    lineNames.Add(line.Name);
            }            
            LinesCombo.ItemsSource = lineNames;
            
        }


        private void LinesComboChanged(object sender, SelectionChangedEventArgs e)
        {
            LineNetworkMap.Children.Clear();
            String lineName = LinesCombo.SelectedItem.ToString();
            LinesCombo.Text = lineName;
            foreach (Line line in Lines)
            {
                if (line.Name.Equals(lineName)) 
                {
                MapPolyline polyline = new MapPolyline();
                polyline.Stroke = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Orange);
                polyline.StrokeThickness = 5;
                polyline.Opacity = 0.7;
                polyline.Locations = new LocationCollection();
                int counter = 1;
                Location startLocation = line.Stations[0].Location;
                polyline.Locations.Add(startLocation);
                Pushpin startPin = new Pushpin();
                startPin.Content = counter.ToString();
                counter++;
                startPin.Location = new Location(startLocation);
                LineNetworkMap.Children.Add(startPin);
                foreach (Station staion in line.Stations)
                    {
                    polyline.Locations.Add(staion.Location);
                    Pushpin endStation = new Pushpin();
                    endStation.Content = counter.ToString();
                    counter++;
                    endStation.Location = staion.Location;
                    LineNetworkMap.Children.Add(endStation);
                    }
                    LineNetworkMap.Children.Add(polyline);
                }
             }
         }

            
    }
}

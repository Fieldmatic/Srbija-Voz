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

namespace SrbijaVoz.clientPages
{
    /// <summary>
    /// Interaction logic for ClientRouteView.xaml
    /// </summary>
    public partial class ClientRouteView : Window
    {
        private Ticket Ticket;
        public ClientRouteView(Ticket ticket)
        {
            InitializeComponent();
            Ticket = ticket;
            TicketRouteView.Center = ticket.StartStation.Location;
            TicketRouteView.Focus();
            DrawRoute();

        }

        void DrawRoute()
        {          
            int counter = 1;
            Pushpin startPin = new Pushpin();
            startPin.Content = counter.ToString();
            counter++;
            startPin.Location = new Location(Ticket.LineSchedule.TrainStops[0].StartStation.Location);
            TicketRouteView.Children.Add(startPin);
            foreach (TrainStop trainStop in Ticket.LineSchedule.TrainStops)
            {
                Pushpin endStation = new Pushpin();
                endStation.Location = new Location(trainStop.EndStation.Location);
                endStation.Content = counter.ToString();
                counter++;
                TicketRouteView.Children.Add(endStation);
            }
            bool greenColor = false;
            foreach (TrainStop trainStop in Ticket.LineSchedule.TrainStops)
            {
                MapPolyline polyline = new MapPolyline();
                if (!greenColor) polyline.Stroke = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Orange);
                else polyline.Stroke = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Green);
                polyline.StrokeThickness = 5;
                polyline.Opacity = 0.7;
                polyline.Locations = new LocationCollection();
                if (trainStop.StartStation.Name.Equals(Ticket.StartStation.Name))
                {
                    polyline.Stroke = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Green);
                    greenColor = true;                
                }
                polyline.Locations.Add(trainStop.StartStation.Location);
                polyline.Locations.Add(trainStop.EndStation.Location);
                if (trainStop.EndStation.Name.Equals(Ticket.ExitStation.Name)) greenColor = false;                   
                TicketRouteView.Children.Add(polyline);
            }
            
        }


    
    }
}

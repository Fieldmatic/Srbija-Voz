using SrbijaVoz.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Maps.MapControl.WPF;
using GeoCoordinatePortable;

namespace SrbijaVoz.database
{
    public class Database
    {
        public List<Client> Clients { get; set; }
        public List<Manager> Managers { get; set; }
        public List<Train> Trains { get; set; }
        public List<Station> Stations { get; set; }
        public List<TrainStop> TrainStops { get; set; }
        public List<Line> Lines { get; set; }
        public List<LineSchedule> LineSchedules { get; set; }

        public List<Ticket> Tickets { get; set; }

        public Database()
        {
            Clients = new List<Client>();
            Managers = new List<Manager>();
            Trains = new List<Train>();
            Stations = new List<Station>();
            TrainStops = new List<TrainStop>();
            Lines = new List<Line>();
            LineSchedules = new List<LineSchedule>();
            Tickets = new List<Ticket>();
            LoadData();
        }

        public List<String> GetStationNames()
        {
            List<String> stations = new List<String>();
            foreach(Station station in Stations)
                stations.Add(station.Name);
            return stations;
        }

        public Station GetStationByName(String name)
        {
            foreach (Station station in Stations)
                if (station.Name.Equals(name)) return station;
            return null;
        }

        public Dictionary<SeatClass, int> getPriceByDistance(Line Line, Station Start, Station End)
        {
            Dictionary<SeatClass, int> prices = new Dictionary<SeatClass, int>();
            Location routeStart = Line.TrainStops.First().StartStation.Location;
            Location routeEnd = Line.TrainStops.Last().EndStation.Location;
            var routeStartCoord = new GeoCoordinate(routeStart.Latitude, routeStart.Longitude);
            var routeEndCoord = new GeoCoordinate(routeEnd.Latitude, routeEnd.Longitude);
            var wantedStart = new GeoCoordinate(Start.Location.Latitude, Start.Location.Longitude);
            var wantedEnd = new GeoCoordinate(End.Location.Latitude, End.Location.Longitude);
            double pricePerMeterIIClass = Line.Train.Prices[SeatClass.II] / routeStartCoord.GetDistanceTo(routeEndCoord);
            double pricePerMeterIClass = Line.Train.Prices[SeatClass.I] / routeStartCoord.GetDistanceTo(routeEndCoord);
            prices.Add(SeatClass.I, (int) (wantedStart.GetDistanceTo(wantedEnd) * pricePerMeterIClass));
            prices.Add(SeatClass.II, (int) (wantedStart.GetDistanceTo(wantedEnd) * pricePerMeterIIClass));
            return prices;
        }

        public Seat getSeatScheduledLine(LineSchedule lineSchedule, int seatNumber)
        {
            foreach(Seat seat in lineSchedule.Line.Train.Seats)
            {
                if (seat.Number == seatNumber) return seat;

            }
            return null;

        }


        private void LoadData()
        {
            Clients = JsonSerializer.Deserialize<List<Client>>(File.ReadAllText("../../../data/clientData.json"));
            Managers = JsonSerializer.Deserialize<List<Manager>>(File.ReadAllText("../../../data/managerData.json"));
            Trains = JsonSerializer.Deserialize<List<Train>>(File.ReadAllText("../../../data/trainData.json"));
            Stations = JsonSerializer.Deserialize<List<Station>>(File.ReadAllText("../../../data/stationData.json"));
            TrainStops = JsonSerializer.Deserialize<List<TrainStop>>(File.ReadAllText("../../../data/trainStopsData.json"));
            Lines = JsonSerializer.Deserialize<List<Line>>(File.ReadAllText("../../../data/lineData.json"));
            LineSchedules = JsonSerializer.Deserialize<List<LineSchedule>>(File.ReadAllText("../../../data/lineScheduleData.json"));
            foreach(LineSchedule ls in LineSchedules) 
            {
                foreach (int day in ls.DaysNumbers)
                {
                    ls.Days.Add((DayOfWeek)day);
                }
                ls.TakenSeats = new Dictionary<DateTime, List<int>>();
            }
        }
    }
}

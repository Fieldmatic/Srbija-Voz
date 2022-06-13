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

        public Line GetLineById(int Id)
        {
            foreach(Line line in Lines)
            {
                if (line.Id == Id) return line;
            }
            return null;
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

        public Dictionary<SeatClass, int> getPriceByDuration(LineSchedule LineSchedule, TimeSpan StartTime, TimeSpan EndTime)
        {
            Dictionary<SeatClass, int> prices = new Dictionary<SeatClass, int>();
            TimeSpan travelDuration = EndTime - StartTime;
           
            int priceIIClass = (int)(LineSchedule.Train.PricesPerMinute[SeatClass.II] * travelDuration.TotalMinutes);
            int priceIClass = (int) (LineSchedule.Train.PricesPerMinute[SeatClass.I] * travelDuration.TotalMinutes);
            prices.Add(SeatClass.I, priceIClass);
            prices.Add(SeatClass.II, priceIIClass);
            return prices;
        }

        public Seat getSeatScheduledLine(LineSchedule lineSchedule, int seatNumber)
        {
            foreach(Wagon wagon in lineSchedule.Train.Wagons)
            {
                foreach (Seat seat in wagon.Seats)
                {
                    if (seat.Number == seatNumber) return seat;

                }
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
            Stations.Add(new Station(5, "Niš", new Location(43.315704, 21.877389)));
            Stations.Add(new Station(6, "Kraljevo", new Location(43.728099, 20.692424)));
            Stations.Add(new Station(7, "Subotica", new Location(46.092255, 19.661154)));
            Stations.Add(new Station(8, "Zrenjanin", new Location(45.380948, 20.375985)));
            Lines.Add(new Line(3, "Subotica - Niš", new List<Station> { GetStationByName("Subotica"), GetStationByName("Novi Sad"), GetStationByName("Zrenjanin"), GetStationByName("Beograd Centar"), GetStationByName("Kraljevo"), GetStationByName("Niš")}));
        } 
    }
}

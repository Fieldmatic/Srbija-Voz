using SrbijaVoz.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.IO;

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

        public Database()
        {
            Clients = new List<Client>();
            Managers = new List<Manager>();
            Trains = new List<Train>();
            Stations = new List<Station>();
            TrainStops = new List<TrainStop>();
            Lines = new List<Line>();
            LineSchedules = new List<LineSchedule>();
            LoadData();
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
            foreach(LineSchedule ls in LineSchedules) { ls.Day = (DayOfWeek)ls.DayNumber;}
        }
    }
}

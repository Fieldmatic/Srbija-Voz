using SrbijaVoz.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SrbijaVoz.dataGridRecord
{
    public class LineRecord
    {
        public int Id { get; set; }
        
        public string Train { get; set; }

        public string TrainStops { get; set; }

        public LineRecord(Line line)
        {
            Id = line.Id;
            Train = line.Train.Name;
            string trainStops = line.TrainStops[0].StartStation.Name + " ("+ line.TrainStops[0].DepartureTime.ToString()+") ";
            foreach (TrainStop stop in line.TrainStops) trainStops += " - " + stop.EndStation.Name + " (" + stop.ArrivalTime.ToString() + ") ";
            TrainStops = trainStops;
        }

    }
}

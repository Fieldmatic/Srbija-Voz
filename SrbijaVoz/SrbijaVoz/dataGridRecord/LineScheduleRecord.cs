using SrbijaVoz.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SrbijaVoz.dataGridRecord
{
    public class LineScheduleRecord
    {
        public int Id { get; set; }

        public string Day { get; set; }

        public string Train { get; set; }
        public string TrainStops { get; set; }

        public LineScheduleRecord(LineSchedule schedule)
        {
            Id = schedule.Line.Id;
            Train = schedule.Line.Train.Name;
            switch (schedule.Day.ToString())
            {
                case "Monday": 
                            Day = "Ponedeljak";
                            break;
                case "Tuesday":
                            Day = "Utorak";
                            break;
                case "Wednesday":
                            Day = "Sreda";
                            break;
                case "Thursday":
                            Day = "Cetvrtak";
                            break;
                case "Friday":
                            Day = "Petak";
                            break;
                case "Saturday":
                            Day = "Subota";
                            break;
                case "Sunday":
                            Day = "Nedelja";
                            break;
            }
            string trainStops = schedule.Line.TrainStops[0].StartStation.Name + " (" + schedule.Line.TrainStops[0].DepartureTime.ToString() + ") ";
            foreach (TrainStop stop in schedule.Line.TrainStops) trainStops += " - " + stop.EndStation.Name + " (" + stop.ArrivalTime.ToString() + ") ";
            TrainStops = trainStops;
        }
    }
}

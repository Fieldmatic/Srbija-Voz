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

        public string Days { get; set; }

        public string Train { get; set; }

        public string TrainStops { get; set; }

        public LineScheduleRecord(LineSchedule schedule)
        {
            Id = schedule.Line.Id;
            Train = schedule.Train.Name;
            foreach (DayOfWeek day in schedule.Days)
            {
                switch (day.ToString())
                {
                    case "Monday":
                        Days += "Pon";
                        break;
                    case "Tuesday":
                        Days += "Uto";
                        break;
                    case "Wednesday":
                        Days += "Sre";
                        break;
                    case "Thursday":
                        Days += "Čet";
                        break;
                    case "Friday":
                        Days += "Pet";
                        break;
                    case "Saturday":
                        Days += "Sub";
                        break;
                    case "Sunday":
                        Days += "Ned";
                        break;
                }
                if (day != schedule.Days.Last())
                    Days += ", ";
            }
            TrainStops = schedule.TrainStops[0].StartStation.Name + " (" + schedule.TrainStops[0].DepartureTime.ToString() + ") " + " - ";
            foreach (TrainStop stop in schedule.TrainStops)
            {
                if (stop == schedule.TrainStops.Last())
                    TrainStops += stop.EndStation.Name + " (" + stop.ArrivalTime.ToString() + ") ";
                else
                    TrainStops += stop.EndStation.Name + " (" + stop.ArrivalTime.ToString() + ") " + " - ";
            }
        }
    }
}

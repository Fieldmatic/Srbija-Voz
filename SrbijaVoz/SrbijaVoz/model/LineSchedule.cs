using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SrbijaVoz.model
{
    public class LineSchedule
    {
        public Line Line { get; set; }

        public List<DayOfWeek> Days { get; set; }

        public List<int> DaysNumbers { get; set; }


        public Dictionary<DateTime, List<int>> TakenSeats;

        public LineSchedule(Line line, List<int> days, Dictionary<DateTime, List<int>> takenSeats)
        {
            Line = line;
            Days = new List<DayOfWeek>();
            foreach (int day in days)
            {
                Days.Add((DayOfWeek)day);
            }
            TakenSeats = takenSeats;
        }

        public LineSchedule() 
        {
            Days = new List<DayOfWeek>();
        }

        



    }
}

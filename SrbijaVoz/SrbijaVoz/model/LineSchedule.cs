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

        public DayOfWeek Day { get; set; }

        public int DayNumber { get; set; }

        public Dictionary<DateTime, List<int>> TakenSeats;
        public LineSchedule(Line line, int day, Dictionary<DateTime, List<int>> takenSeats)
        {
            Line = line;
            Day = (DayOfWeek)day;
            TakenSeats = takenSeats;
        }

        public LineSchedule() {}

        



    }
}

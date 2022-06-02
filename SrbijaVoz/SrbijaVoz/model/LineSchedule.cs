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

        public LineSchedule(Line line, int day)
        {
            Line = line;
            Day = (DayOfWeek)day;
        }

        public LineSchedule() { }

        



    }
}

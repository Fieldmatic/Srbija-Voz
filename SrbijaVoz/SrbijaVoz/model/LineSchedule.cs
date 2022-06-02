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

        public DateOnly DepartureDate { get; set; }

        public LineSchedule(Line line, DateOnly departureDate)
        {
            Line = line;
            DepartureDate = departureDate;
        }

        public LineSchedule() {}



    }
}

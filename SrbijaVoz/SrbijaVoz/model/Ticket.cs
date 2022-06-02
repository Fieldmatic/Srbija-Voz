using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SrbijaVoz.model
{
    public class Ticket
    {
        public int Id { get; set; }
        public LineSchedule LineSchedule {get; set;}

        public Ticket(int id, LineSchedule lineSchedule)
        {
            Id = id;
            LineSchedule = lineSchedule;
        }

        public Ticket () {}

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SrbijaVoz.model
{
    public class Seat
    {
        public int Number { get; set; }
        public SeatClass SeatClass { get; set; }

        public Seat() { }

        public Seat(int number, SeatClass seatClass)
        {
            Number = number;
            SeatClass = seatClass;
        }
    }


    public enum SeatClass
    {
        I,
        II
    }
}

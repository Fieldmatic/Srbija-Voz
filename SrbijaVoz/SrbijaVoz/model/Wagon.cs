using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SrbijaVoz.model
{
    public class Wagon
    {
        public Wagon()
        {
        }

        public Wagon(int number, List<Seat> seats)
        {
            Number = number;
            Seats = seats;
        }

        public int Number { get; set; }
        public List<Seat> Seats { get; set; }

        public int getNumberOfSeatsClass(SeatClass seatClass)
        {
            int seatsNum = 0;
            foreach (Seat seat in Seats)
            {
                if (seat.SeatClass == seatClass) seatsNum ++;
            }
            return seatsNum;
        }
    }
}

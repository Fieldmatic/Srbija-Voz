using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SrbijaVoz.model
{
    public class Train
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Seat> Seats { get; set; }

        public Dictionary<SeatClass, int> Prices { get; set; }

        public Train(int id, string name, List<Seat> seats, Dictionary<SeatClass, int> Prices)
        {
            Id = id;
            Name = name;
            Seats = seats;
            this.Prices = Prices;
        }

        public Train () {}

    }
}

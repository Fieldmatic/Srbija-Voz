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

        public List<Wagon> Wagons { get; set; }
        public Dictionary<SeatClass, int> PricesPerMinute { get; set; }

        public Train(int id, string name, List<Wagon> wagons, Dictionary<SeatClass, int> prices)
        {
            Id = id;
            Name = name;
            Wagons = wagons;
            PricesPerMinute= prices;
        }

        public Train () {}

        public int getFirstClassSeatsCount()
        {
            int count = 0;
            foreach(Wagon wagon in Wagons)
            {
                foreach (Seat seat in wagon.Seats)
                {
                    if (seat.SeatClass == SeatClass.I) count ++;
                }
            }
            return count;

        }

        public int getSecondClassSeatsCount()
        {
            int count = 0;
            foreach (Wagon wagon in Wagons)
            {
                foreach (Seat seat in wagon.Seats)
                {
                    if (seat.SeatClass == SeatClass.II) count ++;
                }
            }
            return count;

        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

    }
}

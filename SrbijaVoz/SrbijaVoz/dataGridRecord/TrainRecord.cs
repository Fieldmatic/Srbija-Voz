using SrbijaVoz.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SrbijaVoz.dataGridRecord
{
    public class TrainRecord
    {
     
        public int Id { get; set; }
        public string Name { get; set; }
        public int FirstClassSeatsNumber { get; set; }
        public int SecondClassSeatsNumber { get; set; }

        public int FirstClassPrice { get; set; }

        public int WagonsNumber { get; set; }

        public int SecondClassPrice { get; set; }

        public TrainRecord(Train train)
        {
            Id = train.Id;
            Name = train.Name;
            FirstClassSeatsNumber = train.getFirstClassSeatsCount();
            SecondClassSeatsNumber = train.getSecondClassSeatsCount();
            WagonsNumber = train.Wagons.Count();
            FirstClassPrice = train.PricesPerMinute[SeatClass.I];
            SecondClassPrice = train.PricesPerMinute[SeatClass.II];
        }

        public TrainRecord() { }

        public override string? ToString()
        {
            return base.ToString();
        }
    }
}

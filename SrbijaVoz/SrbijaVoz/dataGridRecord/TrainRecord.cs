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

        public int SecondClassPrice { get; set; }

        public TrainRecord(Train train)
        {
            Id = train.Id;
            Name = train.Name;
            FirstClassSeatsNumber = train.getFirstClassSeatsCount();
            SecondClassSeatsNumber = train.getSecondClassSeatsCount();
            FirstClassPrice = train.Prices[SeatClass.I];
            SecondClassPrice = train.Prices[SeatClass.II];
        }

        public TrainRecord() { }

        public override string? ToString()
        {
            return base.ToString();
        }
    }
}

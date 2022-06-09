using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SrbijaVoz.dataGridRecord
{
    public class TicketRecord
    {
        public TicketRecord(string username, string line, string date, string departure, string arrival, string train, string price)
        {
            Username = username;
            Line = line;
            Date = date;
            Departure = departure;
            Arrival = arrival;
            Train = train;
            Price = price;
        }

        public String Username { get; set; }
        public String Line { get; set; }
        public String Date { get; set; }
        public String Departure { get; set; }
        public String Arrival { get; set; }
        public String Train { get; set; }
        public String Price { get; set; }
    }
}

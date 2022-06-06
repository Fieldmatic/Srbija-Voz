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
        public TicketStatus TicketStatus { get; set; }
        public Seat Seat { get; set; }
        public Client Client { get; set; }
        public double Price { get; set; }

        public Station StartStation { get; set; }

        public Station ExitStation { get; set; }

        public TimeSpan Departure { get; set; }
        public TimeSpan Arrival { get; set; }

        public DateTime Date { get; set; }


        public Ticket () {}

        public Ticket(LineSchedule lineSchedule, TicketStatus ticketStatus, Seat seat, Client client, double price, Station startStation, Station exitStation, TimeSpan departure, TimeSpan arrival, DateTime date)
        {
            LineSchedule = lineSchedule;
            TicketStatus = ticketStatus;
            Seat = seat;
            Client = client;
            Price = price;
            StartStation = startStation;
            ExitStation = exitStation;
            Departure = departure;
            Arrival = arrival;
            Date = date;
        }
    }


    public enum TicketStatus
    {
        RESERVED,
        BOUGHT
    }
}

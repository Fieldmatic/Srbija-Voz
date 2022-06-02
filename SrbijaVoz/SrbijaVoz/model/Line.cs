using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SrbijaVoz.model
{
    public class Line
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<TrainStop> TrainStops { get; set; }
        public Train Train { get; set; }

        public Line(int id, string name, List<TrainStop> trainStops, Train train)
        {
            Id = id;
            Name = name;
            TrainStops = trainStops;
            Train = train;
        }

        public Line () {}




    }
}

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

        public List<Station> Stations { get; set; }

        public Line(int id, string name, List<Station> stations)
        {
            Id = id;
            Name = name;
            Stations = stations;
        }

        public Line () {}

    }
}

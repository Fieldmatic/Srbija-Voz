using Microsoft.Maps.MapControl.WPF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SrbijaVoz.model
{
    public class Station
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public Location Location { get; set; }

        public Station(int id, string name, Location location)
        {
            Id = id;
            Name = name;
            Location = location;
        }

        public Station() {}


    }
}

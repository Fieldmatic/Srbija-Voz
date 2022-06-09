using SrbijaVoz.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SrbijaVoz.dataGridRecord
{
    public class LineRecord
    {
        public int Id { get; set; }

        public string Stations { get; set; }

        public LineRecord(Line line)
        {
            Id = line.Id;
            Stations = "";
            foreach (Station station in line.Stations)
            {
                if (station == line.Stations.Last())
                    Stations += station.Name;
                else
                    Stations += station.Name + " - ";
            }

        }

    }
}

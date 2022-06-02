﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SrbijaVoz.model
{
    public class TrainStop
    {
        public Station StartStation { get; set; }
        public Station EndStation { get; set; }
        public TimeOnly DepartureTime { get; set; }
        public TimeOnly ArrivalTime { get; set; }

        public TrainStop(Station startStation, Station endStation, TimeOnly departureTime, TimeOnly arrivalTime)
        {
            StartStation = startStation;
            EndStation = endStation;
            DepartureTime = departureTime;
            ArrivalTime = arrivalTime;
        }

        public TrainStop () {}


    }
}

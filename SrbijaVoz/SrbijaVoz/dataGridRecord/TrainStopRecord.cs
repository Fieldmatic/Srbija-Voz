using SrbijaVoz.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SrbijaVoz.dataGridRecord
{
    public class TrainStopRecord
    {
        public string StartStaionWithTime { get; set; }

        public string EndStaionWithTime { get; set; }

        public TrainStopRecord(TrainStop trainStop)
        {
            StartStaionWithTime = trainStop.StartStation.Name + " (" + trainStop.DepartureTime.ToString() + ") ";
            EndStaionWithTime = trainStop.EndStation.Name + " (" + trainStop.ArrivalTime.ToString() + ") ";
        }
    }
}

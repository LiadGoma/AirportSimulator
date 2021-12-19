using AirportServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AirportServer.Bl
{
    public class StationCompletedEventArgs:EventArgs
    {
        public Airplane AirplaneInStation { get; set; }
        public StationCompletedEventArgs(Airplane airplane)
        {
            AirplaneInStation = airplane;
        }
    }
}

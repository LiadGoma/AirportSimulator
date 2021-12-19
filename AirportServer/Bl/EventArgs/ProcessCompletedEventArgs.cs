using AirportServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AirportServer.Bl
{
    public class ProcessCompletedEventArgs:EventArgs
    {
        public Airplane AirplaneInRoute { get; set; }
        public ProcessCompletedEventArgs(Airplane airplane)
        {
            AirplaneInRoute = airplane;
        }
    }
}

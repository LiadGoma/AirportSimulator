using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AirportServer.Models
{
    public class Airport
    {
        public int Id { get; set; }
        public DateTime ImageTime { get; set; }
        public List<Station> AirportStations { get; set; }
        public List<Landing> Landings { get; set; }
        public List<Departure> Departures { get; set; }
        
    }
}

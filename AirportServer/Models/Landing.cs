using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AirportServer.Models
{
    public class Landing
    {
        public int Id { get; set; }
        public int PlaneId { get; set; }
        public int AirportId { get; set; }
        public DateTime EndingTime { get; set; }
        public bool IsFinished { get; set; }
    }

 
}

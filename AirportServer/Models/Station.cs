using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;

namespace AirportServer.Models
{
    public class Station
    {
        public int Id { get; set; }
        public StationType Type { get; set; }
        public int AirportId { get; set; }
        public bool IsStationFree { get; set; }
        public bool IsStationCompleted { get; set; }
        public int AirplaneId { get; set; }
        [NotMapped]
        public Airplane AirplaneStation { get; set; }
        public TimeSpan MinTimeToWaitInStation { get; set; }
       
    }
    public enum StationType
    {
        LandingRequest, DropingLatitudes, OpenWheels, Runway, DriveWayToGarage, Garage1, Garage2, DriveWayToRunway, TakeOff
    }
}

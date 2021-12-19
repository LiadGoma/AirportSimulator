using AirportServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AirportServer.Bl
{
    public interface ILogic
    {
        event EventHandler<List<Station>> NotifyMovementToHub;
        event EventHandler<List<Landing>> NotifyNewLandingToHub;
        event EventHandler<List<Departure>> NotifyNewDepartureToHub;
        List<Station> Start();
        List<Landing> GetLandings();
        List<Departure> GetDepartures();
        void NewLanding();
        void NewDeparture();
    }
}

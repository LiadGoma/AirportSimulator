using AirportServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AirportServer.repositories
{
    public interface IAirportRepository
    {
        Task<List<Station>> GetLandingRoute();
        Task<List<Station>> GetDepartureRoute();
        Task<List<Landing>> GetLandings();
        Task<List<Departure>> GetDepartures();
        Task<Airport> GetAirportAsync();
        Task<List<Station>> GetStations();
        Task<Airplane> SaveAirplane(Airplane newStation);
        Task SaveStation(Station station);
        Task SaveAllStation(List<Station> stations);
        Task SaveLanding(Landing newlanding);
        Task SaveDeparture(Departure newDeparture);
    }
}

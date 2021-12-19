using AirportServer.Data;
using AirportServer.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AirportServer.repositories
{
    public class AirportRepository : IAirportRepository
    {
        private readonly AirportContext context;
        SemaphoreSlim semaphore = new SemaphoreSlim(1);
        public AirportRepository(AirportContext context)
        {
            this.context = context;


        }

        public Task<Airport> GetAirportAsync()
        {
            var airport = context.Airport.Include(c => c.Landings).Include(c => c.Departures).Include(c => c.AirportStations);
            return (Task<Airport>)airport;
        }

        public async Task<List<Station>> GetDepartureRoute()
        {
            try
            {
                await semaphore.WaitAsync();
                var stations = context.Stations.ToList();
                var departureRoute = stations.FindAll(s => s.Id == 6 || s.Id == 7 || s.Id == 8 || s.Id == 4);
                return departureRoute;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                semaphore.Release();

            }

        }

        public async Task<List<Departure>> GetDepartures()
        {
            try
            {
                await semaphore.WaitAsync();
                var departures = context.Departures.ToList();
                return departures;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                semaphore.Release();

            }

        }

        public async Task<List<Station>> GetLandingRoute()
        {
            try
            {
                await semaphore.WaitAsync();
                var stations = context.Stations.ToList();
                var landingRoute = stations.FindAll(s => s.Id == 1 || s.Id == 2 || s.Id == 3 || s.Id == 4 || s.Id == 5 || s.Id == 6 || s.Id == 7);
                return landingRoute;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                semaphore.Release();

            }

        }

        public async Task<List<Landing>> GetLandings()
        {
            try
            {
                await semaphore.WaitAsync();
                var landings = context.Landings.ToList();
                return landings;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                semaphore.Release();

            }

        }

        public async Task<List<Station>> GetStations()
        {
            try
            {
                await semaphore.WaitAsync();
                var stations = context.Stations.ToList();
                return stations;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                semaphore.Release();

            }
            
        }

        public async Task<Airplane> SaveAirplane(Airplane newAirplane)
        {

            try
            {
                await semaphore.WaitAsync();
                context.Airplanes.Add(newAirplane);
                context.SaveChanges();
                return context.Airplanes.OrderBy(c => c.Id).Last();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return null;
            }
            finally
            {
                semaphore.Release();

            }


        }

        public async Task SaveAllStation(List<Station> stations)
        {
            await Task.Run(async () =>
           {
               foreach (var station in stations)
               {
                   await SaveStation(station);
               }
           });


        }

        public async Task SaveDeparture(Departure newDeparture)
        {
            try
            {
                await semaphore.WaitAsync();
                context.Departures.Add(newDeparture);
                context.SaveChanges();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);

            }
            finally
            {
                semaphore.Release();
            }
        }

        public async Task SaveLanding(Landing newlanding)
        {
            try
            {
                await semaphore.WaitAsync();
                context.Landings.Add(newlanding);
                context.SaveChanges();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);

            }
            finally
            {
                semaphore.Release();
            }

        }

        public async Task SaveStation(Station station)
        {
            try
            {
                await semaphore.WaitAsync();
                var stationInDb = context.Stations.SingleOrDefault(s => s.Id == station.Id);
                stationInDb.IsStationFree = station.IsStationFree;
                stationInDb.IsStationCompleted = station.IsStationCompleted;
                stationInDb.AirplaneId = station.AirplaneId;
                context.SaveChanges();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);

            }
            finally
            {
                semaphore.Release();
            }

        }

     
    }
}

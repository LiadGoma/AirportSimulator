using AirportServer.Hubs;
using AirportServer.Models;
using AirportServer.repositories;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace AirportServer.Bl
{
    public class Logic : ILogic
    {
        LandingProcess landingProcess;
        DepartureProcess departureProcess;
        private readonly IAirportRepository repo;
        List<StationManager> landingStations = new List<StationManager>();
        List<StationManager> departureStations = new List<StationManager>();
        List<StationManager> Stations = new List<StationManager>();
        public event EventHandler<List<Station>> NotifyMovementToHub;
        public event EventHandler<List<Landing>> NotifyNewLandingToHub;
        public event EventHandler<List<Departure>> NotifyNewDepartureToHub;

        IHubContext<AirportHub> hub;

        public Logic(IAirportRepository repo, IHubContext<AirportHub> hub)
        {
            this.repo = repo;
            this.hub = hub;
            var stations = repo.GetStations().Result;
            Stations = ConvertToStationManager(stations);
            departureStations = Stations.FindAll(s =>  s.Id == 7 || s.Id == 8).ToList();
            departureStations.Add(Stations.Find(s => s.Id == 4));
            landingStations = Stations.FindAll(s => s.Id == 1 || s.Id == 2 || s.Id == 3 || s.Id == 4 || s.Id == 5 || s.Id == 6 ).ToList();


        }
        public async void NewLanding()
        {
            Airplane newPlane = new Airplane { TimeCreatedAt = DateTime.Now };
            newPlane = repo.SaveAirplane(newPlane).Result;
            newPlane.IsLanding = true;
            landingProcess = new LandingProcess(landingStations);
            landingProcess.NotifyMovementToLogic += NotifyMovement;
            landingProcess.LandingProcessOverEvent += LandingSuccededAsync;
            await landingProcess.StartMoveAsync(newPlane);

        }
        public async void NewDeparture()
        {
            Airplane newPlane = new Airplane { TimeCreatedAt = DateTime.Now };
            newPlane = repo.SaveAirplane(newPlane).Result;
            newPlane.IsLanding = false;
            departureProcess = new DepartureProcess(departureStations);
            departureProcess.NotifyMovementToLogic += NotifyMovement;
            departureProcess.DepartureProcessOverEvent += DepartureSuccededAsync;
            await departureProcess.StartMoveAsync(newPlane);

        }

        private async void DepartureSuccededAsync(object sender, ProcessCompletedEventArgs e)
        {
            Departure newDeparture = new Departure { IsFinished = true, PlaneId = e.AirplaneInRoute.Id, AirportId = 1, EndingTime = DateTime.Now };
            await repo.SaveDeparture(newDeparture);
            NotifyNewDepartureToHub?.Invoke(this, repo.GetDepartures().Result);
            try
            {
                await alertDeparturesToClient();
            }
            catch (Exception)
            { 
                throw;
            }
            
        }

        private async Task alertDeparturesToClient()
        {
            await hub.Clients.All.SendAsync("SendDepartures", repo.GetDepartures().Result);
        }

        private async void NotifyMovement(object sender, EventArgs e)
        {
            await AlertClient();
            NotifyMovementToHub?.Invoke(this, ConvertStationManagerToStationModel(Stations));
            //await hub.Clients.All.SendAsync("SendMovement", ConvertStationManagerToStationModel(landingStations));
            //await hub.Clients.All.SendAsync("wow", "fuck");

        }

        private async Task AlertClient()
        {
            await hub.Clients.All.SendAsync("SendMovement", ConvertStationManagerToStationModel(Stations));
        }

        private List<Station> ConvertStationManagerToStationModel(List<StationManager> landingRoute)
        {
            List<Station> stations = new List<Station>();
            foreach (var station in landingRoute)
            {
                if (station.AirplaneInStation != null)
                {
                    station.AirplaneId = station.AirplaneInStation.Id;
                    station.AirplaneStation = station.AirplaneInStation;
                }
                else
                {
                    station.AirplaneId = -1;
                }
                stations.Add(station);
            }
            return stations;
        }

        private async void LandingSuccededAsync(object sender, ProcessCompletedEventArgs e)
        {
            Landing newLanding = new Landing { IsFinished = true, PlaneId = e.AirplaneInRoute.Id, AirportId = 1, EndingTime = DateTime.Now };
            await repo .SaveLanding(newLanding);
            NotifyNewLandingToHub?.Invoke(this, repo.GetLandings().Result);
            await alertLandingToClient();
            
        }

        private async Task alertLandingToClient()
        {
            await hub.Clients.All.SendAsync("SendLandings", repo.GetLandings().Result);
        }

        public List<Station> Start()
        {
            var stations = repo.GetStations().Result;
            return stations;
        }
        public List<Landing> GetLandings()
        {
            var landings = repo.GetLandings().Result.ToList();
            return landings;
        }
        public List<Departure> GetDepartures()
        {
            var departures = repo.GetDepartures().Result.ToList();
            return departures;
        }

        private List<StationManager> ConvertToStationManager(List<Station> stations)
        {
            List<StationManager> stationManagers = new List<StationManager>();
            foreach (var station in stations)
            {

                stationManagers.Add(new StationManager(station));
            }
            return stationManagers;
        }
    }
}

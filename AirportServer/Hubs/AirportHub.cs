using AirportServer.Bl;
using AirportServer.Models;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace AirportServer.Hubs
{
    public class AirportHub : Hub
    {
        private readonly ILogic logic;
        public AirportHub(ILogic logic)
        {
            this.logic = logic;
            this.logic.NotifyMovementToHub += async (s, e) => SendMovement(e);
            this.logic.NotifyNewLandingToHub += async (s, e) => SendLandings(e);
            this.logic.NotifyNewDepartureToHub += async (s, e) => SendDepartures(e);
        }
        public async Task SendMovement(List<Station> e)
        {

            await Clients.All.SendAsync("SendMovement", e);
        }
        public async Task SendLandings(List<Landing> e)
        {
            await Clients.All.SendAsync("SendLandings", e);
        }
        public async Task SendDepartures(List<Departure> e)
        {
            await Clients.All.SendAsync("SendDepartures", e);
        }


    }
}

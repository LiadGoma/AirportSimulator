using AirportServer.Bl;
using AirportServer.Models;
using DepartureSimulator;
using LandingSimulator;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AirportServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AirportController : ControllerBase
    {
        private readonly ILogic logic;
        private readonly ILandingSimulator landingSimulator;
        private readonly IDepartureSimulator departureSimulator;
        public AirportController(ILogic logic, ILandingSimulator landingSimulator,IDepartureSimulator departureSimulator)
        {
            this.logic = logic;
            this.landingSimulator = landingSimulator;
            this.departureSimulator = departureSimulator;
        }

        [Route("Start")]
        public List<Station> Start()
        {
            var result=logic.Start();
            landingSimulator.Start();
            departureSimulator.Start();
            return result;
        }
        [Route("Stop")]
        public void Stop()
        {
            departureSimulator.Stop();
            landingSimulator.Stop();
        }
        [Route("Continue")]
        public void Continue()
        {
            departureSimulator.Start();
            landingSimulator.Start();
        }
        [Route("GetLandings")]
        public List<Landing> GetLandings()
        {
            var landings= logic.GetLandings();
            return landings;
        }
        [Route("GetDepartures")]
        public List<Departure> GetDepartures()
        {
            var departures = logic.GetDepartures();
            return departures;
        }
        [Route("NewLanding")]
        public void NewLanding()
        {
            logic.NewLanding(); 
        }
        [Route("NewDeparture")]
        public void newDeparture()
        {
            logic.NewDeparture();
        }
       
        [Route("SetLandingSimulatorMinus")]
        [HttpPost]
        public void SetLandingSimulatorMinus()
        {
            landingSimulator.SetTime(-1);

        }
        [Route("SetLandingSimulatorAdd")]
        [HttpPost]
        public void SetLandingSimulatorAdd()
        {
            landingSimulator.SetTime(1);

        }
        [HttpPost]
        [Route("SetDepartureSimulatorAdd")]
        public void SetDepartureSimulatorAdd()
        {
            departureSimulator.SetTime(1);
        }
        [HttpPost]
        [Route("SetDepartureSimulatorMinus")]
        public void SetDepartureSimulatorMinus()
        {
            departureSimulator.SetTime(-1);
        }


    }
}

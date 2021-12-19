using AirportServer.Models;
using AirportServer.repositories;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AirportServer.Bl
{
    public class LandingProcess
    {
        public event EventHandler SaveChangesEvent;
        public event EventHandler<ProcessCompletedEventArgs> LandingProcessOverEvent;
        public event EventHandler<EventArgs> NotifyMovementToLogic;
        public List<StationManager> LandingRoute { get; set; }
        public Airplane AirplaneInRoute { get; set; }
        public int CurrentStationIndex { get; set; }
        public Station CurrentStation { get; set; }

        public LandingProcess(List<StationManager> stations)
        {

            LandingRoute = stations;
            CurrentStationIndex = -1;
            int i;
            for (i = 0; i < LandingRoute.Count - 1; i++)
            {
                LandingRoute[i].StationMissionCompletedEvent += StationMissionCompleted;
                LandingRoute[i].NotifyMovement += NotifyMovementToHub;
                LandingRoute[i].RegisterToNextStation(LandingRoute[i + 1]);
            }
            LandingRoute[i].StationMissionCompletedEvent += StationMissionCompleted;
        }

        private void NotifyMovementToHub(object sender, EventArgs e)
        {
            NotifyMovementToLogic.Invoke(this,new EventArgs());
        }

      

        private async void StationMissionCompleted(object sender, StationCompletedEventArgs e)
        {
            //SaveChangesEvent?.Invoke(this, new EventArgs());
            if (e?.AirplaneInStation?.Id == AirplaneInRoute.Id)
            {
                if (CurrentStationIndex + 1 < LandingRoute.Count)
                {
                    CurrentStationIndex++;
                    await LandingRoute[CurrentStationIndex].EnterStation(AirplaneInRoute);
                }
                else
                {
                    LandingRoute[CurrentStationIndex].MoveToTerminal();
                    LandingProcessOverEvent?.Invoke(this, new ProcessCompletedEventArgs(AirplaneInRoute));
                }
            }


        }

        public async Task StartMoveAsync(Airplane airplane)
        {
            AirplaneInRoute = airplane;
            Debug.WriteLine($"airplane {AirplaneInRoute.Id}");
            CurrentStationIndex = 0;
            await LandingRoute[0]?.EnterStation(AirplaneInRoute);
        }

    }
}

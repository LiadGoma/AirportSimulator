using AirportServer.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace AirportServer.Bl
{
    public class DepartureProcess
    {
        public event EventHandler SaveChangesEvent;
        public event EventHandler<ProcessCompletedEventArgs> DepartureProcessOverEvent;
        public event EventHandler<EventArgs> NotifyMovementToLogic;
        public List<StationManager> DepartureRoute { get; set; }
        public Airplane AirplaneInRoute { get; set; }
        public int CurrentStationIndex { get; set; }
        public Station CurrentStation { get; set; }

        public DepartureProcess(List<StationManager> stations)
        {

            DepartureRoute = stations;
            CurrentStationIndex = -1;
            int i;
            for (i = 0; i < DepartureRoute.Count - 1; i++)
            {
                DepartureRoute[i].StationMissionCompletedEvent += StationMissionCompleted;
                DepartureRoute[i].NotifyMovement += NotifyMovementToHub;
                DepartureRoute[i].RegisterToNextStation(DepartureRoute[i + 1]);
            }
            DepartureRoute[i].StationMissionCompletedEvent += StationMissionCompleted;
        }

        private void NotifyMovementToHub(object sender, EventArgs e)
        {
            NotifyMovementToLogic.Invoke(this, new EventArgs());
        }



        private async void StationMissionCompleted(object sender, StationCompletedEventArgs e)
        {
            //SaveChangesEvent?.Invoke(this, new EventArgs());
            if (e?.AirplaneInStation?.Id == AirplaneInRoute.Id)
            {
                if (CurrentStationIndex + 1 < DepartureRoute.Count)
                {
                    CurrentStationIndex++;
                    await DepartureRoute[CurrentStationIndex].EnterStation(AirplaneInRoute);
                }
                else
                {
                    DepartureRoute[CurrentStationIndex].MoveToTerminal();
                    DepartureProcessOverEvent?.Invoke(this, new ProcessCompletedEventArgs(AirplaneInRoute));
                }
            }


        }

        public async Task StartMoveAsync(Airplane airplane)
        {
            AirplaneInRoute = airplane;
            Debug.WriteLine($"airplane {AirplaneInRoute.Id}");
            CurrentStationIndex = 0;
            await DepartureRoute[0]?.EnterStation(AirplaneInRoute);
        }

    }
}

using AirportServer.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace AirportServer.Bl
{
    public class StationManager : Station
    {
        System.Timers.Timer timer;
        object locker = new object();
        SemaphoreSlim semaphore;
        //Semaphore semaphore;
        public event EventHandler<StationCompletedEventArgs> AirplaneMovedEvent;
        public event EventHandler<StationCompletedEventArgs> StationMissionCompletedEvent;
        public event EventHandler<EventArgs> NotifyMovement;
        public Airplane AirplaneInStation { get; set; }

        public StationManager(Station station)
        {
            semaphore = new SemaphoreSlim(1);
            //semaphore = new Semaphore(1, 1);
            Id = station.Id;
            IsStationCompleted = station.IsStationCompleted;
            IsStationFree = station.IsStationFree;
            MinTimeToWaitInStation = station.MinTimeToWaitInStation;
            Type = station.Type;
        }
        public void RegisterToNextStation(StationManager nextStation)
        {
            nextStation.AirplaneMovedEvent += AirplaneSuccessfulyMoved;
        }

        private void AirplaneSuccessfulyMoved(object sender, StationCompletedEventArgs e)
        {
            if (AirplaneInStation == null|| e?.AirplaneInStation?.Id!=AirplaneInStation?.Id)
                return;
            Debug.WriteLine($"semaphore count:{semaphore.CurrentCount} in station {Id}");
            AirplaneInStation = null;
            IsStationFree = true;
            IsStationCompleted = false;
            semaphore.Release();

        }
        public void MoveToTerminal()
        {

            Debug.WriteLine($"{AirplaneInStation.Id} moved to terminal");
            AirplaneInStation = null;
            IsStationFree = true;
            IsStationCompleted = false;
            semaphore.Release();
        }

        public async Task EnterStation(Airplane airplane)
        {
            await Task.Run(async () =>
           {
               await semaphore.WaitAsync();
               {
                   IsStationFree = false;
                   AirplaneInStation = airplane;
                   Debug.WriteLine($"were in {AirplaneInStation.Id}, in station {Id}");
                   timer = new System.Timers.Timer(MinTimeToWaitInStation.TotalMilliseconds);
                   timer.Elapsed += StationCompleted;
                   timer.Start();
                   AirplaneMovedEvent?.Invoke(this, new StationCompletedEventArgs(AirplaneInStation));
                   NotifyMovement?.Invoke(this, new EventArgs());
               }

           });

        }


        private void StationCompleted(object sender, ElapsedEventArgs e)
        {
            //Debug.WriteLine(Id + ", " + AirplaneId);
            IsStationCompleted = true;
            timer.Stop();
            StationMissionCompletedEvent?.Invoke(this, new StationCompletedEventArgs(AirplaneInStation));


        }
    }
}

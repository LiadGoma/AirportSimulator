using System;
using System.Net.Http;
using System.Timers;

namespace DepartureSimulator
{
    public class Simulator : IDepartureSimulator
    {
        HttpClient client = new HttpClient();
        Timer timer;
        public Simulator()
        {
            timer = new Timer(6000);
            timer.Elapsed += CreateNewDeparture;
        }

        public void SetTime(int seconds)
        {
            if (timer.Interval + seconds * 1000 >= 1000)
                timer.Interval += seconds * 1000;
        }

        public void Start()
        {
            timer.Start();
        }

        public void Stop()
        {
            timer.Stop();
        }

        private void CreateNewDeparture(object sender, ElapsedEventArgs e)
        {
            client.GetAsync("https://localhost:44320/api/airport/newDeparture");
        }
    }
}

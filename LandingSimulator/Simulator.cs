using System;
using System.Diagnostics;
using System.Net.Http;
using System.Timers;

namespace LandingSimulator
{
    public class Simulator : ILandingSimulator
    {
        HttpClient client = new HttpClient();
        Timer timer;
        public Simulator()
        {
            timer = new Timer(8000);
            timer.Elapsed += CreateNewLanding;
        }

        public void SetTime(int seconds)
        {
            if (timer.Interval+ seconds * 1000 >= 1000)
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

        private void CreateNewLanding(object sender, ElapsedEventArgs e)
        {
            client.GetAsync("https://localhost:44320/api/airport/newLanding");
        }
    }
}

using AirportServer.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AirportServer.Data
{
    public class AirportContext : DbContext
    {
        public AirportContext(DbContextOptions<AirportContext> options) : base(options) { }
        public DbSet<Airport> Airport { get; set; }
        public DbSet<Landing> Landings { get; set; }
        public DbSet<Departure> Departures { get; set; }
        public DbSet<Airplane> Airplanes { get; set; }
        public DbSet<Station> Stations { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            List<Station> stations = new List<Station>();
            stations.Add(new Station { Id = 1, AirplaneId = -1, Type = StationType.LandingRequest, MinTimeToWaitInStation = TimeSpan.FromSeconds(2), IsStationFree = true });
            stations.Add(new Station { Id = 2, AirplaneId = -1, Type = StationType.DropingLatitudes, MinTimeToWaitInStation = TimeSpan.FromSeconds(4), IsStationFree = true });
            stations.Add(new Station { Id = 3, AirplaneId = -1, Type = StationType.OpenWheels, MinTimeToWaitInStation = TimeSpan.FromSeconds(2), IsStationFree = true });
            stations.Add(new Station { Id = 4, AirplaneId = -1, Type = StationType.Runway, MinTimeToWaitInStation = TimeSpan.FromSeconds(7), IsStationFree = true });
            stations.Add(new Station { Id = 5, AirplaneId = -1, Type = StationType.DriveWayToGarage, MinTimeToWaitInStation = TimeSpan.FromSeconds(3), IsStationFree = true });
            stations.Add(new Station { Id = 6, AirplaneId = -1, Type = StationType.Garage1, MinTimeToWaitInStation = TimeSpan.FromSeconds(1), IsStationFree = true });
            stations.Add(new Station { Id = 7, AirplaneId = -1, Type = StationType.Garage2, MinTimeToWaitInStation = TimeSpan.FromSeconds(1), IsStationFree = true });
            stations.Add(new Station { Id = 1, AirplaneId = -1, Type = StationType.LandingRequest, MinTimeToWaitInStation = TimeSpan.FromSeconds(3), IsStationFree = true });
            stations.Add(new Station { Id = 8, AirplaneId = -1, Type = StationType.TakeOff, MinTimeToWaitInStation = TimeSpan.FromSeconds(2), IsStationFree = true });

            modelBuilder.Entity<Station>().HasData(
            new Station { Id = 1, AirportId = 1, Type = StationType.LandingRequest, MinTimeToWaitInStation = TimeSpan.FromSeconds(4), IsStationFree = true },
            new Station { Id = 2, AirportId = 1, Type = StationType.DropingLatitudes, MinTimeToWaitInStation = TimeSpan.FromSeconds(4), IsStationFree = true },
            new Station { Id = 3, AirportId = 1, Type = StationType.OpenWheels, MinTimeToWaitInStation = TimeSpan.FromSeconds(3), IsStationFree = true },
            new Station { Id = 4, AirportId = 1, Type = StationType.Runway, MinTimeToWaitInStation = TimeSpan.FromSeconds(2), IsStationFree = true },
            new Station { Id = 5, AirportId = 1, Type = StationType.DriveWayToGarage, MinTimeToWaitInStation = TimeSpan.FromSeconds(5), IsStationFree = true },
            new Station { Id = 6, AirportId = 1, Type = StationType.Garage1, MinTimeToWaitInStation = TimeSpan.FromSeconds(2), IsStationFree = true },
            new Station { Id = 7, AirportId = 1, Type = StationType.Garage2, MinTimeToWaitInStation = TimeSpan.FromSeconds(2), IsStationFree = true },
            new Station { Id = 8, AirportId = 1, Type = StationType.DriveWayToRunway, MinTimeToWaitInStation = TimeSpan.FromSeconds(5), IsStationFree = true },
            new Station { Id = 9, AirportId = 1, Type = StationType.TakeOff, MinTimeToWaitInStation = TimeSpan.FromSeconds(2), IsStationFree = true }
                );

            modelBuilder.Entity<Airport>().HasData(
                new Airport { Id = 1 }
            );
        }

    }

}

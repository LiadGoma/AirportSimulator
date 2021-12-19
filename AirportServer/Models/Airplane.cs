using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AirportServer.Models
{
    public class Airplane
    {
        public int Id { get; set; }
        public DateTime TimeCreatedAt { get; set; }
        public int CurrentPosition { get; set; }
        [NotMapped]
        public bool IsLanding { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DepartureSimulator
{
    public interface IDepartureSimulator
    {
        void Start();
        void Stop();
        void SetTime(int seconds);

    }
}

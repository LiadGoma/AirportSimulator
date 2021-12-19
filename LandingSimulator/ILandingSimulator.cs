using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LandingSimulator
{
    public interface ILandingSimulator
    {
        void Start();
        void Stop();
        void SetTime(int seconds);
    }
}

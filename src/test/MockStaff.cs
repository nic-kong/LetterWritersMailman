using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LetterWritersMailman.test
{
    class MockStaff : IStaff
    {
        public MovingWindows MovingWindows { get; set; }

        public int Productivity
        {
            get { return MovingWindows.Total; }
        }

        public void BossOrder()
        {
            MovingWindows.PrevWindowIncrement();
        }

        public void Rest()
        {
            MovingWindows.Add(0);
        }
    }
}

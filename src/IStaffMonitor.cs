using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LetterWritersMailman
{
    interface IStaffMonitor
    {
        int Productivity { get; }

        void BossOrder();
        void Rest();
    }
}

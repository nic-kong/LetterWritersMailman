using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LetterWritersMailman
{
    class MovingWindows : FixedSizeCircularArray<int>
    {
        public MovingWindows(int size) : base(size) { }

        public int Total
        {
            get { return this.array.Sum(); }
        }
    }
}

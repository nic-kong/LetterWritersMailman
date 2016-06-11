using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LetterWritersMailman
{
    class FixedSizeCircularArray<T>
    {
        protected readonly T[] array;
        private uint next;

        public FixedSizeCircularArray(uint size)
        {
            this.array = new T[size];
        }

        public void Add(T value)
        {
            uint pos = next % (uint)array.Length;
            array[pos] = value;
            ++next;
        }

        public T this[int index]
        {
            get { return array[At(index)]; }
            set { array[At(index)] = value; }
        }

        private ulong At(int index)
        {
            return (next + (uint)index) % (uint)array.Length;
        }
    }
}

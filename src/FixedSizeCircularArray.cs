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
        private ulong next;

        public FixedSizeCircularArray(int size)
        {
            this.array = new T[size];
        }

        public void Add(T value)
        {
            ulong pos = next % (ulong)array.Length;
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
            return (next + (ulong)index) % (ulong)array.Length;
        }
    }
}

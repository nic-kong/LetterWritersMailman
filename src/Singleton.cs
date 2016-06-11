using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LetterWritersMailman
{
    class Singleton<T> where T : class, new()
    {
        private static readonly T instance = new T();

        private Singleton() { }

        public static T Instance
        {
            get { return instance; }
        }
    }
}

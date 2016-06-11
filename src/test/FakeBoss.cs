using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LetterWritersMailman.test
{
    class FakeBoss : Boss
    {
        public FakeBoss() : base(60000) { }

        public void CallAssignJob()
        {
            AssignJob(null);
        }
    }
}

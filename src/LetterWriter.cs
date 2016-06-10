using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LetterWritersMailman
{
    class LetterWriter : BaseThread, IStaff
    {
        public string writerName;
        private MovingWindows productivity;

        public LetterWriter(string writerName, int roundPerCycle) : base()
        {
            this.writerName = writerName;
            this.productivity = new MovingWindows(roundPerCycle);
        }

        public int Productivity
        {
            get { return productivity.Total; }
        }

        public void BossOrder()
        {
            productivity.OverwritePrevWindow(1);
            Event.Set();
        }

        public void Rest()
        {
            productivity.Add(0);
        }

        protected override void DoWork()
        {
            var letter = new Letter(writerName)
            {
                SendTime = DateTime.Now,
            };
            Singleton<Mailman>.Instance.Post(letter);
        }
    }
}

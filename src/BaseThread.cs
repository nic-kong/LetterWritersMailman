using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LetterWritersMailman
{
    abstract class BaseThread
    {
        private readonly Thread thread;
        protected AutoResetEvent Event { get; private set; }
        private volatile bool shouldStop;

        protected BaseThread()
        {
            thread = new Thread(new ThreadStart(this.Run));
            Event = new AutoResetEvent(false);
        }

        public void Start()
        {
            thread.Start();
        }

        public void Stop()
        {
            shouldStop = true;
            Event.Set();
        }

        public void Join()
        {
            thread.Join();
        }

        public bool IsAlive
        {
            get { return thread.IsAlive; }
        }

        protected void Run()
        {
            while (Event.WaitOne())
            {
                if (shouldStop) break;

                DoWork();
            }
            Console.WriteLine("worker thread ({0}): terminating gracefully.", Thread.CurrentThread.Name);
        }

        protected abstract void DoWork();
    }
}

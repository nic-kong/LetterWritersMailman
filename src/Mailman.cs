using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LetterWritersMailman
{
    class Mailman : BaseThread
    {
        private ConcurrentQueue<Letter> queue;
        public Mailbox Mailbox { get; set; }

        public Mailman() : base()
        {
            this.queue = new ConcurrentQueue<Letter>();
        }

        public void Post(Letter letter)
        {
            queue.Enqueue(letter);
            Event.Set();
        }

        protected override void DoWork()
        {
            if (!queue.IsEmpty && Mailbox != null)
            {
                Letter letter;
                while (queue.TryDequeue(out letter))
                    Mailbox.Delivery(letter);
            }
        }
    }
}

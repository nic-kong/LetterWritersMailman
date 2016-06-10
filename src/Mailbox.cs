using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LetterWritersMailman
{
    class Mailbox
    {
        public List<Letter>[] Boxes { get; private set; }
        
        public Mailbox(int numBoxes)
        {
            Boxes = new List<Letter>[numBoxes];
            for (var i = 0; i < numBoxes; i++)
                Boxes[i] = new List<Letter>();
        }

        public void Delivery(Letter letter)
        {
            var box = FindBox(letter);
            letter.ReceiveTime = DateTime.Now;
            lock (box) // for main thread console only; otherwise, it is unnecessary
            {
                box.Add(letter);
            }
        }

        private List<Letter> FindBox(Letter letter)
        {
            uint index = (uint)letter.GetHashCode() % (uint)Boxes.Length;
            return Boxes[index];
        }
    }
}

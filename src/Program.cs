using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LetterWritersMailman
{
    class Program
    {
        static void Main(string[] args)
        {
            const int roundPerMins = 15;
            const int numWriters = 5;
            const int numMailboxes = 10;
            var writers = new LetterWriter[numWriters] {
                new LetterWriter("Anna", roundPerMins), new LetterWriter("Bard", roundPerMins),
                new LetterWriter("Cara", roundPerMins), new LetterWriter("Demi", roundPerMins),
                new LetterWriter("Echo", roundPerMins), };
            var boss = new Boss()
            {
                WakeupFrequency = 60000 / roundPerMins,
                Staffs = writers,
            };
            var mailbox = new Mailbox(numMailboxes);

            // mailman gets ready for the post
            Singleton<Mailman>.Instance.Mailbox = mailbox;
            Singleton<Mailman>.Instance.Start();
            // writers get ready for boss' order
            writers.All(writer => { writer.Start(); return true; });
            boss.Start();

            for (var i = 0; i < 30; i++)
            {
                Thread.Sleep(4000);
                mailbox.Boxes.All(box =>
                {
                    List<Letter> tempBox;
                    lock (box)
                    {
                        tempBox = box.ToList();
                    }
                    var time = tempBox.Any() ? tempBox.Average(letter => letter.GetDeliveryTime().TotalMilliseconds) : 0;
                    Console.Write("{0:0.0}/{1}, ", time, tempBox.Count);
                    return true;
                });
                Console.WriteLine();
            }

            boss.Stop();
            writers.All(writer =>
            {
                if (writer.IsAlive)
                {
                    writer.Stop();
                    writer.Join();
                }
                return true;
            });
            if (Singleton<Mailman>.Instance.IsAlive)
            {
                Singleton<Mailman>.Instance.Stop();
                Singleton<Mailman>.Instance.Join();
            }
        }
    }
}

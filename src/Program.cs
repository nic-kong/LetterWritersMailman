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
        private static bool quit = false;

        static void Main(string[] args)
        {
            const int roundPerMins = 20;
            const int numWriters = 5;
            const int numMailboxes = 10;
            var writers = new LetterWriter[numWriters] {
                new LetterWriter("Anna", roundPerMins), new LetterWriter("Bard", roundPerMins),
                new LetterWriter("Cara", roundPerMins), new LetterWriter("Demi", roundPerMins),
                new LetterWriter("Echo", roundPerMins), };
            var boss = new Boss()
            {
                WakeupFrequency = 3000,
                Staffs = writers,
            };
            var mailbox = new Mailbox(numMailboxes);

            Startup(writers, boss, mailbox);
            DateTime startTime = DateTime.Now;

            ConsoleDisplay(mailbox, startTime);

            Cleanup(writers, boss);
        }

        private static void Startup(LetterWriter[] writers, Boss boss, Mailbox mailbox)
        {
            // mailman gets ready for the post
            Singleton<Mailman>.Instance.Mailbox = mailbox;
            Singleton<Mailman>.Instance.Start();
            // writers get ready for boss' order
            writers.All(writer => { writer.Start(); return true; });
            boss.Start();
        }

        private static void Cleanup(LetterWriter[] writers, Boss boss)
        {
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

        private static void ConsoleDisplay(Mailbox mailbox, DateTime startTime)
        {
            Console.CancelKeyPress += delegate(object sender, ConsoleCancelEventArgs e)
            {
                e.Cancel = true;
                quit = true;
            };
            while (!quit)
            {
                TimeSpan elapsedtime = DateTime.Now - startTime;
                Console.WriteLine("Elapsed time: {0}", elapsedtime.ToString(@"mm\:ss"));

                int total = 0;
                mailbox.Boxes.All(box =>
                {
                    List<Letter> tempBox;
                    lock (box)
                    {
                        tempBox = box.ToList();
                    }
                    var time = tempBox.Any() ? tempBox.Average(letter => letter.GetDeliveryTime().TotalMilliseconds) : 0;
                    total += tempBox.Count;
                    Console.Write("{0:0.0}/{1}, ", time, tempBox.Count);
                    return true;
                });
                Console.WriteLine("total: {0}", total);

                Thread.Sleep(60000);
            }
        }
    }
}

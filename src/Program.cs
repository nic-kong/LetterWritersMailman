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
            Console.CancelKeyPress += delegate(object sender, ConsoleCancelEventArgs e)
            {
                e.Cancel = true;
                quit = true;
            };

            const int roundPerMins = 15;
            const int numWriters = 5;
            const int numMailboxes = 10;
            var writers = new LetterWriter[numWriters] {
                new LetterWriter("Anna", roundPerMins), new LetterWriter("Bard", roundPerMins),
                new LetterWriter("Cara", roundPerMins), new LetterWriter("Demi", roundPerMins),
                new LetterWriter("Echo", roundPerMins), };
            var boss = new Boss(60000 / roundPerMins)
            {
                ProductivityTarget = 15,
                Staffs = writers,
            };
            var mailbox = new Mailbox(numMailboxes);
            Singleton<Mailman>.Instance.Mailbox = mailbox;

            Startup(writers, boss, mailbox);            

            ConsoleDisplay(mailbox);

            Cleanup(writers, boss);
        }

        private static void Startup(LetterWriter[] writers, Boss boss, Mailbox mailbox)
        {
            // mailman gets ready for the post            
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

        private static void ConsoleDisplay(Mailbox mailbox)
        {
            DateTime startTime = DateTime.Now;
            while (!quit)
            {
                TimeSpan elapsedtime = DateTime.Now - startTime;
                Console.WriteLine("Elapsed time: {0}", elapsedtime.ToString(@"mm\:ss"));

                var unionBox = new List<Letter>();
                mailbox.Boxes.All(box =>
                {
                    lock (box)
                    {
                        unionBox.AddRange(box);
                    }
                    return true;
                });
                var avgTime = unionBox.Any() ?
                    unionBox.Average(letter => letter.GetDeliveryTime().TotalMilliseconds) :
                    0;
                Console.WriteLine("avg time: {0:0.000}ms, total: {1}", avgTime, unionBox.Count);

                Thread.Sleep(60000);
            }
        }
    }
}

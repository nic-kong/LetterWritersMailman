using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LetterWritersMailman
{
    class Boss
    {
        private Timer timer;
        public int WakeupFrequency { private get; set; }
        public IStaff[] Staffs { get; set; }

        public void Start()
        {
            timer = new Timer(AssignJob, null, 0, WakeupFrequency); // Wake up every 4 secs
        }

        public void Stop()
        {
            if (timer != null)
                timer.Dispose();
        }

        private void AssignJob(object state)
        {
            if (Staffs == null)
                return;

            Staffs.All(staff =>
            {
                Console.Write("{0}, ", staff.Productivity);
                return true;
            });
            Console.WriteLine();

            var busyStaffs = new List<IStaff>();
            Staffs.Where(staff => staff.Productivity == 0).All(staff => // every minute each staff has to do something
            {
                staff.BossOrder(); // boss asks the staff to do something
                busyStaffs.Add(staff);
                return true;
            });

            var freeStaffs = Staffs.Except(busyStaffs);
            if (freeStaffs.Any())
            {
                int teamProductivity = Staffs.Sum(staff => staff.Productivity);
                if (teamProductivity < 15) // boss thinks team can work harder
                {
                    // find the lowest workload staff
                    var freeStaff = freeStaffs.OrderBy(staff => staff.Productivity).First();
                    freeStaff.BossOrder();

                    busyStaffs.Add(freeStaff);
                }
            }

            Staffs.Except(busyStaffs).All(staff =>
            {
                staff.Rest(); // other staffs take a rest
                return true;
            });
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace LetterWritersMailman
{
    class Boss
    {
        private Timer timer;
        public int WakeupFrequency { private get; set; }
        public IStaffMonitor[] Staffs { get; set; }

        public void Start()
        {
            timer = new Timer(WakeupFrequency); // Wake up every 4 secs
            timer.Elapsed += new ElapsedEventHandler(AssignJob);
            timer.Enabled = true;
        }

        public void Stop()
        {
            if (timer != null)
                timer.Stop();
        }

        private void AssignJob(object sender, ElapsedEventArgs e)
        {
            if (Staffs == null)
                return;

            var lazyStaffs = Staffs.Where(staff => staff.Productivity == 0); // every minute each staff has to do something
            lazyStaffs.All(staff =>
            {
                staff.BossOrder(); // boss asks the staff to do something
                return true;
            });

            var busyStaffs = lazyStaffs.ToList(); // all lazy staffs are now working hard
            if (!lazyStaffs.Any()) // everyone has a job in cycle
            {
                int teamProductivity = Staffs.Sum(staff => staff.Productivity);
                if (teamProductivity < 15) // boss thinks team can work harder
                {
                    // find the lowest workload staff
                    var freeStaff = Staffs.OrderBy(staff => staff.Productivity).First();
                    freeStaff.BossOrder();

                    busyStaffs.Add(freeStaff);
                }
            }

            Staffs.Except(busyStaffs).All(staff =>
            {
                staff.Rest();
                return true;
            });
        }
    }
}

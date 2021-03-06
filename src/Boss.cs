﻿using System;
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
        private int wakeupFrequency;
        public int ProductivityTarget { private get; set; }
        public IStaff[] Staffs { get; set; }

        public Boss(int wakeupFrequency)
        {
            this.wakeupFrequency = wakeupFrequency;
        }

        public void Start()
        {
            if (timer == null)
                timer = new Timer(AssignJob, null, 0, wakeupFrequency); // Wake up every 4 secs
        }

        public void Stop()
        {
            if (timer != null)
                timer.Dispose();
        }

        protected void AssignJob(object state)
        {
            if (Staffs == null)
                return;

            Staffs.All(staff =>
            {
                staff.Rest(); // all staffs take rest before boss calls them out
                return true;
            });

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
                if (teamProductivity < ProductivityTarget) // boss thinks team can work harder
                {
                    // find the staff with lowest workload
                    var freeStaff = freeStaffs.OrderBy(staff => staff.Productivity).First();
                    freeStaff.BossOrder();
                    busyStaffs.Add(freeStaff);
                }
            }            
        }
    }
}

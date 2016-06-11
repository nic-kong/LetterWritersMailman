using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LetterWritersMailman.test
{
    [TestClass]
    public class BossUT
    {
        [TestMethod]
        public void AssignJobTest()
        {
            var staffA = new MockStaff() { MovingWindows = new MovingWindows(3), };
            var staffB = new MockStaff() { MovingWindows = new MovingWindows(3), };
            var staffs = new MockStaff[2] { staffA, staffB, };
            var boss = new FakeBoss()
            {
                ProductivityTarget = 4,
                Staffs = staffs,
            };

            boss.CallAssignJob(); // all staffs no productivity case, no free staffs case
            // A | 1
            // B | 1
            Assert.AreEqual(1, staffA.Productivity);
            Assert.AreEqual(1, staffB.Productivity);

            boss.CallAssignJob(); // all staffs free case
            // A | 1 1
            // B | 1 0
            Assert.AreEqual(3, staffA.Productivity + staffB.Productivity);

            boss.CallAssignJob();
            // A | 1 1 0
            // B | 1 0 1
            Assert.AreEqual(4, staffA.Productivity + staffB.Productivity);

            staffA.MovingWindows.PrevWindowIncrement();
            staffB.MovingWindows.PrevWindowIncrement();
            // A | 1 1 1
            // B | 1 0 2
            boss.CallAssignJob(); // reach target case
            // A 1 | 1 1 0
            // B 1 | 0 2 0
            Assert.AreEqual(4, staffA.Productivity + staffB.Productivity);

            staffA.MovingWindows[-1] = -1;
            staffB.MovingWindows[-1] = -1;
            // A 1 | 1 1 -1
            // B 1 | 0 2 -1
            boss.CallAssignJob(); // staffA no productivity (partial staff no productivity case), team productivity < target
            // A 1 1 | 1 -1 1
            // B 1 0 | 2 -1 1
            Assert.AreEqual(3, staffA.Productivity + staffB.Productivity);
        }
    }
}

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LetterWritersMailman.test
{
    [TestClass]
    public class MovingWindowsUT
    {
        [TestMethod]
        public void DataTntegrityTest()
        {
            var windows = new MovingWindows(5);

            windows.Add(1);
            Assert.AreEqual(1, windows[-1]);

            windows.Add(2);
            Assert.AreEqual(2, windows[-1]);
            Assert.AreEqual(1, windows[-2]);

            windows.Add(3);
            Assert.AreEqual(3, windows[-1]);
            Assert.AreEqual(2, windows[-2]);
            Assert.AreEqual(1, windows[-3]);

            windows.Add(4);
            Assert.AreEqual(4, windows[-1]);
            Assert.AreEqual(3, windows[-2]);
            Assert.AreEqual(2, windows[-3]);
            Assert.AreEqual(1, windows[-4]);

            windows.Add(5);
            Assert.AreEqual(5, windows[-1]);
            Assert.AreEqual(4, windows[-2]);
            Assert.AreEqual(3, windows[-3]);
            Assert.AreEqual(2, windows[-4]);
            Assert.AreEqual(1, windows[-5]);

            windows.Add(6);
            Assert.AreEqual(6, windows[-1]);
            Assert.AreEqual(5, windows[-2]);
            Assert.AreEqual(4, windows[-3]);
            Assert.AreEqual(3, windows[-4]);
            Assert.AreEqual(2, windows[-5]);
            Assert.AreEqual(6, windows[-6]);
        }

        [TestMethod]
        public void PrevWindowIncrementTest()
        {
            var windows = new MovingWindows(5);

            windows.Add(1);
            Assert.AreEqual(1, windows[-1]);

            windows.PrevWindowIncrement();
            Assert.AreEqual(2, windows[-1]);

            windows.PrevWindowIncrement();
            Assert.AreEqual(3, windows[-1]);

            windows.Add(4);
            Assert.AreEqual(4, windows[-1]);

            windows.PrevWindowIncrement();
            Assert.AreEqual(5, windows[-1]);
            Assert.AreEqual(3, windows[-2]);
        }

        [TestMethod]
        public void SumTest()
        {
            var windows = new MovingWindows(5);

            windows.Add(1);
            Assert.AreEqual(1, windows.Total);

            windows.Add(2);
            Assert.AreEqual(1 + 2, windows.Total);

            windows.Add(3);
            Assert.AreEqual(1 + 2 + 3, windows.Total);

            windows.Add(4);
            Assert.AreEqual(1 + 2 + 3 + 4, windows.Total);

            windows.Add(5);
            Assert.AreEqual(1 + 2 + 3 + 4 + 5, windows.Total);
        }
    }
}

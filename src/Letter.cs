using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LetterWritersMailman
{
    public class Letter
    {
        public static readonly string MillisecondTimeFormat = "HH:mm:ss.fff";

        public string SenderName { get; set; }
        public string Address { get; private set; }
        public DateTime SendTime { get; set; }
        public DateTime ReceiveTime { get; set; }

        public Letter(string senderName) : this()
        {
            SenderName = senderName;
        }

        public Letter()
        {
            Address = DateTime.Now.ToString(MillisecondTimeFormat);
        }

        public TimeSpan GetDeliveryTime()
        {
            return ReceiveTime - SendTime;
        }

        public override int GetHashCode()
        {
            return Address.GetHashCode();
        }
    }
}

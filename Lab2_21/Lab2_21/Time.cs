using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2_21
{
    public class Time : IComparable<Time>
    {
        private readonly int _hour;
        private readonly int _minute;
        private readonly int _second;
        private readonly bool _isNormalTime;
        public int Hour { get { return _hour; } }
        public int Minute { get { return _minute; } }
        public int Second { get { return _second; } }
        public bool IsNormalTime { get { return _isNormalTime; } }

        public Time(string timeStr)
        {
            var buf = timeStr.Split(':', StringSplitOptions.RemoveEmptyEntries).Select(t => Convert.ToInt32(t.Trim())).ToList();
            _hour = buf[0];
            _minute = buf[1];
            _second = buf[2];
            _isNormalTime = _hour >= 1 && _hour <= 12 && _minute >= 0 && _minute <= 59 && _second >= 0 && _second <= 59;
        }

        /// <summary>
        /// 11:21:03
        /// -
        /// 13:25:01
        /// Повертає різницю в секундах
        /// </summary>
        public static int operator -(Time t1, Time t2)
        {
            int result = 0;
            if (t1.Hour < t2.Hour)
            {
                result += (t1.Hour + 12 - t2.Hour) * 60 * 60;
            }
            else
            {
                result += (t1.Hour - t2.Hour) * 60 * 60;
            }
            result += ((t1.Minute - t2.Minute) * 60) + ((t1.Second - t2.Second) * 60);
            return result;
        }

        public override string ToString()
        {
            return new DateTime(1, 1, 1, Hour, Minute, Second).ToLongTimeString();
        }

        public int CompareTo(Time? other)
        {
            if (other == null)
            {
                throw new ArgumentException("Null value!");
            }
            int result;
            result = this.Hour.CompareTo(other.Hour);
            if (result == 0)
            {
                result = this.Minute.CompareTo(other.Minute);
            }
            if (result == 0)
            {
                result = this.Second.CompareTo(other.Second);
            }
            return result;
        }
    }
}

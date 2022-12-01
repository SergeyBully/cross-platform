namespace LabsLibrary
{
	public static class Lab2
	{
		public static string StartLab(string input = "INPUT.TXT")
		{
			var inputData = File.ReadLines(input).ToList();
			int countOfWatches = Convert.ToInt32(inputData.First().Trim());
			List<Time> timeList = inputData.Skip(1).Select(t => new Time(t.Trim())).ToList();

			if (countOfWatches > 50000 || !timeList.Any() || timeList.Any(t => !t.IsNormalTime))
			{
				return "Incorrect data!";
			}
			else
			{
				return GetOptimalTime(timeList).ToString();
			}
		}

		public static Time GetOptimalTime(List<Time> timeList)
		{
			List<Tuple<Time, int>> buf = new List<Tuple<Time, int>>();
			for (int i = 0; i < timeList.Count; i++)
			{
				int fullTimeToSet = 0;
				for (int j = 0; j < timeList.Count; j++)
				{
					if (j == i)
					{
						continue;
					}
					fullTimeToSet += timeList[i] - timeList[j];
				}
				buf.Add(new Tuple<Time, int>(timeList[i], fullTimeToSet));
			}
			return buf.OrderBy(t => t.Item2).ThenBy(t => t.Item1).First().Item1;
		}
	}

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

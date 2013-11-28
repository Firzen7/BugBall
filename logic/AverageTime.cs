using System;
using System.Collections.Generic;

namespace BugBall
{
	public class AverageTime
	{
		List<long> times = new List<long>();

		public AverageTime()
		{

		}

		public double Time {
			get {
				double count = times.Count;
				double sum = 0;
				foreach (long item in times) {
					sum += (double)item / count;
				}
				return sum;
			}
		}

		public void AddTime(long time) {
			times.Add(time);
		}

		public override string ToString()
		{
			return "Times count: " + times.Count
				+ ", average time: " + this.Time;
		}
	}
}


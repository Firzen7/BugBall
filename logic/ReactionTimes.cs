using System;
using System.Collections;
using System.Text;

namespace BugBall
{
	[Serializable]
	public class ReactionTimes
	{
		Hashtable times = new Hashtable();
		int count = 0;

		public ReactionTimes()
		{

		}

		public int TimesCount ()
		{
			return count;
		}

		public void RemoveTime (Word tail)
		{
			if (tail != null) {
				times.Remove (tail.ToString ());
			}
		}

		public void RemoveTime (String tail)
		{
			if (tail != null) {
				times.Remove (tail);
			}
		}

		public void AddTime(String tail, long time) {
			AddTime(new Word(tail), time);
		}

		public void AddTime(Word tail, long time) {
			if(times.ContainsKey(tail.ToString())) {
				AverageTime avg = (AverageTime) times[tail.ToString()];
				avg.AddTime(time);
			}
			else {
				AverageTime avg = new AverageTime();
				avg.AddTime(time);
				times.Add(tail.ToString(), avg);
			}

			count++;
		}

		public String FastestPrefix() {
			double min = -1;
			String output = "";
			foreach(DictionaryEntry item in times) {
				double value = ((AverageTime)item.Value).Time;
				String key = (String)item.Key;

				if(value < min || min == -1) {
					min = value;
					output = key;
				}
			}
			return output;
		}

		public String SlowestPrefix() {
			double max = -1;
			String output = "";
			foreach(DictionaryEntry item in times) {
				double value = ((AverageTime)item.Value).Time;
				String key = (String)item.Key;

				if(value > max) {
					max = value;
					output = key;
				}
			}
			times.Remove(output);
			return output;
		}

		public void StartMeasure() {
			TimeMeasurer.StartMeasure();
		}

		public void EndMeasure(Word tail) {
			this.AddTime(tail, TimeMeasurer.EndMeasure());
		}

		public void EndMeasure(String tail) {
			this.AddTime(tail, TimeMeasurer.EndMeasure());
		}

		public void AddFail(String tail) {
			this.AddTime(tail, 100000);
			count++;
		}

		public void AddFail(Word tail) {
			this.AddTime(tail, 100000);
			count++;
		}

		public override string ToString()
		{
			StringBuilder buffer = new StringBuilder();

			foreach(DictionaryEntry entry in times) {
				buffer.Append("prefix: ");
				buffer.Append(entry.Key);
				buffer.Append(", time: ");
				buffer.Append(((AverageTime)entry.Value).Time);
				buffer.Append("\n");
			}

			return buffer.ToString();
		}
	}
}


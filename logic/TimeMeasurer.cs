using System;

namespace BugBall
{
	public class TimeMeasurer
	{
		static long reactionTimeMillis = 0;

		public TimeMeasurer()
		{

		}

		public static long ReactionTimeMillis {
			get {
				return reactionTimeMillis;
			}
			set {
				reactionTimeMillis = value;
			}
		}

		public static void StartMeasure() {
			reactionTimeMillis = DateTime.Now.ToFileTime();
		}

		public static long EndMeasure() {
			long now = DateTime.Now.ToFileTime();
			reactionTimeMillis = (now - reactionTimeMillis) / 10000;
			return reactionTimeMillis;
		}
	}
}


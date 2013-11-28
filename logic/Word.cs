using System;
using System.Text;

namespace BugBall {

	[Serializable]
	public class Word {
		String text = "";

		public Word() {

		}

		public Word(String text) {
			this.Text = text;
		}

		public String Text {
			get {
				return text;
			}
			set {
				text = value;
				text = text.ToLower();
			}
		}

		public int Length {
			get {
				return text.Length;
			}
		}

		public bool IsEmpty() {
			return text.Equals("");
		}

		public String GetTail(int length) {
			if(length > text.Length) {
				length = text.Length;
			}
			if(length < 0) {
				length = 0;
			}
			return text.Substring(text.Length - length);
		}

		public String GetPrefix(int length) {
			if(length > text.Length) {
				length = text.Length;
			}
			if(length < 0) {
				length = 0;
			}
			return text.Substring(0, length);
		}

		public bool HasPrefix(String prefix) {
			return text.StartsWith(prefix);
		}

		public bool HasTail(String tail) {
			return text.EndsWith(tail);
		}

		public override string ToString() {
			return text;
		}

		public override bool Equals(Object other) {
			if(other == null) {
				return false;
			}
			if(other.GetType() == this.GetType()) {
				return ((Word)other).Text.Equals(this.Text);
			}
			else {
				return false;
			}
		}
	}
}

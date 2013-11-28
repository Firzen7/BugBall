using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BugBall
{
	[Serializable]
	public class Words
	{
		private List<Word> data = new List<Word>();
		private List<Word> saidWords = new List<Word>();

		public Words()
		{

		}

		public List<Word> AllWords {
			get {
				return data;
			}
			set {
				data = value;
			}
		}

		public void AddWord(Word word) {
			data.Add(word);
		}

		public void AddWord(String word) {
			data.Add(new Word(word));
		}

		public void RemoveWord(Word word) {
			data.Remove(word);
		}

		public void RemoveWord(String word) {
			data.Remove(new Word(word));
		}

		public void RemoveWord(int index) {
			data.RemoveAt(index);
		}

		public bool WordExist(String word) {
			return WordExist(new Word(word));
		}

		public void ResetSaidWords() {
			saidWords = new List<Word>();
		}

		public void AddSaidWord(Word word) {
			saidWords.Add(word);
		}

		public void AddSaidWord(String word) {
			AddSaidWord(new Word(word));
		}

		public bool IsAlreadySaid(Word word) {
			return saidWords.Contains(word);
		}

		public bool IsAlreadySaid(String word) {
			return IsAlreadySaid(new Word(word));
		}

		public bool WordExist(Word word) {
			return data.Contains(word);
		}

		private Words FilterWords(String prefix) {
			return FilterWords(prefix, "");
		}

		private Words FilterWords(String prefix, String tail) {
			Words output = new Words();

			foreach(Word word in data) {
				if(word.HasTail(tail) && word.HasPrefix(prefix)
				   && !IsAlreadySaid(word)) {
					output.AddWord(word);
				}
			}

			return output;
		}

		private Word GetRandomWord() {
			int size = data.Count;

			if(size != 0) {
				Random rnd = new Random();
				int index = rnd.Next(0, size);

				return (Word)data[index];
			}
			else {
				return null;
			}
		}

		public Word GetWord(String prefix) {
			return FilterWords(prefix).GetRandomWord();
		}

		public Word GetWord(String prefix, String tail) {
			return FilterWords(prefix, tail).GetRandomWord();
		}

		public void LoadWords(String file) {
			data = new List<Word>();
			string[] lines = File.ReadAllLines(file, Encoding.UTF8);
			for (int i = 0; i < lines.Length; i++) {
				this.AddWord(lines[i]);
			}
		}

		public void SaveWords(String file) {
			int size = data.Count;
			string[] words = new string[size];
			int i = 0;
			foreach(Word w in data) {
				words[i] = w.ToString();
				i++;
			}

			System.IO.File.WriteAllLines(file, words);
		}

		public override string ToString()
		{
			StringBuilder buffer = new StringBuilder();
			int size = data.Count;

			Console.WriteLine("Size: " + size);

			foreach(Word w in data) {
				buffer.Append(w);
				buffer.Append(", ");
			}

			return buffer.ToString();
		}
	}
}

using System;
using System.Collections;
using System.Collections.Generic;

namespace BugBall
{
	[Serializable]
	public class Manager
	{
		Words words = new Words();
		List<Player> players = new List<Player>();
		String lastTail = "";
		String wordsFile;
		int charsCount;
		List<ReactionTimes> stats = new List<ReactionTimes>();
		bool repairKonsoleBug = false;

		public Manager()
		{

		}

		public Words Words {
			get {
				return words;
			}
			set {
				words = value;
			}
		}

		public bool RepairKonsoleBug {
			get {
				return repairKonsoleBug;
			}
			set {
				repairKonsoleBug = value;
			}
		}

		public String WordsFile {
			get {
				return wordsFile;
			}
			set {
				wordsFile = value;
			}
		}

		public int CharsCount {
			get {
				return charsCount;
			}
			set {
				charsCount = value;
			}
		}

		public List<Player> Players {
			get {
				return players;
			}
			set {
				foreach (Player item in value) {
					AddPlayer(item);
				}
			}
		}

		public int GetCpusCount ()
		{
			int count = 0;
			foreach (Player item in players) {
				if(!item.IsHuman()) {
					count++;
				}
			}
			return count;
		}

		public int GetHumansCount ()
		{
			int count = 0;
			foreach (Player item in players) {
				if(item.IsHuman()) {
					count++;
				}
			}
			return count;
		}

		public void SaveWords ()
		{
			words.SaveWords(wordsFile);
		}

		public void AddPlayer(Player player) {
			players.Add(player);
			stats.Add(new ReactionTimes());
		}

		public void RemovePlayer (int index)
		{
			players.RemoveAt(index);
		}

		private void AddWord(Word word)
		{
			Console.ForegroundColor = ConsoleColor.White;
			Console.WriteLine("Chcete slovo " + word +
				" přidat do slovníku? [a/n]");
			Console.ResetColor();
			for (;;) {
				ConsoleKeyInfo key = Console.ReadKey ();
				if (key.Key == ConsoleKey.A) {
					words.AddWord (word);
					words.AddSaidWord (word);
					break;
				}
				if (key.Key == ConsoleKey.N) {
					break;
				}
			}
			Console.WriteLine();
		}

		private bool IsWordOk (Word w) {
			if (!w.HasPrefix (lastTail)) {
				Colors.WriteError("Slovo " + w + " nezačíná na "
					+ lastTail + ".");
				return false;
			}
			else if (words.IsAlreadySaid (w)) {
				Colors.WriteError("Toto slovo jsme už říkali.");
				return false;
			}
			else if (!words.WordExist (w)) {
				Colors.WriteError("Slovo " + w + " neznám.");
				AddWord(w);
				return false;
			}
			else {
				return true;
			}
		}

		private bool ControlVictory(Player player, int index) {
			foreach(Player pl in players) {
				if(!pl.IsDead() && !pl.Equals(player)) {
					return false;
				}
			}

			Console.ForegroundColor = ConsoleColor.Black;
			Console.BackgroundColor = player.Color;
			Console.WriteLine("\nHráč " + (index + 1) + " zvítězil!");
			Console.ResetColor();
			Console.ReadLine();

			return true;
		}

		private int NextPlayerIndex (int actualIndex)
		{
			if (actualIndex + 1 < players.Count) {
				return actualIndex + 1;
			} else {
				return 0;
			}
		}

		private bool ControlWordsFile() {
			try {
				words.LoadWords (wordsFile);
				return true;
			} catch (Exception) {
				Colors.WriteError("Nemohu otevřít soubor se slovní zásobou!");
				Console.ReadLine();
				return false;
			}
		}

		private void WriteWhoPlays(Player actual, int i) {
			Console.ForegroundColor = actual.Color;
			Console.Write("Hraje hráč " + (i + 1));
			Console.ResetColor();
			Console.Write(" [život: " + actual.Lives + "]: ");
		}

		public void StartGame ()
		{
			if(!ControlWordsFile()) return;
			bool begin = true;

			for(;;) {
				for(int i = 0; i < players.Count; i++) {
					Player actual = players[i];

					if(ControlVictory(actual, i)) {
						return;
					}

					if(actual.IsDead()) {
						continue;
					}

					for(;;) {
						WriteWhoPlays(actual, i);

						// začátek měření času slova
						if(!begin) {
							stats[i].StartMeasure();
						}

						int x = Console.CursorLeft;
						int y = Console.CursorTop;
						ReactionTimes times = stats[NextPlayerIndex(i)];

						if(!actual.IsHuman()) {
							Colors.WriteThinking();
						}
						Word w = actual.SayWord(lastTail, ref words, ref times);
							if(!actual.IsHuman()) {
							Colors.DeleteThinking();
						}

						if(w == null || w.IsEmpty()) {
							stats[i].AddFail(lastTail);
							actual.Lives--;
							if(actual.IsDead()) {
								Console.ForegroundColor = ConsoleColor.White;
								Console.WriteLine("Hráč " + (i + 1) +
								                  " skončil.");
								Console.ResetColor();
							}
							else {
								Colors.WriteFail(x, y, actual.IsHuman(), repairKonsoleBug);
								lastTail = "";
								continue;
							}
						}
						else {
							if(!IsWordOk(w)) {
								continue;
							}

							// konec měření času slova
							if(lastTail != "" && !begin) {
								stats[i].EndMeasure(lastTail);
							}

							Colors.WriteWord(w, charsCount, actual.IsHuman(),
							                 x, y, repairKonsoleBug);

							lastTail = w.GetTail(charsCount);
							words.AddSaidWord(w);
						}

						begin = false;
						break;
					}
				}
			}
		}
	}
}


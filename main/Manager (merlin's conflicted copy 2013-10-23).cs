using System;
using System.Collections;

namespace BugBall
{
	public class Manager
	{
		private Words words = new Words();
		private ArrayList players = new ArrayList();
		private String lastTail = "";
		private String wordsFile;
		private int charsCount;

		public Manager()
		{

		}

		~Manager () {
			if (words.AllWords.Count > 0) {
				words.SaveWords (wordsFile);
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

		public void AddPlayer(Player player) {
			players.Add(player);
		}

		private void AddWord(Word word)
		{
			Console.WriteLine("Chcete slovo " + word +
				" přidat do slovníku? [a/n]");
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
				Console.WriteLine ("Slovo " + w + " nezačíná na "
					+ lastTail + ".");
				return false;
			}
			else if (words.IsAlreadySaid (w)) {
				Console.WriteLine ("Toto slovo jsme už říkali.");
				return false;
			}
			else if (!words.WordExist (w)) {
				Console.WriteLine ("Slovo " + w + " neznám.");
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

			Console.WriteLine("Hráč " + (index + 1) + " zvítězil!");

			return true;
		}

		public void StartGame() {

			// TODO exception!!
			try {
				words.LoadWords(wordsFile);
			} catch(Exception) {
				Console.WriteLine("Chyba při načítání slovní zásoby!\n" +
					"Zkontrolujte prosím, zda existuje soubor \"" +
				                  WordsFile + "\"\na spusťte program znovu.\n");
				return;
			}

			for(;;) {
				for(int i = 0; i < players.Count; i++) {
					Player actual = (Player)players[i];

					if(ControlVictory(actual, i)) {
						return;
					}

					if(actual.IsDead()) {
						continue;
					}

					for(;;) {
						Console.Write("Hraje hráč " + (i + 1) + " [život: "
						              + actual.Lives + "]: ");

						Word w = actual.SayWord(lastTail, ref words);

						if(w != null) {
							if(!IsWordOk(w)) {
								continue;
							}
						}

						if(w == null) {
							actual.Lives--;
							if(actual.IsDead()) {
								Console.WriteLine("Hráč " + (i + 1) +
								                  " skončil.");
							}
							else {
								Console.WriteLine("* neví! *");
								lastTail = "";
								continue;
							}
						}
						else {
							if(!actual.IsHuman()) {
								Console.WriteLine(w);
							}
							words.AddSaidWord(w);
							lastTail = w.GetTail(charsCount);
						}

						break;
					}
				}
			}
		}
	}
}


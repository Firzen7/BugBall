using System;
using System.Collections.Generic;

namespace BugBall
{
	public class Menu
	{
		Manager game = new Manager();
		Manager actual;
		const String configFile = "config.xml";
		const String wordsFile = "slova.txt";

		public Menu ()
		{
			game.CharsCount = 1;
			game.WordsFile = wordsFile;
			try {
				Config.LoadConfig(configFile);
				game.CharsCount = Config.CharsCount;
				game.Players = Config.Players;
				game.RepairKonsoleBug = Config.RepairBug;
			} catch (Exception) {

			}
		}

		~Menu() {
			Config.Players = game.Players;
			Config.CharsCount = game.CharsCount;
			Config.RepairBug = game.RepairKonsoleBug;
			Config.SaveConfig(configFile);
		}

		public void ShowMenu ()
		{
			for (;;) {
				Console.Clear();
				Colors.WriteTitle ("BugBall - Hlavní menu");
				Colors.WriteChoiceLn (1, "Zvolit počet písmen, na které se má hrát (počet nyní: " + game.CharsCount + ")");
				Colors.WriteChoice (2, "Oprava vykreslování Konsole (nyní: ");
				if(game.RepairKonsoleBug) {
					Console.WriteLine("zapnuto)");
				}
				else {
					Console.WriteLine("vypnuto)");
				}
				Colors.WriteChoice (3, "Nastavit hráče ");
				int cpus = game.GetCpusCount ();
				int humans = game.GetHumansCount ();

				if (cpus > 0 && humans > 0) {
					Console.WriteLine ("(" + humans + "x člověk, " + cpus + "x počítač)");
				} else if (cpus > 0) {
					Console.WriteLine ("(" + cpus + "x počítač)");
				} else if (humans > 0) {
					Console.WriteLine ("(" + humans + "x človek)");
				} else {
					Console.WriteLine ("(žádní hráči)");
				}

				Colors.WriteChoiceLn (4, "Spustit hru");
				Colors.WriteChoiceLn (5, "Ukončit program");

				ConsoleKeyInfo key = Console.ReadKey ();
				if (key.Key == ConsoleKey.D1) {
					SetCharsCount ();
					continue;
				}
				if (key.Key == ConsoleKey.D2) {
					SetRepairBug ();
					continue;
				}
				if (key.Key == ConsoleKey.D3) {
					SetPlayers ();
					continue;
				}
				if (key.Key == ConsoleKey.D4) {
					if(game.Players.Count >= 2) {
						actual = MyDeepClone.DeepClone<Manager>(game);
						Console.Clear();
						actual.StartGame();
						actual.SaveWords();
					}
					else {
						Colors.WriteError("\nVe hře musí být alespoň dva hráči!");
						Console.ReadLine();
					}
					continue;
				}
				if (key.Key == ConsoleKey.D5) {
					return;
				}
			}
		}

		private void SetRepairBug() {
			Console.Clear();
			Colors.WriteTitle ("Nastavení opravy vykreslování v Konsoli");
			int i = Colors.ReadIntBetween("Zvolte možnost" +
				"(0 = vypnuto, 1 = zapnuto): ", 0, 1);
			if(i == 0) {
				game.RepairKonsoleBug = false;
			}
			else {
				game.RepairKonsoleBug = true;
			}
		}

		private void SetCharsCount ()
		{
			Console.Clear();
			Colors.WriteTitle ("Nastavení počtu písmen");
			game.CharsCount = Colors.ReadIntFrom("Zadejte nový počet písmen: ", 1);
		}

		private void SetPlayers ()
		{
			for (;;) {
				Console.Clear();
				Colors.WriteTitle("Nastavení hráčů");

				List<Player> players = game.Players;
				int i = 1;
				foreach (Player item in players) {
					Console.ForegroundColor = item.Color;
					Console.Write("Hráč " + i);
					Console.ResetColor();
					Console.WriteLine(": " + item);
					i++;
				}

				if (players.Count == 0) {
					Console.WriteLine("žádní hráči!");
				}

				Console.WriteLine("\n----------------------------------\n");
				Colors.WriteChoiceLn(1, "Přidat hráče");
				Colors.WriteChoiceLn(2, "Odstranit hráče");
				Colors.WriteChoiceLn(3, "Změnit číslo hráče");
				Colors.WriteChoiceLn(4, "Zpět do menu");

				ConsoleKeyInfo key = Console.ReadKey ();
				if (key.Key == ConsoleKey.D1) {
					AddPlayer();
				}
				if (key.Key == ConsoleKey.D2) {
					RemovePlayer();
				}
				if (key.Key == ConsoleKey.D3) {
					ChangePlayerNumber();
				}
				if (key.Key == ConsoleKey.D4) {
					return;
				}
			}
		}

		private void ChangePlayerNumber ()
		{
			int original = 0, newpos = 0;
			original = Colors.ReadIntBetween("\nZadejte číslo hráče k přesunu: ",
				                      1, game.Players.Count);
			newpos = Colors.ReadIntBetween("Zadejte novou pozici hráče: ", 
			                               1, game.Players.Count);
			newpos--;
			original--;
			Player pl = game.Players[original];
			game.Players[original] = game.Players[newpos];
			game.Players[newpos] = pl;
		}

		private void RemovePlayer ()
		{
			int num = Colors.ReadIntBetween ("\nZadejte číslo hráče," +
				" kterého chcete smazat (0 pro návrat): ", 0, game.Players.Count);
			if (num != 0) {
				game.RemovePlayer (num - 1);
			}
		}

		private Iq ParseIntelligence (int num)
		{
			switch (num) {
			case 1: return Iq.VERY_DUMB;
			case 2: return Iq.DUMB;
			case 3: return Iq.NORMAL;
			case 4: return Iq.CLEVER;
			case 5: return Iq.VERY_CLEVER;
			default: return Iq.VERY_DUMB;
			}
		}

		private void AddPlayer ()
		{
			Player output = new Player ();

			output.Personality = ChoosePlayerType();

			if (output.Personality == PlayerType.COMPUTER) {
				output.Intelligence = ChooseIntelligence();
			}

			output.Lives = ChooseLives();
			output.Color = Colors.ChooseColor();

			game.AddPlayer(output);
		}

		private int ChooseLives()
		{
			return Colors.ReadIntFrom("Zadejte počet životů: ", 1);
		}

		private PlayerType ChoosePlayerType ()
		{
			PlayerType output = PlayerType.HUMAN;

			for (;;) {
				String type = Colors.ReadText("\nBude hráč člověk " +
					"nebo počítač? ");

				if (type.Equals ("člověk") || type.Equals ("clovek")) {
					output = PlayerType.HUMAN;
				} else if (type.Equals ("počítač") || type.Equals ("pocitac")) {
					output = PlayerType.COMPUTER;
				} else {
					Colors.WriteError("Nevím, co znamená \""
					                  + type + "\", zadejte prosím " +
					                  	"\"počítač\" nebo \"člověk\".");
					continue;
				}
				break;
			}

			return output;
		}

		private Iq ChooseIntelligence() {
			int num = Colors.ReadIntBetween("Zvolte obtížnost (1 až 5): ", 1, 5);
			return ParseIntelligence(num);
		}
	}
}


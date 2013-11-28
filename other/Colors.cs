using System;

namespace BugBall
{
	public class Colors
	{
		public Colors ()
		{
		}

		public static void WriteTitle(String text)
		{
			Console.ForegroundColor = ConsoleColor.Black;
			Console.BackgroundColor = ConsoleColor.Yellow;
			Console.WriteLine(" " + text + " ");
			Console.WriteLine();
			Console.ResetColor();
		}

		public static void WriteChoice(int number, String text,
		                               ConsoleColor color = ConsoleColor.Gray) {
			Console.ForegroundColor = ConsoleColor.White;
			Console.Write(number + ".");
			Console.ForegroundColor = color;
			Console.Write(" " + text);
			Console.ResetColor();
		}

		public static void WriteChoiceLn(int number, String text,
		                                 ConsoleColor color = ConsoleColor.Gray) {
			WriteChoice(number, text, color);
			Console.WriteLine();
		}

		public static void WriteError(String text) {
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine(text);
			Console.ResetColor();
		}

		public static String ReadText(String desc) {
			Console.Write(desc);
			Console.ForegroundColor = ConsoleColor.White;
			String output = Console.ReadLine();
			if(output == null) {
				output = "";
			}
			Console.ResetColor();
			return output;
		}

		public static int ReadInt(String desc)
		{
			int output = 0;
			for (;;) {
				String text = ReadText (desc);
				try {
					output = int.Parse(text);
					break;
				} catch (Exception) {
					WriteError(text + " není celé číslo!");
				}
			}
			return output;
		}

		public static int ReadIntFrom(String desc, int from)
		{
			int output = 0;
			for (;;) {
				int num = ReadInt(desc);
				if(num >= from) {
					output = num;
					break;
				}
				else {
					WriteError(num + " není celé číslo větší, než " + (from - 1)
					           + "!");
				}
			}
			return output;
		}

		public static int ReadIntBetween(String desc, int from, int to) {
			int output = 0;
			for (;;) {
				int num = ReadInt(desc);
				if(num >= from && num <= to) {
					output = num;
					break;
				}
				else {
					WriteError(num + " není celé číslo v rozmení " + from
					           + " - " + to + "!");
				}
			}
			return output;
		}

		public static void WriteWord(Word word, int tailLength, bool human,
		                             int x, int y, bool repairBug) {
			if(human) {
				if(repairBug && Console.WindowHeight - 1 == y) {
					y--;
				}
				Console.SetCursorPosition(x, y);
			}

			String prefix = word.GetPrefix(word.Length - tailLength);
			String tail = word.GetTail(tailLength);

			Console.Write(prefix);
			Console.ForegroundColor = ConsoleColor.White;
			Console.WriteLine(tail);
			Console.ResetColor();
		}

		public static void WriteFail(int x, int y, bool human, bool repairBug) {
			if(human) {
				if(repairBug && Console.WindowHeight - 1 == y) {
					y--;
				}
				Console.SetCursorPosition(x, y);
			}

			Console.ForegroundColor = ConsoleColor.DarkGray;
			Console.WriteLine("* neví! *");
			Console.ResetColor();
		}

		public static void WriteThinking() {
			Console.ForegroundColor = ConsoleColor.DarkGray;
			Console.Write("přemýšlí ...");
			Console.ResetColor();
		}

		public static void DeleteThinking ()
		{
			for (int i = 0; i < 12; i++) {
				Console.Write ("\b");
			}
			for (int i = 0; i < 12; i++) {
				Console.Write (" ");
			}
			for (int i = 0; i < 12; i++) {
				Console.Write ("\b");
			}
		}

		public static ConsoleColor ChooseColor() {
			for(;;) {
				Console.WriteLine("Zvolte barvu: ");
				Colors.WriteChoiceLn(1, "červená", ConsoleColor.Red);
				Colors.WriteChoiceLn(2, "zelená", ConsoleColor.Green);
				Colors.WriteChoiceLn(3, "modrá", ConsoleColor.Blue);
				Colors.WriteChoiceLn(4, "žlutá", ConsoleColor.Yellow);
				Colors.WriteChoiceLn(5, "fialová", ConsoleColor.Magenta);
				Colors.WriteChoiceLn(6, "azurová", ConsoleColor.Cyan);
				Colors.WriteChoiceLn(7, "šedá", ConsoleColor.Gray);
				Colors.WriteChoiceLn(8, "bílá", ConsoleColor.White);

				ConsoleKeyInfo key = Console.ReadKey ();
				switch(key.Key) {

				case ConsoleKey.D1: return ConsoleColor.Red;
				case ConsoleKey.D2: return ConsoleColor.Green;
				case ConsoleKey.D3: return ConsoleColor.Blue;
				case ConsoleKey.D4: return ConsoleColor.Yellow;
				case ConsoleKey.D5: return ConsoleColor.Magenta;
				case ConsoleKey.D6: return ConsoleColor.Cyan;
				case ConsoleKey.D7: return ConsoleColor.Gray;
				case ConsoleKey.D8: return ConsoleColor.White;
				case ConsoleKey.NumPad1: return ConsoleColor.Red;
				case ConsoleKey.NumPad2: return ConsoleColor.Green;
				case ConsoleKey.NumPad3: return ConsoleColor.Blue;
				case ConsoleKey.NumPad4: return ConsoleColor.Yellow;
				case ConsoleKey.NumPad5: return ConsoleColor.Magenta;
				case ConsoleKey.NumPad6: return ConsoleColor.Cyan;
				case ConsoleKey.NumPad7: return ConsoleColor.Gray;
				case ConsoleKey.NumPad8: return ConsoleColor.White;
				default: continue;
				}
			}
		}
	}
}


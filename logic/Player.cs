using System;
using System.Xml.Linq;
using System.Collections.Generic;

namespace BugBall
{
	[Serializable]
	public class Player
	{
		PlayerType personality;
		Iq intelligence;
		int lives = 3;
		ConsoleColor color;

		public Player()
		{

		}

		public Player(PlayerType personality)
		{
			this.personality = personality;
		}

		public Player(PlayerType personality, int lives)
		{
			this.personality = personality;
			this.lives = lives;
		}

		public Player(PlayerType personality, Iq iq)
		{
			this.personality = personality;
			intelligence = iq;
		}

		public Player(PlayerType personality, Iq iq, int lives)
		{
			this.personality = personality;
			intelligence = iq;
			this.lives = lives;
		}

		public Player(PlayerType personality, Iq iq, int lives,
		              ConsoleColor color)
		{
			this.personality = personality;
			intelligence = iq;
			this.lives = lives;
			this.color = color;
		}

		public ConsoleColor Color {
			get {
				return color;
			}
			set {
				color = value;
			}
		}

		public int Lives {
			set {
				lives = value;
			}
			get {
				return lives;
			}
		}

		public PlayerType Personality {
			get {
				return personality;
			}
			set {
				personality = value;
			}
		}

		public Iq Intelligence {
			get {
				return intelligence;
			}
			set {
				intelligence = value;
			}
		}

		public bool IsHuman() {
			return personality == PlayerType.HUMAN;
		}

		public bool IsDead() {
			return this.Lives <= 0;
		}

		public Word SayWord(String prefix, ref Words words, ref ReactionTimes stats)
		{
			if (personality == PlayerType.HUMAN) {
				String input = Console.ReadLine();
				if(input == null) {
					return null;
				}

				input = input.ToLower();
				return new Word(input);
			}
			else {
				Word w = words.GetWord(prefix);

				Random rnd = new Random();
				int num = 0;
				switch(intelligence) {
					case Iq.VERY_DUMB: num = rnd.Next(0, 3); break;
					case Iq.DUMB: num = rnd.Next(0, 7); break;
					case Iq.NORMAL: num = rnd.Next(0, 10); break;
					case Iq.CLEVER:
						num = rnd.Next(0, 40);

						if(stats.TimesCount() > 10) {
							w = words.GetWord(prefix, stats.SlowestPrefix());

							if(w == null) {
								stats.RemoveTime(prefix);
								w = words.GetWord(prefix);
							}
						}
						break;
					case Iq.VERY_CLEVER:

						num = rnd.Next(0, 150);

						if(stats.TimesCount() > 6) {
							w = words.GetWord(prefix, stats.SlowestPrefix());

							if(w == null) {
								stats.RemoveTime(prefix);
								w = words.GetWord(prefix);
							}
						}
						break;
				}

				if(!prefix.Equals("") && num == 1) {
					return null;
				}
				else {
					return w;
				}
			}
		}

		private String GetIqName ()
		{
			switch (intelligence) {
			case Iq.VERY_DUMB: return "velmi hloupý";
			case Iq.DUMB: return "hloupý";
			case Iq.NORMAL: return "normální";
			case Iq.CLEVER: return "chytrý";
			case Iq.VERY_CLEVER: return "velmi chytrý";
			default: return "neznámý";
			}
		}

		public override string ToString ()
		{
			if (IsHuman ()) {
				return "Člověk, životy: " + lives;
			} else {
				return "Počítač, inteligence: " + GetIqName() + ", životy: " + lives;
			}
		}

		public XDocument ToXml() {
			XDocument doc = new XDocument(
		    new XElement("player",
		            new XElement("personality", ((int)personality).ToString()),
			        new XElement("iq", ((int)intelligence).ToString()),
			        new XElement("lives", lives.ToString()),
			        new XElement("color", ((int)color).ToString())));
			return doc;
		}

		public static Player ParseXml(XDocument doc) {
			XElement root = doc.Root;

			String personality = root.Element("personality").Value;
			String iq = root.Element("iq").Value;
			String lives = root.Element("lives").Value;
			String color = root.Element("color").Value;

			Player output = new Player((PlayerType)int.Parse(personality),
			                           (Iq)int.Parse(iq), int.Parse(lives),
			                           (ConsoleColor)int.Parse(color));
			return output;
		}
	}
}

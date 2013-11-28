using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace BugBall
{
	public class Config
	{
		static int charsCount = 0;
		static List<Player> players = new List<Player>();
		static bool repairBug = false;

		public static int CharsCount {
			get {
				return charsCount;
			}
			set {
				charsCount = value;
			}
		}

		public static bool RepairBug {
			get {
				return repairBug;
			}
			set {
				repairBug = value;
			}
		}

		public static List<Player> Players {
			get {
				return players;
			}
			set {
				players = value;
			}
		}

		public static void SaveConfig(String fileName) {
			XDocument doc = new XDocument();
			XElement root = new XElement("root");
			doc.Add(root);

			root.Add(new XElement("chars", charsCount.ToString()));
			root.Add(new XElement("repairbug", repairBug.ToString()));

			foreach (Player item in players) {
				root.Add(item.ToXml().FirstNode);
			}

			doc.Save(fileName);
		}

		public static void LoadConfig(String fileName) {
			XDocument doc = XDocument.Load(fileName);
			XElement root = doc.Root;
			charsCount = int.Parse(root.Element("chars").Value);
			repairBug = bool.Parse(root.Element("repairbug").Value);

			foreach (XElement node in root.Descendants("player"))
			{
				XDocument d = new XDocument();
				d.Add(node);
				players.Add(Player.ParseXml(d));    			
			}
		}
	}
}


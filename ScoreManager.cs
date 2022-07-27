using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace Minesweeper
{
	[Serializable]
	public class HighScore
	{
		public DifficultyName Difficulty { get; set; }

		public DateTime DateTime { get; set; }

		public int PlayTime { get; set; }
	}


	public static class ScoreManager
	{
		private static readonly string folderPath = "..\\..\\..\\HighScores\\";


		private static string GetPath(DifficultyName difficulty) => folderPath + difficulty.ToString() + ".xml";


		public static List<HighScore> LoadScores(DifficultyName difficulty)
		{
			string path = GetPath(difficulty);

			if (path == null)
				return new List<HighScore>();

			return DeserializeFromXml(path);
		}


		public static void SaveScore(HighScore score)
        {
			string path = GetPath(score.Difficulty);

			if (path == null)
				return;

			List<HighScore> scores = DeserializeFromXml(path);

			if (scores == null)
				scores = new List<HighScore>();

			scores.Add(score);
			SerializeToXml(path, scores);
		}


		private static List<HighScore> DeserializeFromXml(string path)
		{
			if (!File.Exists(path))
				return new List<HighScore>();

			using StreamReader reader = new StreamReader(new FileStream(path, FileMode.Open));
			XmlSerializer serializer = new XmlSerializer(typeof(List<HighScore>));
			return (List<HighScore>)serializer.Deserialize(reader);
		}


		private static void SerializeToXml(string path, List<HighScore> scores)
        {
			using StreamWriter writer = new StreamWriter(new FileStream(path, FileMode.Create));
			XmlSerializer serializer = new XmlSerializer(typeof(List<HighScore>));
			serializer.Serialize(writer, scores);
		}
	}
}



using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace Minesweeper
{
	[Serializable]
	public class Score
	{
		public DifficultyName Difficulty { get; set; }

		public string PlayerName { get; set; }

		public DateTime DateTime { get; set; }

		public int PlayTime { get; set; }

		public int CorrectlyMarkedMines { get; set; }
	}


	public static class SaveFileManager
	{
		private static readonly string scoresFolderPath = "Scores\\";

		private static readonly string playerFolderPath = "PlayerStats\\";

		private static List<Score> lastUpdatedScores;

		private static List<Score> lastUpdatedPlayer;


		private static string GetPath(DifficultyName difficulty) => scoresFolderPath + difficulty.ToString() + ".xml";

		private static string GetPath(DifficultyName difficulty, string playerName) 
			=> playerFolderPath + playerName + "_" + difficulty.ToString() + ".xml";



		// Returns a list of scores read from an XML file or an empty list if the file cannot be accessed
		private static List<Score> DeserializeFromXml(string path)
		{
			if (!File.Exists(path))
				return new List<Score>();

			using StreamReader reader = new StreamReader(new FileStream(path, FileMode.Open));
			XmlSerializer serializer = new XmlSerializer(typeof(List<Score>));
			return (List<Score>)serializer.Deserialize(reader);
		}

		// Writes a list of scores to an XML file
		private static void SerializeToXml(string path, List<Score> scores)
		{
			using StreamWriter writer = new StreamWriter(new FileStream(path, FileMode.Create));
			XmlSerializer serializer = new XmlSerializer(typeof(List<Score>));
			serializer.Serialize(writer, scores);
		}


		
		// Returns a list of scores from games of the given difficulty, read from an XML file
		public static List<Score> LoadScores(DifficultyName difficulty)
		{
			if (lastUpdatedScores != null && 
				lastUpdatedScores.Count != 0 &&
				lastUpdatedScores[0].Difficulty == difficulty)
				return lastUpdatedScores;
			
			return DeserializeFromXml(GetPath(difficulty));
		}

		// Returns a list of scores from games of the given difficulty played by given player, read from an XML file
		public static List<Score> LoadScores(DifficultyName difficulty, string playerName)
		{
			if (lastUpdatedPlayer != null &&
				lastUpdatedPlayer.Count != 0 &&
				lastUpdatedPlayer[0].PlayerName == playerName)
				return lastUpdatedPlayer;

			return DeserializeFromXml(GetPath(difficulty, playerName));
		}



		// Saves the score to the XML file for scores from games of the same difficulty and games from the same player
		public static void SaveScore(Score score)
		{
			lastUpdatedScores = SaveTo(score, scoresFolderPath, GetPath(score.Difficulty));
			lastUpdatedPlayer = SaveTo(score, playerFolderPath, GetPath(score.Difficulty, score.PlayerName));
		}

		// Saves score to a given XML file and returns a list of scores in that XML file
		private static List<Score> SaveTo(Score score, string folderPath, string filePath)
        {
			if (!Directory.Exists(folderPath))
				Directory.CreateDirectory(folderPath);

			// Load old scores
			List<Score> saveList = DeserializeFromXml(filePath);

			// Add new score
			saveList.Add(score);
			SerializeToXml(filePath, saveList);

			return saveList;
		}
	}
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace Minesweeper
{
	[Serializable]
	public class HighScore
	{
		public DateTime DateTime { get; }

		public int Playtime { get; }

		public Difficulty Difficulty { get; }

		public HighScore(Difficulty difficulty, int playtime, DateTime dateTime)
		{
			Difficulty = difficulty;
			Playtime = playtime;
			DateTime = dateTime;
		}
	}


	public class HighScoreManager : List<HighScore>
    {

    }
}
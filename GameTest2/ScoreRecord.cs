using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameTest2
{
    [Serializable]
    public class ScoreRecord
    {
        public ScoreRecord(string aName, int aScore, DateTime aTime)
        {
            Name = aName;
            Score = aScore;
            Time = aTime;
        }

        public string Name { get; set; }
        public int Score { get; set; }
        public DateTime Time { get; set; }
    }
}

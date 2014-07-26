using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameTest2
{
    [Serializable]
    public class ScoreRecord
    {
        public ScoreRecord(string aName, int aScore)
        {
            Name = aName;
            Score = aScore;
        }

        public string Name { get; set; }
        public int Score { get; set; }
    }
}

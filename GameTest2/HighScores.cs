using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace GameTest2
{
    [Serializable]
    public class HighScores
    {
        public HighScores()
        {
            HighScoresList = new ObservableCollection<ScoreRecord>();
        }
        public static HighScores Deserialize(string aPath)
        {
            try
            {
                IFormatter lFormatter = new BinaryFormatter();
                Stream lStream = new FileStream(aPath, FileMode.Open);

                return (HighScores)lFormatter.Deserialize(lStream);
            }
            catch (Exception lEx)
            {
                return new HighScores();
            }
        }
        public static void Serialize(HighScores aHighScores, string aPath)
        {
            try
            {
                using (Stream lStream = new FileStream(aPath, FileMode.Create))
                {
                    IFormatter lFormatter = new BinaryFormatter();
                    lFormatter.Serialize(lStream, aHighScores);
                }
            }
            catch (Exception lEx)
            {

            }
        }

        public ObservableCollection<ScoreRecord> HighScoresList
        {
            get;
            set;
        }

        public void AddHighScore(string aName, int aScore, DateTime aTime)
        {
            HighScoresList.Add(new ScoreRecord(aName, aScore, aTime));
            Sort();

            while (HighScoresList.Count > 10)
            {
                HighScoresList.RemoveAt(10);
            }
        }
        public bool IsHighScore(int aScore)
        {
            if (HighScoresList.Count > 0)
            {
                return aScore > HighScoresList.Last().Score || HighScoresList.Count < 10;
            }
            else
                return true;
        }
        private void Sort()
        {
            HighScoresList = new ObservableCollection<ScoreRecord>(HighScoresList.OrderByDescending(x => x.Score));
        }
    }
}

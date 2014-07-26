using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace GameTest2
{
    public class HighScoresViewModel : BaseViewModel
    {
        private HighScores mHighScores;
        public HighScores HighScores
        {
            get
            {
                return mHighScores;
            }
            set
            {
                if (mHighScores == value) return;
                mHighScores = value;
                OnPropertyChanged("HighScores");
            }
        }
    }
}

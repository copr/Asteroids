using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace GameTest2
{
    public class MainWindowViewModel : BaseViewModel
    {
        public event Action StartGame;
        public event Action StopGame;

        public MainWindowViewModel()
        {
            mHighScores = HighScores.Deserialize(mHighScoresPath);
        }

        public void RaiseStartGameEvent()
        {
            if (StartGame != null)
            {
                StartGame();
            }
        }
        public void RaiseStopGameEvent()
        {
            if (StopGame != null)
            {
                StopGame();
            }
        }
        public void SaveHighScores()
        {
            HighScores.Serialize(mHighScores, mHighScoresPath);
        }
        public void HandleNewScore(int aScore)
        {
            if (mHighScores.IsHighScore(aScore))
            {
                InputNamePopUp lWindow = new InputNamePopUp();
                lWindow.ShowDialog();

                mHighScores.AddHighScore(lWindow.NameTextBox.Text, aScore);
            }
        }

        public void ShowHighScores()
        {
            HighScoresWindow lWindow = new HighScoresWindow();
            HighScoresViewModel lContext = new HighScoresViewModel();
            lContext.HighScores = mHighScores;
            lWindow.DataContext = lContext;

            lWindow.ShowDialog();
        }

        private bool mGameRunning;
        public bool GameRunning
        {
            get { return mGameRunning; }
            set
            {
                if (mGameRunning != value) mGameRunning = value;
                OnPropertyChanged("GameRunning");
                OnPropertyChanged("GameNotRunning");
                OnPropertyChanged("StartButtonText");
            }
        }
        private bool mGamePaused;
        public bool GamePaused
        {
            get { return mGamePaused; }
            set
            {
                if (mGamePaused != value) mGamePaused = value;
                OnPropertyChanged("GamePaused");
                OnPropertyChanged("StartButtonText");
            }
        }

        private HighScores mHighScores;
        private string mHighScoresPath = "highscores";
    }
}

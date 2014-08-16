using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Windows;

using Engine;

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
                int lPosition = mHighScores.HighScorePosition(aScore);
                DateTime lTime = DateTime.Now;
                InputNamePopUp lWindow = new InputNamePopUp();

                var lContext = lWindow.DataContext as InputNamePopUpViewModel;
                if (lContext != null)
                {
                    lContext.NewScore = aScore;
                    lContext.Position = lPosition;
                }

                lWindow.ShowDialog();

                mHighScores.AddHighScore(lWindow.NameTextBox.Text, aScore, lTime);
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
                OnPropertyChanged("PausedTextVisibility");
                OnPropertyChanged("GameInfoVisibility");
            }
        }

        public Visibility PausedTextVisibility
        {
            get
            {
                if (!GamePaused)
                    return Visibility.Hidden;
                else
                    return Visibility.Visible;
            }
        }

        public Visibility GameInfoVisibility
        {
            get
            {
                if (!GameRunning)
                    return Visibility.Hidden;
                else
                    return Visibility.Visible;
            }
        }
        private Rocket mRocket;
        public Rocket Rocket
        {
            get { return mRocket; }
            set
            {
                if (mRocket == value) return;
                mRocket = value;
                OnPropertyChanged("Rocket");
            }
        }

        private HighScores mHighScores;
        private string mHighScoresPath = "highscores";
    }
}

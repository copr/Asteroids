using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Windows;

namespace GameTest2
{
    public class MainMenuViewModel : BaseViewModel
    {
        public MainMenuViewModel(MainWindowViewModel aMain)
        {
            mMain = aMain;

        }

        public Visibility Visibility
        {
            get
            {
                if (!GameRunning || GamePaused)
                    return Visibility.Visible;
                else
                    return Visibility.Hidden;
            }
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
                OnPropertyChanged("Visibility");
                OnPropertyChanged("EndButtonText");
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
                OnPropertyChanged("Visibility");
            }
        }
        public bool GameNotRunning
        {
            get { return !GameRunning; }
        }

        public string StartButtonText
        {
            get
            {
                if (!mGameRunning)
                {
                    return "Start new game";
                }
                else
                {
                    return "Continue game";
                }
            }
        }
        public string EndButtonText
        {
            get
            {
                if (!mGameRunning)
                {
                    return "Quit game";
                }
                else
                {
                    return "Stop game";
                }
            }
        }

        public MainWindowViewModel mMain;
    }
}

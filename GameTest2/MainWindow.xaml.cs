using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Timers;
using System.Diagnostics;

using Engine;
using EngineGui;

namespace GameTest2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            MainWindowViewModel lContext = new MainWindowViewModel();
            lContext.StartGame += RunGame;
            lContext.StopGame += EndGame;

            this.DataContext = lContext;

            mMainMenuViewModel = new MainMenuViewModel(lContext);
            mMainMenu.DataContext = mMainMenuViewModel;

            mRocketBitmapFrame = (BitmapFrame)Resources.MergedDictionaries[0]["Rocket"];
            mAsteroidBitmapFrame = (BitmapFrame)Resources.MergedDictionaries[0]["Asteroid"];
            mProjectileBitmapFrame = (BitmapFrame)Resources.MergedDictionaries[0]["Projectile"];
            mExplosionBitmapFrame = (BitmapFrame)Resources.MergedDictionaries[0]["Explosion"];
            mMissileBitmapFrame = (BitmapFrame)Resources.MergedDictionaries[0]["Missile"];

            mGameRoom.ControlActionEvent += InvokeAction;

            mLevelManager.Interval = 1000;
            mLevelManager.AutoReset = true;
            mLevelManager.Elapsed += LevelManagerElapsed;

            mLevelManager.Start();
        }

        void LevelManagerElapsed(object sender, ElapsedEventArgs e)
        {
            if (mRocket != null && mAsteroidGenerator != null)
            {
                lock (mLock)
                {
                    if (mRocket.Score < mMaxChanceScore)
                        mAsteroidGenerator.Chance = mMinChance + (mMaxChance - mMinChance) * mRocket.Score / mMaxChanceScore;
                    else
                        mAsteroidGenerator.Chance = mMaxChance;
                }
            }
        }

        private void keyDown(object sender, KeyEventArgs e)
        {
            mGameRoom.KeyDown(e);

            MainWindowViewModel lContext = this.DataContext as MainWindowViewModel;
            if (lContext != null)
            {
                if (lContext.GameRunning)
                {
                    if (e.Key == Key.P || e.Key == Key.Escape)
                    {
                        if (mGameRoom.IsRunning)
                        {
                            PauseGame();
                        }
                        else
                        {
                            ContinueGame();
                        }
                    }
                }
            }
        }
        private void keyUp(object sender, KeyEventArgs e)
        {
            mGameRoom.KeyUp(e);
        }
        public void RunGame()
        {
            MainWindowViewModel lContext = this.DataContext as MainWindowViewModel;
            if (lContext != null)
            {
                if (!lContext.GameRunning)
                {
                    StartNewGame();
                }
                else if (lContext.GamePaused)
                {
                    ContinueGame();
                }
            }
        }
        private void StartNewGame()
        {
            MainWindowViewModel lContext = this.DataContext as MainWindowViewModel;
            if (lContext != null)
            {
                lock (mLock)
                {
                    mRocket = new Rocket(mRocketBitmapFrame, mProjectileBitmapFrame, mMissileBitmapFrame, mExplosionBitmapFrame, 96, 64,
                        new Point(mGameRoom.RoomWidth / 2, mGameRoom.RoomHeight / 2),
                        new List<Key> { Key.Up, Key.Down, Key.Left, Key.Right, Key.LeftCtrl, Key.LeftShift });

                    mAsteroidGenerator = new AsteroidGenerator(mAsteroidBitmapFrame, mExplosionBitmapFrame);
                    mAsteroidGenerator.Chance = 0.005;

                    ActualScoreLabel.DataContext = mRocket;

                    mGameRoom.InvokeAction(ERoomAction.AddObject, mRocket);
                    mGameRoom.InvokeAction(ERoomAction.AddObject, mAsteroidGenerator);

                    mGameRoom.Run();

                    lContext.GameRunning = true;
                    mMainMenuViewModel.GameRunning = true;

                    lContext.GamePaused = false;
                    mMainMenuViewModel.GamePaused = false;
                }
            }
        }
        private void ContinueGame()
        {
            MainWindowViewModel lContext = this.DataContext as MainWindowViewModel;
            if (lContext != null)
            {
                mGameRoom.Run();
                lContext.GamePaused = false;
                mMainMenuViewModel.GamePaused = false;
            }
        }
        private void PauseGame()
        {
            MainWindowViewModel lContext = this.DataContext as MainWindowViewModel;
            if (lContext != null)
            {
                mGameRoom.Stop();
                lContext.GamePaused = true;
                mMainMenuViewModel.GamePaused = true;
            }
        }
        public void InvokeAction(EControlAction aAction, object arg)
        {
            switch (aAction)
            {
                case EControlAction.GameOver:
                    StopGame();
                    break;
                default:
                    break;
            }
        }
        public void EndGame()
        {
            MainWindowViewModel lContext = this.DataContext as MainWindowViewModel;
            if (lContext != null)
            {
                if (lContext.GameRunning)
                {
                    StopGame();
                }
                else
                {
                    this.Close();
                }
            }
        }
        private void StopGame()
        {
            MainWindowViewModel lContext = this.DataContext as MainWindowViewModel;
            if (lContext != null)
            {
                if (lContext.GameRunning)
                {
                    int lScore = mRocket.Score;
                    lock (mLock)
                    {
                        mRocket = null;
                        mAsteroidGenerator = null;
                    }
                    mGameRoom.Reset();
                    mGameRoom.Stop();

                    lContext.GameRunning = false;
                    mMainMenuViewModel.GameRunning = false;

                    lContext.GamePaused = false;
                    mMainMenuViewModel.GamePaused = false;

                    lContext.HandleNewScore(lScore);

                    ActualScoreLabel.DataContext = null;
                }
            }
        }
        private void MainWindowName_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MainWindowViewModel lContext = this.DataContext as MainWindowViewModel;
            if (lContext != null)
            {
                lContext.SaveHighScores();
            }
        }


        private Timer mLevelManager = new Timer();

        Random mRandom = new Random();
        
        private BitmapFrame mAsteroidBitmapFrame;
        private BitmapFrame mRocketBitmapFrame;
        private BitmapFrame mProjectileBitmapFrame;
        private BitmapFrame mExplosionBitmapFrame;
        private BitmapFrame mMissileBitmapFrame;

        private Rocket mRocket;
        private AsteroidGenerator mAsteroidGenerator;

        private object mLock = new object();

        private double mMinChance = 0.005;
        private double mMaxChance = 0.5;
        private double mMaxChanceScore = 100000;

        private MainMenuViewModel mMainMenuViewModel;

    }
}

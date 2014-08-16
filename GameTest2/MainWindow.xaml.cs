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
            mAsteroid2BitmapFrame = (BitmapFrame)Resources.MergedDictionaries[0]["Asteroid2"];
            mAsteroid3BitmapFrame = (BitmapFrame)Resources.MergedDictionaries[0]["Asteroid3"];
            mProjectileBitmapFrame = (BitmapFrame)Resources.MergedDictionaries[0]["Projectile"];
            mExplosionBitmapFrame = (BitmapFrame)Resources.MergedDictionaries[0]["Explosion"];
            mExplosion2BitmapFrame = (BitmapFrame)Resources.MergedDictionaries[0]["Explosion2"];
            mMissileBitmapFrame = (BitmapFrame)Resources.MergedDictionaries[0]["Missile"];

            mGameRoom.ControlActionEvent += InvokeAction;

            mLevelManager.Interval = 1000;
            mLevelManager.AutoReset = true;
            mLevelManager.Elapsed += LevelManagerElapsed;

            GenerateAsteroidTypes();

            mLevelManager.Start();
        }

        private void GenerateAsteroidTypes()
        {
            AsteroidType lType1 = new AsteroidType()
            {
                BitmapFrame = mAsteroidBitmapFrame,
                ExplosionFrame = mExplosionBitmapFrame,
                MaxSize = 64,
                MinSize = 48,
                MinSizeForChildren = 16,
                MinSpeed = 2,
                MaxSpeed = 4,
                MissileDamage = 1,
                PointsMultiplier = 1,
                ProjectileDamage = 1,
                Strength = 1,
                TypeName = "Basic",
                MaxRotationSpeed = 4
            };
            ChancesLineConfiguration lChances1 = new ChancesLineConfiguration()
            {
                MaxChance = 0.18,
                MaxChanceScore = 18000,
                MinChance = 0.01,
                MinChanceScore = 0
            };
            AsteroidType lType2 = new AsteroidType()
            {
                BitmapFrame = mAsteroid2BitmapFrame,
                ExplosionFrame = mExplosionBitmapFrame,
                MaxSize = 128,
                MinSize = 80,
                MinSizeForChildren = 32,
                MinSpeed = 2,
                MaxSpeed = 3,
                MissileDamage = 0.5,
                PointsMultiplier = 2.5,
                ProjectileDamage = 0.2,
                Strength = 2,
                TypeName = "Advanced",
                MaxRotationSpeed = 1
            };
            ChancesLineConfiguration lChances2 = new ChancesLineConfiguration()
            {
                MaxChance = 0.02,
                MaxChanceScore = 50000,
                MinChance = 0.000,
                MinChanceScore = 4000
            };
            AsteroidType lType3 = new AsteroidType()
            {
                BitmapFrame = mAsteroid3BitmapFrame,
                ExplosionFrame = mExplosionBitmapFrame,
                MaxSize = 48,
                MinSize = 24,
                MinSizeForChildren = 96,
                MinSpeed = 8,
                MaxSpeed = 12,
                MissileDamage = 0.5,
                PointsMultiplier = 3.5,
                ProjectileDamage = 0.05,
                Strength = 8,
                TypeName = "Elite",
                MaxRotationSpeed = 0
            };
            ChancesLineConfiguration lChances3 = new ChancesLineConfiguration()
            {
                MaxChance = 0.015,
                MaxChanceScore = 40000,
                MinChance = 0.00,
                MinChanceScore = 10000
            };

            mAsteroidTypes.Add(lType1, lChances1);
            mAsteroidTypes.Add(lType2, lChances2);
            mAsteroidTypes.Add(lType3, lChances3);
        }

        void LevelManagerElapsed(object sender, ElapsedEventArgs e)
        {
            (sender as System.Timers.Timer).Stop();
            if (mRocket != null && mAsteroidGenerator != null)
            {
                lock (mLock)
                {
                    foreach (var lType in mAsteroidTypes)
                    {
                        if (mRocket.Score < lType.Value.MinChanceScore)
                        {
                            mAsteroidGenerator.SetChance(lType.Key, lType.Value.MinChance);
                        }
                        else if (mRocket.Score > lType.Value.MaxChanceScore)
                        {
                            mAsteroidGenerator.SetChance(lType.Key, lType.Value.MaxChance);
                        }
                        else
                        {
                            double lChance = ((double)mRocket.Score - lType.Value.MinChanceScore) / (lType.Value.MaxChanceScore - lType.Value.MinChanceScore) * (lType.Value.MaxChance - lType.Value.MinChance) + lType.Value.MinChance;
                            mAsteroidGenerator.SetChance(lType.Key, lChance);
                        }
                    }
                }
            }
            (sender as System.Timers.Timer).Start();
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
                    mRocket = new Rocket(mRocketBitmapFrame, mProjectileBitmapFrame, mMissileBitmapFrame, mExplosion2BitmapFrame, 96, 64,
                        new Point(mGameRoom.RoomWidth / 2, mGameRoom.RoomHeight / 2),
                        new List<Key> { Key.Up, Key.Down, Key.Left, Key.Right, Key.LeftCtrl, Key.LeftShift });

                    mAsteroidGenerator = new AsteroidGenerator();

                    foreach (var lType in mAsteroidTypes)
                    {
                        mAsteroidGenerator.AddAsteroidType(lType.Key);
                    }
                    lContext.Rocket = mRocket;

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
                    StopGameDelayed(2000);
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
        private void StopGameDelayed(double aTimeMs)
        {
            lock (mStopLock)
            {
                if (mStopTimer == null)
                {
                    mStopTimer = new Timer();
                    mStopTimer.Interval = aTimeMs;
                    mStopTimer.Elapsed += new ElapsedEventHandler((x, y) =>
                    {
                        mStopTimer.Stop();
                        mStopTimer.Dispose();
                        mStopTimer = null;

                        this.Dispatcher.Invoke(new Action(() =>
                        {
                            StopGame();
                        }));
                    });

                    mStopTimer.Start();
                }
            }
        }
        private Timer mStopTimer;
        private object mStopLock = new object();
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
                        lContext.Rocket = null;
                        mAsteroidGenerator = null;
                    }
                    mGameRoom.Reset();
                    mGameRoom.Stop();

                    lContext.GameRunning = false;
                    mMainMenuViewModel.GameRunning = false;

                    lContext.GamePaused = false;
                    mMainMenuViewModel.GamePaused = false;

                    lContext.HandleNewScore(lScore);
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

        private System.Timers.Timer mLevelManager = new System.Timers.Timer();

        Random mRandom = new Random();

        private BitmapFrame mAsteroidBitmapFrame;
        private BitmapFrame mAsteroid2BitmapFrame;
        private BitmapFrame mAsteroid3BitmapFrame;
        private BitmapFrame mRocketBitmapFrame;
        private BitmapFrame mProjectileBitmapFrame;
        private BitmapFrame mExplosionBitmapFrame;
        private BitmapFrame mExplosion2BitmapFrame;
        private BitmapFrame mMissileBitmapFrame;

        private Rocket mRocket;
        private AsteroidGenerator mAsteroidGenerator;

        private object mLock = new object();

        private Dictionary<AsteroidType, ChancesLineConfiguration> mAsteroidTypes = new Dictionary<AsteroidType, ChancesLineConfiguration>();

        private MainMenuViewModel mMainMenuViewModel;
        
    }
}

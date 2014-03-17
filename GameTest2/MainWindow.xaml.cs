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
using System.Threading;


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
            
            mRocketBitmapFrame = (BitmapFrame)Resources.MergedDictionaries[0]["Rocket"];
            mAsteroidBitmapFrame = (BitmapFrame)Resources.MergedDictionaries[0]["Asteroid"];
            mProjectileBitmapFrame = (BitmapFrame)Resources.MergedDictionaries[0]["Projectile"];
            mExplosionBitmapFrame = (BitmapFrame)Resources.MergedDictionaries[0]["Explosion"];

            mGameRoom.ControlActionFunction = new UserControlLibrary.GameRoom.ControlActionRequest(InvokeAction);

            mClock = new Timer(ClockTick, null, 0, 10);
        }

        private void keyDown(object sender, KeyEventArgs e)
        {
            mGameRoom.KeyDown(e);

            if (e.Key == Key.P)
            {
                if (mGameRunning)
                    mGameRunning = false;
                else
                    mGameRunning = true;
            }
        }

        private void keyUp(object sender, KeyEventArgs e)
        {
            mGameRoom.KeyUp(e);
        }

        private void ClockTick(Object state)
        {
            Dispatcher.Invoke(new Action(UpdateObjects), null);
        }

        private void UpdateObjects()
        {
            if (mGameRunning)
                mGameRoom.ClockTick();
        }

        private void GameRoomLoaded(object sender, RoutedEventArgs e)
        {

        }

        private void mStartGameButton_Click(object sender, RoutedEventArgs e)
        {
            StartNewGame();
            mStartGameButton.Visibility = System.Windows.Visibility.Hidden;
        }
        private void StartNewGame()
        {
            mGameRoom.AddObject(new Rocket(mRocketBitmapFrame, mProjectileBitmapFrame, mExplosionBitmapFrame, 96, 64,
                new Point(mGameRoom.RoomWidth / 2, mGameRoom.RoomHeight / 2),
                new List<Key> { Key.Up, Key.Down, Key.Left, Key.Right, Key.LeftCtrl }));

            mGameRoom.AsteroidGenerator = new AsteroidGenerator(mAsteroidBitmapFrame, mExplosionBitmapFrame);
            mGameRoom.AsteroidChance = 0.04;
            
            mGameRunning = true;
        }

        public void InvokeAction(EControlAction aAction, object arg)
        {
            switch (aAction)
            {
                case EControlAction.GameOver:
                    mGameRoom.Reset();
                    mGameRunning = false;
                    mStartGameButton.Visibility = System.Windows.Visibility.Visible;
                    break;
                default:
                    break;
            }
        }

        Random mRandom = new Random();

        private Timer mClock;
        private BitmapFrame mAsteroidBitmapFrame;
        private BitmapFrame mRocketBitmapFrame;
        private BitmapFrame mProjectileBitmapFrame;
        private BitmapFrame mExplosionBitmapFrame;

        private bool mGameRunning = false;
    }
}

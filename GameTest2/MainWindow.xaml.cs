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

            mObjects = new List<INotifiable>();

            mRocket = new Rocket(Key.Down, Key.Up, Key.Left, Key.Right,
               "Image/rocket.png",
                96, 96, GameCanvas, new Point(GameCanvas.Width / 2, GameCanvas.Height / 2));

            mObjects.Add(mRocket);
            mObjects.Add(new Asteroid(GameCanvas));
            mTimer = new Timer(Update, null, 0, 10);
        }

        private void keyDown(object sender, KeyEventArgs e)
        {
            mRocket.KeyDown(e);
            if(e.Key == Key.Space)
            {
                String lPath = "C:/Users/copr/Documents/Visual Studio 2013/Projects/Game/GameTest2/Image/shot.png";
                mObjects.Add(new Shot(lPath, mRocket.Position, GameCanvas, mRocket.mVerticalSpeed , mRocket.mHorizontalSpeed));
            }
        }

        private void keyUp(object sender, KeyEventArgs e)
        {
            mRocket.KeyUp(e);
        }

        private void Update(Object state)
        {          
            Dispatcher.Invoke(new Action(MoveRocket), null);         
        }

        private void MoveRocket()
        {
            foreach (INotifiable x in mObjects)
            { 
                x.Notify(null);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            mObjects.Add(new Asteroid(GameCanvas));
        }
        
        private Rocket mRocket;

        private List<INotifiable> mObjects;
        private Timer mTimer;

       
    }
}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using GameTest2;

namespace UserControlLibrary
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class GameRoom : UserControl
    {
        public GameRoom()
        {
            InitializeComponent();
            mCanvas.ClipToBounds = true;
        }
        public void Repaint()
        {
            foreach (MovingObject o in mAllObjects)
            {
                Canvas.SetLeft(o.Image, o.Position.X - o.Image.Width / 2);
                Canvas.SetTop(o.Image, o.Position.Y - o.Image.Height / 2);
            }
        }
        public void AddObject(MovingObject o)
        {
            mCanvas.Children.Add(o.Image);
            mAllObjects.Add(o);
            if (o is Asteroid)
                mAsteroids.Add(o);
            o.CreateObjectFunction = new MovingObject.AddObject(AddObjectRequest);
        }
        public void AddObjectRequest(MovingObject o)
        {
            mAddRequests.Add(o);
        }
        private void ReturnObjectFromOutside(MovingObject o)
        {
            if (o.Position.X > RoomWidth + o.OutsideSize)
            {
                o.Position = new Point(-o.OutsideSize, o.Position.Y);
            }
            if (o.Position.X < -o.OutsideSize)
            {
                o.Position = new Point(RoomWidth + o.OutsideSize, o.Position.Y);
            }
            if (o.Position.Y > RoomHeight + o.OutsideSize)
            {
                o.Position = new Point(o.Position.X, -o.OutsideSize);
            }
            if (o.Position.Y < -o.OutsideSize)
            {
                o.Position = new Point(o.Position.X, RoomHeight + o.OutsideSize);
            }
        }
        public void RemoveObject(MovingObject o)
        {
            mCanvas.Children.Remove(o.Image);
            mAllObjects.Remove(o);
            if (o is Asteroid)
                mAsteroids.Remove(o);
        }
        public void ClockTick()
        {
            MoveObjects();
            DealWithOutsideObjects();
            if (mRandom.NextDouble() < AsteroidChance && mAsteroidGenerator != null)
            {
                AddObject(mAsteroidGenerator.CreateAsteroid());
            }
            foreach (MovingObject o in mAddRequests)
            {
                AddObject(o);
            }
            mAddRequests.Clear();
            Repaint();
        }
        public new void KeyDown(KeyEventArgs e)
        {
            foreach (MovingObject o in mAllObjects)
            {
                if (o is ControllableMovingObject)
                {
                    (o as ControllableMovingObject).KeyDown(e);
                }
            }
        }
        public new void KeyUp(KeyEventArgs e)
        {
            foreach (MovingObject o in mAllObjects)
            {
                if (o is ControllableMovingObject)
                {
                    (o as ControllableMovingObject).KeyUp(e);
                }
            }
        }
        
        private void MoveObjects()
        {
            foreach (MovingObject o in mAllObjects)
            {
                o.ClockTick();
            }
        }
        private void DealWithOutsideObjects()
        {
            List<MovingObject> lObjectsToRemove = new List<MovingObject>();

            foreach (MovingObject o in mAllObjects)
            {
                if (o.Position.X > RoomWidth + o.OutsideSize
                    || o.Position.X < -o.OutsideSize
                    || o.Position.Y > RoomHeight + o.OutsideSize
                    || o.Position.Y < -o.OutsideSize)
                {
                    if (o.OutsideRoomAction == EOutsideRoomAction.Destroy)
                        lObjectsToRemove.Add(o);
                    else if (o.OutsideRoomAction == EOutsideRoomAction.Return)
                    {
                        ReturnObjectFromOutside(o);
                    }
                }

            }
            
            foreach (MovingObject o in lObjectsToRemove)
            {
                RemoveObject(o);
            }
        }
        
        public double RoomWidth
        {
            get
            {
                return mCanvas.ActualWidth;
            }
        }
        public double RoomHeight
        {
            get
            {
                return mCanvas.ActualHeight;
            }
        }

        public AsteroidGenerator AsteroidGenerator
        {
            get
            {
                return mAsteroidGenerator;
            }
            set
            {
                mAsteroidGenerator = value;
                if (mAsteroidGenerator != null)
                {
                    mAsteroidGenerator.RoomHeight = RoomHeight;
                    mAsteroidGenerator.RoomWidth = RoomWidth;
                }
            }
        }
        public double AsteroidChance
        {
            get;
            set;
        }
        public double MaxAsteroidCount
        { 
            get;
            set;
        }

        private HashSet<MovingObject> mAllObjects = new HashSet<MovingObject>();
        private HashSet<MovingObject> mAsteroids = new HashSet<MovingObject>();
        private List<MovingObject> mAddRequests = new List<MovingObject>();

        private AsteroidGenerator mAsteroidGenerator;
        private Random mRandom = new Random();
    }
}
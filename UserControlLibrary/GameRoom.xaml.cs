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
            foreach (BasicObject o in mAllObjects)
            {
                Canvas.SetLeft(o.Image, o.Position.X - o.Image.Width / 2);
                Canvas.SetTop(o.Image, o.Position.Y - o.Image.Height / 2);
            }
        }
        public void AddObject(BasicObject o)
        {
            mCanvas.Children.Add(o.Image);
            mAllObjects.Add(o);
            if (o is Asteroid)
                mAsteroids.Add(o);

            o.RoomActionFunction = new BasicObject.RoomActionRequest(InvokeAction);
            o.Initialize();
        }

        public void SolveRequests()
        {
            bool lGameOverRequest = false;
            foreach (Tuple<ERoomAction, object> request in mRequests)
            {
                switch (request.Item1)
                {
                    case ERoomAction.AddObject:
                        AddObject((BasicObject)request.Item2);
                        break;
                    case ERoomAction.RemoveObject:
                        RemoveObject((BasicObject)request.Item2);
                        break;
                    case ERoomAction.GameOver:
                        lGameOverRequest = true;
                        break;
                    default:
                        break;
                }
                if (lGameOverRequest)
                    break;
            }
            if (lGameOverRequest)
                mControlActionRequest(EControlAction.GameOver, null);

            mRequests.Clear();
        }
        private void ReturnObjectFromOutside(BasicObject o)
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
        public void RemoveObject(BasicObject o)
        {
            mCanvas.Children.Remove(o.Image);
            mAllObjects.Remove(o);
            if (o is Asteroid)
                mAsteroids.Remove(o);
        }
        private void SolveCollisions()
        {
            foreach (BasicObject o1 in mAllObjects)
            {
                foreach (BasicObject o2 in mAllObjects)
                {
                    if (o1 != o2)
                    {
                        o1.SolveCollision(o2);
                    }
                }
            }
        }
        public void ClockTick()
        {
            foreach (BasicObject o in mAllObjects)
            {
                if (o is Rocket)
                {
                    RocketHealth = (o as Rocket).mHealth;
                }        
                o.ClockTick();                     
            }

            DealWithOutsideObjects();
            SolveCollisions();
            
            if (mRandom.NextDouble() < AsteroidChance && mAsteroidGenerator != null)
            {
                AddObject(mAsteroidGenerator.CreateAsteroid());
            }

            SolveRequests();
            Repaint();
        }
        public new void KeyDown(KeyEventArgs e)
        {
            foreach (BasicObject o in mAllObjects)
            {
                if (o is ControllableMovingObject)
                {
                    (o as ControllableMovingObject).KeyDown(e);
                }
            }
        }
        public void InvokeAction(ERoomAction aAction, object arg)
        {
            mRequests.Add(new Tuple<ERoomAction, object>(aAction, arg));
        }
        public void Reset()
        {
            mAsteroids.Clear();
            mAllObjects.Clear();
            mRequests.Clear();
            mCanvas.Children.Clear();
            mAsteroidGenerator = null;
        }
        public new void KeyUp(KeyEventArgs e)
        {
            foreach (BasicObject o in mAllObjects)
            {
                if (o is ControllableMovingObject)
                {
                    (o as ControllableMovingObject).KeyUp(e);
                }
            }
        }
        private void DealWithOutsideObjects()
        {
            List<BasicObject> lObjectsToRemove = new List<BasicObject>();

            foreach (BasicObject o in mAllObjects)
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
            
            foreach (BasicObject o in lObjectsToRemove)
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

        public double RocketHealth { get; set; }
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

        public ControlActionRequest ControlActionFunction
        {
            set
            {
                mControlActionRequest = value;
            }
        }

        private HashSet<BasicObject> mAllObjects = new HashSet<BasicObject>();
        private HashSet<BasicObject> mAsteroids = new HashSet<BasicObject>();

        private List<Tuple<ERoomAction, object>> mRequests = new List<Tuple<ERoomAction, object>>();

        private AsteroidGenerator mAsteroidGenerator;
        private Random mRandom = new Random();

        private ControlActionRequest mControlActionRequest;

        public delegate void ControlActionRequest(EControlAction aAction, object arg);
    }
}
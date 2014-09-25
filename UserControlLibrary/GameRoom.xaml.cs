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
using System.Timers;

using Engine;

namespace EngineGui
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class GameRoom : UserControl, IGameRoom
    {
        #region Constructor

        public GameRoom()
        {
            InitializeComponent();
            mCanvas.ClipToBounds = true;

            mClock.Elapsed += new ElapsedEventHandler(ClockTick);
            mClock.AutoReset = true;
            mClock.Interval = mClockInterval;
        }

        #endregion
        #region Public methods

        public void Run()
        {
            mClock.Start();
            mIsRunning = true;
        }
        public void Stop()
        {
            mClock.Stop();
            mIsRunning = false;
        }
        public void InvokeAction(ERoomAction aAction, object arg)
        {
            lock (mRequests)
            {
                mRequests.Add(new Tuple<ERoomAction, object>(aAction, arg));
            }
        }
        public new void KeyDown(KeyEventArgs e)
        {
            foreach (ControllableMovingObject o in mObjects.OfType<ControllableMovingObject>())
            {
                o.KeyDown(e);
            }
        }
        public new void KeyUp(KeyEventArgs e)
        {
            foreach (ControllableMovingObject o in mObjects.OfType<ControllableMovingObject>())
            {
                o.KeyUp(e);
            }
        }
        public void Reset()
        {
            mObjects.Clear();
            mRequests.Clear();
            mCanvas.Children.Clear();
        }
        public IEnumerable<T> GetObjectsOfType<T>()
        {
            return mObjects.OfType<T>();
        }

        #endregion
        #region Protected methods

        protected void RaiseControlActionEvent(EControlAction aAction, object arg)
        {
            if (ControlActionEvent != null)
            {
                ControlActionEvent(aAction, arg);
            }
        }

        #endregion
        #region Private methods

        private void ClockTick(object arg, ElapsedEventArgs e)
        {
            Dispatcher.Invoke(new Action(() =>
            {
                foreach (BaseObject o in mObjects)
                {
                    if (!o.IsDestroyed)
                    {
                        o.ClockTick();
                    }
                }

                DealWithOutsideObjects();
                SolveCollisions();
                SolveRequests();
                Repaint();
            }), null);
        }
        private void SolveRequests()
        {
            bool lGameOverRequest = false;

            var lRequestsSoFar = mRequests.ToList();

            foreach (Tuple<ERoomAction, object> request in lRequestsSoFar)
            {
                switch (request.Item1)
                {
                    case ERoomAction.AddObject:
                        AddObject((BaseObject)request.Item2);
                        break;
                    case ERoomAction.RemoveObject:
                        RemoveObject((BaseObject)request.Item2);
                        break;
                    case ERoomAction.GameOver:
                        lGameOverRequest = true;
                        break;
                    default:
                        break;
                }
            }
            if (lGameOverRequest)
            {
                RaiseControlActionEvent(EControlAction.GameOver, null);
            }

            mRequests.RemoveAll(x => lRequestsSoFar.Contains(x));
        }
        private void AddObject(BaseObject o)
        {
            if (o is PhysicalObject)
            {
                var po = o as PhysicalObject;
                mCanvas.Children.Add(po.Image);

                foreach (var lCollisionMask in po.CollisionMask)
                {
                    mCanvas.Children.Add(lCollisionMask.Image);
                }
            }

            o.GameRoom = this;
            mObjects.Add(o);
            o.RoomActionEvent += InvokeAction;
            o.Initialize();
        }
        private void Repaint()
        {
            int lGlobalOffsetX = 0;
            int lGlobalOffsetY = 0;

            foreach (PhysicalObject o in mObjects.OfType<PhysicalObject>())
            {
                Canvas.SetLeft(o.Image, o.Position.X - o.Image.Width / 2 + lGlobalOffsetX);
                Canvas.SetTop(o.Image, o.Position.Y - o.Image.Height / 2 + lGlobalOffsetY);

                foreach (var lCollisionMask in o.CollisionMask)
                {
                    if (lCollisionMask.IsVisible)
                    {
                        Canvas.SetLeft(lCollisionMask.Image, lCollisionMask.Position.X + lCollisionMask.HorizontalShift + lGlobalOffsetX);
                        Canvas.SetTop(lCollisionMask.Image, lCollisionMask.Position.Y + lCollisionMask.VerticalShift + lGlobalOffsetY);
                    }
                }
            }
        }
        private void DealWithOutsideObjects()
        {
            foreach (BaseObject o in mObjects)
            {
                if (o is PhysicalObject)
                {
                    PhysicalObject po = (PhysicalObject)o;
                    if (po.Position.X > RoomWidth + po.OutsideSize
                        || po.Position.X < -po.OutsideSize
                        || po.Position.Y > RoomHeight + po.OutsideSize
                        || po.Position.Y < -po.OutsideSize)
                    {
                        if (po.OutsideRoomAction == EOutsideRoomAction.Destroy)
                            po.Destroy();
                        else if (po.OutsideRoomAction == EOutsideRoomAction.Return)
                        {
                            ReturnObjectFromOutside(po);
                        }
                    }
                }
            }
        }
        private void ReturnObjectFromOutside(PhysicalObject o)
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
        private void RemoveObject(BaseObject o)
        {
            if (o is PhysicalObject)
            {
                PhysicalObject po = (PhysicalObject)o;
                mCanvas.Children.Remove(po.Image);

                foreach (var lCollisionMask in po.CollisionMask)
                {
                    mCanvas.Children.Remove(lCollisionMask.Image);
                }
            }
            mObjects.Remove(o);
        }
        private void SolveCollisions()
        {
            var lCollisionPairs = new List<Tuple<PhysicalObject, PhysicalObject>>();
            var lPhysicalObjects = mObjects.OfType<PhysicalObject>().ToList();

            for (int lIndex1 = 0; lIndex1 < lPhysicalObjects.Count - 1; lIndex1++)
            {
                for (int lIndex2 = lIndex1 + 1; lIndex2 < lPhysicalObjects.Count; lIndex2++)
                {
                    var lObject1 = lPhysicalObjects[lIndex1];
                    var lObject2 = lPhysicalObjects[lIndex2];
                }
            }

                foreach (PhysicalObject o1 in mObjects.OfType<PhysicalObject>())
                {
                    foreach (PhysicalObject o2 in mObjects.OfType<PhysicalObject>())
                    {
                        if (o1 != o2)
                        {
                            o1.SolveIfCollision(o2);
                        }
                    }
                }
        }

        #endregion
        #region Properties

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
        public bool IsRunning
        {
            get
            {
                return mIsRunning;
            }
        }

        #endregion
        #region Events

        public event ControlActionRequest ControlActionEvent;

        #endregion
        #region Members
        private Timer mClock = new Timer();
        private HashSet<BaseObject> mObjects = new HashSet<BaseObject>();
        private List<Tuple<ERoomAction, object>> mRequests = new List<Tuple<ERoomAction, object>>();
        private Random mRandom = new Random();
        private bool mIsRunning = false;
        private int mClockInterval = 10;
        #endregion
        #region Delegates

        public delegate void ControlActionRequest(EControlAction aAction, object arg);

        #endregion

    }
}
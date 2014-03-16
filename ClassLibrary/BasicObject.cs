using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace GameTest2
{
    public abstract class BasicObject
    {
        public BasicObject(BitmapFrame aBitmapFrame, double aWidth, double aHeight, Point aPosition)
        {
            mImage = new Image();
            mBitmapFrame = aBitmapFrame;
            mImage.Source = mBitmapFrame;
            mImage.RenderTransformOrigin = new Point(.5, .5);
            mImage.Stretch = System.Windows.Media.Stretch.Fill;
            mImage.Width = aWidth;
            mImage.Height = aHeight;

            mOutsideSize = Math.Min(mImage.Width, mImage.Height) / 2;

            mPosition = aPosition;

            
        }
        public void RotateImage(double aAngle)
        {
            RotateTransform lRotateTransform = new RotateTransform();
            lRotateTransform.Angle = aAngle;
            Image.RenderTransform = lRotateTransform;   
        }
        public double Distance(BasicObject aOther)
        {
            return Math.Sqrt((mPosition.X - aOther.Position.X) * (mPosition.X - aOther.Position.X)
                + (mPosition.Y - aOther.Position.Y) * (mPosition.Y - aOther.Position.Y));
        }
        public abstract void ClockTick();

        public virtual void DestroyEffect()
        {

        }
        public virtual void Initialize()
        {

        }

        public void DefaultCollisionSolve(BasicObject o)
        {
            if (Distance(o) < CollisionRadius + o.CollisionRadius && mInvincibleSteps == 0)
            {
                DestroyEffect();
                mRemoveObject(this);
            }
        }

        public Point Position
        {
            get
            {
                return mPosition;
            }
            set
            {
                mPosition = value;
            }
        }

        public virtual double CollisionRadius
        {
            get
            {
                return 0;
            }
        }
        public double OutsideSize
        {
            get
            {
                return mOutsideSize;
            }
        }
        public Image Image
        {
            get
            {
                return mImage;
            }
        }

        public virtual EOutsideRoomAction OutsideRoomAction
        {
            get
            {
                return EOutsideRoomAction.Destroy;
            }
        }

        public ActionWithObject CreateObjectFunction
        {
            set
            {
                mAddObject = value;
            }
        }

        public ActionWithObject RemoveObjectFunction
        {
            set
            {
                mRemoveObject = value;
            }
        }

        public void SolveCollision(BasicObject o)
        {
            ActionWithObject lAction;
            if (mCollisionBehavior.TryGetValue(o.GetType(), out lAction))
            {
                lAction(o);
            }

        }



        private BitmapFrame mBitmapFrame;
        private Image mImage;
        private Point mPosition = new Point();
        protected ActionWithObject mAddObject;
        protected ActionWithObject mRemoveObject;
        protected Random mRandom = new Random();

        private double mOutsideSize;
        protected int mInvincibleSteps = 10;

        public delegate void ActionWithObject(BasicObject o);

        protected Dictionary<Type, ActionWithObject> mCollisionBehavior =
            new Dictionary<Type, ActionWithObject>();
    }
}

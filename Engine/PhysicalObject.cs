using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Engine
{
    public abstract class PhysicalObject : BaseObject
    {
        #region Constructor

        public PhysicalObject(BitmapFrame aBitmapFrame, double aWidth, double aHeight, Point aPosition)
        {
            mBitmapFrame = aBitmapFrame;

            mImage.Source = mBitmapFrame;
            mImage.Width = aWidth;
            mImage.Height = aHeight;

            mOutsideSize = Math.Min(mImage.Width, mImage.Height) / 2;

            mPosition = aPosition;
        }

        #endregion
        protected void RotateImage(double aAngle)
        {
            RotateTransform lRotateTransform = new RotateTransform();
            lRotateTransform.Angle = aAngle;
            Image.RenderTransform = lRotateTransform;   
        }
        public double Distance(PhysicalObject aOther)
        {
            return Math.Sqrt(SquaredDistance(aOther));
        }
        public double SquaredDistance(PhysicalObject aOther)
        {
            return (mPosition.X - aOther.Position.X) * (mPosition.X - aOther.Position.X)
                + (mPosition.Y - aOther.Position.Y) * (mPosition.Y - aOther.Position.Y);
        }

        protected override void CustomDestroyActions()
        {
            base.CustomDestroyActions();
            if (mImage != null)
            {
                mImage.Visibility = Visibility.Hidden;
            }
        }

        protected virtual void DestroyEffect()
        {

        }

        protected bool IsCollision(PhysicalObject o)
        {
            return (Distance(o) < CollisionRadius + o.CollisionRadius && mInvincibleSteps == 0);
        }

        protected void DefaultCollisionSolve(PhysicalObject o)
        {
            if (IsCollision(o))
            {
                Destroy();
                DestroyEffect();
            }
        }

        public void SolveCollision(PhysicalObject o)
        {
            if (!IsDestroyed)
            {
                ActionWithObject<PhysicalObject> lAction;
                if (mCollisionBehavior.TryGetValue(o.GetType(), out lAction))
                {
                    lAction(o);
                }
            }
        }

        #region Properties
        public int Depth
        {
            get
            {
                return Canvas.GetZIndex(mImage);
            }
            set
            {
                Canvas.SetZIndex(mImage, value);
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
        public Image Image
        {
            get
            {
                return mImage;
            }
        }
        public double OutsideSize
        {
            get
            {
                return mOutsideSize;
            }
        }
        public virtual double CollisionRadius
        {
            get
            {
                return double.MinValue;
            }
        }
        public virtual EOutsideRoomAction OutsideRoomAction
        {
            get
            {
                return EOutsideRoomAction.Destroy;
            }
        }

        #endregion
        #region Members

        private int mInvincibleSteps = 0;
        protected int InvincibleSteps
        {
            get
            {
                return mInvincibleSteps;
            }
            set
            {
                mInvincibleSteps = value;
            }
        }
        protected Dictionary<Type, ActionWithObject<PhysicalObject>> mCollisionBehavior =
            new Dictionary<Type, ActionWithObject<PhysicalObject>>();

        private BitmapFrame mBitmapFrame;
        private Image mImage = new Image()
        {
            RenderTransformOrigin = new Point(.5, .5),
            Stretch = System.Windows.Media.Stretch.Fill
        };
        private Point mPosition = new Point();
        private double mOutsideSize;
        #endregion
    }
}


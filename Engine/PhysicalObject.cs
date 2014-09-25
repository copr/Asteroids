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
        #region Methods
        protected void SetImageAngle(double aAngle)
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
        
        protected void DefaultCollisionSolve(PhysicalObject o)
        {
            Destroy();
            DestroyEffect();
        }

        private bool IsValidCollision(PhysicalObject o)
        {
            if (!IsDestroyed && mInvincibleSteps == 0)
            {
                return IsCollision(o);
            }
            else
            {
                return false;
            }
        }
        private bool IsCollision(PhysicalObject o)
        {
            bool lIsCollision = false;
            foreach (var lMyCollisionMask in CollisionMask)
            {
                foreach (var lOthersCollisionmask in o.CollisionMask)
                {
                    if (lMyCollisionMask.IsCollision(lOthersCollisionmask))
                    {
                        lIsCollision = true;
                        break;
                    }
                }
                if (lIsCollision)
                {
                    break;
                }
            }
            return lIsCollision;
        }
        public void SolveIfCollision(PhysicalObject o)
        {
            var lIsValidCollision = IsValidCollision(o);

            if (lIsValidCollision)
            {
                SolveCollision(o);
            }
        }
        protected void SolveCollision(PhysicalObject o)
        {
            ActionWithObject<PhysicalObject> lAction;
            if (mCollisionBehavior.TryGetValue(o.GetType(), out lAction))
            {
                lAction(o);
            }
        }

        public override void ClockTick()
        {
            SetImageAngle(mAngle);
            foreach (var lCollisionMask in CollisionMask)
            {
                lCollisionMask.SetPosition(this.mPosition, this.mAngle);
            }
            if (mInvincibleSteps > 0)
                mInvincibleSteps--;
        }

        #endregion

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
                foreach (var lCollisionMask in CollisionMask)
                {
                    lCollisionMask.Depth = value + 1;
                }
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
        public double Angle
        {
            get { return mAngle; }
        }

        public virtual EOutsideRoomAction OutsideRoomAction
        {
            get
            {
                return EOutsideRoomAction.Destroy;
            }
        }
        public List<CollisionMask> CollisionMask
        {
            get
            {
                return mCollisionMask;
            }
            set
            {
                mCollisionMask = value;
            }
        }
        #endregion
        #region Members

        private List<CollisionMask> mCollisionMask = new List<CollisionMask>();

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

        protected double mAngle;
        #endregion
    }
}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Engine;
using System.Windows.Media.Imaging;
using System.Windows;

namespace GameTest2
{
    public class GuidedMissile : SimpleMovingObject
    {
        public GuidedMissile(double aWidth, double aHeight, BitmapFrame aBitmapFrame, Point aPosition,
            double aDirection, double aSpeed, double aMaxTurnDegrees)
            : this(aBitmapFrame, aWidth, aHeight, aPosition, 
            aSpeed * Math.Sin(aDirection * Math.PI / 180),
            aSpeed * Math.Cos(aDirection * Math.PI / 180),
            aMaxTurnDegrees)
        {

        }
        public GuidedMissile(BitmapFrame aBitmapFrame, double aWidth, double aHeight, Point aPosition,
            double aVerticalSpeed, double aHorizontalSpeed, double aMaxTurnDegrees)
            : base(aBitmapFrame, aWidth, aHeight, aPosition, aVerticalSpeed, aHorizontalSpeed)
        {
            mCollisionBehavior.Add(typeof(Asteroid), DefaultCollisionSolve);

            CollisionMask.Add(new Circle(Math.Min(aWidth, aHeight) / 3.0, this.Position, 0, 0));
            Depth = -10;
            mMaxTurnDegrees = aMaxTurnDegrees;
        }

        public override void ClockTick()
        {
            if (mTarget != null)
            {
                double lDirectionToTarget = 180 / Math.PI * Math.Atan2(mTarget.Position.Y - this.Position.Y, mTarget.Position.X - this.Position.X);
                double lDirection = Direction;

                double lDirectionDifference = lDirectionToTarget - lDirection;

                while (lDirectionDifference < 0)
                    lDirectionDifference += 360;
                while (lDirectionDifference >= 360)
                    lDirectionDifference -= 360;

                double lDirectionChange = 0;
                int lDirectionChangeSign = 0;

                if (lDirectionDifference < 180)
                {
                    lDirectionChangeSign = 1;
                    lDirectionChange = Math.Min(mMaxTurnDegrees, lDirectionDifference);
                }
                else
                {
                    lDirectionChangeSign = -1;
                    lDirectionChange = Math.Min(mMaxTurnDegrees, 360 - lDirectionDifference);
                }

                double lNewDirection = lDirection + lDirectionChangeSign * lDirectionChange;
                double lSpeed = Speed;

                mVerticalSpeed = lSpeed * Math.Sin(lNewDirection * Math.PI / 180);
                mHorizontalSpeed = lSpeed * Math.Cos(lNewDirection * Math.PI / 180);

                mAngle = lNewDirection;
            }

            base.ClockTick();
        }

        public PhysicalObject Target
        {
            get
            {
                return mTarget;
            }
            set
            {
                if (mTarget != null)
                {
                    mTarget.DestroyedEvent -= OnTargetDestroyed;
                }
             
                mTarget = value;
                
                if (mTarget != null)
                {
                    mTarget.DestroyedEvent += OnTargetDestroyed;
                }
            }
        }

        private void OnTargetDestroyed(BaseObject aSender)
        {
            Target = null;
        }

        public Rocket Owner
        {
            get;
            set;
        }

        private PhysicalObject mTarget = null;
        private double mMaxTurnDegrees = 0;
    }
}

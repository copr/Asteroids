using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace GameTest2
{
    public class Asteroid : SimpleMovingObject
    {
        public Asteroid(BitmapFrame aBitmapFrame, BitmapFrame aExplosionFrame, double aSize, Point aPosition,
            double aDirection, double aSpeed)
            : base(aSize, aSize, aBitmapFrame, aPosition, aDirection, aSpeed)
        {
            Random lRandom = new Random();
            mAngle = lRandom.NextDouble() * 360;
            mRotationSpeed = lRandom.NextDouble() * 4 - 2;
            mExplosionFrame = aExplosionFrame;

            mCollisionBehavior.Add(typeof(Asteroid), CollisionSolve);
            mCollisionBehavior.Add(typeof(BasicProjectile), CollisionSolve);
            mCollisionBehavior.Add(typeof(Rocket), RocketCollisionSolve);

            mInvincibleSteps = 4;
        }

        public void CreateChildren()
        {
            if (Image.Width > 16)
            {
                double lAngle = 360 * mRandom.NextDouble();
                mRoomActionRequest(ERoomAction.AddObject, new Asteroid((BitmapFrame)Image.Source, mExplosionFrame, Image.Width / 2,
                    Position,
                    lAngle - 90 + mRandom.NextDouble() * 180, Speed));
                mRoomActionRequest(ERoomAction.AddObject, new Asteroid((BitmapFrame)Image.Source, mExplosionFrame, Image.Width / 2,
                    Position,
                    lAngle + 180 - 90 + mRandom.NextDouble() * 180, Speed));
            }
        }

        public override void ClockTick()
        {
            base.ClockTick();
            
            mAngle += mRotationSpeed;
            RotateImage(mAngle);
            if (mInvincibleSteps > 0)
                mInvincibleSteps--;
        }
        public override double CollisionRadius
        {
            get
            {
                return (Image.Width / 2) * 0.8;
            }
        }

        public void RocketCollisionSolve(BasicObject o)
        {
            if (Distance(o) < CollisionRadius + o.CollisionRadius && mInvincibleSteps == 0)
            {
                DestroyEffect();
                mRoomActionRequest(ERoomAction.RemoveObject, this);
            }
        }
        public void CollisionSolve(BasicObject o)
        {
            if (Distance(o) < CollisionRadius + o.CollisionRadius && mInvincibleSteps == 0)
            {
                CreateChildren();
                DestroyEffect();
                mRoomActionRequest(ERoomAction.RemoveObject, this);
            }
        }
        public override void DestroyEffect()
        {
            mRoomActionRequest(ERoomAction.AddObject, new Explosion(mExplosionFrame, 1.8 * Image.Width, 1.8 * Image.Height, Position));
            mRoomActionRequest(ERoomAction.AddObject, new Explosion(mExplosionFrame, 1.8 * Image.Width, 1.8 * Image.Height, Position));
        }

        private double mAngle;
        private double mRotationSpeed;
        private BitmapFrame mExplosionFrame;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

using Engine;

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

            mCollisionBehavior.Add(typeof(Asteroid), DestroyWithChildren);
            mCollisionBehavior.Add(typeof(BasicProjectile), DestroyWithChildren);
            mCollisionBehavior.Add(typeof(GuidedMissile), DestroyWithoutChildren);
            mCollisionBehavior.Add(typeof(Rocket), DestroyWithoutChildren);

            mInvincibleSteps = 20;
        }

        private void CreateChildren()
        {
            double lAngle = 360 * mRandom.NextDouble();

            List<Asteroid> lChildren = new List<Asteroid>();

            lChildren.Add(new Asteroid((BitmapFrame)Image.Source, mExplosionFrame, Image.Width / 2,
                Position,
                lAngle - 90 + mRandom.NextDouble() * 180, 1.4 * Speed));
            lChildren.Add(new Asteroid((BitmapFrame)Image.Source, mExplosionFrame, Image.Width / 2,
                Position,
                lAngle - 90 + mRandom.NextDouble() * 180, 1.4 * Speed));
            RaiseRoomActionEvent(ERoomAction.AddObject, new Asteroid((BitmapFrame)Image.Source, mExplosionFrame, Image.Width / 2,
                Position,
                lAngle + 180 - 90 + mRandom.NextDouble() * 180, 1.4 * Speed));

            foreach (Asteroid a in lChildren)
            {
                RaiseRoomActionEvent(ERoomAction.AddObject, a);
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

        private void DestroyWithoutChildren(PhysicalObject o)
        {
            if (Distance(o) < CollisionRadius + o.CollisionRadius && mInvincibleSteps == 0)
            {
                Destroy();
                DestroyEffect();
            }
        }
        private void DestroyWithChildren(PhysicalObject o)
        {
            if (Distance(o) < CollisionRadius + o.CollisionRadius && mInvincibleSteps == 0)
            {
                if (Image.Width > 16)
                {
                    Destroy();
                    DestroyEffect();
                    CreateChildren();
                }
                else
                {
                    Destroy();
                    DestroyEffect();
                }
            }
        }

        public override void Destroy()
        {
            base.Destroy();
            RaiseDestroyedEvent();
        }

        protected override void DestroyEffect()
        {
            RaiseRoomActionEvent(ERoomAction.AddObject, new Explosion(mExplosionFrame, 1.8 * Image.Width, 1.8 * Image.Height, Position));
        }

        private double mAngle;
        private double mRotationSpeed;
        private BitmapFrame mExplosionFrame;
    }
}

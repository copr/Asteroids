using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

using Engine;
using System.IO;

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

            mCollisionBehavior.Add(typeof(Asteroid), CollisionWithAsteroid);
            mCollisionBehavior.Add(typeof(BasicProjectile), CollisionWithProjectile);
            mCollisionBehavior.Add(typeof(GuidedMissile), CollisionWithProjectile);
            mCollisionBehavior.Add(typeof(Rocket), DefaultCollisionSolve);

            InvincibleSteps = 20;
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
            if (InvincibleSteps > 0)
                InvincibleSteps--;
        }
        public override double CollisionRadius
        {
            get
            {
                return (Image.Width / 2) * 0.8;
            }
        }
        private void CollisionWithAsteroid(PhysicalObject o)
        {
            if (IsCollision(o))
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
        private void CollisionWithProjectile(PhysicalObject o)
        {
            if (IsCollision(o))
            {
                if (o is BasicProjectile)
                {
                    (o as BasicProjectile).Owner.Score += (int)Image.Width * 10;
                }
                if (o is GuidedMissile)
                {
                    (o as GuidedMissile).Owner.Score += (int)Image.Width * 10;
                }

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
        protected override void DestroyEffect()
        {
            RaiseRoomActionEvent(ERoomAction.AddObject, new Explosion(mExplosionFrame, 1.8 * Image.Width, 1.8 * Image.Height, Position));
        }

        private double mAngle;
        private double mRotationSpeed;
        private BitmapFrame mExplosionFrame;
    }
}

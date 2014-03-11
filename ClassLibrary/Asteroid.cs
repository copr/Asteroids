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
        public Asteroid(BitmapFrame aBitmapFrame, BitmapFrame aExplosionFrame, double aWidth, double aHeight, Point aPosition,
            double aDirection, double aSpeed)
            : base(aWidth, aHeight, aBitmapFrame, aPosition, aDirection, aSpeed)
        {
            Random lRandom = new Random();
            mAngle = lRandom.NextDouble() * 360;
            mRotationSpeed = lRandom.NextDouble() * 4 - 2;
            mExplosionFrame = aExplosionFrame;

            mCollisionBehavior.Add(typeof(Asteroid), DefaultCollisionSolve);
            mCollisionBehavior.Add(typeof(BasicProjectile), DefaultCollisionSolve);
        }

        public override void ClockTick()
        {
            base.ClockTick();
            
            mAngle += mRotationSpeed;
            RotateImage(mAngle);
        }
        public override double CollisionRadius
        {
            get
            {
                return (Image.Width / 2) * 0.8;
            }
        }

        public override void DestroyEffect()
        {
            mAddObject(new Explosion(mExplosionFrame, 1.8 * Image.Width, 1.8 * Image.Height, Position));
        }

        private double mAngle;
        private double mRotationSpeed;
        private BitmapFrame mExplosionFrame;
    }
}

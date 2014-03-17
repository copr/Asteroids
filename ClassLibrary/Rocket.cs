using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Input;
using System.Threading;

namespace GameTest2
{
    public class Rocket : ControllableMovingObject
    {
        /// <summary>
        /// Creates new Rocket object
        /// </summary>
        /// <param name="aBitmapFrame"></param>
        /// <param name="aWidth"></param>
        /// <param name="aHeight"></param>
        /// <param name="aPosition"></param>
        /// <param name="aKeys">Up, Down, Left, Right, Shoot</param>
        public Rocket(BitmapFrame aBitmapFrame, BitmapFrame aProjectileBitmapFrame
            , BitmapFrame aExplosionFrame, double aWidth, double aHeight
            , Point aPosition, List<Key> aKeys)
            : base(aBitmapFrame, aWidth, aHeight, aPosition, aKeys)
        {
            mProjectileBitmapFrame = aProjectileBitmapFrame;
            mExplosionFrame = aExplosionFrame;
            mCollisionBehavior.Add(typeof(Asteroid), CollisionSolve);
            mHealth = 5;
        }

        private void CollisionSolve(BasicObject o)
        {
            if (Distance(o) < CollisionRadius + o.CollisionRadius)
            {
                mHealth--;
                if (mLifes.Count != 0)
                {
                    mRoomActionRequest(ERoomAction.RemoveObject, mLifes.Pop());
                    if (mHealth == 0)
                    {
                        DestroyEffect();
                        GameOver();
                    }
                }
            }
        }
        private void GameOver()
        {
            mRoomActionRequest(ERoomAction.RemoveObject, this);
            mRoomActionRequest(ERoomAction.GameOver, null);
        }
        public override void DestroyEffect()
        {
            mRoomActionRequest(ERoomAction.AddObject, new Explosion(mExplosionFrame, 1.8 * Image.Width, 1.8 * Image.Width, Position));
        }
        public override double CollisionRadius
        {
            get
            {
                return (Image.Width / 2) * 0.8;
            }
        }

        public override void KeyDown(KeyEventArgs e)
        {
            if (e.Key == mKeys[0])
            {
                mAccelerationSign = 1;
            }
            if (e.Key == mKeys[1])
            {
                mAccelerationSign = -.6;
            }
            if (e.Key == mKeys[2])
            {
                mAngleChangeSign = -1;
            }
            if (e.Key == mKeys[3])
            {
                mAngleChangeSign = 1;
            }

            if (e.Key == mKeys[4])
            {
                mWantShoot = true;
            }
        }
        public override void KeyUp(KeyEventArgs e)
        {
            if (e.Key == mKeys[0] || e.Key == mKeys[1])
            {
                mAccelerationSign = 0;
            }
            if (e.Key == mKeys[2] || e.Key == mKeys[3])
            {
                mAngleChangeSign = 0;
            }
            if (e.Key == mKeys[4])
            {
                mWantShoot = false;
            }
        }
        public override void Initialize()
        {
            for(int i = 0; i < mHealth; i++)
            {
                AddHealth();
                mRoomActionRequest(ERoomAction.AddObject, mLifes.Peek());
            }
        }
        public void AddHealth()
        {
            mLifes.Push(new Life((BitmapFrame)Image.Source, 40, 30, new Point(Position.X * 2 - mLifes.Count * 50 - 20, Position.Y * 2 - 50), 270));
        }
        public override void ClockTick()
        {
            //Rotation
            mAngle += mAngleChangeSign * cAngleChangeSpeed;
            RotateImage(mAngle);

            //Translation
            mVerticalSpeed += mAcceleration * mAccelerationSign * Math.Sin(mAngle * Math.PI / 180);
            mHorizontalSpeed += mAcceleration * mAccelerationSign * Math.Cos(mAngle * Math.PI / 180);

            double lTotalSpeed = Math.Sqrt(mHorizontalSpeed * mHorizontalSpeed + mVerticalSpeed * mVerticalSpeed);
            if (lTotalSpeed > mMaxSpeed)
            {
                mVerticalSpeed *= mMaxSpeed / lTotalSpeed;
                mHorizontalSpeed *= mMaxSpeed / lTotalSpeed;
            }

            if (lTotalSpeed < mAcceleration / 2)
            {
                mVerticalSpeed = 0;
                mHorizontalSpeed = 0;
            }

            Position = new Point(Position.X + mHorizontalSpeed, Position.Y + mVerticalSpeed);

            //Shooting
            if (mShootTicksRemaining == 0)
            {
                mCanShoot = true;
            }
            if (mShootTicksRemaining > 0)
            {
                mShootTicksRemaining--;
            }
            if (mWantShoot && mCanShoot)
            {
                mRoomActionRequest(ERoomAction.AddObject, new BasicProjectile(16, 16, mProjectileBitmapFrame,
                    Position, mAngle, mProjectileSpeed));

                mCanShoot = false;
                mShootTicksRemaining = mShootTimeOutTicks;
            }

            
        }

        public override EOutsideRoomAction OutsideRoomAction
        {
            get
            {
                return EOutsideRoomAction.Return;
            }
        }
        public double mHealth { get; set; }

        private BitmapFrame mProjectileBitmapFrame;
        private BitmapFrame mExplosionFrame;

        private Stack<Life> mLifes = new Stack<Life>();

        private double mAngle = -90;
        private const double cAngleChangeSpeed = 4;
        private double mAngleChangeSign = 0;

        private double mHorizontalSpeed = 0;
        private double mVerticalSpeed = 0;
        private double mAcceleration = .2;
        private double mAccelerationSign = 0;
        private double mMaxSpeed = 8;

        private bool mWantShoot = false;
        private bool mCanShoot = true;
        private int mShootTimeOutTicks = 1;
        private int mShootTicksRemaining;
        private double mProjectileSpeed = 16;
    }
}

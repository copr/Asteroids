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
using System.ComponentModel;

using Engine;

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
        /// <param name="aKeys">Up, Down, Left, Right, Gun shoot, Missile shoot</param>
        public Rocket(
            BitmapFrame aBitmapFrame, BitmapFrame aProjectileBitmapFrame,
            BitmapFrame aMissileBitmapFrame,
            BitmapFrame aExplosionFrame, double aWidth, double aHeight,
            Point aPosition, List<Key> aKeys)
            : base(aBitmapFrame, aWidth, aHeight, aPosition, aKeys)
        {
            mExplosionFrame = aExplosionFrame;
            mCollisionBehavior.Add(typeof(Asteroid), CollisionWithAsteroid);

            Health = mMaxHealth;

            InvincibleSteps = 60;

            mPrimaryGun = new PrimaryGun(this.Position, aProjectileBitmapFrame);
            mMissileLauncher = new MissileLauncher(this.Position, aMissileBitmapFrame);

            mPrimaryGun.Owner = this;
            mMissileLauncher.Owner = this;
        }
        public override void Initialize()
        {
            RaiseRoomActionEvent(ERoomAction.AddObject, mPrimaryGun);
            RaiseRoomActionEvent(ERoomAction.AddObject, mMissileLauncher);
        }

        private void CollisionWithAsteroid(PhysicalObject o)
        {
            if (IsCollision(o))
            {
                if (o is Asteroid)
                {
                    Asteroid lAsteroid = o as Asteroid;
                    int lEnergyLoss = (int)Math.Floor((o.Image.Height > o.Image.Width) ? o.Image.Height : o.Image.Width / 10);

                    Health -= lEnergyLoss * lAsteroid.Strength;

                    if (Health <= 0.0)
                    {
                        Destroy();
                        DestroyEffect();
                        GameOver();
                    }
                }
            }
        }
        private void GameOver()
        {
            RaiseRoomActionEvent(ERoomAction.GameOver, null);
        }
        protected override void DestroyEffect()
        {
            RaiseRoomActionEvent(ERoomAction.AddObject, new Explosion(mExplosionFrame, 1.8 * Image.Width, 1.8 * Image.Width, Position));
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
                mWantShootGun = true;
            }
            if (e.Key == mKeys[5])
            {
                mWantShootMissile = true;
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
                mWantShootGun = false;
            }
            if (e.Key == mKeys[5])
            {
                mWantShootMissile = false;
            }
        }
        public override void ClockTick()
        {
            if (InvincibleSteps > 0)
                InvincibleSteps--;

            Score = Score + 1;
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
            mPrimaryGun.Position = this.Position;
            mPrimaryGun.AimDirection = this.mAngle;

            mMissileLauncher.Position = this.Position;
            mMissileLauncher.AimDirection = this.mAngle;
            
            if (mWantShootGun)
            {
                mPrimaryGun.ShootRequest();
            } 
            if (mWantShootMissile)
            {
                mMissileLauncher.ShootRequest();
            }
        }

        public override EOutsideRoomAction OutsideRoomAction
        {
            get
            {
                return EOutsideRoomAction.Return;
            }
        }

        private int mScore;

        public int Score
        {
            get
            {
                return mScore;
            }
            set
            {
                if (value != this.mScore)
                {
                    this.mScore = value;
                    OnPropertyChanged("Score");                    
                }
            }
        }

        private double mHealth;
        public double Health
        {
            get { return mHealth; }
            private set
            {
                if (value == mHealth) return;
                mHealth = value;
                OnPropertyChanged("Health");
                OnPropertyChanged("HealthPercentage");
            }
        }
        private double mMaxHealth = 100;
        public double MaxHealth
        {
            get { return mMaxHealth; }
            private set
            {
                if (value == mMaxHealth) return;
                mMaxHealth = value;
                OnPropertyChanged("MaxHealth");
                OnPropertyChanged("HealthPercentage");
            }
        }
        public double HealthPercentage
        {
            get
            {
                return mHealth / mMaxHealth * 100d;
            }
        }
            
        private BitmapFrame mExplosionFrame;

        private Stack<Life> mEnergies = new Stack<Life>();
        private Stack<Life> mLives = new Stack<Life>();

        private double mAngle = -90;
        private double cAngleChangeSpeed = 4;
        private double mAngleChangeSign = 0;

        private double mHorizontalSpeed = 0;
        private double mVerticalSpeed = 0;
        private double mAcceleration = .2;
        private double mAccelerationSign = 0;
        private double mMaxSpeed = 8;

        private bool mWantShootGun = false; 
        private bool mWantShootMissile = false;

        private PrimaryGun mPrimaryGun;
        private MissileLauncher mMissileLauncher;
    }
}

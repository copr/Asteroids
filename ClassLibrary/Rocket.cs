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
            Point aPosition, List<Key> aKeys,
            double aInitialAngle = -90,
            double aInitialSpeed = 0)
            : base(aBitmapFrame, aWidth, aHeight, aPosition, aKeys)
        {
            mExplosionFrame = aExplosionFrame;
            mCollisionBehavior.Add(typeof(Asteroid), CollisionWithAsteroid);
            mAngle = aInitialAngle;

            mVerticalSpeed = aInitialSpeed * Math.Sin(mAngle * Math.PI / 180);
            mHorizontalSpeed = aInitialSpeed * Math.Cos(mAngle * Math.PI / 180);

            Health = mMaxHealth;

            InvincibleSteps = 60;

            mPrimaryGun = new PrimaryGun(this.Position, aProjectileBitmapFrame) { OverheatCoefficient = 10 };
            mMissileLauncher = new MissileLauncher(this.Position, aMissileBitmapFrame);

            mPrimaryGun.Owner = this;
            mMissileLauncher.Owner = this;

            //Napasování kolizní masky na konkrétní raketu!!
            //Při změně obrázku nebo velikosti je třeba změnit
            
            CollisionMask.Add(new Circle(24, Position, 8, 0));
            CollisionMask.Add(new Circle(12, Position, 33, 0));
            CollisionMask.Add(new Circle(20, Position, 16, 180));

            CollisionMask.Add(new Circle(10, Position, 28, 135));
            CollisionMask.Add(new Circle(10, Position, 28, -135));

            CollisionMask.Add(new Circle(6, Position, 44, 145));
            CollisionMask.Add(new Circle(6, Position, 44, -145));
            
            //CollisionMask.Add(new Ray(Position, 0, 0, 256, 0));
            
            Depth = 0;
        }
        public override void Initialize()
        {
            RaiseRoomActionEvent(ERoomAction.AddObject, mPrimaryGun);
            RaiseRoomActionEvent(ERoomAction.AddObject, mMissileLauncher);
        }
        private void CollisionWithAsteroid(PhysicalObject o)
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
        private void GameOver()
        {
            RaiseRoomActionEvent(ERoomAction.GameOver, null);
        }
        protected override void DestroyEffect()
        {
            RaiseRoomActionEvent(ERoomAction.AddObject, new Explosion(mExplosionFrame, 1.8 * Image.Width, 1.8 * Image.Width, Position));
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
            Score++;
            //Rotation
            mAngle += mAngleChangeSign * cAngleChangeSpeed;

            //Translation
            mVerticalSpeed += mAcceleration * mAccelerationSign * Math.Sin(mAngle * Math.PI / 180);
            mHorizontalSpeed += mAcceleration * mAccelerationSign * Math.Cos(mAngle * Math.PI / 180);

            //property speed calculates current speed from hor and ver speeds
            double lTotalSpeed = Speed;
            if (lTotalSpeed > mMaxSpeed)
            {
                mVerticalSpeed *= mMaxSpeed / lTotalSpeed;
                mHorizontalSpeed *= mMaxSpeed / lTotalSpeed;
            }

            if (lTotalSpeed < 2 * mAcceleration / 3.0)
            {
                mVerticalSpeed = 0;
                mHorizontalSpeed = 0;
            }

            //Position = new Point(Position.X + mHorizontalSpeed, Position.Y + mVerticalSpeed);

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

            base.ClockTick();
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
                if (mScore == value) return;
                mScore = value;
                OnPropertyChanged("Score");
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

        private double cAngleChangeSpeed = 5;
        private double mAngleChangeSign = 0;

        //private double mHorizontalSpeed = 0;
        //private double mVerticalSpeed = 0;
        private double mAcceleration = .2;
        private double mAccelerationSign = 0;
        private double mMaxSpeed = 8;

        private bool mWantShootGun = false; 
        private bool mWantShootMissile = false;

        private PrimaryGun mPrimaryGun;
        private MissileLauncher mMissileLauncher;
    }
}

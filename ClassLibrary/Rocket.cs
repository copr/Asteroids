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
            mCollisionBehavior.Add(typeof(Asteroid), CollisionSolve);
            NumOfLives = 3;
            Energy = 20;

            mPrimaryGun = new PrimaryGun(this.Position, aProjectileBitmapFrame);
            mMissileLauncher = new MissileLauncher(this.Position, aMissileBitmapFrame);
        }
        public override void Initialize()
        {
            InitializeHealth();
            InitializeEnergy();

            RaiseRoomActionEvent(ERoomAction.AddObject, mPrimaryGun);
            RaiseRoomActionEvent(ERoomAction.AddObject, mMissileLauncher);
        }

        private void CollisionSolve(PhysicalObject o)
        {
            if (Distance(o) < CollisionRadius + o.CollisionRadius)
            {
                int lEnergyLoss = (int)Math.Floor((o.Image.Height > o.Image.Width) ? o.Image.Height : o.Image.Width / 10);
                
                Energy -= lEnergyLoss;
                DropEnergy((int)lEnergyLoss);

                if (Energy <= 0)
                {
                    Energy = 20;
                    NumOfLives--;
                    if (NumOfLives < 0)
                    {
                        Destroy();
                        DestroyEffect();
                        GameOver();
                    }
                    else
                    {
                        RaiseRoomActionEvent(ERoomAction.RemoveObject, mLives.Pop());
                        mAngle = -90;
                        InitializeEnergy();
                        DestroyEffect();
                    }
                    Position = new Point(GameRoom.RoomWidth / 2, GameRoom.RoomHeight / 2);
                    mVerticalSpeed = 0;
                    mHorizontalSpeed = 0;
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
        private void InitializeHealth()
        {
            for (int i = mLives.Count; i < NumOfLives; i++)
            {
                mLives.Push(new Life((BitmapFrame)Image.Source, 40, 30,
                    new Point(GameRoom.RoomWidth - mLives.Count * 50 - 20, GameRoom.RoomHeight - 50), 270));
                RaiseRoomActionEvent(ERoomAction.AddObject, mLives.Peek());
            }
        }

        private void InitializeEnergy()
        {
            for (int i = mEnergies.Count; i < Energy; i++)
            {
                mEnergies.Push(new Life((BitmapFrame)Image.Source, 20, 15,
                    new Point(GameRoom.RoomWidth - mEnergies.Count * 15 - 15, GameRoom.RoomHeight - 20), 270));
                RaiseRoomActionEvent(ERoomAction.AddObject, mEnergies.Peek());
            }
        }

        private void DropEnergy(int aDropped)
        {
            for (int i = 0; i < aDropped; i++)
            {
                if (mEnergies.Count != 0)
                {
                    RaiseRoomActionEvent(ERoomAction.RemoveObject, mEnergies.Pop());
                }
            }
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
        public int NumOfLives { get; private set; }
        public int Energy { get; private set; }
    
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

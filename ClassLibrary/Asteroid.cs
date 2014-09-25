using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using System.IO;

using Engine;

namespace GameTest2
{
    public class Asteroid : SimpleMovingObject
    {
        public Asteroid(BitmapFrame aBitmapFrame, BitmapFrame aExplosionFrame,
            Point aPosition,
            double aDirection, double aSpeed, 
            AsteroidSettings aSettings)
            : base(aSettings.Size, aSettings.Size, aBitmapFrame, aPosition, aDirection, aSpeed)
        {
            SetImageAngle(Direction);
            mHealth = 1;
            //Random lRandom = new Random();
            mAngle = Direction;
            mExplosionFrame = aExplosionFrame;

            mCollisionBehavior.Add(typeof(Asteroid), CollisionWithAsteroid);
            mCollisionBehavior.Add(typeof(BasicProjectile), CollisionWithProjectile);
            mCollisionBehavior.Add(typeof(GuidedMissile), CollisionWithProjectile);
            mCollisionBehavior.Add(typeof(Rocket), DefaultCollisionSolve);

            InvincibleSteps = 20;

            mSettings = aSettings;

            CollisionMask.Add(new Circle(mSettings.Size / 2 * 4.5 / 5, this.Position, 0, 0)); 
            Depth = -1;
        }

        private void CreateChildren()
        {
            double lDirection1 = Direction + 10 + 30 * mRandom.NextDouble();
            double lDirection2 = Direction - 10 - 30 * mRandom.NextDouble();

            List<Asteroid> lChildren = new List<Asteroid>();

            AsteroidSettings aChild1Settings = mSettings.Clone();
            aChild1Settings.Size = this.mSettings.Size / 2;
            AsteroidSettings aChild2Settings = mSettings.Clone();
            aChild2Settings.Size = this.mSettings.Size / 2;

            lChildren.Add(new Asteroid((BitmapFrame)Image.Source, mExplosionFrame,
                Position,
                lDirection1,
                1.1 * Speed,
                aChild1Settings));
            lChildren.Add(new Asteroid((BitmapFrame)Image.Source, mExplosionFrame,
                Position,
                lDirection2,
                1.1 * Speed,
                aChild2Settings));

            foreach (Asteroid a in lChildren)
            {
                RaiseRoomActionEvent(ERoomAction.AddObject, a);
            }
        }

        public override void ClockTick()
        {
            mAngle += mSettings.RotationSpeed;
            SetImageAngle(mAngle);

            base.ClockTick();
        }
        private void CollisionWithAsteroid(PhysicalObject o)
        {
            if (o is Asteroid)
            {
                if (Strength > (o as Asteroid).Strength)
                {
                    mHealth -= (o as Asteroid).Size / ((o as Asteroid).Size + Size);
                }
                else
                {
                    mHealth = 0;
                }

                if (mHealth <= 0)
                {
                    if (mSettings.Size > mSettings.MinSizeForChildren)
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
        }
        private void CollisionWithProjectile(PhysicalObject o)
        {
            if (o is BasicProjectile)
            {
                (o as BasicProjectile).Owner.Score.Points += (int)(Image.Width * 10 * mSettings.PointsMultiplier);
                mHealth -= mSettings.ProjectileDamage;
            }
            if (o is GuidedMissile)
            {
                (o as GuidedMissile).Owner.Score.Points += (int)(Image.Width * 10 * mSettings.PointsMultiplier);
                mHealth -= mSettings.MissileDamage;
            }

            if (mHealth <= 0)
            {
                if (mSettings.Size > mSettings.MinSizeForChildren)
                {
                    Destroy();
                    DestroyEffect();
                    //CreateChildren();
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

        public int Strength
        {
            get
            {
                return mSettings.Strength;
            }
        }
        public double Size
        {
            get { return mSettings.Size; }
        }

        private BitmapFrame mExplosionFrame;

        private AsteroidSettings mSettings;
        private double mHealth;
    }
}

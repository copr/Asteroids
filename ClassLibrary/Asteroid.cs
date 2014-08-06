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
            mHealth = 1;
            Random lRandom = new Random();
            mAngle = lRandom.NextDouble() * 360;
            mRotationSpeed = lRandom.NextDouble() * 4 - 2;
            mExplosionFrame = aExplosionFrame;

            mCollisionBehavior.Add(typeof(Asteroid), CollisionWithAsteroid);
            mCollisionBehavior.Add(typeof(BasicProjectile), CollisionWithProjectile);
            mCollisionBehavior.Add(typeof(GuidedMissile), CollisionWithProjectile);
            mCollisionBehavior.Add(typeof(Rocket), DefaultCollisionSolve);

            InvincibleSteps = 20;

            mSettings = aSettings;
        }

        private void CreateChildren()
        {
            double lAngle = 360 * mRandom.NextDouble();

            List<Asteroid> lChildren = new List<Asteroid>();

            AsteroidSettings aChild1Settings = mSettings.Clone();
            aChild1Settings.Size = this.mSettings.Size / 2;
            AsteroidSettings aChild2Settings = mSettings.Clone();
            aChild2Settings.Size = this.mSettings.Size / 2;

            lChildren.Add(new Asteroid((BitmapFrame)Image.Source, mExplosionFrame,
                Position,
                lAngle - 90 + mRandom.NextDouble() * 180,
                1.4 * Speed,
                aChild1Settings));
            lChildren.Add(new Asteroid((BitmapFrame)Image.Source, mExplosionFrame,
                Position,
                lAngle - 90 + mRandom.NextDouble() * 180,
                1.4 * Speed,
                aChild2Settings));

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
            if (o is Asteroid)
            {
                if (Strength > (o as Asteroid).Strength)
                {
                    mHealth -= (o as Asteroid).mSettings.Size / ((o as Asteroid).mSettings.Size + mSettings.Size);
                }
                else
                {
                    if (IsCollision(o))
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
        }
        private void CollisionWithProjectile(PhysicalObject o)
        {
            if (IsCollision(o))
            {
                if (o is BasicProjectile)
                {
                    (o as BasicProjectile).Owner.Score += (int)(Image.Width * 10 * mSettings.PointsMultiplier);
                    mHealth -= mSettings.ProjectileDamage;
                }
                if (o is GuidedMissile)
                {
                    (o as GuidedMissile).Owner.Score += (int)(Image.Width * 10 * mSettings.PointsMultiplier);
                    mHealth -= mSettings.MissileDamage;
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

        private double mAngle;
        private double mRotationSpeed;
        private BitmapFrame mExplosionFrame;

        private AsteroidSettings mSettings;
        private double mHealth;
    }
}

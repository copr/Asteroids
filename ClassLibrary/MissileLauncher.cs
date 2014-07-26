using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Engine;
using System.Windows;
using System.Windows.Media.Imaging;

namespace GameTest2
{
    public class MissileLauncher : PhysicalObject
    {
        public MissileLauncher(Point aPosition, BitmapFrame aProjectileBitmapFrame)
            : base(null, 0, 0, aPosition)
        {
            mProjectileImage = aProjectileBitmapFrame;
        }
        public override void ClockTick()
        {

        }

        public override EOutsideRoomAction OutsideRoomAction
        {
            get
            {
                return EOutsideRoomAction.Nothing;
            }
        }

        public void ShootRequest()
        {
            Shoot();
        }

        private void OnTargetDestroyed(BaseObject aSender)
        {
            mTargetedObjects.Remove((PhysicalObject)aSender);
        }

        private void OnMisileDestroyed(BaseObject aSender)
        {
            if (aSender is GuidedMissile)
            {
                mTargetedObjects.Remove(((GuidedMissile)aSender).Target);
            }
        }
        public double AimDirection
        {
            get
            {
                return mAimDirection;
            }
            set
            {
                mAimDirection = value;
            }
        }

        private void Shoot()
        {
            List<Asteroid> lPossibleTargets = GameRoom.GetObjectsOfType<Asteroid>().ToList();
            lPossibleTargets.RemoveAll(x => mTargetedObjects.Contains(x));

            if (lPossibleTargets != null)
            {
                if (lPossibleTargets.Count > 0)
                {
                    double lMinSquareDistance = double.MaxValue;
                    PhysicalObject lTarget = null;
                    foreach (PhysicalObject t in lPossibleTargets)
                    {
                        double lDistance = SquaredDistance(t);

                        if (lDistance < lMinSquareDistance
                            )
                        {
                            lMinSquareDistance = lDistance;
                            lTarget = t;
                        }
                    }

                    double lDirectionToTarget = 180 / Math.PI * Math.Atan2(lTarget.Position.Y - Position.Y, lTarget.Position.X - Position.X);

                    GuidedMissile lNewMissile = new GuidedMissile(32, 12, mProjectileImage, this.Position, mAimDirection, 12, 8);

                    lNewMissile.Target = lTarget;
                    lNewMissile.Owner = this.Owner;
                    lNewMissile.DestroyedEvent += OnMisileDestroyed;
                    lTarget.DestroyedEvent += OnTargetDestroyed;
                    mTargetedObjects.Add(lTarget);

                    RaiseRoomActionEvent(ERoomAction.AddObject, lNewMissile);
                }
            }
        }
        public Rocket Owner
        {
            get;
            set;
        }

        private BitmapFrame mProjectileImage;
        private List<PhysicalObject> mTargetedObjects = new List<PhysicalObject>();
        private double mAimDirection;

    }
}

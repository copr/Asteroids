using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media.Imaging;

using Engine;

namespace GameTest2
{
    public class PrimaryGun : PhysicalObject
    {
        public PrimaryGun(Point aPosition, BitmapFrame aProjectileBitmapFrame)
            : base(null, 0, 0, aPosition)
        {
            mProjectileImage = aProjectileBitmapFrame;
        }
        public override void ClockTick()
        {
            if (mReadyTimeout == 0)
            {
                mCanShoot = true;
            }
            if (mReadyTimeout > 0)
            {
                mReadyTimeout--;
            }
            mTemperature -= 1;
            if (mTemperature < 0)
            {
                mTemperature = 0;
            }
        }
        public void ShootRequest()
        {
            if (mCanShoot)
            {
                Shoot();
            }
        }
        private void Shoot()
        {
            RaiseRoomActionEvent(ERoomAction.AddObject, new BasicProjectile(16, 16, mProjectileImage, this.Position, mAimDirection, 12));
            mCanShoot = false;
            mTemperature += 10;
            mReadyTimeout = (int)(mTemperature / 4);
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

        public override EOutsideRoomAction OutsideRoomAction
        {
            get
            {
                return EOutsideRoomAction.Nothing;
            }
        }

        private bool mCanShoot = true;
        private int mReadyTimeout = 0;
        private double mTemperature = 0;

        private BitmapFrame mProjectileImage;

        private double mAimDirection = 0;
    }
}

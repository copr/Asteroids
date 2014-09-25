using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media.Imaging;

using Engine;

namespace GameTest2
{
    public class LaserGun : PhysicalObject
    {
        public LaserGun(Point aPosition, BitmapFrame aLaserBitmapFrame)
            : base(null, 0, 0, aPosition)
        {
            mLaserBitmapFrame = aLaserBitmapFrame;
        }

        public void ShootRequest()
        {
            if (!mIsActive)
            {
                Activate();
            }
        }
        private void Activate()
        {
            mActiveRemainingTime = 2;
            mActualLaser = new Laser(mLaserBitmapFrame, this.Position);
            RaiseRoomActionEvent(ERoomAction.AddObject, mActualLaser);
        }
        private void Deactivate()
        {
            RaiseRoomActionEvent(ERoomAction.RemoveObject, mActualLaser);
            mActualLaser = null;
            mIsActive = false;
        }

        public override void ClockTick()
        {
            if (mActiveRemainingTime > 0)
            {
                mActiveRemainingTime--;
            }
            if (mActiveRemainingTime == 0 && mIsActive)
            {
                Deactivate();
            }

            if (mActualLaser != null)
            {
                mActualLaser.Position = this.Position;
            }

            base.ClockTick();
        }

        public bool IsActive
        {
            get
            {
                return mIsActive;
            }
        }
        private bool mIsActive = false;
        private int mActiveRemainingTime = 0;
        private Laser mActualLaser;
        private BitmapFrame mLaserBitmapFrame;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media.Imaging;

using Engine;

namespace GameTest2
{
    public class Explosion : PhysicalObject
    {
        public Explosion(BitmapFrame aBitmapFrame, double aWidth, double aHeight, Point aPosition)
            : base(aBitmapFrame, aWidth, aHeight, aPosition)
        {
            mInitialWidth = aWidth;
            mInitialHeight = aHeight;
        }

        public override void ClockTick()
        {
            Image.Width *= mShrinkCoefficient;
            Image.Height *= mShrinkCoefficient;

            if (Image.Width < 0.2 * mInitialWidth && Image.Height < 0.2 * mInitialHeight)
            {
                Position = new Point(-100, -100);
            }
        }

        private double mShrinkCoefficient = .98;
        private double mInitialWidth;
        private double mInitialHeight;
    }
}

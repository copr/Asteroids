using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace GameTest2
{
    public class Asteroid : SimpleMovingObject
    {
        public Asteroid(BitmapFrame aBitmapFrame, int aWidth, int aHeight, Point aPosition,
            double aDirection, double aSpeed)
            : base(aBitmapFrame, aWidth, aHeight, aPosition, aDirection, aSpeed)
        {
            Random lRandom = new Random();
            mAngle = lRandom.NextDouble() * 360;
            mRotationSpeed = lRandom.NextDouble() * 4 - 2;
        }

        public override void ClockTick()
        {
            base.ClockTick();
            
            mAngle += mRotationSpeed;
            RotateImage(mAngle);
        }

        private double mAngle;
        private double mRotationSpeed;
    }
}

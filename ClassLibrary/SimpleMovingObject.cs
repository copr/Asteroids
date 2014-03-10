using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace GameTest2
{
    public abstract class SimpleMovingObject : BasicObject
    {
        public SimpleMovingObject(BitmapFrame aBitmapFrame, double aWidth, double aHeight, Point aPosition,
            double aDirection, double aSpeed)
            : base(aBitmapFrame, aWidth, aHeight, aPosition)
        {
            mVerticalSpeed = aSpeed * Math.Sin(aDirection * Math.PI / 180);
            mHorizontalSpeed = aSpeed * Math.Cos(aDirection * Math.PI / 180);
        }
        public SimpleMovingObject(BitmapFrame aBitmapFrame, int aWidth, int aHeight, Point aPosition,
            double aDirection, double aVerticalSpeed, double aHorizontalSpeed)
            : base(aBitmapFrame, aWidth, aHeight, aPosition)
        {
            mHorizontalSpeed = aHorizontalSpeed;
            mVerticalSpeed = aVerticalSpeed;
        }

        public override void ClockTick()
        {
            Position = new Point(Position.X + mHorizontalSpeed, Position.Y + mVerticalSpeed);
        }

        private double mHorizontalSpeed;
        private double mVerticalSpeed;
    }
}

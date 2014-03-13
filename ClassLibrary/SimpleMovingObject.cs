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
        public SimpleMovingObject(double aWidth, double aHeight, BitmapFrame aBitmapFrame, Point aPosition,
            double aDirection, double aSpeed)
            : this(aBitmapFrame, aWidth, aHeight, aPosition, 
            aSpeed * Math.Sin(aDirection * Math.PI / 180),
            aSpeed * Math.Cos(aDirection * Math.PI / 180)
            )
        {

        }
        public SimpleMovingObject(BitmapFrame aBitmapFrame, double aWidth, double aHeight, Point aPosition,
            double aVerticalSpeed, double aHorizontalSpeed)
            : base(aBitmapFrame, aWidth, aHeight, aPosition)
        {
            mHorizontalSpeed = aHorizontalSpeed;
            mVerticalSpeed = aVerticalSpeed;
        }

        public override void ClockTick()
        {
            Position = new Point(Position.X + mHorizontalSpeed, Position.Y + mVerticalSpeed);
        }
        public double Speed
        {
            get
            {
                return Math.Sqrt(mVerticalSpeed * mVerticalSpeed + mHorizontalSpeed * mHorizontalSpeed);
            }
        }

        private double mHorizontalSpeed;
        private double mVerticalSpeed;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Engine
{
    public abstract class SimpleMovingObject : PhysicalObject
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

        /// <summary>
        /// Moves the object according to its horizontal and vertical speed
        /// </summary>
        public override void ClockTick()
        {
            Position = new Point(Position.X + mHorizontalSpeed, Position.Y + mVerticalSpeed);
            
            base.ClockTick();
        }
        public double Speed
        {
            get
            {
                return Math.Sqrt(mVerticalSpeed * mVerticalSpeed + mHorizontalSpeed * mHorizontalSpeed);
            }
        }
        public double Direction
        {
            get
            {
                return 180 / Math.PI * Math.Atan2(mVerticalSpeed, mHorizontalSpeed);
            }
        }
        public double HorizontalSpeed
        {
            get
            {
                return mHorizontalSpeed;
            }
        }
        public double VerticalSpeed
        {
            get
            {
                return mVerticalSpeed;
            }
        }

        protected double mHorizontalSpeed;
        protected double mVerticalSpeed;
    }
}

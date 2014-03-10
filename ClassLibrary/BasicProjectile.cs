using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace GameTest2
{
    public class BasicProjectile : SimpleMovingObject
    {
        public BasicProjectile(double aWidth, double aHeight, BitmapFrame aBitmapFrame, Point aPosition,
            double aDirection, double aSpeed)
            : this(aBitmapFrame, aWidth, aHeight, aPosition, 
            aSpeed * Math.Sin(aDirection * Math.PI / 180),
            aSpeed * Math.Cos(aDirection * Math.PI / 180))
        {

        }
        public BasicProjectile(BitmapFrame aBitmapFrame, double aWidth, double aHeight, Point aPosition, 
            double aVerticalSpeed, double aHorizontalSpeed)
            : base(aBitmapFrame, aWidth, aHeight, aPosition, aVerticalSpeed, aHorizontalSpeed)
        {
            mCollisionBehavior.Add(typeof(Asteroid), DefaultCollisionSolve);
        }
        public override void ClockTick()
        {
            base.ClockTick();
        }
        public override double CollisionRadius
        {
            get
            {
                return (Image.Width / 2) * 0.8;
            }
        }
    }
}

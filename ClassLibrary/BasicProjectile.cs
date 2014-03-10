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
        public BasicProjectile(BitmapFrame aBitmapFrame, double aWidth, double aHeight, Point aPosition,
            double aDirection, double aSpeed)
            : base(aBitmapFrame, aWidth, aHeight, aPosition, aDirection, aSpeed)
        {
            mCollisionBehavior.Add(typeof(Asteroid), DefaultCollisionSolve);
        }
        public BasicProjectile(BitmapFrame aBitmapFrame, int aWidth, int aHeight, Point aPosition,
            double aDirection, double aVerticalSpeed, double aHorizontalSpeed)
            : base(aBitmapFrame, aWidth, aHeight, aPosition, aDirection, aVerticalSpeed, aHorizontalSpeed)
        {
            mCollisionBehavior.Add(typeof(Asteroid), (BasicObject o2) =>
            {
                if (Distance(o2) < CollisionRadius + o2.CollisionRadius)
                {
                    mRemoveObject(this);
                }
            });
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

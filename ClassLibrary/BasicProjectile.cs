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
        public BasicProjectile(BitmapFrame aBitmapFrame, int aWidth, int aHeight, Point aPosition,
            double aDirection, double aSpeed)
            : base(aBitmapFrame, aWidth, aHeight, aPosition, aDirection, aSpeed)
        {

        }
        public BasicProjectile(BitmapFrame aBitmapFrame, int aWidth, int aHeight, Point aPosition,
            double aDirection, double aVerticalSpeed, double aHorizontalSpeed)
            : base(aBitmapFrame, aWidth, aHeight, aPosition, aDirection, aVerticalSpeed, aHorizontalSpeed)
        {

        }
        public override void ClockTick()
        {
            base.ClockTick();
        }
    }
}

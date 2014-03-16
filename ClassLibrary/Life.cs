using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Input;
using System.Threading;

namespace GameTest2
{
    public class Life : BasicObject
    {
        public Life(BitmapFrame aBitmapFrame, double aWidth, double aHeight, Point aPosition, double aAngle) :
            base(aBitmapFrame, aWidth, aHeight, aPosition)
        {
            RotateImage(aAngle);
        }

        public override void ClockTick()
        {
        }
    }
}

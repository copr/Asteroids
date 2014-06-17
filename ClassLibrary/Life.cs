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

using Engine;

namespace GameTest2
{
    public class Life : PhysicalObject
    {
        public Life(BitmapFrame aBitmapFrame, double aWidth, double aHeight, Point aPosition, double aAngle) :
            base(aBitmapFrame, aWidth, aHeight, aPosition)
        {
            RotateImage(aAngle);
            Depth = -50;
        }

        public override void ClockTick()
        {
        }
    }
}

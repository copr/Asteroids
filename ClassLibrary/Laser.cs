using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Engine;
using System.Windows.Media.Imaging;
using System.Windows;

namespace GameTest2
{
    public class Laser : PhysicalObject
    {
        public Laser(BitmapFrame aBitmapFrame,
            Point aPosition)
            : base(aBitmapFrame, 10000, 4, aPosition)
        {

        }

        public override void ClockTick()
        {


            base.ClockTick();
        }
    }
}

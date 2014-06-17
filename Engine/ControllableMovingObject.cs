using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace Engine
{
    public abstract class ControllableMovingObject : PhysicalObject
    {
        public ControllableMovingObject(BitmapFrame aBitmapFrame, double aWidth, double aHeight, Point aPosition, List<Key> aKeys)
            : base(aBitmapFrame, aWidth, aHeight, aPosition)
        {
            mKeys = aKeys;
        }
        public abstract void KeyDown(KeyEventArgs e);
        public abstract void KeyUp(KeyEventArgs e);

        protected List<Key> mKeys;
    }
}

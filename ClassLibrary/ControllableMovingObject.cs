using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace GameTest2
{
    public abstract class ControllableMovingObject : MovingObject
    {
        public ControllableMovingObject(BitmapFrame aBitmapFrame, int aWidth, int aHeight, Point aPosition, List<Key> aKeys)
            : base(aBitmapFrame, aWidth, aHeight, aPosition)
        {
            mKeys = aKeys;
        }
        public abstract void KeyDown(KeyEventArgs e);
        public abstract void KeyUp(KeyEventArgs e);

        protected List<Key> mKeys;
    }
}

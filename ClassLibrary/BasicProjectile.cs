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
            double aDirection, double aSpeed, Rocket aCreator)
            : this(aBitmapFrame, aWidth, aHeight, aPosition, 
            aSpeed * Math.Sin(aDirection * Math.PI / 180),
            aSpeed * Math.Cos(aDirection * Math.PI / 180), aCreator)
        {
          
        }
        public BasicProjectile(BitmapFrame aBitmapFrame, double aWidth, double aHeight, Point aPosition, 
            double aVerticalSpeed, double aHorizontalSpeed, Rocket aCreator)
            : base(aBitmapFrame, aWidth, aHeight, aPosition, aVerticalSpeed, aHorizontalSpeed)
        {
            mCollisionBehavior.Add(typeof(BasicObject), DefaultCollisionSolve);
            mCollisionBehavior.Add(typeof(Asteroid), ProjectileCollisionSolve);
            mCreator = aCreator;
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

        public void ProjectileCollisionSolve(BasicObject o)
        {
            if (Distance(o) < CollisionRadius + o.CollisionRadius)
            {
                mCreator.mScore += o.Image.ActualWidth;
                DestroyEffect();
                mRoomActionRequest(ERoomAction.RemoveObject, this);
            }
        }

        public Rocket mCreator { get; set; }
    }
}

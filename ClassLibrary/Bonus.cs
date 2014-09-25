using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using System.IO;

using Engine;

namespace GameTest2
{
    public class Bonus : SimpleMovingObject
    {
        public Bonus(BitmapFrame aBitmapFrame,
            Point aPosition,
            double aDirection, double aSpeed, 
            double aSize, BonusType aType)
            : base(aSize, aSize, aBitmapFrame, aPosition, aDirection, aSpeed)
        {
            SetImageAngle(Direction);

            mAngle = Direction;
            mCollisionBehavior.Add(typeof(Rocket), DefaultCollisionSolve);

            CollisionMask.Add(new Circle(aSize / 2 * 4.5 / 5, this.Position, 0, 0));
            Depth = -1;
            Health = aType.PlusHealth;
            MissileTime = aType.PlusMissiles;
            Invulnerability = aType.Invulnerability;
        }

        public double Health { get; set; }
        public double MissileTime { get; set; }
        public double Invulnerability { get; set; }
    }
}

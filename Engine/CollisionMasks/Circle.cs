using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Engine
{
    public class Circle : CollisionMask
    {
        public Circle(double aRadius, Point aPosition,
            double aPositionOffsetDistance,
            double aPositionOffsetAngle,
            Color aColor)
            : base(aPosition, aPositionOffsetDistance, aPositionOffsetAngle,
            0)
        {
            Ellipse lImage = new Ellipse();
            lImage.Width = lImage.Height = aRadius * 2;
            lImage.Fill = new SolidColorBrush(aColor);
            Image = lImage;

            mRadius = aRadius;

            mRotateBehavior = ERotateBehavior.RotateAroundOwner;

            SetImageAngle(0);
        }
        public Circle(double aRadius, Point aPosition, 
            double aPositionOffsetDistance,
            double aPositionOffsetAngle)
            : this(aRadius, aPosition, 
            aPositionOffsetDistance, 
            aPositionOffsetAngle, 
            Colors.Black)
        {

        }
        public override bool IsCollision(CollisionMask aOther)
        {
            if (aOther is Circle)
            {
                Circle lCircle = aOther as Circle;

                double lSquaredDistance = (this.Position.X - lCircle.Position.X) * (this.Position.X - lCircle.Position.X) + (this.Position.Y - lCircle.Position.Y) * (this.Position.Y - lCircle.Position.Y);

                return lSquaredDistance < (this.mRadius + lCircle.mRadius) * (this.mRadius + lCircle.mRadius);
            }

            return false;
        }

        private double mRadius;
    }
}

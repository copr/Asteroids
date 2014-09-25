using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Engine
{
    public class Ray : CollisionMask
    {
        public Ray(Point aPosition,
            double aPositionOffsetDistance,
            double aPositionOffsetAngle,
            double aLength,
            double aInitialAngle,
            Color aColor)
            : base(aPosition, aPositionOffsetDistance, aPositionOffsetAngle,
            aInitialAngle)
        {
            mLength = aLength;

            Line lImage = new Line();
            lImage.Stroke = new SolidColorBrush(aColor);
            Image = lImage;
            lImage.X1 = 0;
            lImage.Y1 = 0;
            lImage.X2 = mLength;
            lImage.Y2 = 0;
            
            mRotateBehavior = ERotateBehavior.RotateCompletely;

            SetImageAngle(aInitialAngle);
        }
        public Ray(Point aPosition,
            double aPositionOffsetDistance,
            double aPositionOffsetAngle,
            double aLength,
            double aInitialAngle)
            : this(aPosition, aPositionOffsetDistance, aPositionOffsetAngle,
            aLength, aInitialAngle, Colors.Black)
        {

        }
        public override bool IsCollision(CollisionMask aOther)
        {
            if (aOther is Circle)
            {
                var lCircle = (Circle)aOther;
            }
            if (aOther is Ray)
            {
                var lRay = aOther as Ray;

                //positions
                var x1 = this.Position.X;
                var y1 = this.Position.Y;

                var x2 = lRay.Position.X;
                var y2 = lRay.Position.Y;

                //direction vectors (unit length)
                var u1x = Math.Cos(mActualAngle * Math.PI / 180);
                var u1y = Math.Sin(mActualAngle * Math.PI / 180);

                var u2x = Math.Cos(lRay.mActualAngle * Math.PI / 180);
                var u2y = Math.Sin(lRay.mActualAngle * Math.PI / 180);

                //parallel lines
                if (u1x * u2y - u1y * u2x == 0.0)
                {
                    //false for simplicity, not considering one ray covering the other one
                    return false;
                }

                //parameter of the crossing point for ray 1 ("this" ray)
                var t1 = ((y1 - y2) * u2x - (x1 - x2) * u2y) / (u1x * u2y - u1y * u2x);

                if (t1 <= 0)
                {
                    //crossing point is behind the origin of ray 1
                    return false;
                }

                var t2 = 0.0;

                //there are two ways to calculate the second parameter,
                //at least one should be always possible
                if (u2x != 0)
                {
                    t2 = (x1 - x2 + t1 * u1x) / u2x;
                }
                else if (u2y != 0)
                {
                    t2 = (y1 - y2 + t1 * u1y) / u2y;
                }
                else
                {
                    return false;
                }

                if (t1 > 0 && t2 > 0 && t1 < this.mLength && t2 < lRay.mLength)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            if (aOther is Circle)
            {
                var lCircle = aOther as Circle;

                //base point of the ray
                var x0 = this.Position.X;
                var y0 = this.Position.Y;

                //direction vector of the ray, unit length
                var ux = Math.Cos(mActualAngle * Math.PI / 180);
                var uy = Math.Sin(mActualAngle * Math.PI / 180);

                //circle position
                var m = lCircle.Position.X;
                var n = lCircle.Position.Y;

                //circle radius
                var r = lCircle.Radius;

                var dx = x0 - m;
                var dy = y0 - n;

                //solving for t: (ux^2 + uy^2) * t^2 + 2 * (dx*ux + dy*uy) * t + dx^2 + dy^2 - r^2 = 0
                var a = 1; //ux^2 + uy^2 = 1, direction vector has unit length
                var b = 2 * (dx * ux + dy * uy);
                var c = dx * dx + dy * dy - r * r;

                var D = b * b - 4 * a * c;

                var t1 = 0.0;
                var t2 = 0.0;

                if (D <= 0)
                {
                    return false;
                }
                else
                {
                    t1 = (-b - Math.Sqrt(D)) / (2 * a);
                    t2 = (-b + Math.Sqrt(D)) / (2 * a);

                    if (t1 > 0 && t1 < mLength)
                    {
                        return true;
                    }
                    if (t2 > 0 && t2 < mLength)
                    {
                        return true;
                    }
                    return false;
                }
            }
            return false;
        }

        private double mLength;

        public override double HorizontalShift
        {
            get { return 0; }
        }

        public override double VerticalShift
        {
            get { return 0; }
        }
    }
}

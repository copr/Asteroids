using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Engine
{
    public abstract class CollisionMask
    {
        public CollisionMask(Point aPosition,
            double aPostionOffsetDistance, 
            double aPositionOffsetAngle,
            double aInitialAngle)
        {
            PositionOffsetDistance = aPostionOffsetDistance;
            PositionOffsetAngle = aPositionOffsetAngle;
            mInitialAngle = mActualAngle = aInitialAngle;

            IsVisible = false;

            SetPosition(aPosition, 0);
        }
        public abstract bool IsCollision(CollisionMask aOther);
        public void SetPosition(Point aOwnerPosition, double aOwnerAngle)
        {
            switch (mRotateBehavior)
            {
                case ERotateBehavior.Static:

                    this.mPosition = new Point(
                        aOwnerPosition.X + PositionOffsetDistance * Math.Cos((PositionOffsetAngle) * Math.PI / 180),
                        aOwnerPosition.Y + PositionOffsetDistance * Math.Sin((PositionOffsetAngle) * Math.PI / 180)
                        );

                    break;

                case ERotateBehavior.RotateAroundOwner:

                    this.mPosition = new Point(
                        aOwnerPosition.X + PositionOffsetDistance * Math.Cos((PositionOffsetAngle + aOwnerAngle) * Math.PI / 180),
                        aOwnerPosition.Y + PositionOffsetDistance * Math.Sin((PositionOffsetAngle + aOwnerAngle) * Math.PI / 180)
                        );
                    mActualAngle = mInitialAngle + aOwnerAngle;

                    break;
                case ERotateBehavior.RotateCompletely:
                                        
                    this.mPosition = new Point(
                        aOwnerPosition.X + PositionOffsetDistance * Math.Cos((PositionOffsetAngle + aOwnerAngle) * Math.PI / 180),
                        aOwnerPosition.Y + PositionOffsetDistance * Math.Sin((PositionOffsetAngle + aOwnerAngle) * Math.PI / 180)
                        );
                    mActualAngle = mInitialAngle + aOwnerAngle;
                    SetImageAngle(mActualAngle);

                    break;
                default:
                    break;
            }           
        }

        protected void SetImageAngle(double aAngle)
        {
            if (mImage != null)
            {
                RotateTransform lRotateTransform = new RotateTransform();
                lRotateTransform.Angle = aAngle;
                mImage.RenderTransform = lRotateTransform;
            }
        }
        public bool IsVisible
        {
            get 
            { 
                if (mImage != null)
                {
                    return mImage.IsVisible;
                }
                else
                {
                    return false;
                }
            }
            set
            {
                if (mImage != null)
                {
                    mImage.Visibility = value ? Visibility.Visible : Visibility.Hidden;
                }
                mLastSetVisible = value;
            }
        }
        private bool mLastSetVisible;
        public Shape Image 
        {
            get { return mImage; }
            protected set
            {
                mImage = value;
                if (mImage != null)
                {
                    mImage.Visibility = mLastSetVisible ? Visibility.Visible : Visibility.Hidden;
                    mImage.Opacity = 0.75;
                }
            }
        }
        private Shape mImage;

        public int Depth
        {
            get
            {
                return Canvas.GetZIndex(mImage);
            }
            set
            {
                Canvas.SetZIndex(mImage, value);
            }
        }

        public Point Position { get { return mPosition; } }
        private Point mPosition;

        public double PositionOffsetDistance { get; set; }
        public double PositionOffsetAngle { get; set; }

        protected double mInitialAngle;
        protected double mActualAngle;

        public ERotateBehavior RotateBehavior { get { return mRotateBehavior; } }
        protected ERotateBehavior mRotateBehavior;
    }
}

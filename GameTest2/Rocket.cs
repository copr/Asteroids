using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Input;


namespace GameTest2
{
    public class Rocket : INotifiable
    {
        public Rocket(Key aDownKey, Key aUpKey, Key aLeftKey, Key aRightKey, string aImagePath,
            double aHeight, double aWidth, Canvas aCanvas, Point aPosition)
        {
            mRightKey = aRightKey;
            mLeftKey = aLeftKey;
            mUpKey = aUpKey;
            mDownKey = aDownKey;

            mBitmapImage = new BitmapImage(new Uri(aImagePath, UriKind.Relative));
            mImage.Source = mBitmapImage;
            mImage.RenderTransformOrigin = new Point(0.5, 0.5);
            mImage.Width = aWidth;
            mImage.Height = aHeight;

            mCanvas = aCanvas;
            mCanvas.Children.Add(mImage);
            mPosition = aPosition;
        }

        public Image Image
        {
            get
            {
                return mImage;
            }
        }
        public Point Position
        {
            get
            {
                return mPosition;
            }
            set
            {
                mPosition = value;
            }
        }

        public void KeyDown(KeyEventArgs e)
        {
            if (e.Key == mLeftKey)
            {
                mAngleChangeSign = -1;
            }
            if (e.Key == mRightKey)
            {
                mAngleChangeSign = 1;
            }
            if (e.Key == mUpKey)
            {
                mAccelerationSign = 1;
            }
            if (e.Key == mDownKey)
            {
                mAccelerationSign = -1;
            }
        }
        public void KeyUp(KeyEventArgs e)
        {
            if (e.Key == mDownKey || e.Key == mUpKey)
            {
                mAccelerationSign = 0;
            }
            if (e.Key == mLeftKey || e.Key == mRightKey)
            {
                mAngleChangeSign = 0;
            }
        }

        public void Notify(object[] args)
        {
            //Rotation
            RotateTransform rotateTransform = new RotateTransform();

            mAngle += mAngleChangeSign * mAngleChangeSpeed;
            rotateTransform.Angle = mAngle;

            TransformGroup transformGroup = new TransformGroup();
            transformGroup.Children.Add(rotateTransform);

            mImage.RenderTransform = transformGroup;

            mVerticalSpeed += mAcceleration * mAccelerationSign * Math.Sin(mAngle * Math.PI / 180);
            mHorizontalSpeed += mAcceleration * mAccelerationSign * Math.Cos(mAngle * Math.PI / 180);

            double lTotalSpeed = Math.Sqrt(mHorizontalSpeed * mHorizontalSpeed + mVerticalSpeed * mVerticalSpeed);
            if (lTotalSpeed > cMaxSpeed)
            {
                mVerticalSpeed *= cMaxSpeed / lTotalSpeed;
                mHorizontalSpeed *= cMaxSpeed / lTotalSpeed;
            }

            //Translation
            //Translation

            double lLeft = Position.X;
            double lTop = Position.Y;

            if (lTop > mCanvas.Height + mImage.Height / 2)
                lTop = -mImage.Height / 2;

            if (lLeft > mCanvas.Width + mImage.Width / 2)
                lLeft = -mImage.Width / 2;

            if (lTop < -mImage.Height / 2)
                lTop = mCanvas.Height + mImage.Height / 2;

            if (lLeft < -mImage.Width / 2)
                lLeft = mCanvas.Width + mImage.Width / 2;

            Position = new Point(lLeft, lTop);

            Canvas.SetLeft(mImage, lLeft - mImage.Width / 2);
            Canvas.SetTop(mImage, lTop - mImage.Height / 2);

            mPosition.X += mHorizontalSpeed;
            mPosition.Y += mVerticalSpeed;

        }

        private BitmapImage mBitmapImage;
        private Image mImage = new Image();
        public Point mPosition;

        private Key mRightKey;
        private Key mLeftKey;
        private Key mUpKey;
        private Key mDownKey;

        private double mAngle = -90;
        private double mAngleChangeSpeed = 4;
        private int mAngleChangeSign = 0;

        public double mHorizontalSpeed { get; set; }
        public double mVerticalSpeed { get; set; }
        private double mAcceleration = .2;
        private double mAccelerationSign = 0;
        private const double cMaxSpeed = 12;

        private Canvas mCanvas;

    }
}

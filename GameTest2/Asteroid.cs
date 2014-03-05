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
    public class Asteroid : INotifiable
    {
        public Asteroid(Canvas aCanvas)
            : this("C:/Users/copr/Documents/Visual Studio 2013/Projects/Game/GameTest2/Image/asteroid2.png", 50, 50, new Point(20, 20), aCanvas)
        {}
        public Asteroid(string aImagePath,
            double aHeight, double aWidth, Point aPosition, Canvas aCanvas)
        {
            /*mBitmapImage = new BitmapImage(new Uri(aImagePath, UriKind.Relative));
            mImage.Source = mBitmapImage;
            mImage.Width = 50;
            mImage.Height = 50;

            mCanvas = aCanvas;
            mCanvas.Children.Add(mImage);*/

            mBitmapImage = new BitmapImage(new Uri(aImagePath));
            mImage.Source = mBitmapImage;
            mImage.RenderTransformOrigin = new Point(0.5, 0.5);
            mImage.Width = aWidth;
            mImage.Height = aHeight;

            mCanvas = aCanvas; 
            mPosition = aPosition;
            Canvas.SetLeft(mImage, aPosition.X);
            Canvas.SetTop(mImage, aPosition.Y);
            mCanvas.Children.Add(mImage);

            Random generator = new Random();

            mHorizontalSpeed = generator.Next(1,10);
            mVerticalSpeed = generator.Next(1,10);

            
        }
        public void Notify(object[] args)
        {
            double lLeft = mPosition.X;
            double lTop = mPosition.Y;

            
            //lTop = lTop;

            if (lTop > mCanvas.Height + mImage.Height / 2)
                lTop = -mImage.Height / 2;

            if (lLeft > mCanvas.Width + mImage.Width / 2)
                lLeft = -mImage.Width / 2;

            if (lTop < -mImage.Height / 2)
                lTop = mCanvas.Height + mImage.Height / 2;

            if (lLeft < -mImage.Width / 2)
                lLeft = mCanvas.Width + mImage.Width / 2;
            mPosition.X = lLeft + mHorizontalSpeed;
            mPosition.Y = lTop + mVerticalSpeed;

            Canvas.SetLeft(mImage, lLeft);
            Canvas.SetTop(mImage, lTop);
         
        }

        private BitmapImage mBitmapImage;
        private Image mImage = new Image();
        private Point mPosition;
        private Canvas mCanvas;
        public double mHorizontalSpeed { get; set; }
        public double mVerticalSpeed { get; set; }
       // private double mAcceleration = .2;
       // private double mAccelerationSign = 0;
        private const double cMaxSpeed = 12;
    }
}

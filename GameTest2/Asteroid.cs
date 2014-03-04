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
    class Asteroid : INotifiable
    {
        public Asteroid(Canvas aCanvas)
        {
            mBitmapImage = new BitmapImage(new Uri("C:/Users/copr/Documents/Visual Studio 2013/Projects/Game/GameTest2/Image/asteroid2.png"));
            mImage.Source = mBitmapImage;
            mImage.Width = 50;
            mImage.Height = 50;

            mCanvas = aCanvas;
            mCanvas.Children.Add(mImage);

            Canvas.SetLeft(mImage, 200);
            Canvas.SetTop(mImage, 200);
        }
        public void Notify(object[] args)
        {
            Canvas.SetLeft(mImage, 200);
            Canvas.SetTop(mImage, 200);
            Console.WriteLine("HOJ svete");
        }

        private BitmapImage mBitmapImage;
        private Image mImage = new Image();
        private Point mPosition;
        private Canvas mCanvas;
    }
}

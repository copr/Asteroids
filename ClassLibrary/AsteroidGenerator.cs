using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Windows.Media.Imaging;
using System.Windows.Controls;
using System.Windows;

namespace GameTest2
{
    public class AsteroidGenerator
    {
        public AsteroidGenerator(BitmapFrame aAsteroidBitmapFrame)
        {
            mAsteroidBitmapFrame = aAsteroidBitmapFrame;
            AsteroidSize = 64;
        }
        public Asteroid CreateAsteroid()
        {
            int lAsteroidX = 0;
            int lAsteroidY = 0;
            int lDirection = 0;

            //left /right
            if (mRandom.Next(2) == 0)
            {
                lAsteroidY = mRandom.Next((int)RoomHeight);
                //left
                if (mRandom.Next(2) == 0)
                {
                    lAsteroidX = -AsteroidSize / 2;
                    lDirection = -90 + mRandom.Next(181);
                }
                else//right
                {
                    lAsteroidX = (int)RoomWidth + AsteroidSize / 2;
                    lDirection = 90 + mRandom.Next(181);
                }
            }
            else//top/bottom
            {
                lAsteroidX = mRandom.Next((int)RoomWidth);
                //top
                if (mRandom.Next(2) == 0)
                {
                    lAsteroidY = -AsteroidSize / 2;
                    lDirection = mRandom.Next(181);
                }
                else//bottom
                {
                    lAsteroidY = (int)RoomHeight + AsteroidSize / 2;
                    lDirection = 180 + mRandom.Next(181);
                }
            }

            return new Asteroid(mAsteroidBitmapFrame, AsteroidSize, AsteroidSize,
                new Point(lAsteroidX, lAsteroidY),
                lDirection, mRandom.NextDouble() * 5 + 1);
        }
        public double RoomWidth
        {
            get;
            set;
        }
        public double RoomHeight
        {
            get;
            set;
        }
        public int AsteroidSize
        {
            get;
            set;
        }

        private Random mRandom = new Random();
        private BitmapFrame mAsteroidBitmapFrame;
    }
}

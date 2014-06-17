using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Windows.Media.Imaging;
using System.Windows.Controls;
using System.Windows;

using Engine;

namespace GameTest2
{
    public class AsteroidGenerator : BaseObject
    {
        public AsteroidGenerator()
            : this(0)
        {

        }
        public AsteroidGenerator(double aChance)
        {
            Chance = aChance;
        }
        public override void ClockTick()
        {
            double lRandom = mRandom.NextDouble();

            if (lRandom < Chance)
            {
                RaiseRoomActionEvent(ERoomAction.AddObject, CreateAsteroid());
            }
        }
        public AsteroidGenerator(BitmapFrame aAsteroidBitmapFrame, BitmapFrame aExplosionBitmapFrame)
        {
            mAsteroidBitmapFrame = aAsteroidBitmapFrame;
            mExplosionBitmapFrame = aExplosionBitmapFrame;
            AsteroidSize = 64;
        }
        private Asteroid CreateAsteroid()
        {
            int lAsteroidX = 0;
            int lAsteroidY = 0;
            int lDirection = 0;

            //left /right
            if (mRandom.Next(2) == 0)
            {
                lAsteroidY = mRandom.Next((int)GameRoom.RoomHeight);
                //left
                if (mRandom.Next(2) == 0)
                {
                    lAsteroidX = -AsteroidSize / 2;
                    lDirection = -90 + mRandom.Next(181);
                }
                else//right
                {
                    lAsteroidX = (int)GameRoom.RoomWidth + AsteroidSize / 2;
                    lDirection = 90 + mRandom.Next(181);
                }
            }
            else//top/bottom
            {
                lAsteroidX = mRandom.Next((int)GameRoom.RoomWidth);
                //top
                if (mRandom.Next(2) == 0)
                {
                    lAsteroidY = -AsteroidSize / 2;
                    lDirection = mRandom.Next(181);
                }
                else//bottom
                {
                    lAsteroidY = (int)GameRoom.RoomHeight + AsteroidSize / 2;
                    lDirection = 180 + mRandom.Next(181);
                }
            }

            return new Asteroid(mAsteroidBitmapFrame, mExplosionBitmapFrame, AsteroidSize,
                new Point(lAsteroidX, lAsteroidY),
                lDirection, mRandom.NextDouble() * 3 + 1);
        }
        public int AsteroidSize
        {
            get;
            set;
        }

        public double Chance
        {
            get;
            set;
        }

        private BitmapFrame mAsteroidBitmapFrame;
        private BitmapFrame mExplosionBitmapFrame;

    }
}

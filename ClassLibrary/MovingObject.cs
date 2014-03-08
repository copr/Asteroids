﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace GameTest2
{
    public abstract class MovingObject
    {
        public MovingObject(BitmapFrame aBitmapFrame, int aWidth, int aHeight, Point aPosition)
        {
            mImage = new Image();
            mBitmapFrame = aBitmapFrame;
            mImage.Source = mBitmapFrame;
            mImage.RenderTransformOrigin = new Point(.5, .5);
            mImage.Stretch = System.Windows.Media.Stretch.Fill;
            mImage.Width = aWidth;
            mImage.Height = aHeight;

            mOutsideSize = Math.Min(mImage.Width, mImage.Height) / 2;

            mPosition = aPosition;
        }
        public void RotateImage(double aAngle)
        {
            RotateTransform lRotateTransform = new RotateTransform();
            lRotateTransform.Angle = aAngle;
            Image.RenderTransform = lRotateTransform;   
        }
        public abstract void ClockTick();
        
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
        public double OutsideSize
        {
            get
            {
                return mOutsideSize;
            }
        }
        public Image Image
        {
            get
            {
                return mImage;
            }
        }

        public virtual EOutsideRoomAction OutsideRoomAction
        {
            get
            {
                return EOutsideRoomAction.Destroy;
            }
        }

        public AddObject CreateObjectFunction
        {
            set
            {
                mAddObject = value;
            }
        }

        private BitmapFrame mBitmapFrame;
        private Image mImage;
        private Point mPosition = new Point();
        protected AddObject mAddObject;

        private double mOutsideSize;

        public delegate void AddObject(MovingObject o);

    }
}

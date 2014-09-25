using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Windows.Media.Imaging;
using System.Windows.Controls;
using System.Windows;
using System.IO;

using Engine;

namespace GameTest2
{
    public class BonusGenerator : BaseObject
    {
        public BonusGenerator()
        {

        }

        public override void ClockTick()
        {
            lock (mBonusTypes)
            {                
                foreach (var lType in mBonusTypes)
                {
                    double lRandom = mRandom.NextDouble();

                    if (lRandom < lType.Value)
                    {
                        RaiseRoomActionEvent(ERoomAction.AddObject, CreateBonus(lType));
                    }
                }
            }
        }
        public void AddBonusType(BonusType aType)
        {          
            mBonusTypes.Add(aType);
        }

        public Bonus CreateBonus(BonusType aType)
        {
            double lBonusX = 0;
            double lBonusY = 0;
            double lDirection = 0;

            double lBonusSize = aType.Size;
            double lSpeed = aType.Speed;
            #region position_direction
            //left /right
            if (mRandom.Next(2) == 0)
            {
                lBonusY = mRandom.Next((int)GameRoom.RoomHeight);
                //left
                if (mRandom.Next(2) == 0)
                {
                    lBonusX = -lBonusSize / 2;
                    lDirection = -90 + mRandom.Next(181);
                }
                else//right
                {
                    lBonusX = (int)GameRoom.RoomWidth + lBonusSize / 2;
                    lDirection = 90 + mRandom.Next(181);
                }
            }
            else //top/bottom
            {
                lBonusX = mRandom.Next((int)GameRoom.RoomWidth);
                //top
                if (mRandom.Next(2) == 0)
                {
                    lBonusY = -lBonusSize / 2;
                    lDirection = mRandom.Next(181);
                }
                else //bottom
                {
                    lBonusY = (int)GameRoom.RoomHeight + lBonusSize / 2;
                    lDirection = 180 + mRandom.Next(181);
                }
            }
            #endregion
            return new Bonus(aType.BitmapFrame, new Point(lBonusX, lBonusY), lDirection, lSpeed, lBonusSize, aType);
        }
        private List<BonusType> mBonusTypes = new List<BonusType>();

    }
}

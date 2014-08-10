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
        {

        }
        public override void ClockTick()
        {
            foreach (var lType in mAsteroidsTypes)
            {
                double lRandom = mRandom.NextDouble();

                if (lRandom < lType.Value)
                {
                    RaiseRoomActionEvent(ERoomAction.AddObject, CreateAsteroid(lType.Key));
                }
            }
        }
        private Asteroid CreateAsteroid(AsteroidType aType)
        {
            double lAsteroidX = 0;
            double lAsteroidY = 0;
            double lDirection = 0;

            double lAsteroidSize = aType.MinSize + mRandom.NextDouble() * (aType.MaxSize - aType.MinSize);

            //left /right
            if (mRandom.Next(2) == 0)
            {
                lAsteroidY = mRandom.Next((int)GameRoom.RoomHeight);
                //left
                if (mRandom.Next(2) == 0)
                {
                    lAsteroidX = -lAsteroidSize / 2;
                    lDirection = -90 + mRandom.Next(181);
                }
                else//right
                {
                    lAsteroidX = (int)GameRoom.RoomWidth + lAsteroidSize / 2;
                    lDirection = 90 + mRandom.Next(181);
                }
            }
            else //top/bottom
            {
                lAsteroidX = mRandom.Next((int)GameRoom.RoomWidth);
                //top
                if (mRandom.Next(2) == 0)
                {
                    lAsteroidY = -lAsteroidSize / 2;
                    lDirection = mRandom.Next(181);
                }
                else //bottom
                {
                    lAsteroidY = (int)GameRoom.RoomHeight + lAsteroidSize / 2;
                    lDirection = 180 + mRandom.Next(181);
                }
            }

            double lRotationSpeed = (mRandom.NextDouble() - .5) * 2 * aType.MaxRotationSpeed;

            AsteroidSettings lSettings = new AsteroidSettings()
            {
                MinSizeForChildren = aType.MinSizeForChildren,
                MissileDamage = aType.MissileDamage,
                ProjectileDamage = aType.ProjectileDamage,
                Size = lAsteroidSize,
                Strength = aType.Strength,
                TypeName = aType.TypeName,
                PointsMultiplier = aType.PointsMultiplier,
                RotationSpeed = lRotationSpeed
            };

            double lSpeed = aType.MinSpeed + mRandom.NextDouble() * (aType.MaxSpeed - aType.MinSpeed);

            return new Asteroid(aType.BitmapFrame, aType.ExplosionFrame,
                new Point(lAsteroidX, lAsteroidY),
                lDirection, lSpeed,
                lSettings);
        }

        public void SetChance(AsteroidType aType, double aNewChance)
        {
            if (mAsteroidsTypes.ContainsKey(aType))
            {
                mAsteroidsTypes[aType] = aNewChance;
            }

        }
        public void AddAsteroidType(AsteroidType aNewType, double aChance = 0)
        {
            mAsteroidsTypes.Add(aNewType, aChance);
        }
        public void DeleteAsteroidType(AsteroidType aType)
        {
            if (mAsteroidsTypes.ContainsKey(aType))
            {
                mAsteroidsTypes.Remove(aType);
            }
        }
        public void ClearAsteroidTypes()
        {
            mAsteroidsTypes.Clear();
        }

        private Dictionary<AsteroidType, double> mAsteroidsTypes = new Dictionary<AsteroidType, double>();
    }
}

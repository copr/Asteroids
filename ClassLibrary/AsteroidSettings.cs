using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameTest2
{
    public class AsteroidSettings
    {
        public double ProjectileDamage { get; set; }
        public double MissileDamage { get; set; }
        public double MinSizeForChildren { get; set; }
        public double Size { get; set; }
        public string TypeName { get; set; }
        public int Strength { get; set; }
        public double PointsMultiplier { get; set; }
        public double RotationSpeed { get; set; }

        public AsteroidSettings Clone()
        {
            return new AsteroidSettings()
            {
                ProjectileDamage = this.ProjectileDamage,
                MissileDamage = this.MissileDamage,
                MinSizeForChildren = this.MinSizeForChildren,
                Size = this.Size,
                TypeName = this.TypeName,
                Strength = this.Strength,
                PointsMultiplier = this.PointsMultiplier,
                RotationSpeed = this.RotationSpeed
            };
        }
    }
}

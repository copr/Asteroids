using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;

namespace GameTest2
{
    public class AsteroidType
    {
        public BitmapFrame BitmapFrame { get; set; }
        public BitmapFrame ExplosionFrame { get; set; }
        public double ProjectileDamage { get; set; }
        public double MissileDamage { get; set; }
        public string TypeName { get; set; }
        public double MaxSize { get; set; }
        public double MinSize { get; set; }
        public double MinSizeForChildren { get; set; }
        public int Strength { get; set; }
        public double PointsMultiplier { get; set; }
        public double MinSpeed { get; set; }
        public double MaxSpeed { get; set; }
        public double MaxRotationSpeed { get; set; }
    }
}

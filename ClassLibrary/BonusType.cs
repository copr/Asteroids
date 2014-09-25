using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;

namespace GameTest2
{
    public class BonusType
    {
        public BitmapFrame BitmapFrame { get; set; }
        public string TypeName { get; set; }
        public double Size { get; set; }
        public double Speed { get; set; }
        public double PlusHealth { get; set;}
        public double PlusMissiles { get; set; }
        public double Value { get; set; }
        public double Invulnerability { get; set; }
    }
}

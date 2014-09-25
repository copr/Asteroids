using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Engine;

namespace GameTest2
{
    public class Score : BaseViewModel
    {
        public int Points
        {
            get
            {
                return mPoints;
            }
            set
            {
                if (mPoints == value) return;
                mPoints = value;
                OnPropertyChanged("Points");
            }
        }
        public int AsteroidsShot { get; set; }
        public int BonusesTaken { get; set; }
        public int ShotsShot { get; set; }
        public int MissilesShot { get; set; }
        public int HealthHealed { get; set; }
        public int TimeInvulnerable { get; set; }
        public double TimeAlive { get; set; }
        public int TimesGotHit { get; set; }
        private int mPoints;
    }
}

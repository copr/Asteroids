using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameTest2
{
    public class InputNamePopUpViewModel : BaseViewModel
    {
        public InputNamePopUpViewModel()
        {

        }

        private int? mNewScore;
        public int? NewScore
        {
            get { return mNewScore; }
            set 
            {
                if (mNewScore == value) return;
                mNewScore = value;
                OnPropertyChanged("NewScore");
            }
        }

        private int? mPosition;
        public int? Position
        {
            get { return mPosition; }
            set
            {
                if (mPosition == value) return;
                mPosition = value;
                OnPropertyChanged("Position");
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GameTest2
{
    /// <summary>
    /// Interaction logic for HealthBar.xaml
    /// </summary>
    public partial class HealthBar : UserControl
    {
        public HealthBar()
        {
            InitializeComponent();
            mUpdateTimer = new Timer();
            mUpdateTimer.Interval = mUpdatePeriodMs;
            mUpdateTimer.Elapsed += TimerProc;
            mUpdateTimer.Start();
        }

        public static readonly DependencyProperty PercentageProperty = 
            DependencyProperty.Register("Percentage", typeof(double), typeof(HealthBar));

        public double Percentage
        {
            get
            {
                return (double)GetValue(PercentageProperty);
            }
            set
            {
                SetValue(PercentageProperty, value);
            }
        }

        private Timer mUpdateTimer;

        private void TimerProc(object o, ElapsedEventArgs e)
        {
            (o as Timer).Stop();
            this.Dispatcher.Invoke(new Action(() => { Update(); }), null);
            (o as Timer).Start();
        }
        private void Update()
        {
            Rectangle.Width = Math.Max(Percentage / 100d * this.ActualWidth, 0);
        }

        private int mUpdatePeriodMs = 100;
    }
}

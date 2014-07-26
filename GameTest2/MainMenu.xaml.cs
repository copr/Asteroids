using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    /// Interaction logic for MainMenu.xaml
    /// </summary>
    public partial class MainMenu : UserControl
    {
        public MainMenu()
        {
            InitializeComponent();
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            MainMenuViewModel lContext = this.DataContext as MainMenuViewModel;
            if (lContext != null)
            {
                lContext.mMain.RaiseStartGameEvent();
            }
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            MainMenuViewModel lContext = this.DataContext as MainMenuViewModel;
            if (lContext != null)
            {
                lContext.mMain.RaiseStopGameEvent();
            }
        }

        private void HighScoresButton_Click(object sender, RoutedEventArgs e)
        {
            MainMenuViewModel lContext = this.DataContext as MainMenuViewModel;
            if (lContext != null)
            {
                lContext.mMain.ShowHighScores();
            }
        }
    }
}

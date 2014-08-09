﻿using System;
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
using System.Windows.Shapes;

namespace GameTest2
{
    /// <summary>
    /// Interaction logic for InputNamePopUp.xaml
    /// </summary>
    public partial class InputNamePopUp : Window
    {
        public InputNamePopUp()
        {
            InitializeComponent();
            NameTextBox.Focus();
        }

        

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            if (NameTextBox.Text.Length == 0)
            {
                NameTextBox.Text = "unknown";
                return;
            }
            if (!Char.IsLetter(NameTextBox.Text[0]))
            {
                NameTextBox.Text = "_" + NameTextBox.Text;
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                this.Close();
        }
    }
}

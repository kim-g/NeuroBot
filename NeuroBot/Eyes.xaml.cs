﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace NeuroBot
{
    /// <summary>
    /// Логика взаимодействия для Eyes.xaml
    /// </summary>
    public partial class Eyes : Window
    {
        System.Windows.Threading.DispatcherTimer timer;
        public Eyes()
        {
            InitializeComponent();
            timer = new System.Windows.Threading.DispatcherTimer();

            timer.Tick += new EventHandler((object sender, EventArgs e) => field.Step());
            timer.Interval = new TimeSpan(0, 0, 0, 0, 100);
            timer.Start();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            field.Mir();
            timer.Start();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            field.Step();
        }
    }
}

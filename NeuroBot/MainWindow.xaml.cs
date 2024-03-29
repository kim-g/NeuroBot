﻿using System;
using System.Windows;

namespace NeuroBot
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        NeuralNet NN = new NeuralNet(new Random());
        
        public MainWindow()
        {
            InitializeComponent();
            NN.Randomize();
            VNN.Neurons = NN;
            VNN.Update();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NN.Randomize();
            VNN.Update();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            NN.MutateHarsh();
            VNN.Update();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            NN.Mutate();
            VNN.Update();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            VNN.Update();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            BrainInput input = new BrainInput();
            input.Energy = Convert.ToSingle(EnergyTB.Text);
            input.Vision = Convert.ToSingle(VisionTB.Text);
            input.isRelative = Convert.ToSingle(IsRelativeTB.Text);
            input.Rotation = Convert.ToSingle(RotationTB.Text);
            input.Height = Convert.ToSingle(HeightTB.Text);

            BrainOutput output = NN.MakeChoice(input);
            VNN.Update();
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            Eyes eyes = new Eyes();
            eyes.Show();
        }
    }
}

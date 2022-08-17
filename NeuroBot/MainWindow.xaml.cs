using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NeuroBot
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        NeuralNet NN = new NeuralNet();
        
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
    }
}

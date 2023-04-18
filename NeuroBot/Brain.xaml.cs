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
using System.Windows.Shapes;

namespace NeuroBot
{
    /// <summary>
    /// Логика взаимодействия для Brain.xaml
    /// </summary>
    public partial class Brain : Window
    {
        public Brain(NeuralNet net)
        {
            InitializeComponent();

            VNN.Neurons = net;
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            VNN.Update();
        }
    }
}

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
    /// Логика взаимодействия для Eyes.xaml
    /// </summary>
    public partial class Eyes : Window
    {
        public Eyes()
        {
            InitializeComponent();
        }

        private void Turn_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Turn.Text == "") return;

            Bantic.Turn = Convert.ToInt32(Turn.Text);
        }

        private void Color_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (ColorR == null) return;
            if (ColorG == null) return;
            if (ColorB == null) return;
            if (ColorR?.Text == "") return;
            if (ColorG?.Text == "") return;
            if (ColorB?.Text == "") return;

            Bantic.ColorE = Color.FromArgb(255, Convert.ToByte(ColorR.Text), Convert.ToByte(ColorG.Text), Convert.ToByte(ColorB.Text));
        }

        private void TopE_TextInput(object sender, TextChangedEventArgs e)
        {
            if (TopE.Text == "") return;
            Bantic.LandHeight = Convert.ToInt32(TopE.Text);
        }

    }
}

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
    /// Логика взаимодействия для Field.xaml
    /// </summary>
    public partial class Field : UserControl
    {
        private List <bot> listB = new List<bot>();
        private bot[,] FieldArray = new bot[180, 101];

        
        public Field()
        {
            InitializeComponent();
        }

        public void Mir()
        {
            Grid1.Children.Clear();
            listB.Clear();
            FieldArray = new bot[180, 101];

            Random random = new Random();

            for (int i = 0; i < 100; i++)
            {
                bot B = new bot(random);

                Grid1.Children.Add(B);
                listB.Add(B);
                FieldArray[B.Horizontal, B.LandHeight] = B;

                B.Horizontal = random.Next(0, 179);
                B.LandHeight = random.Next(0, 100);

                B.HorizontalAlignment = HorizontalAlignment.Left;
                B.VerticalAlignment = VerticalAlignment.Top;

                B.Turn = random.Next(0, 7);

                B.ColorZ = Color.FromRgb( Convert.ToByte(random.Next(0,255)), Convert.ToByte(random.Next(0, 255)), Convert.ToByte(random.Next(0, 255)));
                B.ColorE = Color.FromRgb((byte)(255 - B.ColorZ.R), (byte)(255 - B.ColorZ.G), (byte)(255 - B.ColorZ.B));

                B.Time = random.Next(-20, 10);

                B.ColorO = Color.FromRgb(Convert.ToByte(random.Next(0, 255)), Convert.ToByte(random.Next(0, 255)), Convert.ToByte(random.Next(0, 255)));

                B.Energy = random.Next(10, 50);


            }
        }

        public void Step()
        {
            foreach (bot B in listB)
                B.Step();
        }

    }
    
}

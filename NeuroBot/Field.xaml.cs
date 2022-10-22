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

        private List<bot> ToDelete = new List<bot>();
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

            for (int i = 0; i < 1000; i++)
            {
                bot B = new bot(random);

                Grid1.Children.Add(B);
                listB.Add(B);
                

                B.Horizontal = random.Next(0, 179);
                B.LandHeight = random.Next(0, 100);

                FieldArray[B.Horizontal, B.LandHeight] = B;

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
            DeleteAll();
        }

        /// <summary>
        /// клетка занята
        /// 
        /// 
        /// </summary>
        /// <param name="x">значение по горизонтали</param>
        /// <param name="y">значение по вертикали</param>
        /// <returns></returns>
        public bool Occupyed(int x, int y)
        {
            return FieldArray[x, y] != null;
        }

        public void Move(bot Bot, int new_x, int new_y)
        {
            if (Occupyed(new_x, new_y)) return;

            
            FieldArray[Bot.Horizontal, Bot.LandHeight] = null;
            Bot.Horizontal = new_x;
            Bot.LandHeight = new_y;
            FieldArray[Bot.Horizontal, Bot.LandHeight] = Bot;
        }

        public void Delete(bot Bot)
        {
            ToDelete.Add(Bot);
        }

        public void DeleteAll()
        {
            foreach (bot Bot in ToDelete)
            {
                listB.Remove(Bot);
                FieldArray[Bot.Horizontal, Bot.LandHeight] = null;
                Grid1.Children.Remove(Bot);
            }
            ToDelete.Clear();
        }
    }
    
}

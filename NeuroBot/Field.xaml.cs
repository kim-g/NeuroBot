using System;
using System.Collections.Generic;
using System.IO;
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
        public const int H = 90;
        public const int W = 50;
        
        private List <bot> listB = new List<bot>();
        private bot[,] FieldArray = new bot[H, W];

        private List<bot> ToDelete = new List<bot>();
        private List<bot> ToAdd = new List<bot>();
        private int StepN = 0;
        public Field()
        {
            InitializeComponent();
        }

        public void Mir()
        {

            StepN = 0;
            Grid1.Children.Clear();
            listB.Clear();
            FieldArray = new bot[H, W];

            Random random = new Random();

            for (int i = 0; i < 1000; i++)
            {
                bot B = new bot(random);

                Add(B);

                B.Horizontal = random.Next(0, H-1);
                B.LandHeight = random.Next(0, W-1);

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
            AddAll();
        }

        public void Step()
        {
            StepN++;
            foreach (bot B in listB)
                B.Step();
            DeleteAll();
            AddAll();


            RenderTargetBitmap rtb = new RenderTargetBitmap((int)this.ActualWidth, 
                (int)this.ActualHeight, 96, 96, PixelFormats.Pbgra32);
            rtb.Render(this);

            JpegBitmapEncoder jpg = new JpegBitmapEncoder();
            jpg.Frames.Add(BitmapFrame.Create(rtb));

            using (Stream stm = File.Create(@"c:\Frames\Frame" + StepN.ToString() + ".jpg"))
            {
                jpg.Save(stm);
            }
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

        public void Add(bot Bot)
        {
            ToAdd.Add(Bot);
        }

        public void AddAll()
        {
            foreach (bot Bot in ToAdd)
            {
                Grid1.Children.Add(Bot);
                listB.Add(Bot);
                FieldArray[Bot.Horizontal, Bot.LandHeight] = Bot;
            }
            ToAdd.Clear();
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

            GC.Collect();
        } 
        
        public bot Get(Point p)
        {
            return FieldArray[(int)p.X, (int)p.Y];
        }
    }

   
    
}

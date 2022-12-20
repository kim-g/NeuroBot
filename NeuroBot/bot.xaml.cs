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
    /// Логика взаимодействия для bot.xaml
    /// </summary>
    public partial class bot : UserControl
    {
        private Color colorz;
        private Color coloro;
        private Color colore;
        private int energy;
        private int turn;
        private int height;
        private int horizontal;
        private int time;
        private NeuralNet brain;
        private Random rnd;
        private int old = 0;

        private const int FullMutation = 5;
        private const int HalfMutation = 10;
        private const int WeakMutation = 30;

        /// <summary>
        /// Возраст
        /// </summary>
        public int Old
        {
            get
            {
                return old;
            }
            set
            {
                old = value;
                int randomvalue = rnd.Next(0, 999);
                
                if (old < 5) if (randomvalue < 1) Die ();
                if ((old >= 5) && (old <10)) if (randomvalue < 50) Die();
                if ((old >= 10) && (old < 15)) if (randomvalue < 100) Die();
                if (old >= 15) if (randomvalue < 200) Die();

            }

        }

    

        /// <summary>
        /// цвет заливки 
        /// </summary>
        public Color ColorZ
        {
            get
            {
                return colorz;
            }
            set
            {
                colorz = value;
                Rect.Fill = new SolidColorBrush(colorz);
            }
        }
        /// <summary>
        /// цвет обводки
        /// </summary>
        public Color ColorO
        {
            get
            {
                return coloro;
            }
            set
            {
                coloro = value;
                Rect.Stroke = new SolidColorBrush(coloro);

            }
        }
        /// <summary>
        /// цвет глаз
        /// </summary>
        public Color ColorE
        {
            get
            {
                return colore;

            }
            set
            {
                colore = value;
                eye.Fill = new SolidColorBrush(colore);
                eye2.Fill = new SolidColorBrush(colore);
            }
        }

        /// <summary>
        /// поворот
        /// </summary>
        public int Turn
        {
            get
            {
                return turn;
            }
            set
            {
                if (value >= 0 && value <= 7)
                    turn = value;

                switch (turn)
                {
                    case 0:
                        eye.Margin = new Thickness(6, 1, 2, 7);
                        eye2.Margin = new Thickness(2, 1, 6, 7);
                        break;
                    case 1:
                        eye.Margin = new Thickness(5, 1, 3, 7);
                        eye2.Margin = new Thickness(7, 3, 1, 5);
                        break;
                    case 2:
                        eye.Margin = new Thickness(7, 2, 1, 6);
                        eye2.Margin = new Thickness(7, 6, 1, 2);
                        break;
                    case 3:
                        eye.Margin = new Thickness(5, 7, 3, 1);
                        eye2.Margin = new Thickness(7, 5, 1, 3);
                        break;
                    case 4:
                        eye.Margin = new Thickness(2, 7, 6, 1);
                        eye2.Margin = new Thickness(6, 7, 2, 1);
                        break;
                    case 5:
                        eye.Margin = new Thickness(3, 7, 5, 1);
                        eye2.Margin = new Thickness(1, 5, 7, 3);
                        break;
                    case 6:
                        eye.Margin = new Thickness(1, 2, 7, 6);
                        eye2.Margin = new Thickness(1, 6, 7, 2);
                        break;
                    case 7:
                        eye.Margin = new Thickness(3, 1, 5, 7);
                        eye2.Margin = new Thickness(1, 3, 7, 5);
                        break;
                }
            }
        }
        /// <summary>
        /// высота бота над дном
        /// </summary>
        public int LandHeight
        {
            get
            {
                return height;
            }
            set
            {
                if (value >= 0 && value <= Field.W - 1 )
                    height = value;

                double H = ActualHeight == 0 ? 10 : ActualHeight;
                Margin = new Thickness(Margin.Left, height * 10, 0, 0);
            }
        }
        /// <summary>
        /// координата бота по горизонтали
        /// </summary>
        public int Horizontal
        {
            get
            {
                return horizontal;

            }
            set
            {
                horizontal = value;
                if (value < 0)
                    horizontal = value + Field.H - 1;
                if (value > 179)
                    horizontal = value - Field.H + 1;
                double H = ActualWidth == 0 ? 10 : ActualWidth;
                Margin = new Thickness(horizontal * 10, Margin.Top, 0, 0);
            }
        }
        /// <summary>
        /// время до деления бота
        /// </summary>
        public int Time
        {
            get
            {
                return time;
            }
            set
            {
                time = value;
                if (value < 0)
                    time = 0;
            }
        }

        public int Energy
        {
            get
            {
                return energy;
            }
            set
            {
                energy = value;
                if (value > 50)
                    energy = 50;
                if (value < 0)
                    Die();
            }
        }
        public NeuralNet Brain
        {
            get
            {
                return brain;
            }
            set
            {
                brain = value;
            }
        }

        public Field Env
        {
            get
            {
                return (Field)((Grid)Parent).Parent;
            }
        }


        public bot(Random random)
        {
            InitializeComponent();
            rnd = random;
            brain = new NeuralNet(random);
            brain.Randomize(1);
        }

        /// <summary>
        /// умерание
        /// </summary>
        public void Die()
        {
            Env.Delete(this);
        }

        public void Step()
        {
            Old++;
            BrainInput Parameters = new BrainInput(true);
            Parameters.Rotation = Turn / 7;
            Parameters.Height = LandHeight / (Field.W - 1);
            Parameters.Energy = energy / 50;
            if (Occupied())
            {
                Parameters.Vision = 1;
                bot Seen = Env.Get(Direction());
                Parameters.isRelative = ColorZ == Seen.ColorZ ? 1 : 0;
            }
            else 
            {
                Parameters.Vision = 0;
                Parameters.isRelative = 0;
            }

            BrainOutput Decision = Brain.MakeChoice(Parameters);

            Energy -= 1; 
            
            if (Decision.Photosynthesis > 0.95)
            {
                Energy += (Field.W - LandHeight) * 4 / Field.W;
                Time--;
            }
            if (Decision.Rotate > 0.95)
            {
                Turn++;
                Energy -= 1;
                Time--;
                
            }
            if (Decision.Rotate < -0.95)
            {
                Turn--;
                Energy -= 1;
                Time--;
                
            }
            
           
            if (Decision.Divide > 0.95)
            {
                CellDivision();
                Time--;
                
            }

            if (Decision.Move > 0.95)
            {
                Point p = Direction();
                Env.Move(this, (int)p.X, (int)p.Y);
                Energy -= 2;
                Time--;
                
            }

            

            if (Decision.Attack > 0.95)
            {
                Energy -= 4;
                Eat();
                Time--;
                

            }
            
            


            
        }
        /// <summary>
        /// размножение
        /// </summary>
        private void CellDivision()
        {
            int randomvalue = rnd.Next(0, 99);
            List<Point> points = new List<Point>();
            for (int i = 0; i < 8; i++)
            {
                Point point = Direction(i);
                if (!Env.Occupyed(Convert.ToInt32(point.X), Convert.ToInt32(point.Y)))
                    points.Add(point);
            }

            if (points.Count == 0) return;
            int Dir = rnd.Next(0, points.Count - 1);

            if (Energy < 6) return;
            if (Time > 0) return;

            Old = 0;
            bot NewBot = new bot(rnd);
            NewBot.Brain = new NeuralNet(Brain);

            Env.Add(NewBot);
            NewBot.Horizontal = Convert.ToInt32(points[Dir].X);
            NewBot.LandHeight = Convert.ToInt32(points[Dir].Y);

            Energy -= 4;
            Energy /= 2;
            NewBot.Energy = Energy;
            NewBot.Turn = rnd.Next(0, 7);
            NewBot.HorizontalAlignment = HorizontalAlignment.Left;
            NewBot.VerticalAlignment = VerticalAlignment.Top;

            NewBot.ColorE = ColorE;
            NewBot.ColorO = ColorO;
            NewBot.ColorZ = ColorZ;

            NewBot.Time = rnd.Next (8,12);
            Time = rnd.Next (8,12);


            if (randomvalue < FullMutation)
            {
                NewBot.Brain.Randomize();
                NewBot.ColorZ = Color.FromRgb(Convert.ToByte(rnd.Next(0, 255)), Convert.ToByte(rnd.Next(0, 255)), Convert.ToByte(rnd.Next(0, 255)));
                return;
            }
            if (randomvalue < FullMutation + HalfMutation)
            {
                NewBot.Brain.MutateHarsh();
                NewBot.ColorZ = Color.FromRgb(Convert.ToByte(rnd.Next(0, 255)), Convert.ToByte(rnd.Next(0, 255)), Convert.ToByte(rnd.Next(0, 255)));
                return;
            }
            if (randomvalue < FullMutation + HalfMutation + WeakMutation)
            {
                NewBot.Brain.Mutate();
                return;
            }

        }

        private void Eat()
        {
            Point p = Direction();
            if (Env.Occupyed(Convert.ToInt32(p.X), Convert.ToInt32(p.Y)) == false) return;

            bot Prey = Env.Get(p);
            Energy += Prey.Energy;
            Prey.Energy = 0;
        }

        private bool Occupied()
        {
            Point p = Direction();
            return Env.Occupyed((int)p.X, (int)p.Y);
        }


        private Point Direction(int Dir = -1)
        {
            int x = Horizontal;
            int y = LandHeight;

            int Direct = Dir == -1 ? Turn : Dir;

            switch (Direct)
            {
                case 0:
                    y += -1;
                    break;
                case 1:
                    x += 1;
                    y += -1;
                    break;
                case 2:
                    x += 1;
                    break;
                case 3:
                    x += +1;
                    y += 1;
                    break;
                case 4:
                    y += 1;
                    break;
                case 5:
                    x += -1;
                    y += +1;
                    break;
                case 6:
                    x += -1;
                    break;
                case 7:
                    x += -1;
                    y += -1;
                    break;
            }
            if (x < 0) x += Field.H;
            if (x > Field.H-1) x -= Field.H-1;

            if (y < 0) y = 0;
            if (y > Field.W-1) y = Field.W-1;
            return new Point(x, y);
        }

        private void Grid_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Brain NewBrain = new Brain(brain);
            NewBrain.ShowDialog();
        }
    }
}

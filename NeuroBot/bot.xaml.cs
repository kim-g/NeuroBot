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
                if (value >= 0 && value <= 100 )
                    height = value;

                double H = ActualHeight == 0 ? 10 : ActualHeight;
                Margin = new Thickness(Margin.Left, height * H, 0, 0);
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
                    horizontal = value + 180;
                if (value > 179)
                    horizontal = value - 180;
                double H = ActualWidth == 0 ? 10 : ActualWidth;
                Margin = new Thickness(horizontal * H, Margin.Top, 0, 0);
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
            BrainInput Parameters = new BrainInput(true);
            Parameters.Rotation = Turn;
            Parameters.Height = LandHeight;
            Parameters.Energy = energy;
            Parameters.Vision = Occupied() ? 1 : 0;
            Parameters.isRelative = 0;

            BrainOutput Decision = Brain.MakeChoice(Parameters);

            Energy -= 1;
            if (Decision.Rotate > 0.95)
            {
                Turn++;
                Energy -= 1;
            }
            if (Decision.Rotate < -0.95)
            {
                Turn--;
                Energy -= 1;
            }
            if (Decision.Move > 0.95)
            {
                Point p = Direction();
                Env.Move(this, (int)p.X, (int)p.Y);
                Energy -= 2;
            }
            if (Decision.Photosynthesis > 0.95)
            {
                Energy += 1;
            }

        }

        private bool Occupied()
        {
            Point p = Direction();
            return Env.Occupyed((int)p.X, (int)p.Y);
        }


        private Point Direction()
        {
            int x = Horizontal;
            int y = LandHeight;

            switch (turn)
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
            if (x < 0) x += 180;
            if (x > 179) x -= 180;

            if (y < 0) y = 0;
            if (y > 100) y = 100;
            return new Point(x, y);
        }

    }
}

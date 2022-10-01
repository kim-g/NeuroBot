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
                if (value > 0)
                    height = value;

                Margin = new Thickness(Margin.Left, height * ActualHeight, 0, 0);
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
                if (value > 0)
                    horizontal = value;
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
            }
        }



        public bot()
        {
            InitializeComponent();
        }

    }
}

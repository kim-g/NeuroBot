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
            }
        }

        public bot()
        {
            InitializeComponent();
        }
    }
}

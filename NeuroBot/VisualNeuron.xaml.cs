using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace NeuroBot
{
    /// <summary>
    /// Логика взаимодействия для VisualNeuron.xaml
    /// </summary>
    public partial class VisualNeuron : UserControl
    {
        protected Neuron neuron;
        protected Dictionary<NeuronType, Brush> NeuronColors = new Dictionary<NeuronType, Brush>() 
        { 
            {NeuronType.input, new SolidColorBrush(Colors.Lime) }, 
            {NeuronType.output, new SolidColorBrush(Colors.Red) }, 
            {NeuronType.basic, new SolidColorBrush(Colors.LightSkyBlue) }, 
            {NeuronType.random, new SolidColorBrush(Colors.Violet) }, 
            {NeuronType.memory, new SolidColorBrush(Colors.Yellow) }, 
            {NeuronType.radialbasis, new SolidColorBrush(Colors.Blue) }
        };
        protected Dictionary<NeuronType, string> NeuronTypes = new Dictionary<NeuronType, string>()
        {
            {NeuronType.input, "IN" },
            {NeuronType.output, "OUT" },
            {NeuronType.basic, "B" },
            {NeuronType.random, "RND" },
            {NeuronType.memory, "M" },
            {NeuronType.radialbasis, "R" }
        };

        protected Brush BlueStroke = new SolidColorBrush(Colors.Blue);
        protected Brush RedStroke = new SolidColorBrush(Colors.Red);

        protected Line[] Lines = new Line[Settings.MaxConnectionsPerNeuron];

        /// <summary>
        /// Нейрон для визуализации
        /// </summary>
        public Neuron NeuronForVisualization
        {
            get => neuron;
            set 
            { 
                neuron = value;
                Update();
            }
        }

        /// <summary>
        /// Выдаёт центр круга
        /// </summary>
        public Point Center
        {
            get
            {
                double x = ActualWidth / 2 + Margin.Left;
                double y = ellipse.ActualHeight / 2 + Margin.Top;
                return new Point(x, y);
            }
        }

        /// <summary>
        /// Номер слоя, к которому относится нейрон
        /// </summary>
        public int Layer = 0;
        
        public VisualNeuron()
        {
            InitializeComponent();
            for (int i = 0; i < Settings.MaxConnectionsPerNeuron; i++)
            {
                Lines[i] = new Line();
            }
        }

        /// <summary>
        /// Обновление вида нейрона
        /// </summary>
        public void Update()
        {
            ellipse.Fill = NeuronColors[neuron.Type];
            TypeLabel.Content = NeuronTypes[neuron.Type];
            ValueLabel.Content = neuron.Value.ToString("F2");
            BiasLabel.Content = neuron.bias.ToString("F2");
            MemoryLabel.Content = neuron.Type == NeuronType.memory ?  neuron.Memory.ToString("F2") : " ";
        }

        public void SetLine(int N, Point To)
        {
            Lines[N].X1 = Center.X;
            Lines[N].Y1 = Center.Y;

            if (N >= neuron.Connections)
            {
                Lines[N].X2 = Center.X;
                Lines[N].Y2 = Center.Y;
                return;
            }

            Lines[N].X2 = To.X;
            Lines[N].Y2 = To.Y;
            Lines[N].Stroke = neuron.allConnections[N].Weight > 0 ? BlueStroke : RedStroke;
            Lines[N].StrokeThickness = Math.Abs(neuron.allConnections[N].Weight) * 2;
        }

        public void LinesSetUp(Grid Parent)
        {
            foreach(Line line in Lines)
            {
                line.X1 = Center.X;
                line.Y1 = Center.Y;
                line.X2 = Center.X;
                line.Y2 = Center.Y;
                Panel.SetZIndex(line, 1000);
                Parent.Children.Add(line);
            }
        }
    }
}

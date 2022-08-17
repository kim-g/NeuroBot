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
    /// Логика взаимодействия для VisualNN.xaml
    /// </summary>
    public partial class VisualNN : UserControl
    {
        protected VisualNeuron[,] NeuronElements;
        
        protected NeuralNet neurons;

        public NeuralNet Neurons
        {
            get => neurons;
            set
            {
                neurons = value;
                for (int i = 0; i < Settings.NumNeuronLayers; i++)
                    for (int j = 0; j < Settings.NeuronsInLayer; j++)
                        NeuronElements[i, j].NeuronForVisualization = neurons.Neurons[i, j];
            }
        }
        
        public VisualNN()
        {
            InitializeComponent();

            NeuronElements = new VisualNeuron[Settings.NumNeuronLayers, Settings.NeuronsInLayer];
            for (int i = 0; i < Settings.NumNeuronLayers; i++)
                for (int j = 0; j < Settings.NeuronsInLayer; j++)
                {
                    NeuronElements[i, j] = new VisualNeuron();
                    MainGrid.Children.Add(NeuronElements[i, j]);
                    NeuronElements[i, j].Width = Settings.NeuronWidth;
                    NeuronElements[i, j].Height = Settings.NeuronHeight;
                    NeuronElements[i, j].Margin = new Thickness(i * Settings.NeuronWidth, j * Settings.NeuronHeight, 0, 0);
                    NeuronElements[i, j].VerticalAlignment = VerticalAlignment.Top;
                    NeuronElements[i, j].HorizontalAlignment = HorizontalAlignment.Left;
                    NeuronElements[i, j].Layer = i;
                    NeuronElements[i, j].LinesSetUp(MainGrid);
                    Panel.SetZIndex(NeuronElements[i, j], 10000);

                }
        }

        public void Update()
        {
            foreach (VisualNeuron neuron in NeuronElements)
            {
                neuron.Update();
                for (int i = 0; i < Settings.MaxConnectionsPerNeuron; i++)
                {
                    if (neuron.NeuronForVisualization.allConnections[i].Num == -1) continue;
                    if (neuron.Layer == Settings.NumNeuronLayers - 1) continue;
                    Point To = NeuronElements[neuron.Layer + 1, neuron.NeuronForVisualization.allConnections[i].Num].Center;
                    neuron.SetLine(i, To);
                }
            }
        }
    }
}

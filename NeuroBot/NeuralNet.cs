using System;

namespace NeuroBot
{
    /// <summary>
    /// Входные данные для нейросети
    /// </summary>
    public struct BrainInput
    {
        /// <summary>
        /// Энергия бота
        /// </summary>
        public float Energy;

        /// <summary>
        /// Зрение бота
        /// </summary>
        public float Vision;

        /// <summary>
        /// Является ли бот относительным
        /// </summary>
        public float isRelative;

        /// <summary>
        /// Поворот бота
        /// </summary>
        public float Rotation;

        /// <summary>
        /// Высота расположения бота
        /// </summary>
        public float Height;

        public BrainInput(bool Set = true)
        {
            Energy = 0f;
            Vision = 0f;
            isRelative = 0f;
            Rotation = 0f;
            Height = 0f;
        }
    }

    /// <summary>
    /// Выходные данные (решение) нейросети
    /// </summary>
    public struct BrainOutput
    {
        /// <summary>
        /// Количество полей выходных данных
        /// </summary>
        public const int NumerOfFields = 5;

        /// <summary>
        /// Выходные данные в виде массива
        /// </summary>
        public int[] fields;

        /// <summary>
        /// Поворот бота
        /// </summary>
        public int Rotate { get => fields[0]; set => fields[0] = value; }

        /// <summary>
        /// Движение бота
        /// </summary>
        public int Move { get => fields[1]; set => fields[1] = value; }

        /// <summary>
        /// Бот будет фотосинтезировать
        /// </summary>
        public int Photosynthesis { get => fields[2]; set => fields[2] = value; }

        /// <summary>
        /// Бот будет делиться
        /// </summary>
        public int Divide { get => fields[3]; set => fields[3] = value; }

        /// <summary>
        /// Бот будет атаковать бота в поле видимости
        /// </summary>
        public int Attack { get => fields[2]; set => fields[2] = value; }

        public BrainOutput(bool Set=true)
        {
            fields = new int[NumerOfFields];
        }
    }

    /// <summary>
    /// Нейросеть бота
    /// </summary>
    public class NeuralNet
    {
        /// <summary>
        /// Генератор случайных чисел
        /// </summary>
        protected Random random;
        
        /// <summary>
        /// Массив нейронов
        /// </summary>
        public Neuron[,] Neurons { get; protected set; } = new Neuron[Settings.NumNeuronLayers, Settings.NeuronsInLayer];

        /// <summary>
        /// Вывод работы нейронной сети
        /// </summary>
        public BrainOutput Output
        {
            get
            {
                BrainOutput Out = new BrainOutput();
                for (int i = 0; i < Settings.NeuronsInLayer; i++)
                    Out.fields[i] = Convert.ToInt32(Neurons[Settings.NumNeuronLayers-1, i].Value);
                return Out;
            }
        }

        /// <summary>
        /// Создание новой нейронной сети
        /// </summary>
        public NeuralNet(Random randomV)
        {
            random = randomV;
            for (int i = 0; i < Settings.NumNeuronLayers; i++)
                for (int j = 0; j < Settings.NeuronsInLayer; j++)
                    Neurons[i, j] = new Neuron(random);
            
            for (int i = 0; i < Settings.NeuronsInLayer; i++)
            {
                Neurons[0, i].Type = NeuronType.input;
                Neurons[Settings.NumNeuronLayers - 1, i].Type = NeuronType.output;
            }

            foreach (Neuron neuron in Neurons)
                neuron.Memory = 0f;
        }

        /// <summary>
        /// Копирование нейронной сети родителя
        /// </summary>
        /// <param name="Parent">Родитель</param>
        public NeuralNet(NeuralNet Parent)
        {
            random = Parent.random;
            for (int i = 0; i < Settings.NumNeuronLayers; i++)
                for (int j = 0; j < Settings.NeuronsInLayer; j++)
                    Neurons[i, j] = Parent.Neurons[i, j].Copy();

            for (int i = 0; i < Settings.NeuronsInLayer; i++)
            {
                Neurons[0, i].Type = NeuronType.input;
                Neurons[Settings.NumNeuronLayers - 1, i].Type = NeuronType.output;
            }

            foreach (Neuron neuron in Neurons)
                neuron.Memory = 0f;
        }

        /// <summary>
        /// Основной процесс принятия решения нейронной сетью
        /// </summary>
        public void Process()
        {
            for (int i = 1; i < Settings.NumNeuronLayers; i++)
                for (int j = 0; j < Settings.NeuronsInLayer; j++)
                    Neurons[i, j].Value = 0f;


                    // активация нейронов входного и скрытых слоёв
                   
            for (int i = 0; i < Settings.NumNeuronLayers - 1; i++)
                for (int j = 0; j < Settings.NeuronsInLayer; j++)
                {
                    Neuron neuron = Neurons[i, j];
                    neuron.Activation();
                    for (int Connection = 0; Connection < neuron.Connections; Connection++)
                    {
                        if (neuron.allConnections[i].Num == -1) continue;
                        Neurons[i + 1, neuron.allConnections[i].Num].Value += neuron.Value * neuron.allConnections[i].Weight;
                    }
                }

            // Активация выходных нейронов
            int Output = Settings.NumNeuronLayers - 1;

            // активация нейрона выходного слоя, ответственного за вращение
            Neurons[Output, 0].RotateActivation();

            // Активация остальных нейронов
            for (int i = 1; i < Settings.NeuronsInLayer; i++)
                Neurons[Output, i].Activation();
        }

        /// <summary>
        /// Принять решение на основе входных данных и памяти
        /// </summary>
        /// <param name="input">Входные данные</param>
        /// <returns></returns>
        public BrainOutput MakeChoice(BrainInput input)
        {
            Neurons[0, 0].Value = input.Energy;
            Neurons[0, 1].Value = input.Vision;
            Neurons[0, 2].Value = input.isRelative;
            Neurons[0, 3].Value = input.Rotation;
            Neurons[0, 4].Value = input.Height;

            Process();

            BrainOutput output = new BrainOutput(true);
            for (int i = 0; i < Settings.NeuronsInLayer; i++)
                output.fields[i] = Convert.ToInt32(Neurons[Settings.NumNeuronLayers - 1, i].Value);
            return output;
        }

        /// <summary>
        /// Слабая мутация. 
        /// Один из нейронов слегка изменяется, не меняя типа
        /// </summary>
        public void MutateSlightly()
        {
            int Layer = random.Next(Settings.NumNeuronLayers);  
            int N = random.Next(Settings.NeuronsInLayer);
            Neurons[Layer, N].SlightlyChange();
        }

        /// <summary>
        /// Единичная мутация. Один из нейронов меняется полностью. 
        /// Если случай попадает на входной или выходной нейроны, то они меняются частично, тип остаётся неизменным.
        /// </summary>
        public void Mutate()
        {
            int Layer = random.Next(Settings.NumNeuronLayers);
            int N = random.Next(Settings.NeuronsInLayer);
            Neurons[Layer, N].Randomize();
        }

        /// <summary>
        /// Сильная мутация.
        /// Половина нейронов меняется полностью
        /// </summary>
        public void MutateHarsh()
        {
            Randomize(0.5f);
        }

        /// <summary>
        /// Полная мутация.
        /// Все нейроны меняются на случайные
        /// </summary>
        /// <param name="chance">Шанс изменения. 
        /// 0f - ничего не меняется
        /// 1f - все меняются.</param>
        public void Randomize(float chance=1f)
        {
            foreach (Neuron neuron in Neurons)
                neuron.Randomize(chance);
        }

        /// <summary>
        /// Сделать заглушку
        /// </summary>
        public void SetDummy()
        {
            Neurons[Settings.NumNeuronLayers - 1, 2].bias = 1f;
            for (int i = 0; i < Settings.NumNeuronLayers - 1; i++)
                Neurons[i, 3].Tunnel(3);
        }
    }
}

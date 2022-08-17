using System;

namespace NeuroBot
{
    public struct NeuronConnection
    {
        public int Num;
        public float Weight;

        public NeuronConnection(bool Set = true)
        {
            Num = -1;
            Weight = 0;
        }
    }

    public enum NeuronType
    {
        basic,          // Базовый нейрон
        input,          // Входной нейрон
        output,         // Выходной нейрон
        random,         // Генератор случайных чисел
        radialbasis,    // Радиальный базис
        memory          // Память
    }
    
    public class Neuron
    {
        /// <summary>
        /// Тип нейрона
        /// </summary>
        private NeuronType type = NeuronType.basic;

        /// <summary>
        /// Сдвиг нейрона
        /// </summary>
        public float bias = 0.0f;

        /// <summary>
        /// Количество связей нейрона
        /// </summary>
        public int Connections { get; protected set; } = 0;

        /// <summary>
        /// Связи нейрона
        /// </summary>
        public NeuronConnection[] allConnections { get; protected set; } = new NeuronConnection[Settings.NeuronsInLayer];

        /// <summary>
        /// Генератор случайных чисел
        /// </summary>
        private Random random = new Random();

        /// <summary>
        /// Текущее значение нейрона
        /// </summary>
        public float Value = 0;

        /// <summary>
        /// Текущее значение памяти нейрона
        /// </summary>
        public float Memory = 0;

        public NeuronType Type { get => type; set => type = value; }

        public Neuron(Random rnd)
        {
            random = rnd;
        }

        /// <summary>
        /// Добавляет новую связь
        /// </summary>
        /// <param name="Num">Номер связонного нейрона</param>
        /// <param name="Weight">Вес связи</param>
        public void AddConnection(int Num, float Weight)
        {
            allConnections[Connections].Num = Num;
            allConnections[Connections++].Weight = Weight;    
        }

        /// <summary>
        /// Проверяет, есть ли связь по индексу
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public bool IsConnected(int index)
        {
            foreach (NeuronConnection connection in allConnections)
                if (connection.Num == index) return true;
            return false;
        }

        /// <summary>
        /// Задаёт случайное смещение нейрона в диапазоне [-2, +2]
        /// </summary>
        public void SetRandomBias()
        {
            bias = RandomFloat(2f);
        }

        /// <summary>
        /// Установка случайного типа нейрона
        /// </summary>
        public void SetRandomType()
        {
            if (type == NeuronType.input) return;
            if (type == NeuronType.output) return;

            int RND = random.Next(100);
            if (RND < Settings.RND_Neuron_Persent)
            {
                type = NeuronType.random;
                return;
            }
            if (RND < Settings.RND_Basic_Neuron_Persent)
            {
                type = NeuronType.basic;
                return;
            }
            if (RND < Settings.RND_Basic_RBasis_Neuron_Persent)
            {
                type = NeuronType.radialbasis;
                return;
            }
            type = NeuronType.memory;
        }

        protected void ResetConnections()
        {
            Connections = 0;
            for (int i = 0; i < allConnections.Length; i++)
            {
                allConnections[i].Num = -1;
                allConnections[i].Weight = 0f;
            }
        }

        /// <summary>
        /// Создаёт случайные связи с нейронами следующего слоя
        /// </summary>
        public void SetRandomConnections()
        {
            ResetConnections();

            if (type == NeuronType.output) return;

            int ToConnect = random.Next(Settings.MaxConnectionsPerNeuron + 1);

            /*
             * Потенциально удленняет время принятия одного решения до бесконечности.
             * 
             * В случае проблемы переписать по аналогии с Chess
             * */

            while (ToConnect > 0)
            {
                int Index = random.Next(Settings.NeuronsInLayer);
                if (IsConnected(Index)) continue;

                AddConnection(Index, RandomFloat(2f));
                ToConnect--;
            }
        }

        /// <summary>
        /// Делает нейпрон полностью случайным
        /// </summary>
        public void Randomize(float chance = 1f)
        {
            if (chance < 1f)
                if (random.NextDouble() > chance) return;
            SetRandomBias();
            SetRandomType();
            SetRandomConnections();
        }

        /// <summary>
        /// Полное обнуление нейрона
        /// </summary>
        public void Reset()
        {
            ResetConnections();
            bias = 0f;
        }

        /// <summary>
        /// Туннелирование. Сигнал уходит к одному следующему нейрону без изменений
        /// </summary>
        /// <param name="Index">Номер нейрона в следующем слое</param>
        public void Tunnel(int Index)
        {
            bias = 0.0f;
            ResetConnections();
            Connections = 1;
            allConnections[0].Weight = 1.0f;
            allConnections[0].Num = Index;
        }

        /// <summary>
        /// Слегка изменить. Изменяет сдвиг до +- 0.05, а связи до +- 0.1
        /// </summary>
        public void SlightlyChange()
        {
            bias += RandomFloat(0.05f);
            for (int i = 0; i < Connections; i++)
                allConnections[i].Weight += RandomFloat(0.1f);
        }


        /// <summary>
        /// Выдаёт случайное дробное значение в диапазоне [-range, +range]
        /// </summary>
        /// <param name="range">Предел случайности</param>
        /// <returns></returns>
        protected float RandomFloat(float range)
        {
            return (float)random.NextDouble() * range * 2 - range;
        }

        /// <summary>
        /// Базовая функция активации нейрона
        /// </summary>
        /// <param name="InputValue">Значение для активации</param>
        /// <returns></returns>
        protected float BasicActivation(float InputValue)
        {
            return InputValue >=0.5f
                ? 1f
                : 0f;
        }

        /// <summary>
        /// Функция активации нейрона для значений со знаком
        /// </summary>
        /// <param name="InputValue"></param>
        /// <returns></returns>
        protected float FullRangeActivation(float InputValue)
        {
            if (InputValue >= 0.5f)
                return 1.0f;
            if (InputValue <= -0.5f)
                return -1.0f;
            return 0f;
        }

        /// <summary>
        /// Функция активации радиального базиса
        /// </summary>
        /// <param name="InputValue"></param>
        /// <returns></returns>
        protected float RadialBasisActivation(float InputValue)
        {
            if (InputValue < 0.45f)
                return 0.0f;
            if (InputValue > 0.55f)
                return 0.0f;
            return 1.0f;
        }

        public void Activation()
        {
            if (Connections == 0) return;

            switch(type)
            {
                case NeuronType.basic:
                case NeuronType.output:
                    Value = BasicActivation(Value + bias);
                    break;

                case NeuronType.random:
                    Value = random.NextDouble() <= Value + bias ? 1f : 0f;
                    break;

                case NeuronType.input:
                    Value += bias;
                    break;

                case NeuronType.radialbasis:
                    Value = RadialBasisActivation(Value + bias);
                    break;

                case NeuronType.memory:
                    if (Value + bias <= -0.5f)
                        Memory = 0f;
                    if (Value + bias >= 0.5f)
                    {
                        Memory += BasicActivation(Value + bias);
                        if (Memory > 5f)
                            Memory = 5f;
                    }

                    Value = Memory;
                    break;
            }
        }

        /// <summary>
        /// Активация выводного нейрона на поворот
        /// </summary>
        public void RotateActivation()
        {
            Value = FullRangeActivation(Value + bias);
        }

        /// <summary>
        /// Создание нового экземпляра нейронной сети со структурой имеющегося
        /// </summary>
        /// <returns></returns>
        public Neuron Copy()
        {
            Neuron neuron = new Neuron(random);
            neuron.type = type;
            neuron.bias = bias;
            for (int i = 0; i < allConnections.Length; i++)
                neuron.AddConnection(allConnections[i].Num, allConnections[i].Weight);

            return neuron;
        }
    }
}

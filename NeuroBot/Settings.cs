namespace NeuroBot
{
    public static class Settings
    {
        // Настройки нейронов и сети
        /// <summary>
        /// Количество нейронов в слое
        /// </summary>
        public const int NeuronsInLayer = 5;

        /// <summary>
        /// Максимальное количество связей в нейроне
        /// </summary>
        public const int MaxConnectionsPerNeuron = 3;

        /// <summary>
        /// Количество слоёв нейронов
        /// </summary>
        public const int NumNeuronLayers = 6;

        /// <summary>
        /// Относительное количество нейронов типа random
        /// </summary>
        public const int RND_Neuron_Count = 2;

        /// <summary>
        /// Относительное количество нейронов типа basic
        /// </summary>
        public const int Basic_Neuron_Count = 58;

        /// <summary>
        /// Относительное количество нейронов типа radialbasis
        /// </summary>
        public const int RBasis_Neuron_Count = 35;

        /// <summary>
        /// Относительное количество нейронов типа memory
        /// </summary>
        public const int Memory_Neuron_Count = 5;

        /// <summary>
        /// Общее количество долей нейронов
        /// </summary>
        public const int All_Neuron_Count = RND_Neuron_Count + Basic_Neuron_Count + RBasis_Neuron_Count + Memory_Neuron_Count;

        /// <summary>
        /// Процент нейронов типа random
        /// </summary>
        public const float RND_Neuron_Persent = (float)RND_Neuron_Count / (float)All_Neuron_Count * 100f;

        /// <summary>
        /// Процент нейронов типа random и basic
        /// </summary>
        public const float RND_Basic_Neuron_Persent = (float)(RND_Neuron_Count + Basic_Neuron_Count) / (float)All_Neuron_Count * 100f;

        /// <summary>
        /// Процент нейронов типа random, basic и radialbasis
        /// </summary>
        public const float RND_Basic_RBasis_Neuron_Persent = (float)(RND_Neuron_Count + Basic_Neuron_Count + RBasis_Neuron_Count) / (float)All_Neuron_Count * 100f;

        /// <summary>
        /// Размер визуальизации нейрона по вертикали
        /// </summary>
        public const double NeuronHeight = 100;

        /// <summary>
        /// Размер визуальизации нейрона по горизонтали
        /// </summary>
        public const double NeuronWidth = 100;
    }
}

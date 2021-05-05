using System;
using System.Collections.Generic;
using System.Text;

namespace VoskEngine
{
    class NeuralNetwork
    {
        private int[] layers;
        private int[] activations;
        private float[][] neurons;
        private float[][] biases;
        private float[][][] weights;

        public float fitness = 0;


        public NeuralNetwork(int[] _layers) //
        {
            this.layers = new int[_layers.Length];
            for (int i = 0; i < _layers.Length; i++)
            {
                this.layers[i] = _layers[i];
            }
            InitNeurons();
            InitBiases();
            InitWeights();
        }


        /// <summary>
        /// /Creates empty storage array for the neurons, 
        /// it creates list first due to array being static in length.
        /// </summary>
        private void InitNeurons() {
            List<float[]> neuronList = new List<float[]>();

            for (int i = 0; i < layers.Length; i++) {
                neuronList.Add(new float[layers[i]]);
            }
            neurons = neuronList.ToArray();
        }

        /// <summary>
        /// The bias and neurons size are the same. When initializing biases they need 
        /// to be populated, For a start, the biase is set to fall inbetween "(-0.5) and 0.5"
        /// This might be furhter improved with finding another way to "set the initial bias"
        /// </summary>
        private void InitBiases() {
            List<float[]> biasList = new List<float[]>();
            Random rnd = new Random();

            for (int i = 0; i < layers.Length; i++) {
                float[] bias = new float[layers[i]];
                for (int j = 0; j < layers[i]; j++) {

                    bias[j] = rnd.Next(-1, 1)/2; //I want a random value inbetween "(-0.5) and 0.5" but this might need some tuneing
                }
                biasList.Add(bias);
            }
            biases = biasList.ToArray();
        }

        private void InitWeights()
        {
            List<float[][]> weightList = new List<float[][]>();
            Random rnd = new Random();

            for (int i = 0; i < layers.Length; i++) {
                List<float[]> layerWeightsList = new List<float[]>();
                int neuronsInPrevoiusLayer = layers[i - 1];

                for (int j = 0; j < neurons[i].Length; j++) {
                    float[] neuronWeights = new float[neuronsInPrevoiusLayer];

                    for (int k = 0; k < neuronsInPrevoiusLayer; k++) {
                        neuronWeights[k] = rnd.Next(-1, 1) / 2; //I want a random value inbetween "(-0.5) and 0.5" but this might need some tuneing
                    }
                    layerWeightsList.Add(neuronWeights);
                }
                weightList.Add(layerWeightsList.ToArray());
            }
            weights = weightList.ToArray();
        }
    }
}

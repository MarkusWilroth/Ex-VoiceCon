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

        public double ihWeight00;
        public double ihWeight01;
        public double ihWeight10;
        public double ihWeight11;
        public double ihBias0;
        public double ihBias1;

        public double ihSum0;
        public double ihSum1;
        public double ihResult0;
        public double ihResult1;

        public double hoWeight00;
        public double hoWeight01;
        public double hoWeight10;
        public double hoWeight11;
        public double hoBias0;
        public double hoBias1;

        public double hoSum0;
        public double hoSum1;
        public double hoResult0;
        public double hoResult1;


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


    //holds the activation function (Softmax) for the output layer
    /*   f(z)i = e^zi / Σ(k->k)=1ezk   */
    //Sets the scale and maxvalue based on which part of the output layers is calling
    public double Activation(double x, string layer)
    {
        float result;

        double max = double.MinValue;
        //if (layer == "ih")
        //    max = (ihSum0 > ihSum1) ? ihSum0 : ihSum1;
        //else if (layer == "ho")
        //    max = (hoSum0 > hoSum1) ? hoSum0 : hoSum1;

        //double scale = 0.0;
        //if (layer == "ih")
        //    scale = Math.Exp(ihSum0 - max) + Math.Exp(ihSum1 - max);
        //else if (layer == "ho")
        //    scale = Math.Exp(hoSum0 - max) + Math.Exp(hoSum1 - max);

        //return Math.Exp(x - max) / scale;

    }

    //holds the activation function (Relu) for the hidden layer 
    /*    f(z)i=max(0,zi)    */
    public float HiddenActivation()
    {
        float result;


        return 0.0f; //fix
    } 
}

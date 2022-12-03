using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

namespace Pub
{
    // create a neural network class
    public class NeuralNetwork
    {
        public int[] Layers { get; set; }
        public double[][] Neurons { get; set; }
        public double[][][] Weights { get; set; }
        public double[][] Biases { get; set; }
        public double[][] Errors { get; set; }
        public double[][] Gradients { get; set; }
        public double[][][] WeightsDeltas { get; set; }
        public double[][] BiasesDeltas { get; set; }
        public double Error { get; set; }
        public double Rate { get; set; }
        public double Momentum { get; set; }

        public NeuralNetwork(int[] layers, double rate = 0.1, double momentum = 0.1)
        {
            Layers = new int[layers.Length];
            for (int i = 0; i < layers.Length; i++)
            {
                Layers[i] = layers[i];
            }

            Rate = rate;
            Momentum = momentum;

            InitNeurons();
            InitWeights();
            InitBiases();
            InitErrors();
            InitGradients();
            InitWeightsDeltas();
            InitBiasesDeltas();
        }

        private void InitNeurons()
        {
            List<double[]> neurons = new List<double[]>();
            for (int i = 0; i < Layers.Length; i++)
            {
                neurons.Add(new double[Layers[i]]);
            }
            Neurons = neurons.ToArray();
        }

        private void InitWeights()
        {
            List<double[][]> weights = new List<double[][]>();
            for (int i = 1; i < Layers.Length; i++)
            {
                List<double[]> layerWeights = new List<double[]>();
                int neuronsInPreviousLayer = Layers[i - 1];
                for (int j = 0; j < Neurons[i].Length; j++)
                {
                    double[] neuronWeights = new double[neuronsInPreviousLayer];
                    for (int k = 0; k < neuronsInPreviousLayer; k++)
                    {
                        neuronWeights[k] = GetRandomWeight();
                    }
                    layerWeights.Add(neuronWeights);
                }
                weights.Add(layerWeights.ToArray());
            }
            Weights = weights.ToArray();
        }

        private void InitBiases()
        {
            List<double[]> biases = new List<double[]>();
            for (int i = 1; i < Layers.Length; i++)
            {
                double[] layerBiases = new double[Neurons[i].Length];
                for (int j = 0; j < Neurons[i].Length; j++)
                {
                    layerBiases[j] = GetRandomWeight();
                }
                biases.Add(layerBiases);
            }   
            Biases = biases.ToArray();
        }   

        private void InitErrors()
        {
            List<double[]> errors = new List<double[]>();
            for (int i = 0; i < Layers.Length; i++)
            {
                errors.Add(new double[Layers[i]]);
            }
            Errors = errors.ToArray();
        }   

        private void InitGradients()
        {
            List<double[]> gradients = new List<double[]>();
            for (int i = 0; i < Layers.Length; i++)
            {
                gradients.Add(new double[Layers[i]]);
            }
            Gradients = gradients.ToArray();
        }

        private void InitWeightsDeltas()
        {
            List<double[][]> weightsDeltas = new List<double[][]>();
            for (int i = 1; i < Layers.Length; i++)
            {
                List<double[]> layerWeightsDeltas = new List<double[]>();
                for (int j = 0; j < Neurons[i].Length; j++)
                {
                    double[] neuronWeightsDeltas = new double[Neurons[i - 1].Length];
                    layerWeightsDeltas.Add(neuronWeightsDeltas);
                }
                weightsDeltas.Add(layerWeightsDeltas.ToArray());
            }
            WeightsDeltas = weightsDeltas.ToArray();
        }

        private void InitBiasesDeltas()
        {
            List<double[]> biasesDeltas = new List<double[]>();
            for (int i = 1; i < Layers.Length; i++)
            {
                double[] layerBiasesDeltas = new double[Neurons[i].Length];
                biasesDeltas.Add(layerBiasesDeltas);
            }
            BiasesDeltas = biasesDeltas.ToArray();
        }

        private double GetRandomWeight()
        {
            return 2 * (new Random().NextDouble() - 0.5);
        }

        public double[] FeedForward(double[] inputs)
        {
            for (int i = 0; i < inputs.Length; i++)
            {
                Neurons[0][i] = inputs[i];
            }

            for (int i = 1; i < Layers.Length; i++)
            {
                for (int j = 0; j < Neurons[i].Length; j++)
                {
                    double value = 0.0;
                    for (int k = 0; k < Neurons[i - 1].Length; k++)
                    {
                        value += Neurons[i - 1][k] * Weights[i - 1][j][k];
                    }
                    value += Biases[i - 1][j];
                    Neurons[i][j] = Sigmoid(value);
                }
            }

            return Neurons[Neurons.Length - 1];
        }

        public void BackPropagate(double[] expected)
        {
            CalculateOutputLayerErrors(expected);
            CalculateHiddenLayerErrors();
            UpdateWeights();
            UpdateBiases();
        }

        private void CalculateOutputLayerErrors(double[] expected)
        {
            for (int i = 0; i < Neurons[Neurons.Length - 1].Length; i++)
            {
                Errors[Errors.Length - 1][i] = expected[i] - Neurons[Neurons.Length - 1][i];
            }
        }

        private void CalculateHiddenLayerErrors()
        {
            for (int i = Errors.Length - 2; i > 0; i--)
            {
                for (int j = 0; j < Neurons[i].Length; j++)
                {
                    double error = 0.0;
                    for (int k = 0; k < Neurons[i + 1].Length; k++)
                    {
                        error += Errors[i + 1][k] * Weights[i][k][j];
                    }
                    Errors[i][j] = error;
                }
            }
        }

        private void UpdateWeights()
        {
            for (int i = Weights.Length - 1; i >= 0; i--)
            {
                for (int j = 0; j < Weights[i].Length; j++)
                {
                    for (int k = 0; k < Weights[i][j].Length; k++)
                    {
                        double delta = Rate * Neurons[i][k] * Errors[i + 1][j] + Momentum * WeightsDeltas[i][j][k];
                        Weights[i][j][k] += delta;
                        WeightsDeltas[i][j][k] = delta;
                    }
                }
            }
        }

        private void UpdateBiases()
        {
            for (int i = Biases.Length - 1; i >= 0; i--)
            {
                for (int j = 0; j < Biases[i].Length; j++)
                {
                    double delta = Rate * Errors[i + 1][j] + Momentum * BiasesDeltas[i][j];
                    Biases[i][j] += delta;
                    BiasesDeltas[i][j] = delta;
                }
            }
        }

        private double Sigmoid(double x)
        {
            return 1 / (1 + Math.Exp(-x));
        }

        private double SigmoidDerivative(double x)
        {
            return x * (1 - x);
        }
    }
  
}
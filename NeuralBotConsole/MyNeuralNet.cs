using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Accord.Math;

namespace NeuralBotConsole
{
    public interface INet
    {
        double[,] Forward(double[,] inputs);
    }

    public class MyNeuralNet : INet
    {
        private int[] Layers;

        public double[][,] Weights;
        

        public MyNeuralNet(int[] layersSize)
        {
            int numberOfLayers = layersSize.Length;
            Weights = new double[numberOfLayers-1][,];
            for (int i = 0; i < numberOfLayers -1; i++)
            {
                Weights[i] = new double[layersSize[i],layersSize[i+1]];
            }
            var r = new Random();
        }

        private static double costFunction(double[,] y, double[,] ry)
        {
            var subtracted = Elementwise.Subtract(y, ry);
            var differnce = ApplyFuncToEveryElement(subtracted, x => 0.5 * Math.Pow(x, 2));
            return Matrix.Sum(differnce);
        }

        /*public static double Sigmoid(double z)
        {
            return 1 / (1 + Math.Exp(-z));
        }*/

        public static double ReLu(double d)
        {
            return d > 0 ? d : 0;
        }

        /*public static double SigmoidPrime(double z)
        {
            return Math.Exp(-z) / Math.Pow((1 + Math.Exp(-z)), 2);
        }*/

        /*public static double[,] sigmoid(double[,] z)
        {
            for (int i = 0; i < z.GetLength(0); i++)
            {
                for (int j = 0; j < z.GetLength(1); j++)
                {
                    z[i, j] = Sigmoid(z[i, j]);
                }
            }
            return z;
        }*/

        private static double[,] ApplyFuncToEveryElement(double[,] z, Func<double, double> func)
        {
            for (int i = 0; i < z.GetLength(0); i++)
            {
                for (int j = 0; j < z.GetLength(1); j++)
                {
                    z[i, j] = func(z[i, j]);
                }
            }
            return z;
        }




        public double[,] Forward(double[,] inputs)
        {
            var a = inputs;
            for (int i = 0; i < Weights.Length; i++)
            {
                var w = Weights[i];
                a = Matrix.Dot(a, w);
                a = ApplyFuncToEveryElement(a, ReLu); //applying activation func to every layer?
            }
            return a;
        }
    }
}

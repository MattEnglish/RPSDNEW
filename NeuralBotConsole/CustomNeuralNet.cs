using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Accord.Math;

namespace NeuralBotConsole
{
    public class CustomNeuralNet
    {
        private int inputLayerSize = 5;
        private int outputLayerSize = 3;
        private int hiddenLayerSize = 4;

        public double[,] W1;
        public double[,] W2;

        private double[,] dJdW1;
        private double[,] dJdW2;

        public CustomNeuralNet()
        {
            W1 = new double[inputLayerSize, hiddenLayerSize];
            W2 = new double[hiddenLayerSize, outputLayerSize];
            var r = new Random();

            ApplyFuncToEveryElement(W1, x => r.NextDouble());
            ApplyFuncToEveryElement(W2, x => r.NextDouble());

        }

        private static double costFunction(double[,] y, double[,] ry)
        {
            var subtracted = Elementwise.Subtract(y, ry);
            var differnce = ApplyFuncToEveryElement(subtracted, x => 0.5 * Math.Pow(x, 2));
            return Matrix.Sum(differnce);
        }

        public static double Sigmoid(double z)
        {
            return 1 / (1 + Math.Exp(-z));
        }

        public static double SigmoidPrime(double z)
        {
            return Math.Exp(-z) / Math.Pow((1 + Math.Exp(-z)), 2);
        }

        public static double[,] sigmoid(double[,] z)
        {
            for (int i = 0; i < z.GetLength(0); i++)
            {
                for (int j = 0; j < z.GetLength(1); j++)
                {
                    z[i, j] = Sigmoid(z[i, j]);
                }
            }
            return z;
        }

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
            var x = Matrix.Dot(inputs, W1);
            var a = sigmoid(x);
            var y = Matrix.Dot(x, W2);
            var b = sigmoid(y);
            return b;
        }


        private void input(int[] x)
        {

        }
    }
}

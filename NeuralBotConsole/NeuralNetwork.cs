using Bots;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Accord.Neuro;
using Accord.Statistics;
using RPSDNEW;
using Accord.Neuro.Learning;

namespace NeuralBotConsole
{
    public class NeuralNetworkController
    {
        private ActivationNetwork network;
        private LevenbergMarquardtLearning teacher;

        public NeuralNetworkController()
        {
            network = new ActivationNetwork(new SigmoidFunction(), 5, 4, 3);
            network.Randomize();
            teacher = new LevenbergMarquardtLearning(network);

        }
        public probVector GetProbVector(double currentDrawStreak, double winsLeft, double enemyWinsLeft, double dynamiteLeft, double enemyDynamiteLeft)
        {
            var input = new double[] { currentDrawStreak / 5, winsLeft / 1000, enemyWinsLeft / 1000, dynamiteLeft / 100, enemyDynamiteLeft / 100};

            double[] output = network.Compute(input);

            return new probVector(output[0], output[1], output[2]);
        }

        public void Train(TrainingData data)
        {
            var inputs = data.Inputs.ToArray();
            var outputs = data.Outputs.ToArray();
            var teacher = new LevenbergMarquardtLearning(network);

            
            teacher.RunEpoch(inputs, outputs);

        }
    }
}

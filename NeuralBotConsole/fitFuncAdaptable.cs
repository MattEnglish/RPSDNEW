using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Accord.Genetic;
using Bots;
using RockPaperDynamiteEngine;

namespace NeuralBotConsole
{

    public static class GetNetHelper
    {
        public static MyNeuralNet GetNet(IChromosome chromosome, int[] layers)
        {
            
            var test = (DoubleArrayChromosome)chromosome;
            var value = test.Value;
            var net = new MyNeuralNet(layers);
            

            int geneNumber = 0;
            for (int layerIndex = 0; layerIndex < layers.Length-1; layerIndex++)
            {
                for (int i = 0; i < net.Weights[layerIndex].GetLength(0); i++)
                {
                    for (int j = 0; j < net.Weights[layerIndex].GetLength(1); j++)
                    {
                        net.Weights[layerIndex][i, j] = value[geneNumber];
                        geneNumber++;
                    }
                }
            }

            return net;
        }
    }

    public class fitFuncForAdaptable : IFitnessFunction
    {
        private static Random rand = new Random();
        private List<IBot> bots = new List<IBot>();

        private static int[] layers = {5,4,3};

        public fitFuncForAdaptable(List<IBot> bots)
        {
            this.bots = bots;
        }

       

        public double Evaluate(IChromosome chromosone)
        {
            double score = 0;
            var net = GetNetHelper.GetNet(chromosone, new int[]{5,4,3});
            var GameRunner = new GameRunnerWithData();
            foreach (var bot in bots)
            {
                var data = GameRunner.RunGame(bot, new NeuroEvolveBot(net, rand.Next(1, 256)));
                score += GeneralScore(data.P2WinCount, data.P1WinCount, data);
            }
            double scaling = (double)bots.Count();
            score = Math.Max(score+1,0.01)/ scaling;
            return Math.Exp(score * 3);

        }

        private double GeneralScore(int neuroWins, int otherWins, GameData data)
        {
            if(data.victory == Victory.unknown)
            {
                return -1;
            }
            if (neuroWins < 1000 && otherWins < 1000)
            {
                return -1 - (2000 - neuroWins - otherWins) / 1000.0;
            }

            if (neuroWins < 1000)
            {
                return (neuroWins - otherWins) / 200.0;
            }
            if (neuroWins >= 1000 && otherWins >= 900)
            {
                return (neuroWins - otherWins) / 100.0;
            }

            return 1;
        }
    }

    
}

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



    public class fitFunc : IFitnessFunction
    {
        private static Random rand = new Random();
        private List<IBot> bots = new List<IBot>();

        public fitFunc (List<IBot> bots)
        {
            this.bots = bots;
        }

        public static CustomNeuralNet GetNet(IChromosome chromosome)
        {
            var test = (DoubleArrayChromosome)chromosome;
            var value = test.Value;
            var net = new CustomNeuralNet();

            int geneNumber = 0;
            for (int i = 0; i < net.W1.GetLength(0); i++)
            {
                for (int j = 0; j < net.W1.GetLength(1); j++)
                {
                    net.W1[i, j] = value[geneNumber];
                    geneNumber++;
                }
            }

            for (int i = 0; i < net.W2.GetLength(0); i++)
            {
                for (int j = 0; j < net.W2.GetLength(1); j++)
                {
                    net.W2[i, j] = value[geneNumber];
                    geneNumber++;
                }
            }

            return net;
        }

        public double Evaluate(IChromosome chromosone)
        {
            double score = 0;
            var net = GetNet(chromosone);
            var GameRunner = new GameRunnerWithData();
            foreach (var bot in bots)
            {
                var data = GameRunner.RunGame(bot, new NeuroEvolveBot(net, rand.Next(1, 256)));
                score += GeneralScore(data.P2WinCount, data.P1WinCount, data);
            }
            double scaling = (double)bots.Count();
            return Math.Max(score+1,0.01)/ scaling;
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

    public class fitFuncTen : IFitnessFunction
    {
        private static Random rand = new Random();
        private List<IBot> bots = new List<IBot>();

        public fitFuncTen(List<IBot> bots)
        {
            this.bots = bots;
        }

        public static CustomNeuralNet GetNet(IChromosome chromosome)
        {
            var test = (DoubleArrayChromosome)chromosome;
            var value = test.Value;
            var net = new CustomNeuralNet();

            int geneNumber = 0;
            for (int i = 0; i < net.W1.GetLength(0); i++)
            {
                for (int j = 0; j < net.W1.GetLength(1); j++)
                {
                    net.W1[i, j] = value[geneNumber]/10.0;
                    geneNumber++;
                }
            }

            for (int i = 0; i < net.W2.GetLength(0); i++)
            {
                for (int j = 0; j < net.W2.GetLength(1); j++)
                {
                    net.W2[i, j] = value[geneNumber] / 10.0;
                    geneNumber++;
                }
            }

            return net;
        }

        public double Evaluate(IChromosome chromosone)
        {
            double score = 0;
            var net = GetNet(chromosone);
            var GameRunner = new GameRunnerWithData();
            foreach (var bot in bots)
            {
                var data = GameRunner.RunGame(bot, new NeuroEvolveBot(net, rand.Next(1, 256)));
                score += GeneralScore(data.P2WinCount, data.P1WinCount, data);
            }
            double scaling = (double)bots.Count();
            return Math.Max(score + 1, 0.01) / scaling;
        }

        private double GeneralScore(int neuroWins, int otherWins, GameData data)
        {
            if (data.victory == Victory.unknown)
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

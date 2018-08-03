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
        private static CustomNeuralNet GetNet(IChromosome chromosome)
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
            var data = GameRunner.RunGame(new WeWillRockYou(), new NeuroEvolveBot(net, rand.Next(1,256)));
            score += RockBotScore(data);
            data = GameRunner.RunGame(new DynamiteBot(), new NeuroEvolveBot(net, rand.Next(1, 256)));
            score += DynBotScore(data);
            if (data.victory == Victory.player2Victory)
            {
                //data = GameRunner.RunGame(new DrawBot(), new NeuralBot());
                //score += GeneralScore(data.P2WinCount, data.P1WinCount);
            }

            return Math.Max(score+1,0.01);
        }

        private double GeneralScore(int neuroWins, int otherWins)
        {
            if (neuroWins < 1000 && otherWins < 1000)
            {
                return -1 - (2000 - neuroWins - otherWins) / 1000.0;
            }

            if (neuroWins < 1000)
            {
                return -0.5;
            }
            if (neuroWins >= 1000)
            {
                return 0.5;
            }

            return 0.5;
        }

        private double RockBotScore(GameData data)
        {
            if (data.P2WinCount < 1000 & data.P1WinCount < 1000)
            {
                return -1 - (1000 - data.P2WinCount) / 1000.0;
            }
            if (data.P2WinCount >= 1000 && data.P2WinCount>=800)
            {
                return (1000 - data.P1WinCount) / 200.0;
            }
            if (data.P1WinCount >= 1000)
            {
                return (data.P2WinCount - data.P1WinCount) / 200.0;
            }
            return 1;
        }

        private double DynBotScore(GameData data)
        {
            if (data.P2WinCount < 1000 & data.P1WinCount < 1000)
            {
                return -1 - (1000 - data.P2WinCount) / 1000.0;
            }
            if (data.P1WinCount >= 1000 && data.P1WinCount >= 800)
            {
                return 0.25 + (data.P2WinCount-1000) / 200.0;
            }
            if (data.P2WinCount >= 1000 && data.P2WinCount >= 800)
            {
                return 0.5 + (1000 - data.P1WinCount) / 200.0;
            }
            if (data.P1WinCount >= 1000)
            {
                return 0.5 + (data.P2WinCount - data.P1WinCount) / 200.0;
            }
            return 2;
        }

        private double DrawBotScore(GameData data)
        {
            if (data.P2WinCount < 1000 & data.P1WinCount < 1000)
            {
                return -1 - (1000 - data.P2WinCount) / 1000.0;
            }
            if (data.P2WinCount >= 1000 && data.P2WinCount >= 800)
            {
                return 0.5 + (1000 - data.P1WinCount) / 200.0;
            }
            if (data.P1WinCount >= 1000)
            {
                return 0.5 + (data.P2WinCount - data.P1WinCount) / 200.0;
            }
            return 2;
        }
    }
}

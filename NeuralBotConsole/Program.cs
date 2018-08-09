using Bots;
using RockPaperDynamite;
using RockPaperDynamiteEngine;
using RPSDNEW;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Accord.Genetic;
using Accord.Math.Random;
using System.IO;

namespace NeuralBotConsole
{
    class Program
    {
        private static NeuroEvolveBot getBotGen(int genNumber, IRandomNumberGenerator<double> rand)
        {
            var chromosome = (DoubleArrayChromosome)GetChromosomeGen(genNumber, rand);
            var net = fitFunc.GetNet(chromosome);
            return new NeuroEvolveBot(net, genNumber, genNumber.ToString());
        }

        private static IChromosome GetChromosomeGen(int genNumber, IRandomNumberGenerator<double> rand)
        {
            var newChromeValues = File.ReadAllLines("Chromosome" + genNumber.ToString()).Select(s => double.Parse(s)).ToArray();
            return new DoubleArrayChromosome(rand, rand, rand, newChromeValues);
        }

        private static void ConvertChromeToNew(int genNumber, int newNumber)
        {
            var newChromeValues = File.ReadAllLines("Chromosome" + genNumber.ToString()).Select(s => double.Parse(s)).Select(d => d * 10).ToArray();
            System.IO.File.WriteAllLines("Chromosome" + newNumber.ToString(), newChromeValues.Select(d => d.ToString()));
        }

        private static void RunWaveTest()
        {
            var prevNeuralBots = new List<NeuroEvolveBot>();
            var rand = new ZigguratExponentialGenerator();
            for (int i = 20; i > 5; i--)
            {
                prevNeuralBots.Add(getBotGen(i, rand));
            }

            var leagueRunner = new LeagueRunner();

            foreach (var bot in prevNeuralBots)
            {
                var data = LeagueRunner.RunLeague(new WaveBot(), bot);
                Console.WriteLine(data);
            }
        }
        
        static void Main(string[] args)
        {
            RunWaveTest();


            int largestGen = 19;
            
  
                var prevNeuralBots = new List<NeuroEvolveBot>();
                var rand = new ZigguratExponentialGenerator();
                for (int i = 9; i < largestGen + 1; i++)
                {
                    prevNeuralBots.Add(getBotGen(i, rand));
                }


                var bots = new List<IBot> { new DrawBot() };
                bots.AddRange(prevNeuralBots);

                DoubleArrayChromosome doubleChromosome = (DoubleArrayChromosome)GetChromosomeGen(largestGen, rand);

                for (int i = 0; i < 2; i++)
                {


                    var pop = new Population(250, doubleChromosome, new fitFunc(bots), new EliteSelection());
                    Console.WriteLine(pop.FitnessMax);
                    for (int j = 0; j < 40; j++)
                    {
                        pop.RunEpoch();
                        Console.WriteLine(pop.FitnessMax);
                    }

                    var x = new fitFunc(bots).Evaluate(pop.BestChromosome);
                    doubleChromosome = (DoubleArrayChromosome)pop.BestChromosome;

                    var newNet = fitFunc.GetNet(doubleChromosome);
                    var newBot = new NeuroEvolveBot(newNet, 934 + i, "newBot" + i.ToString());

                    var gameRunner = new GameRunnerWithData();
                    GameData data;
                    foreach (var bot in bots)
                    {
                         data = gameRunner.RunGame(newBot, bot);
                    }
                    data = gameRunner.RunGame(newBot, new WaveBot());
                    bots.Add(newBot);
                    //if (i % 2 == 1)
                    //{
                    //    bots.RemoveAt(bots.Count - 2);
                    //}
                }


                string[] values = doubleChromosome.Value.Select(d => d.ToString()).ToArray();

                System.IO.File.WriteAllLines("Chromosome" + (largestGen + 1).ToString(), values);
                largestGen++;
            
        }
    }
}

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
        private static NeuroEvolveBot getBotGen(int genNumber, IRandomNumberGenerator<double> rand, NetworkType networkType)
        {
            var chromosome = (DoubleArrayChromosome)GetChromosomeGen(genNumber, rand, networkType);
            var net = GetNetHelper.GetNet(chromosome, new int[]{5,4,3});
            return new NeuroEvolveBot(net, genNumber, genNumber.ToString());
        }

        private static IChromosome GetChromosomeGen(int genNumber, IRandomNumberGenerator<double> rand, NetworkType networkType)
        {
            var newChromeValues = File.ReadAllLines("Chromosome" + networkType.fileName + genNumber.ToString()).Select(s => double.Parse(s)).ToArray();
            return new DoubleArrayChromosome(rand, rand, rand, newChromeValues);
        }

        private static void RunWaveTest()
        {
            var prevNeuralBots = new List<NeuroEvolveBot>();
            var rand = new ZigguratExponentialGenerator();
            for (int i = 2; i > 1; i--)
            {
                prevNeuralBots.Add(getBotGen(i, rand, GenTypes.ReLU));
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

            
            NetworkType chromType = GenTypes.ReLU;

            int largestGen = 1;
            var prevNeuralBots = new List<NeuroEvolveBot>();
            var rand = new ZigguratExponentialGenerator();
            /*
                            ;

                            for (int i = 9; i < largestGen + 1; i++)
                            {
                                prevNeuralBots.Add(getBotGen(i, rand, chromType));
                            }*/


            var bots = new List<IBot> { new WeWillRockYou(), new DrawBot() };
                bots.AddRange(prevNeuralBots);


                DoubleArrayChromosome doubleChromosome = new DoubleArrayChromosome(rand, rand, rand, 32);
                //DoubleArrayChromosome doubleChromosome = (DoubleArrayChromosome)GetChromosomeGen(largestGen, rand, chromType);

                for (int i = 0; i < 10; i++)
                {
                    var pop = new Population(100, doubleChromosome, new fitFunc(bots), new RouletteWheelSelection());
                    Console.WriteLine(pop.FitnessMax);
                    for (int j = 0; j < 10; j++)
                    {
                        pop.RunEpoch();
                        Console.WriteLine(pop.FitnessMax);
                        Console.WriteLine(pop.FitnessAvg);
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
                        Console.WriteLine(data);
                    }
                    data = gameRunner.RunGame(newBot, new WaveBot());
                    Console.WriteLine(data);
                bots.Add(newBot);
                    //if (i % 2 == 1)
                    //{
                    //    bots.RemoveAt(bots.Count - 2);
                    //}
                }


                string[] values = doubleChromosome.Value.Select(d => d.ToString()).ToArray();

                System.IO.File.WriteAllLines("Chromosome" + chromType + (largestGen + 1).ToString(), values);
                largestGen++;
            
        }
    }
}

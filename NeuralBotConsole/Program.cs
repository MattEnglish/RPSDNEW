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
        public static List<NeuroEvolveBot> GetBots( NetworkType networkType, IRandomNumberGenerator<double> rand)
        {
            var values = FileHandler.GetGenerations(networkType);
            List<NeuroEvolveBot> bots = new List<NeuroEvolveBot>();
            for (int i = 0; i < values.Count; i++)
            {
                var chrome = new DoubleArrayChromosome(rand, rand, rand, values[i]);
                var net = GetNetHelper.GetNet(chrome, new int[] { 5, 4, 3 });
                var bot = new NeuroEvolveBot(net, i, "NeuroEvolve" + " " + i);
                bots.Add(bot);

            }

            return bots;

        }


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
            var gens = GetBots(GenTypes.ReLU,rand);

            var leagueRunner = new LeagueRunner();

            foreach (var bot in gens)
            {
                var data = LeagueRunner.RunLeague(new WaveBot(), bot);
                Console.WriteLine(data);
            }
        }
        
        static void Main(string[] args)
        {

            
            NetworkType chromType = GenTypes.ReLU;

            var prevNeuralBots = new List<NeuroEvolveBot>();
            var genValues = FileHandler.GetGenerations(GenTypes.ReLU);
            var rand = new ZigguratExponentialGenerator();
            var Bots = GetBots(chromType, rand);
            prevNeuralBots.AddRange(Bots);

            
            
            /*
                            ;

                            for (int i = 0; i < largestGen + 1; i++)
                            {
                                prevNeuralBots.Add(getBotGen(i, rand, chromType));
                            }*/


            var bots = new List<IBot> { new DrawBot() };
                bots.AddRange(prevNeuralBots);

            var bestChromeValues = FileHandler.GetGenerations(chromType);
                DoubleArrayChromosome doubleChromosome = new DoubleArrayChromosome(rand, rand, rand, bestChromeValues[bestChromeValues.Count-1]);
                //DoubleArrayChromosome doubleChromosome = (DoubleArrayChromosome)GetChromosomeGen(largestGen, rand, chromType);

                for (int i = 0; i < 10; i++)
                {
                    var pop = new Population(200, doubleChromosome, new fitFuncForAdaptable(bots), new RouletteWheelSelection());
                    Console.WriteLine(pop.FitnessMax);
                    for (int j = 0; j < 10; j++)
                    {
                        pop.RunEpoch();
                        Console.WriteLine(pop.FitnessMax);
                        Console.WriteLine(pop.FitnessAvg);
                }

                    var x = new fitFuncForAdaptable(bots).Evaluate(pop.BestChromosome);
                    doubleChromosome = (DoubleArrayChromosome)pop.BestChromosome;

                    var newNet = GetNetHelper.GetNet(doubleChromosome, new[]{5,4,3});
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
                }

            double[] values = doubleChromosome.Value;

            FileHandler.WriteToLargestGen(chromType, values);
            RunWaveTest();
            
        }
    }
}

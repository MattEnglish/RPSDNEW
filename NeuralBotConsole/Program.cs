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

namespace NeuralBotConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var rand = new ZigguratExponentialGenerator();
            var chrom = new DoubleArrayChromosome(rand, rand, rand, 32);
            var pop = new Population(1000, chrom, new fitFunc(), new EliteSelection());
            Console.WriteLine(pop.FitnessMax);
            for (int i = 0; i < 200; i++)
            {
                pop.RunEpoch();
                Console.WriteLine(pop.FitnessMax);
            }
            var x = new fitFunc().Evaluate(pop.BestChromosome);

            Console.WriteLine(pop.FitnessMax);
        }
    }
}

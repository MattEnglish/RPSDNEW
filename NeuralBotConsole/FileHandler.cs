using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Accord.Statistics;


namespace NeuralBotConsole
{
    public static class FileHandler
    {
        private static IEnumerable<string> GetFileNames(NetworkType networkType)
        {
            var files = Directory.GetFiles(Directory.GetCurrentDirectory());//Must be a better way lol
            return files.Where(s => s.Contains(networkType.fileName));
        }

        public static List<double[]> GetGenerations(NetworkType networkType)
        {
            
            
            var chromosomes = new List<double[]>();

            foreach (var fileName in GetFileNames(networkType))
            {
                var chromosome = File.ReadAllLines(fileName)
                    .Select(double.Parse)
                    .ToArray();
                chromosomes.Add(chromosome);
            }

            return chromosomes;
        }

        public static void WriteToLargestGen(NetworkType networkType, double[] values)
        {
            string baseChromoFileName = "Chromosome" + networkType.fileName;
            int GenNumber = 1;
            while (File.Exists(baseChromoFileName + GenNumber) )
            {
                GenNumber++;
            }
            var strings = values.Select(d => d.ToString()).ToArray();

            System.IO.File.WriteAllLines(baseChromoFileName + GenNumber, strings);

        }
    }
}

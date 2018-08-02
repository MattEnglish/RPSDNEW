using Bots;
using RockPaperDynamite;
using RockPaperDynamiteEngine;
using RPSDNEW;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralBotConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Random rand = new Random();
            var gameRunner = new GameRunnerWithData();
            var trainRunner = new GameRunnerWithTrainingData();
            var neuralBot1 = new NeuralBot(1, "1");
            var neuralBot2 = new NeuralBot(2);
            var gameData = gameRunner.RunGame(new WeWillRockYou(), neuralBot1);
            Console.WriteLine(gameData.ToString());
            for (int i = 0; i < 10000; i++)
            {
                var data = trainRunner.RunGame(neuralBot1, neuralBot2);
                Console.WriteLine(data.gameData.ToString());

                for (int j = 0; j < 1; j++)
                {
                    var trainingData1 = neuralBot1.CollectedData;
                    var trainingData2 = neuralBot2.CollectedData;
                    if(data.gameData.victory == Victory.PlayersKeepDrawing)
                    {
                        throw new Exception();
                    }

                    if (data.gameData.victory == Victory.player1Victory)
                    {
                        if (data.gameData.P1WinCount < 1000)//Too many dynamites
                        {
                            foreach (var output in trainingData2.Outputs)
                            {
                                output[0] = 0.9 * output[0];
                            }
                        }
                        else if (data.gameData.P2WinCount>950)
                        {
                            foreach(var output in trainingData2.Outputs)
                            {
                                output[0] = (0.95 + 0.1*rand.NextDouble())*output[0];
                                output[1] = (0.95 + 0.1 * rand.NextDouble()) * output[1];
                                output[2] = (0.95 + 0.1 * rand.NextDouble()) * output[2];
                            }
                        }
                        else
                        {
                            trainingData2 = trainingData1;
                        }
                    }
                    if(data.gameData.victory == Victory.player2Victory)
                    {
                        trainingData1 = trainingData2;
                    }
                    neuralBot1.trainNetwork(trainingData1);
                    neuralBot2.trainNetwork(trainingData2);

                }

            }
            gameData = gameRunner.RunGame(new WeWillRockYou(), neuralBot1);
            Console.WriteLine(gameData.ToString());

            var leagueData = new LeagueData("", "");

            leagueData = LeagueRunner.RunLeague(new WeWillRockYou(), neuralBot1);
            Console.WriteLine(leagueData.ToString());
            Console.Read();
        }
    }
}

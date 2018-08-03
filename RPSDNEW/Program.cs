using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RockPaperDynamiteEngine;
using RockPaperDynamite;
using Bots;
using Bots.OrigWaveBot;
using Bots.WaveV2;
using Bots.WaveV3;
using Bots.WaveV4;
using Bots.WaveV5;

namespace RPSDNEW
{
    class Program
    {

        static void Main(string[] args)
        {
            RunGames();
        }
        static int RunGames()
        {
            
            var gameRunner = new GameRunnerWithData();
            var data = gameRunner.RunGame(new WaveBot(), new DynamiteBot());
            var leagueData = new LeagueData("", "");

            leagueData = LeagueRunner.RunLeague(new WaveBot(), new OrigWaveBot());
            Console.WriteLine(leagueData.ToString());
            leagueData = LeagueRunner.RunLeague(new WaveBotV4(), new WaveBot());
            Console.WriteLine(leagueData.ToString());
            leagueData = LeagueRunner.RunLeague(new WaveBotV5(), new WaveBot());
            Console.WriteLine(leagueData.ToString());
            leagueData = LeagueRunner.RunLeague(new WaveBot(), new WaveBotV4());
            Console.WriteLine(leagueData.ToString());

            leagueData = LeagueRunner.RunLeague(new WaveBot(), new DrawBot());
            Console.WriteLine(leagueData.ToString());
            leagueData = LeagueRunner.RunLeague(new WaveBot(), new WaveBotV3());
            Console.WriteLine(leagueData.ToString());
            leagueData = LeagueRunner.RunLeague(new WaveBot(), new WeWillRockYou());
            Console.WriteLine(leagueData.ToString());
            leagueData = LeagueRunner.RunLeague(new WaveBotV3(), new WaveBot());
            Console.WriteLine(leagueData.ToString());

            leagueData = LeagueRunner.RunLeague(new OrigWaveBot(), new WaveBot());
            Console.WriteLine(leagueData.ToString());
            leagueData = LeagueRunner.RunLeague(new OrigWaveBot(), new WaveBotV2());
            Console.WriteLine(leagueData.ToString());
            leagueData = LeagueRunner.RunLeague(new WeWillRockYou(), new WaveBot());
            Console.WriteLine(leagueData.ToString());
            leagueData = LeagueRunner.RunLeague(new ScaleBot(), new WaveBot());
            Console.WriteLine(leagueData.ToString());
            leagueData = LeagueRunner.RunLeague(new DrawBot(), new WaveBot());

            Console.WriteLine(leagueData.ToString());

            leagueData = LeagueRunner.RunLeague(new OrigWaveBot(), new ScaleBot());
            Console.WriteLine(leagueData.ToString());
            leagueData = LeagueRunner.RunLeague(new WaveBot(), new ScaleBot());
            Console.WriteLine(leagueData.ToString());
            leagueData = LeagueRunner.RunLeague(new OrigWaveBot(), new WaveBot());
            Console.WriteLine(leagueData.ToString());
            leagueData = LeagueRunner.RunLeague(new WaveBot(), new WaveBotV2());
            Console.WriteLine(leagueData.ToString());
            leagueData = LeagueRunner.RunLeague(new WaveBot(), new WaveBotV2());
            Console.WriteLine(leagueData.ToString());
            leagueData = LeagueRunner.RunLeague(new DrawBot(), new WaveBotV2());
            Console.WriteLine(leagueData.ToString());

            Console.Read();

            return 1;
        }

        

        private static async Task RunBattle(IBot bot1, IBot bot2)
        {
            await Task.Run(() =>
            {
                Console.WriteLine("New Game");
                var leagueData = new LeagueData("", "");

                leagueData = LeagueRunner.RunLeague(bot1, bot2);
                Console.WriteLine(leagueData.ToString());

                var a = 1;
            }
            );
        }

    }
}

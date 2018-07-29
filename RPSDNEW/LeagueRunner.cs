using System;
using System.Collections.Generic;
using System.Text;
using Bots;
using RockPaperDynamiteEngine;

namespace RockPaperDynamite
{
    public class LeagueRunner
    {
        public static LeagueData RunLeague( IBot bot1, IBot bot2, int NumberOfGames = 100)
        {
            var leagueData = new LeagueData(bot1.Name, bot2.Name);
            var gameRunner = new GameRunnerWithData();
            for (int i = 0; i < NumberOfGames; i++)
            {
                var victory = gameRunner.RunGame(bot1, bot2).victory;
                switch (victory)
                {
                    case Victory.player1Victory:
                        leagueData.BotOneVictoryCount++;
                        break;
                    case Victory.player2Victory:
                        leagueData.BotTwoVictoryCount++;
                        break;
                    default:
                        leagueData.BotDrawCount++;
                        break;
                }

            }
            return leagueData;
            
        }

        public static LeagueData RunQuickLeague(IBot bot1, IBot bot2, int NumberOfGames = 1000)
        {
            var leagueData = new LeagueData(bot1.Name, bot2.Name);
            for (int i = 0; i < NumberOfGames; i++)
            {
                var victory = GameRunnerQuick.RunGame(bot1, bot2);
                switch (victory)
                {
                    case Victory.player1Victory:
                        leagueData.BotOneVictoryCount++;
                        break;
                    case Victory.player2Victory:
                        leagueData.BotTwoVictoryCount++;
                        break;
                    default:
                        leagueData.BotDrawCount++;
                        break;
                }

            }
            return leagueData;

        }
    }
}

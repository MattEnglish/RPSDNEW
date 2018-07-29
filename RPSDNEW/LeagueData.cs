using System;
using System.Collections.Generic;
using System.Text;
using Bots;

namespace RockPaperDynamite
{
    public class LeagueData
    {
        public string Bot1Name;
        public string Bot2Name;
        public int BotOneVictoryCount = 0;
        public int BotTwoVictoryCount = 0;
        public int BotDrawCount = 0;

        public LeagueData(string bot1Name, string bot2Name)
        {
            Bot1Name = bot1Name;
            Bot2Name = bot2Name;
        }
        public override string ToString()
        {
            if (BotOneVictoryCount > BotTwoVictoryCount)
            {
                return Bot1Name + "Wins against " + Bot2Name +" " + BotOneVictoryCount.ToString() + ":" + BotTwoVictoryCount.ToString();
                    }
            if (BotOneVictoryCount < BotTwoVictoryCount)
            {
                return Bot2Name + "Wins against " + Bot1Name + " " + BotOneVictoryCount.ToString() + ":" + BotTwoVictoryCount.ToString();
            }
            return Bot2Name + "Draws against " + Bot1Name + " " + BotOneVictoryCount.ToString() + ":" + BotTwoVictoryCount.ToString();
        }

    }
}

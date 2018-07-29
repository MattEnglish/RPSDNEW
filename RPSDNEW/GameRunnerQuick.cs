using System;
using Bots;

namespace RockPaperDynamiteEngine
{
    public enum Victory { player1Victory, player2Victory, PlayersKeepDrawing, unknown };

    public class GameRunnerQuick
    {
        

        public static Victory RunGame(IBot bot1, IBot bot2)
        {
            int bot1DynamiteCount;
            int bot2DynamiteCount;
            int bot1WinCount;
            int bot2WinCount;
            int currentDrawStreak;
            const int WinsNeeded = 1000;
            const int MaxDynamites = 100;
            const int drawLimit = 1001;

            bot1DynamiteCount = 0;
            bot2DynamiteCount = 0;
            bot1WinCount = 0;
            bot2WinCount = 0;
            currentDrawStreak = 0;

            bot1.NewGame(bot2.Name);
            bot2.NewGame(bot1.Name);

            while(currentDrawStreak<drawLimit)
            {
                var w1 = bot1.GetNextWeaponChoice();

                if (w1 == Weapon.Dynamite)
                {
                    bot1DynamiteCount++;
                    if (bot1DynamiteCount > MaxDynamites)
                    {
                        return Victory.player2Victory;
                    }
                }

                var w2 = bot2.GetNextWeaponChoice();

                if (w2 == Weapon.Dynamite)
                {
                    bot2DynamiteCount++;
                    if (bot2DynamiteCount > MaxDynamites)
                    {
                        return Victory.player1Victory;
                    }
                }

                switch (new Battle(w1, w2).P1BattleResult)
                {
                    case BattleResult.Draw:

                        currentDrawStreak++;
                        bot1.HandleBattleResult(BattleResult.Draw,w1,w2);
                        bot2.HandleBattleResult(BattleResult.Draw,w2,w1);
                        break;

                    case BattleResult.Win:
                        bot1WinCount += 1 + currentDrawStreak;
                        currentDrawStreak = 0;
                        
                        if (bot1WinCount >= WinsNeeded)
                        {
                            return Victory.player1Victory;
                        }
                        bot1.HandleBattleResult(BattleResult.Win,w1,w2);
                        bot2.HandleBattleResult(BattleResult.Lose,w2,w1);
                        break;

                    case BattleResult.Lose:
                        bot2WinCount += 1 + currentDrawStreak;
                        currentDrawStreak = 0;

                        if (bot2WinCount >= WinsNeeded)
                        {
                            return Victory.player2Victory;
                        }
                        bot1.HandleBattleResult(BattleResult.Lose,w1,w2);
                        bot2.HandleBattleResult(BattleResult.Win,w2,w1);
                        break;
                }
            }

            return Victory.PlayersKeepDrawing;          
        }


    }

}

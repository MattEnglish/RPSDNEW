using System;
using System.Collections.Generic;
using System.Text;
using Bots;

namespace RockPaperDynamiteEngine
{
    public class GameDataController
    {
        public const int MaxDynamite = 100;
        public const int WinsNeeded = 1000;

        public GameData gameData { get; }

        public GameDataController(GameData gameData)
        {
            this.gameData = gameData;
        }

        public void NewBattle(Battle battle)
        {
            gameData.Battles.Add(battle);

            if (battle.P1Weapon == Weapon.Dynamite)
            {
                gameData.P1DynamiteUsed++;
                if (gameData.P1DynamiteUsed > MaxDynamite)
                {
                    gameData.victory = Victory.player2Victory;
                    gameData.VictoryReason = "p1 ran out of dynamites";
                }
            }

            if (battle.P2Weapon == Weapon.Dynamite)
            {
                gameData.P2DynamiteUsed++;
                if (gameData.P2DynamiteUsed > MaxDynamite)
                {
                    gameData.victory= Victory.player1Victory;
                    gameData.VictoryReason = "p2 ran out of dynamites";
                }
            }

            if(gameData.Battles.Count == 1000)
            {
                gameData.P1DynamiteAfter1000Battles = gameData.P1DynamiteUsed;
                gameData.P2DynamiteAfter1000Battles = gameData.P2DynamiteUsed;
            }


            switch (battle.P1BattleResult)
            {
                case BattleResult.Draw:

                    gameData.currentDrawStreak++;
                    break;

                case BattleResult.Win:
                    gameData.P1WinCount += gameData.currentDrawStreak + 1;
                    gameData.currentDrawStreak = 0;
                    if (gameData.P1WinCount >= WinsNeeded)
                    {
                        gameData.victory= Victory.player1Victory;
                    }
                    break;

                case BattleResult.Lose:
                    gameData.P2WinCount += gameData.currentDrawStreak + 1;
                    gameData.currentDrawStreak = 0;
                    if (gameData.P2WinCount >= WinsNeeded)
                    {
                        gameData.victory = Victory.player2Victory;
                    }
                    break;
            }
        }
    }
}

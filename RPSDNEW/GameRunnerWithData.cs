using System;
using System.Collections.Generic;
using System.Text;
using Bots;


namespace RockPaperDynamiteEngine
{
    public class GameRunnerWithData
    {
        public GameData RunGame(IBot bot1, IBot bot2)
        {
            const int drawLimit = 1000;

            GameData gameData = new GameData()
            {
                P1Name = bot1.Name,
                P2Name = bot2.Name
            };

            GameDataController gameDataController = new GameDataController(gameData);

            bot1.NewGame(bot2.Name);
            bot2.NewGame(bot1.Name);

            while(gameData.currentDrawStreak < drawLimit)
            { 
                Battle battle = new Battle(bot1.GetNextWeaponChoice(), bot2.GetNextWeaponChoice());
                gameDataController.NewBattle(battle);
                bot1.HandleBattleResult(battle.P1BattleResult,battle.P1Weapon,battle.P2Weapon);
                bot2.HandleBattleResult(battle.P2BattleResult, battle.P2Weapon, battle.P1Weapon);
                if(gameData.victory != Victory.unknown)
                {
                    return gameData;
                }
            }

            gameData.victory = Victory.PlayersKeepDrawing;
            gameData.VictoryReason = "Players have drawed for over :" + (drawLimit * 10).ToString();
            return gameData;
        }





    }
    
}

using System;
using System.Collections.Generic;
using System.Text;
using Bots;
using RPSDNEW;

namespace RockPaperDynamiteEngine
{
    public class GameRunnerWithTrainingData
    {


        public GameAndTrainingData RunGame(IBot bot1, IBot bot2)
        {
            const int drawLimit = 1000;

            TrainingData p1TrainingData = new TrainingData();
            TrainingData p2TrainingData = new TrainingData();

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

                if (gameData.currentDrawStreak > 0)
                {
                    p1TrainingData.AddInputs(gameData.currentDrawStreak, 1000 - gameData.P1WinCount, 1000 - gameData.P2WinCount, 100 - gameData.P1DynamiteUsed, 100 - gameData.P2DynamiteUsed);
                    p2TrainingData.AddInputs(gameData.currentDrawStreak, 1000 - gameData.P2WinCount, 1000 - gameData.P1WinCount, 100 - gameData.P2DynamiteUsed, 100 - gameData.P1DynamiteUsed);
                    p1TrainingData.AddOutputs(battle.P1Weapon);
                    p2TrainingData.AddOutputs(battle.P2Weapon);
                }

                gameDataController.NewBattle(battle);
                bot1.HandleBattleResult(battle.P1BattleResult,battle.P1Weapon,battle.P2Weapon);
                bot2.HandleBattleResult(battle.P2BattleResult, battle.P2Weapon, battle.P1Weapon);
                if(gameData.victory != Victory.unknown)
                {
                    return new GameAndTrainingData { gameData = gameData, P1TrainingData = p1TrainingData, P2TrainingData = p2TrainingData };
                }

                
            }

            gameData.victory = Victory.PlayersKeepDrawing;
            gameData.VictoryReason = "Players have drawed for over :" + (drawLimit * 10).ToString();
            return new GameAndTrainingData { gameData= gameData, P1TrainingData = p1TrainingData, P2TrainingData = p2TrainingData};
        }

        

    }
    
}

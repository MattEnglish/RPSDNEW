using System;
using System.Collections.Generic;
using System.Text;
using Bots;
using RPSDNEW;

namespace RockPaperDynamiteEngine
{
    public class GameTrainingController
    {
        public const int MaxDynamite = 100;
        public const int WinsNeeded = 1000;

        public TrainingData data { get; }

        public GameTrainingController(TrainingData trainData)
        {
            this.data = trainData;
        }
        /*
        public void AddInputs(GameData gameData)
        {
            data.AddInputs(gameData.currentDrawStreak, 1000 - gameData.P1WinCount, 1000 - gameData.P2WinCount, 100 - gameData.P1DynamiteUsed, 100 - gameData.P2DynamiteUsed);  
        }

        public void AddOutputs(Battle battle)
        {
            data.AddOutputs(battle.P1Weapon);
        }*/
    }
}

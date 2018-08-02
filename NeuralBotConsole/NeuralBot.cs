using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bots;
using RockPaperDynamiteEngine;
using RPSDNEW;

namespace NeuralBotConsole
{
    class NeuralBot : IBot
    {
        public string Name { get; }

        private double currentDrawStreak;
        private double MyWinsLeft;
        private double EnemyWinsLeft;
        private double DynamiteLeft;
        private double EnemyDynamiteLeft;
        private NeuralNetworkController network;
        private Random rand;
        public TrainingData CollectedData;


        public NeuralBot(int seed = 3, string name = "neuralNet")
        {
            rand = new Random(seed);
            network = new NeuralNetworkController();
            Name = name;
        }

        public void trainNetwork(TrainingData data)
        {
            
            network.Train(data);
        }

        public Weapon GetNextWeaponChoice()
        {
            if(currentDrawStreak == 0)
            {
                return (Weapon)rand.Next(3);
            }
            var probs = network.GetProbVector(currentDrawStreak, MyWinsLeft, EnemyWinsLeft, DynamiteLeft, EnemyDynamiteLeft);
            CollectedData.AddInputs(currentDrawStreak, MyWinsLeft, EnemyWinsLeft, DynamiteLeft, EnemyDynamiteLeft);
            CollectedData.Outputs.Add(new double[] { probs.dynProb, probs.watProb, probs.rpsProb });
            probs = probs.Normalise();
            var weaponChoice = rand.NextDouble();
            if (weaponChoice < probs.watProb)
            {
                return Weapon.WaterBallon;
            }
            if (weaponChoice < probs.watProb + probs.dynProb)
            {
                return Weapon.Dynamite;
            }

            return (Weapon)rand.Next(3);// returns rock, paper, scissors randomly.
        }
    

        public void HandleBattleResult(BattleResult result, Weapon yourWeapon, Weapon enemiesWeapon)
        {
            if(result == BattleResult.Draw)
            {
                currentDrawStreak++;
            }
            else
            {
                currentDrawStreak = 0;
            }
            if (result == BattleResult.Win)
            {
                MyWinsLeft -= currentDrawStreak + 1;
            }
            if (result == BattleResult.Lose)
            {
                EnemyWinsLeft -= currentDrawStreak + 1;
            }

            if (enemiesWeapon == Weapon.Dynamite)
            {
                EnemyDynamiteLeft--;
            }
            if(yourWeapon == Weapon.Dynamite)
            {
                DynamiteLeft--;
            } 
        }

        public void HandleFinalResult(bool isWin)
        {
            throw new NotImplementedException();
        }

        public void NewGame(string enemyBotName)
        {
        currentDrawStreak = 0;
        MyWinsLeft = 1000;
        EnemyWinsLeft = 1000;
        DynamiteLeft = 100;
        EnemyDynamiteLeft = 100;
        CollectedData = new TrainingData();
        }
    }
}

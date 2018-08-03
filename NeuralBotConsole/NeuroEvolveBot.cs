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
    class NeuroEvolveBot : IBot
    {
        public string Name { get; }

        private double currentDrawStreak;
        private double MyWinsLeft;
        private double EnemyWinsLeft;
        private double DynamiteLeft;
        private double EnemyDynamiteLeft;
        private CustomNeuralNet network;
        private Random rand;
        public TrainingData CollectedData;


        public NeuroEvolveBot(CustomNeuralNet net, int seed = 3, string name = "NeuroEvolve")
        {
            rand = new Random(seed);
            network = net;
            Name = name;
        }

        public Weapon GetNextWeaponChoice()
        {
            if(currentDrawStreak == 0)
            {
                return (Weapon)rand.Next(3);
            }
            var outputs = network.Forward(new double[1,5]{{currentDrawStreak, MyWinsLeft, EnemyWinsLeft, DynamiteLeft, EnemyDynamiteLeft}});
            var probs = new probVector(outputs[0,0], outputs[0,1], outputs[0,2]);
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

using System;
using System.Collections.Generic;
using System.Text;

namespace Bots
{
    public class DynamiteBot : IBot
    {
        public string Name => "DynamiteBot";
        private Random rand;
        private int currentDrawStreak = 0;
        private int dynamiteCounter = 0;
        private int enemyDynamiteCounter = 0;
        private int myWins = 0;
        private int enemyWins = 0;

        public DynamiteBot()
        {
            rand = new Random("Dynamaite".GetHashCode());

        }
        public Weapon GetNextWeaponChoice()
        {
            int approxWinsLeft = 2000 - 2 * Math.Max(myWins, enemyWins);
            double L = (double)approxWinsLeft / (Math.Pow(3, currentDrawStreak));//ApproxNumTimesSituationWillRepeat
            if (L < 100 - dynamiteCounter)
            {
                return Weapon.Dynamite;
            }
            return (Weapon)rand.Next(3);// returns rock, paper, scissors randomly.
        }

        public void HandleBattleResult(BattleResult result, Weapon yourWeapon, Weapon enemiesWeapon)
        {
            if (result == BattleResult.Win)
            {
                myWins += 1 + currentDrawStreak;
            }
            if (result == BattleResult.Lose)
            {
                enemyWins += 1 + currentDrawStreak;
            }
            if (yourWeapon == Weapon.Dynamite)
            {
                dynamiteCounter++;
            }
            if (enemiesWeapon == Weapon.Dynamite)
            {
                enemyDynamiteCounter++;
            }
            if (result == BattleResult.Draw)
            {
                currentDrawStreak++;
            }
            else
            {
                currentDrawStreak = 0;
            }
        }

        public void HandleFinalResult(bool isWin)
        {
            throw new NotImplementedException();
        }

        public void NewGame(string enemyBotName)
        {
            dynamiteCounter = 0;
            myWins = 0;
            enemyWins = 0;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bots
{
    public class ScaleBot : IBot
    {
        public string Name => "ScaleBot";

        private Random rand;
        private int currentDrawStreak = 0;
        private int dynamiteCounter = 0;
        private int enemyDynamiteCounter = 0;
        private int myWins = 0;
        private int enemyWins = 0;
        private Memory memory = new Memory();
        //private WaveMemory waveMemory = new WaveMemory();
        private static List<Weapon> enemyWeaponDrawStreakList = new List<Weapon>();
        private static List<Weapon> myWeaponDrawStreakList = new List<Weapon>();


        public ScaleBot()
        {
            rand = new Random(2);
            rand = new Random(rand.Next());
            rand = new Random(rand.Next());
            rand = new Random(rand.Next());
            rand = new Random(rand.Next());
            rand = new Random(rand.Next());
            rand = new Random(rand.Next());
            rand = new Random(rand.Next());
        }

        public Weapon GetNextWeaponChoice()
        {
            int approxWinsLeft = 2000 - 2 * Math.Max(myWins, enemyWins);

            double L = (double)approxWinsLeft / (Math.Pow(3, currentDrawStreak));//ApproxNumTimesSituationWillRepeat
            double d = -1 + (Math.Log((double)approxWinsLeft) - Math.Log(100 - dynamiteCounter)) / Math.Log(3);//Approx number Of Consecutive Draws Whereby dynamite can be thrown one third of the time.
            d = d * (1.0 - 4.0 / 10 + 5.0 * approxWinsLeft / 10000);//scale
            double v = 0.8 * Math.Max((Math.Floor(d) + 1.0), 0); //Very approx value of dynamite probably underestimate

            double ed = -1 + (Math.Log((double)approxWinsLeft) - Math.Log(100 - enemyDynamiteCounter)) / Math.Log(3);//Approx number Of Consecutive Draws Whereby dynamite can be thrown one third of the time.
            //ed = ed * (1.0 - 2.0 / 10 + 2.0 * approxWinsLeft / 10000);
            double ev = 0.8 * Math.Max((Math.Floor(ed) + 1.0), 0); //Very approx value of dynamite

            
            

            double pW = Math.Max((1 - 1 * ev / (currentDrawStreak + 1)) / 3.0, 0);//I don't understand why does 0.8 work better than 1. SOMETHING WRONG!!!!!!!!!!!!!!!!!!!!!
            pW = Math.Min(pW, 0.3);

            if(currentDrawStreak == 0)
            {
                pW = 0;
            }
            double pD = 0;
            double pRps = 0;
            if (currentDrawStreak > d)
            {
                pD = 0.33;
            }
            else if (currentDrawStreak == Math.Floor(d))
            {
                var a = (double)approxWinsLeft / (Math.Pow(3, currentDrawStreak + 1));//ApproxNumTimesSituationPlusOneDrawWillRepeat
                var b = 0.30 * a;//For whatever reason this works fine
                var c = Math.Max(100 - dynamiteCounter - b, 0);
                pD = c / L;
                pD = Math.Min(pD, 0.33);
            }
            else
            {
                pD = 0;
            }

            pRps = 1 - pD - pW;

            double totalValue = pD + pW + pRps;

            if (currentDrawStreak < Math.Floor(d))
            {
                pD = 0;
            }

            pD = pD / totalValue;
            pW = pW / totalValue;
            pRps = pRps / totalValue;

            var weaponChoice = rand.NextDouble();
            if (weaponChoice < pW)
            {
                return Weapon.WaterBallon;
            }
            if (weaponChoice < pW + pD)
            {
                return Weapon.Dynamite;
            }

            return (Weapon)rand.Next(3);// returns rock, paper, scissors randomly.
        }

        public void HandleBattleResult(BattleResult result, Weapon yourWeapon, Weapon enemiesWeapon)
        {
            if (currentDrawStreak > 2)
            {
                enemyWeaponDrawStreakList.Add(enemiesWeapon);
                myWeaponDrawStreakList.Add(yourWeapon);

            }
            int approxWinsLeft = 2000 - 2 * Math.Max(myWins, enemyWins);
            double ed = -1 + (Math.Log((double)approxWinsLeft) - Math.Log(100 - enemyDynamiteCounter)) / Math.Log(3);//Approx number Of Consecutive Draws Whereby dynamite can be thrown one third of the time.
            double ev = 0.8 * Math.Max((Math.Floor(ed) + 1.0), 0);//Very approx value of dynamite
            memory.addEnemyMove(currentDrawStreak, ev, enemiesWeapon);
            //waveMemory.addEnemyMove(currentDrawStreak, ev, enemiesWeapon);

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

            if (approxWinsLeft < 5)
            {
                var df = myWeaponDrawStreakList.Count(w => w == Weapon.Dynamite);
                var dfe = enemyWeaponDrawStreakList.Count(w => w == Weapon.Dynamite);
                var wf = myWeaponDrawStreakList.Count(w => w == Weapon.WaterBallon);
                var we = enemyWeaponDrawStreakList.Count(w => w == Weapon.WaterBallon);
                var rf = myWeaponDrawStreakList.Count(w => w == Weapon.Rock || w == Weapon.Paper || w == Weapon.Scissors);
                var re = enemyWeaponDrawStreakList.Count(w => w == Weapon.Rock || w == Weapon.Paper || w == Weapon.Scissors);
            }


        }

        public void HandleFinalResult(bool isWin)
        {
        }

        public void NewGame(string enemyBotName)
        {
            currentDrawStreak = 0;
            dynamiteCounter = 0;
            enemyDynamiteCounter = 0;
            myWins = 0;
            enemyWins = 0;

        }
    }
}

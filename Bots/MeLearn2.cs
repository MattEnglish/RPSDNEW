using System;

namespace Bots
{

    public class MemoryForStreak
    {
        public double dynValue = 0.333;
        public double watValue = 0.333;
        public double rpsValue = 0.333;


        public void AddMove(Weapon enemyWeapon)
        {


            if (enemyWeapon == Weapon.Dynamite)
            {
                watValue += 0.25;
            }
            else if (enemyWeapon == Weapon.WaterBallon)
            {
                rpsValue += 0.25;
            }
            else
            {
                dynValue += 0.25;
            }

            double totalValue = dynValue + watValue + rpsValue;

            dynValue = dynValue / totalValue;
            watValue = watValue / totalValue;
            rpsValue = rpsValue / totalValue;

        }
    }

    public class Memory
    {
        MemoryForStreak highMemory = new MemoryForStreak();
        MemoryForStreak medMemory = new MemoryForStreak();
        MemoryForStreak lowMemory = new MemoryForStreak();

        public void addEnemyMove(int drawStreak, double dynamiteValue, Weapon enemyWeapon)
        {
            if (drawStreak > dynamiteValue + 0.5)
            {
                highMemory.AddMove(enemyWeapon);
            }
            else if (drawStreak < dynamiteValue - 0.5)
            {
                lowMemory.AddMove(enemyWeapon);
            }
            else
            {
                medMemory.AddMove(enemyWeapon);
            }
        }

        public MemoryForStreak GetMemory(int drawStreak, double dynamiteValue)
        {
            if (drawStreak > dynamiteValue + 0.5)
            {
                return highMemory;
            }
            else if (drawStreak < dynamiteValue - 0.5)
            {
                return lowMemory;
            }
            else
            {
                return medMemory;
            }
        }
    }

    public class ShortTermMemory2 : IBot
    {
        public string Name => "ShortTermMemoryMULT";

        private Random rand;
        private int currentDrawStreak = 0;
        private int dynamiteCounter = 0;
        private int enemyDynamiteCounter = 0;
        private int myWins = 0;
        private int enemyWins = 0;
        private Memory memory = new Memory();



        public ShortTermMemory2()
        {
            rand = new Random();
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
            double v = 1.4 * Math.Max((Math.Floor(d) + 1.0), 0); //Very approx value of dynamite probably underestimate
            double ed = -1 + (Math.Log((double)approxWinsLeft) - Math.Log(100 - enemyDynamiteCounter)) / Math.Log(3);//Approx number Of Consecutive Draws Whereby dynamite can be thrown one third of the time.
            double ev = 1.4 * Math.Max((Math.Floor(ed) + 1.0), 0); //Very approx value of dynamite

            double pW = Math.Max((1 - 1 * ev / (currentDrawStreak + 1)) / 3.0, 0);//I don't understand why does 0.8 work better than 1. SOMETHING WRONG!!!!!!!!!!!!!!!!!!!!!
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


            var mem = memory.GetMemory(currentDrawStreak, ev);

            pD = Math.Max(0, pD * (mem.dynValue + 0.1));
            pW = Math.Max(0, pW * (mem.watValue + 0.1));
            pRps = Math.Max(0, pRps * (mem.rpsValue + 0.1));

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
            int approxWinsLeft = 2000 - 2 * Math.Max(myWins, enemyWins);
            double ed = -1 + (Math.Log((double)approxWinsLeft) - Math.Log(100 - enemyDynamiteCounter)) / Math.Log(3);//Approx number Of Consecutive Draws Whereby dynamite can be thrown one third of the time.
            double ev = 1.4 * Math.Max((Math.Floor(ed) + 1.0), 0);//Very approx value of dynamite
            memory.addEnemyMove(currentDrawStreak, ev, enemiesWeapon);


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

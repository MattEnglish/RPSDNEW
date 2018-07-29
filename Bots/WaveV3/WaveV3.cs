using System;
using System.Collections.Generic;
using System.Text;


namespace Bots.WaveV3
{
    public class probVector
    {
        public double dynProb;
        public double watProb;
        public double rpsProb;

        public probVector(double dynProb, double watProb, double rpsProb)
        {
            this.dynProb = dynProb;
            this.watProb = watProb;
            this.rpsProb = rpsProb;
        }

        public double getProbOfWeapon(Weapon weaponType)
        {
            if (weaponType == Weapon.Dynamite)
            {
                return dynProb;
            }
            else if (weaponType == Weapon.WaterBallon)
            {
                return watProb;
            }
            return rpsProb;
        }

        public void add(probVector p2)
        {
            this.dynProb += p2.dynProb;
            this.watProb += p2.watProb;
            this.rpsProb += p2.rpsProb;

            normalise();
        }

        public probVector getCounterMove()
        {
            return new probVector(
                rpsProb,
                dynProb,
                watProb);
        }

        private void normalise()
        {
            double totalValue = dynProb + watProb + rpsProb;
            dynProb = dynProb / totalValue;
            watProb = watProb / totalValue;
            rpsProb = rpsProb / totalValue;

            if (totalValue == 0)//Added this from js version not sure if neccessary
            {
                dynProb = 0.333;
                watProb = 0.333;
                rpsProb = 0.333;
            }
        }
    }

    public class WaveMemoryStreakPrediction
    {
        public probVector probVector;
        public double score;
    }
    public class DrawWaveMemory
    {
        List<WaveMemoryStreak> DrawMemories = new List<WaveMemoryStreak>();

        public void addEnemyMove(int drawStreak, Weapon enemyWeapon)
        {

        }

    }

    public class WaveMemory
    {
        WaveMemoryStreak highMemory = new WaveMemoryStreak();
        WaveMemoryStreak medMemory = new WaveMemoryStreak();
        WaveMemoryStreak lowMemory = new WaveMemoryStreak();

        public void addEnemyMove(int drawStreak, double dynamiteValue, Weapon enemyWeapon)
        {
            if (drawStreak > dynamiteValue + 0.5)
            {
                highMemory.AddMove(enemyWeapon);
            }
            else if (drawStreak < dynamiteValue - 1.5)
            {
                lowMemory.AddMove(enemyWeapon);
            }
            else
            {
                medMemory.AddMove(enemyWeapon);
            }
        }

        public WaveMemoryStreak GetMemory(int drawStreak, double dynamiteValue)
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
    public interface IWave
    {
        probVector getMoveProbability(int x);
    }

    public class DoubleWave : Wave
    {
        public Weapon MissingWeapon;
        public DoubleWave(double frequency, double phase, Weapon missingWeapon) : base(frequency, phase)
        { MissingWeapon = missingWeapon; }

        public override probVector getMoveProbability(int x)
        {
            double test = base.frequency * x + base.phase;
            if (MissingWeapon == Weapon.Dynamite)
            {
                return new probVector(0, cosProb(test), sinProb(test));
            }
            else if ((MissingWeapon == Weapon.WaterBallon))
            {
                return new probVector(sinProb(test), 0, cosProb(test));
            }
            else
            {
                return new probVector(sinProb(test), cosProb(test), 0);
            }
        }


    }



    public class Wave
    {
        protected double frequency;
        protected double phase;

        public Wave(double frequency, double phase)
        {
            this.frequency = frequency;
            this.phase = phase;
        }

        public virtual probVector getMoveProbability(int x)
        {
            double test = (x * frequency + phase + 12) % 3;
            if (test < 1)
            {
                return new probVector(cosProb(test), sinProb(test), 0);
            }
            else if (test < 2)
            {
                return new probVector(0, sinProb(test), cosProb(test));
            }
            else if (test < 3)
            {
                return new probVector(sinProb(test), 0, cosProb(test));
            }
            throw new Exception();

        }

        protected double sinProb(double a)
        {
            return Math.Pow(Math.Sin(a * Math.PI / 2), 2);
        }
        protected double cosProb(double a)
        {
            return Math.Pow(Math.Cos(a * Math.PI / 2), 2);
        }

    }

    public class WaveHolder
    {
        public WaveHolder(Wave wave, double score)
        {
            Wave = wave;
            Score = score;
        }

        public Wave Wave;
        public double Score;
    }

    public class WaveMemoryStreak
    {
        public List<WaveHolder> waves = new List<WaveHolder>();
        public List<Weapon> pastWeapons = new List<Weapon>();
        private const double decayRate = 0.8;
        int moveNumber = 0;

        public WaveMemoryStreak()
        {
            for (double frequency = -1.5; frequency < 1.5; frequency += 0.15)
            {
                for (double phase = -1.5; phase < 1.5; phase += 0.15)
                {
                    Wave wave = new Wave(frequency, phase);
                    waves.Add(new WaveHolder(wave, 0));
                }
            }
        }

        public void AddMove(Weapon enemyWeapon)
        {
            foreach (var wave in waves)
            {
                var prediction = wave.Wave.getMoveProbability(moveNumber);
                wave.Score = decayRate * wave.Score;
                wave.Score = wave.Score + prediction.getProbOfWeapon(enemyWeapon);
            }
            moveNumber++;
        }
        private WaveHolder getBestWave()
        {
            var biggestWaveHolder = new WaveHolder(new Wave(0, 0), 0);

            for (int i = 0; i < waves.Count; i++)
            {
                if (waves[i].Score > biggestWaveHolder.Score)
                {
                    biggestWaveHolder = waves[i];
                }
            }
            var x = biggestWaveHolder.Score;
            return biggestWaveHolder;
        }

        public probVector getNextShotProbValues()
        {
            var prob = getBestWave().Wave.getMoveProbability(moveNumber);

            //prob.add(new probVector(0.1, 0.1, 0.1));
            return prob.getCounterMove();
        }

        public WaveMemoryStreakPrediction getNextPrediction()
        {
            var biggestWave = getBestWave();
            return new WaveMemoryStreakPrediction() { probVector = biggestWave.Wave.getMoveProbability(moveNumber).getCounterMove(), score = biggestWave.Score };
        }


    }

    public enum MemoryZone { LowMemory, MedMemory, HighMemory }

    public class MasterMemory
    {
        WaveMemoryStreak highMemory = new WaveMemoryStreak();
        WaveMemoryStreak medMemory = new WaveMemoryStreak();
        MemoryForStreak lowMemory = new MemoryForStreak();

        public MemoryZone GetMemoryZone(int drawStreak, double dynamiteValue)
        {
            if (drawStreak > dynamiteValue + 0.5)
            {
                return MemoryZone.HighMemory;
            }
            else if (drawStreak < dynamiteValue - 0.5)
            {
                return MemoryZone.LowMemory;
            }
            else
            {
                return MemoryZone.MedMemory;
            }
        }

        public void addEnemyMove(int drawStreak, double dynamiteValue, Weapon enemyWeapon)
        {
            AddEnemyMove(GetMemoryZone(drawStreak, dynamiteValue), enemyWeapon);
        }

        public void AddEnemyMove(MemoryZone memoryZone, Weapon enemyWeapon)
        {
            if (memoryZone == MemoryZone.HighMemory)
            {
                highMemory.AddMove(enemyWeapon);
            }
            else if (memoryZone == MemoryZone.LowMemory)
            {
                lowMemory.AddMove(enemyWeapon);
            }
            else
            {
                medMemory.AddMove(enemyWeapon);
            }
        }

        public WaveMemoryStreakPrediction GetMemoryValues(int drawStreak, double dynamiteValue)
        {
            var memoryZone = GetMemoryZone(drawStreak, dynamiteValue);

            if (memoryZone == MemoryZone.HighMemory)
            {
                return highMemory.getNextPrediction();
            }
            else if (memoryZone == MemoryZone.LowMemory)
            {
                return new WaveMemoryStreakPrediction() { probVector = new probVector(lowMemory.dynValue, lowMemory.watValue, lowMemory.rpsValue) };
            }
            else
            {
                return medMemory.getNextPrediction();
            }
        }
    }

    public class WaveBotV3 : IBot
    {
        public string Name => "WaveV3";

        private Random rand;
        private int currentDrawStreak = 0;
        private int dynamiteCounter = 0;
        private int enemyDynamiteCounter = 0;
        private int myWins = 0;
        private int enemyWins = 0;
        private Memory memory = new Memory();
        //private WaveMemory waveMemory = new WaveMemory();
        private MasterMemory masterMemory = new MasterMemory();




        public WaveBotV3()
        {
            rand = new Random();
            rand = new Random(rand.Next());
            rand = new Random(rand.Next());
            rand = new Random(rand.Next());
            rand = new Random(rand.Next());
            rand = new Random(rand.Next());
            rand = new Random(rand.Next());
            rand = new Random(rand.Next());
            rand = new Random(rand.Next());
            rand = new Random(rand.Next());
            rand = new Random(rand.Next());
            rand = new Random(rand.Next());
            rand = new Random(rand.Next());
            rand = new Random(rand.Next());
            rand = new Random(rand.Next());
            rand = new Random(rand.Next());
            rand = new Random(rand.Next());
        }

        private double CalculateDynamiteValue(double consecutiveDrawsWhereDynOneThird)
        {
            return 1.0 * Math.Max((consecutiveDrawsWhereDynOneThird), 0.0); //Very approx value of dynamite
        }

        public Weapon GetNextWeaponChoice()
        {
            int approxWinsLeft = 2000 - 2 * Math.Max(myWins, enemyWins);

            double L = (double)approxWinsLeft / (Math.Pow(3, currentDrawStreak));//ApproxNumTimesSituationWillRepeat
            double d = -1 + (Math.Log((double)approxWinsLeft) - Math.Log(100 - dynamiteCounter)) / Math.Log(3);//Approx number Of Consecutive Draws Whereby dynamite can be thrown one third of the time.
            double v = CalculateDynamiteValue(d);
            double ed = -1 + (Math.Log((double)approxWinsLeft) - Math.Log(100 - enemyDynamiteCounter)) / Math.Log(3);//Approx number Of Consecutive Draws Whereby dynamite can be thrown one third of the time.
            double ev = CalculateDynamiteValue(ed);

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
                var b = 0.33 * a;//For whatever reason this works fine
                var c = Math.Max(100 - dynamiteCounter - b, 0);
                pD = c / L;
                pD = Math.Min(pD, 0.33);
            }
            else
            {
                pD = 0;
            }

            pRps = 1 - pD - pW;


            //var mem = memory.GetMemory(currentDrawStreak, ev);
            //var mem = waveMemory.GetMemory(currentDrawStreak, ev);
            var prediction = masterMemory.GetMemoryValues(currentDrawStreak, ev);
            var values = prediction.probVector;
            pD = Math.Max(0, pD);
            pW = Math.Max(0, pW);
            pRps = Math.Max(0, pRps);

            var memoryZone = masterMemory.GetMemoryZone(currentDrawStreak, ed);

            const double lowScore = 3.0;
            const double medScore = 3.5;
            const double highScore = 4.0;



            if (prediction.score > highScore)
            {
                if (memoryZone == MemoryZone.HighMemory)
                {
                    pD = Math.Max(0, pD * (values.dynProb));
                    pW = Math.Max(0, pW * (values.watProb));
                    pRps = Math.Max(0, pRps * (values.rpsProb));
                }

                else if (memoryZone == MemoryZone.MedMemory)
                {
                    pD = Math.Max(0, pD * (values.dynProb));
                    pW = Math.Max(0, pW * (values.watProb));
                    pRps = Math.Max(0, pRps * (values.rpsProb));
                }

            }

            if (prediction.score > medScore)
            {
                if (memoryZone == MemoryZone.HighMemory)
                {
                    pD = Math.Max(0, pD * (values.dynProb));
                    pW = Math.Max(0, pW * (values.watProb));
                    pRps = Math.Max(0, pRps * (values.rpsProb));
                }

                else if (memoryZone == MemoryZone.MedMemory)
                {
                    pD = Math.Max(0, pD * (values.dynProb));
                    pW = Math.Max(0, pW * (values.watProb));
                    pRps = Math.Max(0, pRps * (values.rpsProb));
                }

            }



            //Scaling
            if (approxWinsLeft > 100)
            {
                pD = 0.9 * pD;
            }

            //Keep dynamite as a threat.
            if (100 - dynamiteCounter == 1 && approxWinsLeft - currentDrawStreak > 1)
            {
                pD = 0.1 * pD;
            }


            double totalValue = pD + pW + pRps;

            if (currentDrawStreak < Math.Floor(d))
            {
                pD = 0;
            }
            if (currentDrawStreak == 0 && pW != 0)
            {
                pW = 0;
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
            double ev = CalculateDynamiteValue(ed);//Very approx value of dynamite
            //memory.addEnemyMove(currentDrawStreak, ev, enemiesWeapon);
            //waveMemory.addEnemyMove(currentDrawStreak, ev, enemiesWeapon);
            masterMemory.addEnemyMove(currentDrawStreak, ev, enemiesWeapon);

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
            masterMemory = new MasterMemory();
        }
    }

}


using System;
using System.Collections.Generic;
using System.Text;
using Bots;

namespace RockPaperDynamiteEngine
{

    public class Battle
    {
        public BattleResult P1BattleResult { get; }
        public BattleResult P2BattleResult { get; }
        public Weapon P1Weapon { get; }
        public Weapon P2Weapon { get; }

        public Battle(Weapon p1Weapon, Weapon p2Weapon)
        {
            P1Weapon = p1Weapon;
            P2Weapon = p2Weapon;
            P1BattleResult = BattleResultForFirstArg(p1Weapon, p2Weapon);
            P2BattleResult = BattleResultForFirstArg(p2Weapon, p1Weapon);
        }

        private BattleResult BattleResultForFirstArg(Weapon w1, Weapon w2)
        {
            if (w1 == w2)
            {
                return BattleResult.Draw;
            }

            if (
                (w1 == Weapon.Dynamite && w2 != Weapon.WaterBallon)
                || (w1 == Weapon.WaterBallon && w2 == Weapon.Dynamite)
                || (w1 != Weapon.Dynamite && w2 == Weapon.WaterBallon)
                || (w1 == Weapon.Rock && w2 == Weapon.Scissors)
                || (w1 == Weapon.Scissors && w2 == Weapon.Paper)
                || (w1 == Weapon.Paper && w2 == Weapon.Rock)
                )
            {
                return BattleResult.Win;
            }

            return BattleResult.Lose;
        }

        public override string ToString()
        {
            return "Player 1 " + P1BattleResult.ToString() + ", " + P1Weapon.ToString() + ", " + P2Weapon.ToString();
        }
    }

}

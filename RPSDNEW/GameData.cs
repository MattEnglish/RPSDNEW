using System;
using System.Collections.Generic;
using System.Text;
using Bots;

namespace RockPaperDynamiteEngine
{
    public class GameData
    {
        public List<Battle> Battles = new List<Battle>();
        public int P1DynamiteUsed { get; set; }
        public int P2DynamiteUsed { get; set; }
        public string P1Name { get; set; }
        public string P2Name { get; set; }
        public int P1WinCount { get; set; }
        public int P2WinCount { get; set; }
        public int currentDrawStreak { get; set; }
        public Victory victory { get; set; } = Victory.unknown;
        public string VictoryReason { get; set; } = "";
        public int P1DynamiteAfter1000Battles { get; set; }
        public int P2DynamiteAfter1000Battles { get; set; }

        public override string ToString()
        {
            if (victory == Victory.player1Victory)
            {
                return P1Name + " Victory against " + P2Name + "   "+ P1WinCount.ToString() + ":" + P2WinCount.ToString();
            }
            return P2Name + "Victory against "+P1Name + "   " + P1WinCount.ToString() + ":" + P2WinCount.ToString();
        }
        

    }

}

using System;
using System.Collections.Generic;
using System.Text;

namespace Bots
{
    public enum Weapon { Rock, Paper, Scissors, Dynamite, WaterBallon }

    public enum BattleResult { Win, Lose, Draw };

    public interface IBot
    {
        string Name { get; }

        void NewGame(string enemyBotName);

        void HandleBattleResult(BattleResult result, Weapon yourWeapon, Weapon enemiesWeapon);

        Weapon GetNextWeaponChoice();

        void HandleFinalResult(bool isWin);

    }
}


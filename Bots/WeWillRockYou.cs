using System;
using System.Collections.Generic;
using System.Text;

namespace Bots
{
    public class WeWillRockYou : IBot
    {
        public string Name => "We Will Rock You";

        public Weapon GetNextWeaponChoice()
        {
            return Weapon.Rock;
        }

        public void HandleBattleResult(BattleResult result, Weapon yourWeapon, Weapon enemiesWeapon)
        {

        }

        public void HandleFinalResult(bool isWin)
        {

        }

        public void NewGame(string enemyBotName)
        {

        }
    }
}

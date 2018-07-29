using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bots;

namespace NeuralBotConsole
{
    class NeuralBot : IBot
    {
        public string Name => NeuralBot;

        public Weapon GetNextWeaponChoice()
        {
            throw new NotImplementedException();
        }

        public void HandleBattleResult(BattleResult result, Weapon yourWeapon, Weapon enemiesWeapon)
        {
            throw new NotImplementedException();
        }

        public void HandleFinalResult(bool isWin)
        {
            throw new NotImplementedException();
        }

        public void NewGame(string enemyBotName)
        {
            throw new NotImplementedException();
        }
    }
}

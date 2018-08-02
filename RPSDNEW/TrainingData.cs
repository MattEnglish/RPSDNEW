using Bots;
using RockPaperDynamiteEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPSDNEW
{
    public class TrainingData
    {
        public List<double[]> Inputs = new List<double[]>();
        public List<double[]> Outputs = new List<double[]>();

        public void AddInputs(double currentDrawStreak, double winsLeft, double enemyWinsLeft, double dynamiteLeft, double enemyDynamiteLeft)
        {
            Inputs.Add(new double[] { currentDrawStreak / 5, winsLeft / 1000, enemyWinsLeft / 1000, dynamiteLeft / 100, enemyDynamiteLeft / 100 });//TODO Add mappings
        }
        public void AddOutputs(Weapon weapon)
        {
            if(weapon == Weapon.Dynamite)
            {
                Outputs.Add(new double[] {1.0, 0, 0});//TODO Add mappings
            }
            else if (weapon == Weapon.WaterBallon)
            {
                Outputs.Add(new double[] { 0, 1.0, 0 });//TODO Add mappings
            }
            else
            {
                Outputs.Add(new double[] { 0, 0, 1.0 });//TODO Add mappings
            }

        }


    }

    
}

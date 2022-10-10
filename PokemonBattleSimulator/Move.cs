using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokemonBattleSimulator
{
    internal class Move
    {
        public string Name { get; set; }
        public int BasePower;
        public PokemonType MoveType;
        public int Accuracy;
        public int PowerPoints;
        public int Priority;
        public MoveCategory Category;

        public int PowerPointsLeft;
        public int PowerPointUpsUsed;
        public Move()
        {
            Name = "";
            PowerPointsLeft = PowerPoints;
            PowerPointUpsUsed = 0;
        }
        public Move(string name, int basePower, PokemonType moveType, int accuracy, int powerPoints, int priority, MoveCategory category)
        {
            Name = name;
            BasePower = basePower;
            MoveType = moveType;
            Accuracy = accuracy;
            PowerPoints = powerPoints;
            Priority = priority;
            Category = category;
            PowerPointsLeft = PowerPoints;
            PowerPointUpsUsed = 0;
        }
        public override string ToString()
        {
            return this.Name;
        }
    }
}

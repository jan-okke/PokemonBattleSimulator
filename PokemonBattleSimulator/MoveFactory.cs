using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokemonBattleSimulator
{
    internal class MoveFactory
    {
        public static Move Tackle = new Move()
        {
            Name = "Tackle",
            BasePower = 40,
            Accuracy = 100,
            Priority = 0,
            MoveType = PokemonType.Normal,
            PowerPoints = 40,
            Category = MoveCategory.Physical
        };
        public static Move Leer = new Move()
        {
            Name = "Leer",
            BasePower = 0,
            Accuracy = 100,
            Priority = 0,
            MoveType = PokemonType.Normal,
            PowerPoints = 40,
            Category = MoveCategory.Status
        };
        public static Move WaterGun = new Move()
        {
            Name = "Water Gun",
            BasePower = 40,
            Accuracy = 100,
            Priority = 0,
            MoveType = PokemonType.Water,
            PowerPoints = 40,
            Category = MoveCategory.Special
        };
        public static Move Ember = new Move()
        {
            Name = "Ember",
            BasePower = 40,
            Accuracy = 100,
            Priority = 0,
            MoveType = PokemonType.Fire,
            PowerPoints = 30,
            Category = MoveCategory.Special
        };
        public static Move VineWhip = new Move()
        {
            Name = "Vine Whip",
            BasePower = 40,
            Accuracy = 100,
            Priority = 0,
            MoveType = PokemonType.Grass,
            PowerPoints = 30,
            Category = MoveCategory.Physical
        };
    }
}

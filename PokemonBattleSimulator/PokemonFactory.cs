using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace PokemonBattleSimulator
{
    internal class PokemonFactory
    {
        public static Pokemon Bulbasaur = new Pokemon()
        {
            Name = "Bulbasaur",
            BaseStats = new Stats(50, 50, 50, 50, 50, 50),
            Type1 = PokemonType.Grass,
            Type2 = PokemonType.Poison
        };
        public static Pokemon Squirtle = new Pokemon()
        {
            Name = "Squirtle",
            BaseStats = new Stats(50, 50, 50, 50, 50, 50),
            Type1 = PokemonType.Water,
            Type2 = PokemonType.None
        };
        public static Pokemon Charmander = new Pokemon()
        {
            Name = "Charmander",
            BaseStats = new Stats(50, 50, 50, 50, 50, 50),
            Type1 = PokemonType.Fire,
            Type2 = PokemonType.None
        };
        public static List<Pokemon> Pokemons = new() { Bulbasaur, Squirtle, Charmander };
    }
}

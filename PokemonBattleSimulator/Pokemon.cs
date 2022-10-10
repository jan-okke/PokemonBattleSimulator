using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokemonBattleSimulator
{
    internal class Pokemon
    {
        public string Name { get; set; }
        public Stats BaseStats;
        public PokemonType Type1;
        public PokemonType Type2;

        public int Level;
        public Stats Stats;
        public Stats EVs;
        public Stats IVs;
        public Stats StatStages;
        public Nature Nature;
        public List<Move> Moves;
        public Item Item;
        public int CurrentHP;
        public bool IsAlive;

        public Pokemon()//string name, Stats baseStats, PokemonType type1, PokemonType type2)
        {
            //Name = name;
            //BaseStats = baseStats;
            //Type1 = type1;
            //Type2 = type2;
            Name = "";
            BaseStats = new(0, 0, 0, 0, 0, 0);
            Type1 = PokemonType.None;
            Type2 = PokemonType.None;
            Level = 0;
            Stats = new(0, 0, 0, 0, 0, 0);
            EVs = new(0, 0, 0, 0, 0, 0);
            IVs = new(0, 0, 0, 0, 0, 0);
            StatStages = new(1, 1, 1, 1, 1, 1);
            Nature = Nature.None;
            Moves = new();
            Item = Item.NoItem;
            CurrentHP = Stats.HP;
            IsAlive = true;
        }
        public override string ToString()
        {
            return this.Name;
        }
    }
}

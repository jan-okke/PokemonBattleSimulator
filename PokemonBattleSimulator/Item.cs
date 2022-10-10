using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokemonBattleSimulator
{
    internal class Item
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public static Item NoItem = new("", "");
        public Item(string name, string description)
        {
            Name = name;
            Description = description;
        }
    }
}

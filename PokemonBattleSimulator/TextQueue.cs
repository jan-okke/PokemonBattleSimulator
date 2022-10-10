using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokemonBattleSimulator
{
    internal class TextQueue
    {
        public List<TextQueueData> Texts;
        public TextQueue()
        {
            Texts = new();
        }
        public void Add(string text)
        {
            Texts.Add(new TextQueueData()
            {
                Text = text,
                Delay = 500,
                UpdateScreen = false
            });
        }
        public void AddHPChange()
        {
            Texts.Add(new TextQueueData()
            {
                Text = String.Empty,
                Delay = 500,
                UpdateScreen = true
            });
        }
        public TextQueueData Next()
        {
            return Texts[0];
        }
        public bool Empty()
        {
            return Texts.Count == 0;
        }
    }
}

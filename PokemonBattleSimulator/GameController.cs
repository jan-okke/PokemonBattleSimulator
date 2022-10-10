using System;
using System.Collections;
using System.Collections.Generic;
using System.DirectoryServices.ActiveDirectory;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace PokemonBattleSimulator
{
    internal class GameController
    {
        private TextQueue TextQueue;
        private MainWindow Window;
        public GameController(MainWindow window) {

            TextQueue = new TextQueue();
            Window = window;
            new Thread(() => DisplayText()).Start();
        }
        private void AddText(string text)
        {
            TextQueue.Add(text);
        }
        private void AddHPChange(Pokemon damageTaker, int amount)
        {
            damageTaker.CurrentHP -= amount;
            if (damageTaker.CurrentHP < 0)
            {
                damageTaker.CurrentHP = 0;
                damageTaker.IsAlive = false;
            }
            TextQueue.AddHPChange();
        }
        private void DisplayText()
        {
            while (Window.Dispatcher.Thread.IsAlive)
            {
                int delay = 0;
                Thread.Sleep(100);
                if (TextQueue.Empty()) continue;
                Window.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                (ThreadStart)delegate ()
                {
                    try {
                        TextQueueData nextData = TextQueue.Next();
                        if (nextData.Text != string.Empty) Window.OutputTextBox.Text = nextData.Text;
                        if (nextData.UpdateScreen) Window.ForceUpdateScreen();
                        delay = nextData.Delay;
                        TextQueue.Texts.Remove(nextData);
                    }
                    catch (Exception)
                    {

                    }
                    }
                );
                Thread.Sleep(delay);
            }
        }
        public bool TeamFainted(List<Pokemon> PokemonTeam)
        {
            foreach (Pokemon pokemon in PokemonTeam)
            {
                if (pokemon.IsAlive) return false;
            }
            return true;
        }
        public bool BattleOnGoing(List<Pokemon> PokemonTeam1, List<Pokemon> PokemonTeam2)
        {
            return !TeamFainted(PokemonTeam1) && (!TeamFainted(PokemonTeam2));
        }
        public void CalculateStats(Pokemon pokemon)
        {
            pokemon.Stats.HP = (int)Math.Floor((2 * pokemon.BaseStats.HP + pokemon.IVs.HP + (double)(int)Math.Floor((double)pokemon.EVs.HP / 4)) * pokemon.Level / 100) + pokemon.Level + 10;
            pokemon.Stats.Attack = (int)Math.Floor((double)((int)Math.Floor((2 * pokemon.BaseStats.Attack + pokemon.IVs.Attack + (double)(int)Math.Floor((double)pokemon.EVs.Attack / 4)) * pokemon.Level / 100) + 5) * GetNatureModAttack(pokemon.Nature));
            pokemon.Stats.Defense = (int)Math.Floor((double)((int)Math.Floor((2 * pokemon.BaseStats.Defense + pokemon.IVs.Defense + (double)(int)Math.Floor((double)pokemon.EVs.Defense / 4)) * pokemon.Level / 100) + 5) * GetNatureModDefense(pokemon.Nature));
            pokemon.Stats.SpecialAttack = (int)Math.Floor((double)((int)Math.Floor((2 * pokemon.BaseStats.SpecialAttack + pokemon.IVs.SpecialAttack + (double)(int)Math.Floor((double)pokemon.EVs.SpecialAttack / 4)) * pokemon.Level / 100) + 5) * GetNatureModSpecialAttack(pokemon.Nature));
            pokemon.Stats.SpecialDefense = (int)Math.Floor((double)((int)Math.Floor((2 * pokemon.BaseStats.SpecialDefense + pokemon.IVs.SpecialDefense + (double)(int)Math.Floor((double)pokemon.EVs.SpecialDefense / 4)) * pokemon.Level / 100) + 5) * GetNatureModSpecialDefense(pokemon.Nature));
            pokemon.Stats.Speed = (int)Math.Floor((double)((int)Math.Floor((2 * pokemon.BaseStats.Speed + pokemon.IVs.Speed + (double)(int)Math.Floor((double)pokemon.EVs.Speed / 4)) * pokemon.Level / 100) + 5) * GetNatureModSpeed(pokemon.Nature));
            pokemon.CurrentHP = (int)pokemon.Stats.HP;
        }
        private double GetNatureModAttack(Nature nature)
        {
            return 1;
        }
        private double GetNatureModDefense(Nature nature)
        {
            return 1;
        }
        private double GetNatureModSpecialAttack(Nature nature)
        {
            return 1;
        }
        private double GetNatureModSpecialDefense(Nature nature)
        {
            return 1;
        }
        private double GetNatureModSpeed(Nature nature)
        {
            return 1;
        }
        private double CalculateStab(Pokemon attacker, Move move)
        {
            if (attacker.Type1 == move.MoveType || attacker.Type2 == move.MoveType) return 1.5;
            return 1;
        }
        private int CalculateDamage(Pokemon attacker, Pokemon defender, Move move)
        {
            if (move.Category == MoveCategory.Status) return 0;
            double damage;
            double power = move.BasePower;
            double attack = attacker.Stats.Attack * attacker.StatStages.Attack;
            double defense = defender.Stats.Defense * defender.StatStages.Defense;
            double specialAttack = attacker.Stats.SpecialAttack * attacker.StatStages.SpecialAttack;
            double specialDefense = defender.Stats.SpecialDefense * defender.StatStages.SpecialDefense;
            double targetMod = 1;
            double parentalBond = 1;
            double weather = 1;
            double critical = 1;
            double random = 1;
            double stab = CalculateStab(attacker, move);
            double effectivity = CalculateEffectivity(move.MoveType, defender.Type1, defender.Type2);
            double burn = 1;
            double other = 1;
            double zMove = 1;
            if (move.Category == MoveCategory.Physical) damage = Math.Floor(Math.Floor(0.4 * attacker.Level + 2) * power * attack / (defense * 50)) * targetMod * parentalBond * weather * critical * random * stab * effectivity * burn * other * zMove;
            else if (move.Category == MoveCategory.Special) damage = Math.Floor(Math.Floor(0.4 * attacker.Level + 2) * power * specialAttack / (specialDefense * 50)) * targetMod * parentalBond * weather * critical * random * stab * effectivity * burn * other * zMove;
            else return -1;
            return (int)damage;
        }
        private double CalculateEffectivity(PokemonType attacker, PokemonType defender)
        {
            switch (attacker)
            {
                case PokemonType.Fire:
                    if (defender == PokemonType.Grass || defender == PokemonType.Ice || defender == PokemonType.Bug || defender == PokemonType.Steel) return 2;
                    if (defender == PokemonType.Dragon || defender == PokemonType.Fire || defender == PokemonType.Rock || defender == PokemonType.Water) return .5;
                    break;
                case PokemonType.Grass:
                    if (defender == PokemonType.Water || defender == PokemonType.Ground || defender == PokemonType.Rock) return 2;
                    if (defender == PokemonType.Fire || defender == PokemonType.Dragon || defender == PokemonType.Flying || defender == PokemonType.Bug || defender == PokemonType.Poison || defender == PokemonType.Grass) return .5;
                    break;
            }
            return 1;
        }
        private double CalculateEffectivity(PokemonType attacker, PokemonType defender1, PokemonType defender2)
        {
            return CalculateEffectivity(attacker, defender1) * CalculateEffectivity(attacker, defender2);
        }
        public Pokemon NewPokemon(string name, int level, List<Move> moves)
        {
            foreach (Pokemon p in PokemonFactory.Pokemons)
            {
                if (p.Name == name)
                {
                    Pokemon pokemon = p;
                    pokemon.Level = level;
                    pokemon.Moves = moves;
                    CalculateStats(pokemon);
                    return pokemon;
                }
            }
            throw new Exception(name);
        }
        public Move ChooseRandomMove(Pokemon pokemon)
        {
            Random random = new();
            return pokemon.Moves[random.Next(pokemon.Moves.Count)];
        }
        private bool DoesDamageKill(Pokemon pokemon, int amount)
        {
            return pokemon.CurrentHP - amount <= 0;
        }
        private bool IsFaster(Move a, Move b)
        {
            return a.Priority > b.Priority;
        }
        private bool IsFaster(Pokemon a, Pokemon b, Move c, Move d)
        {
            if (IsFaster(c, d)) return true;
            return a.Stats.Speed * a.StatStages.Speed > b.Stats.Speed * b.StatStages.Speed;
        }
        private void OnFaint(Pokemon faintedPokemon, Pokemon killingPokemon)
        {
            AddHPChange(faintedPokemon, faintedPokemon.CurrentHP);
            AddText($"{faintedPokemon.Name} fainted!");
        }
        private bool PokemonAttackTurn(Pokemon attacker, Pokemon defender, Move attackerMove)
        {
            int damage;
            double effectivity;
            AddText($"{attacker.Name} used {attackerMove.Name}!");
            damage = CalculateDamage(attacker, defender, attackerMove);
            if (damage > 0)
            {
                AddHPChange(defender, damage);
                effectivity = CalculateEffectivity(attackerMove.MoveType, defender.Type1, defender.Type2);
                if (effectivity == 0) AddText("It had no effect.");
                if (effectivity < 1) AddText("It's not very effective...");
                if (effectivity > 1) AddText("It's super effective!");
                if (!defender.IsAlive)
                {
                    OnFaint(defender, attacker);
                    return true;
                }
            }
            return false;
        }
        private void SwitchNextAIPokemon(List<Pokemon> pokemonList) 
        {
            List<Pokemon> tempList = new();
            foreach (Pokemon pokemon in pokemonList)
            {
                if (pokemon.IsAlive) tempList.Add(pokemon);
            }
            foreach (Pokemon pokemon in pokemonList)
            {
                tempList.Add(pokemon);
            }
            for (int i = 0; i < pokemonList.Count; i++)
            {
                pokemonList.Remove(pokemonList[i]);
            }
            foreach (Pokemon pokemon in tempList)
            {
                pokemonList.Add(pokemon);
            }
        }
        public void Turn(List<Pokemon> attackerParty, List<Pokemon> defenderParty, Move attackerMove, Move defenderMove)
        {
            Pokemon attacker = attackerParty[0];
            Pokemon defender = defenderParty[0];
            if (IsFaster(attacker, defender, attackerMove, defenderMove)) {
                if (PokemonAttackTurn(attacker, defender, attackerMove))
                {
                    if (!TeamFainted(defenderParty))
                    {
                        SwitchNextAIPokemon(defenderParty);
                    }
                    return;
                }
                Thread.Sleep(800);
                PokemonAttackTurn(defender, attacker, defenderMove);
            }
            else
            {
                if (PokemonAttackTurn(defender, attacker, defenderMove))
                {
                    return;
                }
                Thread.Sleep(800);
                if (PokemonAttackTurn(attacker, defender, attackerMove))
                {
                    if (!TeamFainted(defenderParty))
                    {
                        SwitchNextAIPokemon(defenderParty);
                    }
                    return;
                }
            }
            Thread.Sleep(800);
        }
    }
}

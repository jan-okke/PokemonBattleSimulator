using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace PokemonBattleSimulator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<Pokemon> PlayerPokemonList;
        private List<Pokemon> EnemyPokemonList;
        private GameController GameController;
        private string OppTrainerName;
        private string OppTrainerClass;
        public MainWindow()
        {
            InitializeComponent();
            OppTrainerClass = "Rival";
            OppTrainerName = "Gary";
            GameController = new(this);
            Pokemon Charmander = GameController.NewPokemon("Charmander", 5, new() { MoveFactory.Ember, MoveFactory.Tackle, MoveFactory.Leer});
            Pokemon Bulbasaur = GameController.NewPokemon("Bulbasaur", 5, new() { MoveFactory.VineWhip, MoveFactory.Tackle, MoveFactory.Leer });
            Pokemon Squirtle = GameController.NewPokemon("Squirtle", 5, new() { MoveFactory.WaterGun, MoveFactory.Tackle, MoveFactory.Leer });
            PlayerPokemonList = new() { Charmander };
            EnemyPokemonList = new() { Bulbasaur, Squirtle };
            this.PlayerTeamListBox.ItemsSource = PlayerPokemonList;
            new Thread(() => DrawScreen()).Start();
            new Thread(() => DrawScreenUI()).Start();
        }
        public void ForceUpdateScreen()
        {
            DrawScreen();
        }
        private void DrawScreen()
        {
            this.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                (ThreadStart)delegate ()
                {
                    this.EnemyPokemonTextBlock.Text = $"{EnemyPokemonList[0].Name} {EnemyPokemonList[0].CurrentHP} / {EnemyPokemonList[0].Stats.HP}";
                    this.PlayerPokemonTextBlock.Text = $"{PlayerPokemonList[0].Name} {PlayerPokemonList[0].CurrentHP} / {PlayerPokemonList[0].Stats.HP}";
                    this.EnemyTeamListBox.ItemsSource = EnemyPokemonList;
                }
                );
        }
        private void DrawScreenUI()
        {
            while (this.Dispatcher.Thread.IsAlive)
            {
                this.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                (ThreadStart)delegate ()
                {
                    this.Move1Button.Content = "";
                    this.Move1Button.IsEnabled = false;
                    this.Move2Button.Content = "";
                    this.Move2Button.IsEnabled = false;
                    this.Move3Button.Content = "";
                    this.Move3Button.IsEnabled = false;
                    this.Move4Button.Content = "";
                    this.Move4Button.IsEnabled = false;

                    try
                    {
                        if (!GameController.BattleOnGoing(PlayerPokemonList, EnemyPokemonList))
                        {
                            if (GameController.TeamFainted(PlayerPokemonList))
                            {
                                OutputTextBox.Text = $"You lost against {OppTrainerClass} {OppTrainerName}...";
                            }
                            else OutputTextBox.Text = $"You won against {OppTrainerClass} {OppTrainerName}!";
                            return;
                        }
                        this.Move1Button.Content = PlayerPokemonList[0].Moves[0];
                        this.Move1Button.IsEnabled = true;
                        this.Move2Button.Content = PlayerPokemonList[0].Moves[1];
                        this.Move2Button.IsEnabled = true;
                        this.Move3Button.Content = PlayerPokemonList[0].Moves[2];
                        this.Move3Button.IsEnabled = true;
                        this.Move4Button.Content = PlayerPokemonList[0].Moves[3];
                        this.Move4Button.IsEnabled = true;
                    }
                    catch (ArgumentOutOfRangeException)
                    {

                    }
                }
                );
                Thread.Sleep(400);
            }
            
        }

        private void UseMove1(object sender, RoutedEventArgs e)
        {
            new Thread(() => GameController.Turn(PlayerPokemonList, EnemyPokemonList, PlayerPokemonList[0].Moves[0], GameController.ChooseRandomMove(EnemyPokemonList[0]))).Start();
        }

        private void UseMove2(object sender, RoutedEventArgs e)
        {
            new Thread(() => GameController.Turn(PlayerPokemonList, EnemyPokemonList, PlayerPokemonList[0].Moves[1], GameController.ChooseRandomMove(EnemyPokemonList[0]))).Start();
        }

        private void UseMove3(object sender, RoutedEventArgs e)
        {
            new Thread(() => GameController.Turn(PlayerPokemonList, EnemyPokemonList, PlayerPokemonList[0].Moves[2], GameController.ChooseRandomMove(EnemyPokemonList[0]))).Start();
        }

        private void UseMove4(object sender, RoutedEventArgs e)
        {
            new Thread(() => GameController.Turn(PlayerPokemonList, EnemyPokemonList, PlayerPokemonList[0].Moves[3], GameController.ChooseRandomMove(EnemyPokemonList[0]))).Start();
        }
    }
}

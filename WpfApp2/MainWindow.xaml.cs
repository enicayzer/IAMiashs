using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using System.Xml;

namespace WpfApp2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Puissance4 board;

        public MainWindow()
        {
            InitializeComponent();
            Demarrage();
        }

        public void Demarrage()
        {
            // Initialisation des données
            var game = new Jeu();
            game.Ligne = 6;
            game.Colonne = 7;
            game.Statut = Statuts.running;
            game.Profondeur = 4;
            game.Score = 100000;
            game.Round = 0;
            game.Gagnant = null;
            game.Iteration = 0;

            // Initialisation du jeu
            board = new Puissance4(game, new int?[game.Ligne, game.Colonne], 0);
        }

        private void buttonGrid_Click(object sender, RoutedEventArgs e)
        {
            var column = Grid.GetColumn((Button)sender);
            var tuple = board.Placer(column);
            if (!tuple.Item1)
            {
                throw new Exception();
            }
            var row = tuple.Item2;
            var buttons = gridJeu.Children.Cast<Button>().FirstOrDefault(e => Grid.GetRow(e) == tuple.Item2 && Grid.GetColumn(e) == column);
            var button = gridJeu.Children.Cast<UIElement>().First(e => Grid.GetRow(e) == row && Grid.GetColumn(e) == column) as Button;
            buttons.Background = Brushes.Blue;

            // on appelle l'IA 
            var retourIA = board.DecisionIA();
            var place = board.Placer(retourIA);
            if (!place.Item1)
            {
                throw new Exception();
            }
            var buttonIA = gridJeu.Children.Cast<UIElement>().First(e => Grid.GetRow(e) == place.Item2 && Grid.GetColumn(e) == retourIA) as Button;
            buttonIA.Background = Brushes.Red;
        }
    }
}

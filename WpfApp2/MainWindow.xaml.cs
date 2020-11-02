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
        private Puissance4 puissance4;

        public MainWindow()
        {
            InitializeComponent();
            Demarrage();

        }

        public void Demarrage()
        {
            // Récupération des données des combobox
            var minMax = (minMaxBox.SelectedItem as ComboBoxItem).Tag.ToString();
            var profondeur = (profondeurBox.SelectedItem as ComboBoxItem).Tag.ToString();

            // Initialisation des données
            var game = new Jeu();
            game.Ligne = 6;
            game.Colonne = 7;
            game.Statut = Statuts.EnCours;
            game.Profondeur = int.Parse(profondeur);
            game.Score = 100000;
            game.NbrPointsGagnant = 4;
            if(minMax != "MinMax")
            {
                game.EstAlphaBeta = true;
            }

            // Initialisation du jeu
            puissance4 = new Puissance4(game, new int?[game.Ligne, game.Colonne], 0);

            // Si que IA 
            while (AffichageMessageFin(puissance4.VerifierJeu()){
                JoueurMachineAlgo();
            }

        }

        private void buttonGrid_Click(object sender, RoutedEventArgs e)
        {
            #region Partie Joueur 1
            JoueurHumainAlgo();
            #endregion

            #region Partie de l'IA 
            JoueurMachineAlgo();
            #endregion
        }


        private void JoueurMachineAlgo()
        {
            // on appelle l'algorithme minimax 
            var retourIA = puissance4.DecisionIA();

            // On a la colonne sélectionné pr l'IA
            var tupleLigneRetourIA = puissance4.Placer(retourIA);
            if (!tupleLigneRetourIA.Item1)
            {
                throw new Exception();
            }
            var boutonCliquerIA = gridJeu.Children.Cast<UIElement>()
                .FirstOrDefault(e => Grid.GetRow(e) == tupleLigneRetourIA.Item2 && Grid.GetColumn(e) == retourIA) as Button;
            // On change le fond en rouge du bouton sélectionné par l'IA 
            boutonCliquerIA.Background = Color.Red;

            if (AffichageMessageFin(puissance4.VerifierJeu()))
            {
                return;
            }
        }

        private void JoueurHumainAlgo()
        {
            // On récupère la colonne sélectionné par l'utilisateur
            var colonne = Grid.GetColumn((Button)sender);
            var tupleLigneRetour = puissance4.Placer(colonne);
            if (!tupleLigneRetour.Item1)
            {
                return;
            }
            var boutonCliquer = gridJeu.Children.Cast<Button>()
                .FirstOrDefault(e => Grid.GetRow(e) == tupleLigneRetour.Item2 && Grid.GetColumn(e) == colonne);
            // On change le bouton en bleu du bouton cliqué par l'utilisateur
            boutonCliquer.Background = Color.Blue;

            if (AffichageMessageFin(puissance4.VerifierJeu()))
            {
                return;
            }
        }


        private bool AffichageMessageFin(Statuts statuts)
        {
            var stopper = false;
            if (statuts == Statuts.Gagne)
            {
                stopper = true;
                MessageBox.Show("Vous avez gagné");
            }
            else if (statuts == Statuts.Perdu)
            {
                stopper = true;
                MessageBox.Show("Vous avez perdu");
            }
            else if (statuts == Statuts.Nul)
            {
                stopper = true;
                MessageBox.Show("Partie nulle");
            }
            return stopper;
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Demarrage();
            foreach (var uiElement in gridJeu.Children.Cast<UIElement>().ToList())
            {
                var button = uiElement as Button;
                if (button != null && (button.Tag == null 
                    || (button.Tag != null &&  button.Tag.ToString() != "Restart")))
                {
                    BrushConverter bc = new BrushConverter();
                    button.Background = (Brush)bc.ConvertFrom("#FFD6D6D6");
                }
            }
        }
    }
}

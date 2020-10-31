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
            // Initialisation des données
            var game = new Jeu();
            game.Ligne = 6;
            game.Colonne = 7;
            game.Statut = Statuts.EnCours;
            game.Profondeur = 2;
            game.Score = 100000;
            game.Round = 0;
            game.NbrPointsGagnant = 4;

            // Initialisation du jeu
            puissance4 = new Puissance4(game, new int?[game.Ligne, game.Colonne], 0);
        }

        private void buttonGrid_Click(object sender, RoutedEventArgs e)
        {
            if (VerifierJeu())
            {
                return;
            }
            #region Partie Humain
            // On récupère la colonne sélectionné par l'utilisateur
            var colonne = Grid.GetColumn((Button)sender);
            var tupleLigneRetour = puissance4.Placer(colonne);
            if (!tupleLigneRetour.Item1)
            {
                throw new Exception();
            }
            var boutonCliquer = gridJeu.Children.Cast<Button>()
                .FirstOrDefault(e => Grid.GetRow(e) == tupleLigneRetour.Item2 && Grid.GetColumn(e) == colonne);
            // On change le bouton en bleu du bouton cliqué par l'utilisateur
            boutonCliquer.Background = Brushes.Blue;
            #endregion

            #region Partie de l'IA 
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
            boutonCliquerIA.Background = Brushes.Red;


            if (VerifierJeu())
            {
                return;
            }
            #endregion
        }

        private bool VerifierJeu()
        {
            var continuer = false;
            // Si gagnant ou perdant alors on arrête le jeu 
            if (puissance4.jeu.Statut == Statuts.Gagne)
            {
                continuer = true;
                MessageBox.Show("Vous avez gagné");
            }
            else if (puissance4.jeu.Statut == Statuts.Perdu)
            {
                continuer = true;
                MessageBox.Show("Vous avez perdu");
            }
            return continuer;
        }
    }
}

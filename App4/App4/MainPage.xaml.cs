using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace App4
{
    public partial class MainPage : ContentPage
    {
        private Puissance4 puissance4;

        public MainPage()
        {
            InitializeComponent();
            Demarrage();
        }

        public void Demarrage()
        {
            // Récupération des données des combobox
            //var minMax = (minMaxBox.SelectedItem as ComboBoxItem).Tag.ToString();
            //var profondeur = (profondeurBox.SelectedItem as ComboBoxItem).Tag.ToString();

            // Initialisation des données
            var game = new Jeu();
            game.Ligne = 6;
            game.Colonne = 7;
            game.Statut = Statuts.EnCours;
            //game.Profondeur = int.Parse(profondeur);
            game.Profondeur = 4;
            game.Score = 100000;
            game.NbrPointsGagnant = 4;
            //if (minMax != "MinMax")
            //{
            //    game.EstAlphaBeta = true;
            //}

            // Initialisation du jeu
            puissance4 = new Puissance4(game, new int?[game.Ligne, game.Colonne], 0);
        }


        private void buttonGrid_Click(object sender, EventArgs evenement)
        {
            if (AffichageMessageFin(puissance4.VerifierJeu()))
            {
                return;
            }

            #region Partie Humain
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
            boutonCliquer.BackgroundColor = Color.Blue;
            #endregion

            if (AffichageMessageFin(puissance4.VerifierJeu()))
            {
                return;
            }

            #region Partie de l'IA 
            // on appelle l'algorithme minimax 
            var retourIA = puissance4.DecisionIA();



            // On a la colonne sélectionné pr l'IA
            var tupleLigneRetourIA = puissance4.Placer(retourIA);
            if (!tupleLigneRetourIA.Item1)
            {
                throw new Exception();
            }
            var boutonCliquerIA = gridJeu.Children.Cast<Button>()
                .FirstOrDefault(e => Grid.GetRow(e) == tupleLigneRetourIA.Item2 && Grid.GetColumn(e) == retourIA) as Button;
            // On change le fond en rouge du bouton sélectionné par l'IA 
            boutonCliquerIA.BackgroundColor = Color.Red;


            if (AffichageMessageFin(puissance4.VerifierJeu()))
            {
                return;
            }
            #endregion
        }

        private bool AffichageMessageFin(Statuts statuts)
        {
            var stopper = false;
            if (statuts == Statuts.Gagne)
            {
                stopper = true;
                DisplayAlert("Fin de partie", "vous avez gagné", "ok");
            }
            else if (statuts == Statuts.Perdu)
            {
                stopper = true;
                DisplayAlert("Fin de partie", "vous avez perdu", "ok");
            }
            else if (statuts == Statuts.Nul)
            {
                stopper = true;
                DisplayAlert("Fin de partie", "Partie nulle", "ok");
            }
            return stopper;
        }


        private void Button_Click(object sender, EventArgs e)
        {
            Demarrage();
            foreach (var uiElement in gridJeu.Children.Cast<Button>().ToList())
            {
                //var button = uiElement as Button;
                //if (button != null && (button.Tag == null
                //    || (button.Tag != null && button.Tag.ToString() != "Restart")))
                //{

                //    button.BackgroundColor = Color.Gray;
                //}
            }
        }
        private void RetourMenu(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Page1());
        }
    }

}

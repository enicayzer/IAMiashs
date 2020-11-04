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

        protected async override void OnAppearing()
        {
            await Task.Delay(1000);

            // On a la colonne sélectionné pr l'IA
            var tupleLigneRetourIA = puissance4.Placer(3);
            var boutonCliquerIA = gridJeu.Children.Cast<Button>()
              .FirstOrDefault(e => Grid.GetRow(e) == tupleLigneRetourIA.Item2 && Grid.GetColumn(e) == 3) as Button;
            // On change le fond en rouge du bouton sélectionné par l'IA 
            if (puissance4.joueur == Joueur.Joueur1)
            {
                boutonCliquerIA.BackgroundColor = Color.Red;
            }
            else
            {
                boutonCliquerIA.BackgroundColor = Color.Yellow;
            }
            var isJoueur1 = false;
            var loop = 0;
            // Simulation si 2 IA qui s'affronte 
            while (!AffichageMessageFin(puissance4.VerifierJeu()))
            {
                await Task.Delay(200);
                if (JoueurIA(isJoueur1))
                {
                    return;
                }
                isJoueur1 = ChangeBool(isJoueur1);
                loop += 1;
            }
        }

        private bool ChangeBool(bool isJoueur)
        {
            return isJoueur ? false : true;
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
            puissance4 = new Puissance4(game, new int?[game.Ligne, game.Colonne], 0, true);
        }

        private void buttonGrid_Click(object sender, EventArgs evenement)
        {
            #region Partie Humain
            if (JoueurHumain(sender))
            {
                return;
            }
            #endregion

            #region Partie de l'IA 
            if (JoueurIA(false))
            {
                return;
            };
            #endregion
        }

        private bool JoueurIA(bool isjoueur1)
        {
            // on appelle l'algorithme minimax 
            var retourIA = puissance4.DecisionIA(isjoueur1);

            // On a la colonne sélectionné pr l'IA
            var tupleLigneRetourIA = puissance4.Placer(retourIA);
            if (!tupleLigneRetourIA.Item1)
            {
                // Cas  impossible 
            }
            var boutonCliquerIA = gridJeu.Children.Cast<Button>()
                .FirstOrDefault(e => Grid.GetRow(e) == tupleLigneRetourIA.Item2 && Grid.GetColumn(e) == retourIA) as Button;
            // On change le fond en rouge du bouton sélectionné par l'IA 
            if(puissance4.joueur == Joueur.Joueur1)
            {
                boutonCliquerIA.BackgroundColor = Color.Red;
            }
            else
            {
                boutonCliquerIA.BackgroundColor = Color.Yellow;
            }

            if (AffichageMessageFin(puissance4.VerifierJeu()))
            {
                return true;
            }
            return false;
        }

        private bool JoueurHumain(object sender)
        {
            // On récupère la colonne sélectionné par l'utilisateur
            var colonne = Grid.GetColumn((Button)sender);
            var tupleLigneRetour = puissance4.Placer(colonne);
            if (!tupleLigneRetour.Item1)
            {
                return true;
            }
            var boutonCliquer = gridJeu.Children.Cast<Button>()
                .FirstOrDefault(e => Grid.GetRow(e) == tupleLigneRetour.Item2 && Grid.GetColumn(e) == colonne);
            // On change la couleur du bouton cliqué par l'utilisateur (en fonction de la couleur définie)
            boutonCliquer.BackgroundColor = Color.Yellow;
            return false;
        }


        private bool AffichageMessageFin(Statuts statuts)
        {
            var stopper = false;
            if (statuts == Statuts.Gagne)
            {
                stopper = true;
                DisplayAlert("Fin de partie", "Vous avez gagné", "ok");
            }
            else if (statuts == Statuts.Perdu)
            {
                stopper = true;
                DisplayAlert("Fin de partie", "Vous avez perdu", "ok");
            }
            else if (statuts == Statuts.Nul)
            {
                stopper = true;
                DisplayAlert("Fin de partie", "Match nul", "ok");
            }
            return stopper;
        }

        private void NouvellePartie_Click(object sender, EventArgs e)
        {
            Demarrage();
            foreach (var uiElement in gridJeu.Children.Cast<Button>().ToList())
            {
                uiElement.BackgroundColor = Color.White;
            }
        }

        private void RetourMenu_Click(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Page1());
        }
    }

}

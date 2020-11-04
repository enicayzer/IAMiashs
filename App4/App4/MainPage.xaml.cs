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
        private Parametres parametres;

        public MainPage(Parametres parametres)
        {
            InitializeComponent();
            Demarrage(parametres);
        }

        protected async override void OnAppearing()
        {
            IAMatch();
        }

        private async void IAMatch()
        {
            if ((!parametres.Joueur1isIA && !parametres.Joueur2isIA) || (parametres.Joueur2isIA && !parametres.Joueur1isIA))
            {
                return;
            }

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

            if (parametres.Joueur1isIA && parametres.Joueur2isIA)
            {
                var isJoueur1 = false;
                var loop = 0;
                // Simulation si 2 IA qui s'affronte 
                while (!AffichageMessageFin(puissance4.VerifierJeu()))
                {
                    await Task.Delay(200);
                    if (JoueurIA(isJoueur1, parametres.Joueur1isIA ? parametres.NiveauJoueur1 : parametres.NiveauJoueur2))
                    {
                        return;
                    }
                    isJoueur1 = ChangeBool(isJoueur1);
                    loop += 1;
                }
            }
        }

        private bool ChangeBool(bool isJoueur)
        {
            return isJoueur ? false : true;
        }

        public void Demarrage(Parametres parametres)
        {
            // Initialisation des données
            var game = new Jeu();
            game.Ligne = parametres.NbLigne;
            game.Colonne = parametres.NbColonne;
            game.Statut = Statuts.EnCours;
            game.Score = parametres.Score;
            game.NbrPointsGagnant = parametres.NbPointsGagnants;
            
            // On stock en mémoire les paramètres du début
            this.parametres = parametres;
            
            // Initialisation du jeu
            puissance4 = new Puissance4(game, new int?[game.Ligne, game.Colonne], 0, true);
        }

        private void buttonGrid_Click(object sender, EventArgs evenement)
        {
            // Si il s'agit de 2 IA 
            if(parametres.Joueur1isIA && parametres.Joueur2isIA)
            {
                return;
            }

            #region Partie Humain
            if (JoueurHumain(sender))
            {
                return;
            }
            #endregion

            #region Partie de l'IA 
            if (JoueurIA(parametres.Joueur1isIA, parametres.Joueur1isIA ? parametres.NiveauJoueur1 : parametres.NiveauJoueur2))
            {
                return;
            };
            #endregion
        }

        private bool JoueurIA(bool isjoueur1, int profondeur)
        {
            // on appelle l'algorithme minimax 
            var retourIA = puissance4.DecisionIA(isjoueur1, profondeur, parametres.Joueur1isIA ? parametres.Joueur1AlphaBeta : parametres.Joueur2AlphaBeta);

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
            // On envoie la valeur si alpha beta
            puissance4.isJoueurAlphaBeta = parametres.Joueur1isIA ? parametres.Joueur2AlphaBeta : parametres.Joueur1AlphaBeta;

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
            if (puissance4.joueur == Joueur.Joueur1)
            {
                boutonCliquer.BackgroundColor = Color.Red;
            }
            else
            {
                boutonCliquer.BackgroundColor = Color.Yellow;
            }
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
            //Demarrage();
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

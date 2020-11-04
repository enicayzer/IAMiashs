using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App4
{
    public partial class Page1 : ContentPage
    {
        public Page1()
        {
            InitializeComponent();
        }

        private void StartGame(object sender, EventArgs evenement)
        {
            //*** Recuperation des parametres ***
            Parametres parametres = new Parametres();
            parametres.NbLigne = 6;
            parametres.NbColonne = 7;
            parametres.NbPointsGagnants = 4;
            parametres.Score = 100000;
            parametres.Statut = Statuts.EnCours;

            //*** Check si les champs sont renseignés ***
            if (!string.IsNullOrEmpty(j1_Name.Text) && !string.IsNullOrEmpty(j2_Name.Text))
            {
                //*** Parametres Joueurs ***
                parametres.TypeJoueur1 = Joueur.Joueur1;
                parametres.NomJoueur1 = j1_Name.Text;
                parametres.TypeJoueur2 = Joueur.Joueur2;
                parametres.NomJoueur2 = j2_Name.Text;

                //*** On regarde s'il faut des IA ***
                bool is_j1_IA = j1_IA.IsToggled;
                bool is_j2_IA = j2_IA.IsToggled;

                //*** On regarde s'il faut des IA AlphaBeta ***
                bool is_j1_IA_AB = j1_AlphaBeta.IsToggled;
                bool is_j2_IA_AB = j2_AlphaBeta.IsToggled;

                //*** On vérifie que les niveaux sont bien renseignés ***
                if ((is_j1_IA && j1_IA_Level.SelectedIndex == -1) || (is_j2_IA && j2_IA_Level.SelectedIndex == -1))
                {
                    CallPopupIALevel();
                }
                else
                {
                    //*** Gestion des Parametres IA ***
                    if (is_j1_IA)
                    {
                        parametres.Joueur1isIA = is_j1_IA;
                        parametres.Joueur1AlphaBeta = is_j1_IA_AB;
                        parametres.NiveauJoueur1 = parametres.GetIALevelByLabel(j1_IA_Level.Items[j1_IA_Level.SelectedIndex]);
                    }
                    if (is_j2_IA)
                    {
                        parametres.Joueur2isIA = is_j2_IA;
                        parametres.Joueur2AlphaBeta = is_j2_IA_AB;
                        parametres.NiveauJoueur2 = parametres.GetIALevelByLabel(j2_IA_Level.Items[j2_IA_Level.SelectedIndex]);
                    }

                    //*** Lancement du jeu avec les parametres ***
                    Navigation.PushAsync(new MainPage(parametres));
                }
            }
            else
            {
                CallPopupPlayersName(sender, evenement);
            }
        }

        /********************************
         *  Popup Entry Player Name Vide
         * *****************************/
        async void CallPopupPlayersName(object sender, EventArgs e)
        {
            await DisplayAlert("Attention", "Remplir les Noms des joueurs", "OK");
        }
        /*****************************
         *  Popup IA Level Vide
         * **************************/
        async void CallPopupIALevel()
        {
            await DisplayAlert("Attention", "Renseigner le niveau IA", "OK");
        }
    }
}
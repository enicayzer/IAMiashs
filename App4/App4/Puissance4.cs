﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Globalization;
using System.Threading.Tasks;

namespace App4
{
    public class Puissance4
    {
        #region Propriétés 
        public Jeu jeu { get; set; }
        public int?[,] matrice; // [x,y] : x => ligne et y => colonne
        public Joueur joueur { get; set; }
        public bool isJoueur1 { get; set; }
        public bool isJoueurAlphaBeta { get; set; }
        #endregion

        #region Constructeur
        public Puissance4(Jeu jeu, int?[,] matrice, Joueur joueur, bool isJoueur1)
        {
            this.jeu = jeu;
            this.matrice = matrice;
            this.joueur = joueur;
            this.isJoueur1 = isJoueur1;
        }
        #endregion


        public Statuts VerifierJeu()
        {
            if(CompteurPoints() == 100000)
            {
                jeu.Statut = Statuts.Perdu;
            }
            else if(CompteurPoints() == -100000)
            {
                jeu.Statut = Statuts.Gagne;
            }
            else if(EstComplet())
            {
                jeu.Statut = Statuts.Nul;
            }
            return jeu.Statut;
        }

        /// <summary>
        /// Permet de vérifier si le jeu est terminé 
        /// </summary>
        /// <param name="profondeur"></param>
        /// <param name="score"></param>
        /// <returns></returns>
        private bool EstTermine(int profondeur, int score)
        {
            if (profondeur == 0 || score == jeu.Score || score == -jeu.Score || EstComplet())
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Permet de placer dans la matrice un nouveau pion  
        /// </summary>
        /// <param name="colonne"></param>
        /// <returns></returns>
        public Tuple<bool, int?> Placer(int colonne)
        {
            int? valeurRetourLigne = null;
            // On vérfie que la colonne sélectionné existe
            if (colonne < jeu.Colonne && matrice[0, colonne] == null && colonne >= 0)
            {
                /* Dans le puissance4 le [0,0] est en haut à gauche et le [5,6] en bas à droite
                 * 
                 * Dans cette boucle on va vérifier pour une colonne une ligne non vide en décrémentant 
                */
                for (var ligne = jeu.Ligne - 1; ligne >= 0; ligne--)
                {
                    if (matrice[ligne, colonne] == null)
                    {
                        matrice[ligne, colonne] = (int)joueur;
                        valeurRetourLigne = ligne;
                        break;
                    }
                }
                // Récupération de l'autre joueur
                joueur = ChangementJoueur(joueur);
                return new Tuple<bool, int?>(true, valeurRetourLigne);
            }
            else
            {
                return new Tuple<bool, int?>(false, valeurRetourLigne);
            }
        }

        private int PointsParPosition(int ligne, int colonne, int axeY, int axeX)
        {
            var pointJoueur1 = 0;
            var pointJoueur2 = 0;

            for (var i = 0; i < jeu.NbrPointsGagnant; i++)
            {
                if (ligne >= 0 && ligne < jeu.Ligne && colonne >= 0 && colonne < jeu.Colonne && matrice[ligne, colonne] == 0)
                {
                    pointJoueur1 = pointJoueur1 + 1;
                }
                else if (ligne >= 0 && ligne < jeu.Ligne && colonne >= 0 && colonne < jeu.Colonne && matrice[ligne, colonne] == 1)
                {
                    pointJoueur2 = pointJoueur2 + 1;
                }
                ligne += axeY;
                colonne += axeX;
            }

            // Dans le cas ou le joueur1 ou le joueur 2 à 4 pions placés => Fin de la partie
            if (pointJoueur1 == jeu.NbrPointsGagnant)
            {
                if(isJoueur1)
                {
                    return jeu.Score;
                }
                else
                {
                    return -jeu.Score;
                }
            }
            else if (pointJoueur2 == jeu.NbrPointsGagnant)
            {
                if (isJoueur1)
                {
                    return -jeu.Score;
                }
                else
                {
                    return jeu.Score;
                }
            }
            else
            {
                return isJoueur1 ? pointJoueur1 : pointJoueur2;
            }
            //return joueur == Joueur.Joueur1 ? pointJoueur1 : pointJoueur2;


        }

        private Tuple<bool, int> VerifierScore(int score)
        {
            if (score == jeu.Score)
            {
                return new Tuple<bool, int>(true, jeu.Score);
            }
            if (score == -jeu.Score)
            {
                return new Tuple<bool, int>(true, -jeu.Score);
            }
            return new Tuple<bool, int>(false, score);
        }

        private int CompteurPoints()
        {
            int pointsVertical = 0, pointsHorizontal = 0, pointsDiagonal = 0, pointsDiagonal2 = 0;
            // On vérifie pour chaque ligne et pour chaque colonne
            for (var ligne = 0; ligne < jeu.Ligne; ligne++)
            {
                for (var colonne = 0; colonne < jeu.Colonne; colonne++)
                {
                    #region Calcul des points
                    // On compte les points vertical
                    if (ligne < jeu.Ligne - 3)
                    {
                        var point = PointsParPosition(ligne, colonne, 1, 0);
                        var scoreVerifie = VerifierScore(point);
                        if (scoreVerifie.Item1)
                        {
                            return scoreVerifie.Item2;
                        }
                        pointsVertical += point;
                    }
                    // On compte les points horizontal
                    if (colonne < jeu.Colonne - 3)
                    {
                        var point = PointsParPosition(ligne, colonne, 0, 1);
                        var scoreVerifie = VerifierScore(point);
                        if (scoreVerifie.Item1)
                        {
                            return scoreVerifie.Item2;
                        }
                        pointsHorizontal += point;
                    }
                    // On compte les points dans la diagonal
                    if (ligne < jeu.Ligne - 3 && colonne < jeu.Colonne - 3)
                    {
                        var point = PointsParPosition(ligne, colonne, 1, 1);
                        var scoreVerifie = VerifierScore(point);
                        if (scoreVerifie.Item1)
                        {
                            return scoreVerifie.Item2;
                        }
                        pointsDiagonal += point;
                    }
                    // On compte les points dans l'autre diagonal
                    if (ligne > 2 && colonne <= jeu.Colonne - 4)
                    {
                        var point =  PointsParPosition(ligne, colonne, -1, +1);
                        var scoreVerifie = VerifierScore(point);
                        if (scoreVerifie.Item1)
                        {
                            return scoreVerifie.Item2;
                        }
                        pointsDiagonal2 += point;
                    }
                    #endregion
                }
            }
          
            // Retour du nombre de points total
            return pointsHorizontal + pointsVertical + pointsDiagonal + pointsDiagonal2;
        }

        /// <summary>
        /// Méthode qui retourne si le jeu est complet
        /// </summary>
        /// <returns></returns>
        private bool EstComplet()
        {
            for (var colonne = 0; colonne < jeu.Colonne; colonne++)
            {
                if (matrice[0, colonne] == null)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Méthode qui retourne la colonne choisie par l'IA
        /// </summary>
        /// <returns></returns>
        /// <returns></returns>
        public async Task<int> DecisionIA(bool isJoueur1, int profondeur, bool isJoueurAlphaBeta)
        {
            // Récupération des valeurs en retour de l'IA 
            if (isJoueur1)
            {
                this.isJoueur1 = true;
            }
            else
            {
                this.isJoueur1 = false;
            }
            this.isJoueurAlphaBeta = isJoueurAlphaBeta;
            List<int?> retourIA;
            retourIA = await Max(this, profondeur);
            return (int)retourIA[0].Value;
        }

        /// <summary>
        /// Méthode qui change de joueur
        /// </summary>
        /// <param name="round"></param>
        /// <returns></returns>
        public Joueur ChangementJoueur(Joueur round)
        {
            // Changement de joueur si Humain ou machine
            return round == Joueur.Joueur2 ? Joueur.Joueur1 : Joueur.Joueur2;
        }

        /// <summary>
        /// Méthode qui implémente le Max (minimax)
        /// </summary>
        /// <param name="puissance4"></param>
        /// <param name="profondeur"></param>
        /// <param name="a">Alpha</param>
        /// <param name="b">Beta</param>
        /// <returns></returns>
        private async Task<List<int?>> Max(Puissance4 puissance4, int profondeur, int? a = null, int? b = null)
        {
            var points = puissance4.CompteurPoints();
            // Si le jeu est terminée on retour une liste null
            if (puissance4.EstTermine(profondeur, points))
            {
                return new List<int?>() { null, points };
            }
            var maximumPoints = new List<int?>() { null, -99999 };
            for (var colonne = 0; colonne < puissance4.jeu.Colonne; colonne++)
            {
                // Création d'une copie du jeu (on doit clôner la matrice sinon il y a une référence) 
                var copiePuissance4 = new Puissance4(puissance4.jeu, (int?[,])puissance4.matrice.Clone(), puissance4.joueur, puissance4.isJoueur1);
                // On essaye de positionner le pion dans une colonne de la copie du puissance4
                if (copiePuissance4.Placer(colonne).Item1)
                {
                    var prochainCoup = await Min(copiePuissance4, profondeur - 1, a, b);
                    if (maximumPoints[0] == null || prochainCoup[1] > maximumPoints[1])
                    {
                        maximumPoints[0] = colonne;
                        maximumPoints[1] = prochainCoup[1];
                        a = prochainCoup[1];
                    }
                    if(a >= b && isJoueurAlphaBeta)
                    {
                        return maximumPoints;
                    }
                }
            }
            return maximumPoints;
        }

        /// <summary>
        /// Méthode qui implémente le min (minimax)
        /// </summary>
        /// <param name="puissance4"></param>
        /// <param name="profondeur"></param>
        /// <param name="a">Alpha</param>
        /// <param name="b">Beta</param>
        /// <returns></returns>
        private async Task<List<int?>> Min(Puissance4 puissance4, int profondeur, int? a = null, int? b = null)
        {
            var points = puissance4.CompteurPoints();
            // Si le jeu est terminée on retour une liste null
            if (puissance4.EstTermine(profondeur, points))
            {
                return new List<int?>() { null, points };
            }
            var minimumPoints = new List<int?>() { null, 99999 };
            for (var colonne = 0; colonne < puissance4.jeu.Colonne; colonne++)
            {
                // Création d'une copie du jeu (on doit clôner la matrice sinon il y a une référence) 
                var copiePuissance4 = new Puissance4(puissance4.jeu, (int?[,])puissance4.matrice.Clone(), puissance4.joueur, puissance4.isJoueur1);
                // On essaye de positionner le pion dans une colonne de la copie du puissance4
                if (copiePuissance4.Placer(colonne).Item1)
                {
                    var prochainCoup = await Max(copiePuissance4, profondeur - 1, a, b);
                    if (minimumPoints[0] == null || prochainCoup[1] < minimumPoints[1])
                    {
                        minimumPoints[0] = colonne;
                        minimumPoints[1] = prochainCoup[1];
                        b = prochainCoup[1];
                    }
                    if(a >= b && isJoueurAlphaBeta)
                    {
                        return minimumPoints;
                    }
                }
            }
            return minimumPoints;
        }
    }
}

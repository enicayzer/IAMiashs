using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace WpfApp2
{
    public class Puissance4
    {
        #region Propriétés 
        private Jeu jeu;
        private int?[,] matrice; // [x,y] : x => colonne et y => ligne
        public Joueur joueur { get; set; }
        #endregion


        #region Constructeur
        public Puissance4(Jeu jeu, int?[,] matrice, Joueur joueur)
        {
            this.jeu = jeu;
            this.matrice = matrice;
            this.joueur = joueur;
        }
        #endregion

        /// <summary>
        /// Permet de vérifier si le jeu est terminé 
        /// </summary>
        /// <param name="profondeur"></param>
        /// <param name="score"></param>
        /// <returns></returns>
        private bool EstTermine(int profondeur, long score)
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
            if (matrice[0, colonne] == null && colonne < jeu.Colonne && colonne >= 0)
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

        private long PositionScore(int ligne, int colonne, int deltaY, int deltaX)
        {
            var pointsHumain = 0;
            var pointsMachine = 0;

            for (var i = 0; i < 4; i++)
            {
                if (matrice[ligne,colonne] == 0)
                {
                    pointsHumain++;
                }
                else if(matrice[ligne,colonne] == 1)
                {
                    pointsMachine++;
                }
                ligne += deltaY;
                colonne += deltaX;
            }

            // Dans le cas ou l'humain ou la machine à 4 pions placés => Fin de la partie
            if (pointsHumain == 4)
            {
                return -jeu.Score;
            }
            else if (pointsMachine == 4)
            {
                return jeu.Score;
            }

            return pointsMachine;
        }

        private long score()
        {
            long points = 0, pointsVertical = 0, pointsHorizontal = 0, pointsDiagonal = 0, pointsDiagonal2 = 0;

            // Board-size: 7x6 (height x width)
            // Array indices begin with 0
            // => e.g. height: 0, 1, 2, 3, 4, 5

            // Vertical points
            // Check each column for vertical score
            // 
            // Possible situations
            //  0  1  2  3  4  5  6
            // [x][ ][ ][ ][ ][ ][ ] 0
            // [x][x][ ][ ][ ][ ][ ] 1
            // [x][x][x][ ][ ][ ][ ] 2
            // [x][x][x][ ][ ][ ][ ] 3
            // [ ][x][x][ ][ ][ ][ ] 4
            // [ ][ ][x][ ][ ][ ][ ] 5
            for (var ligne = 0; ligne < jeu.Ligne - 3; ligne++)
            {
                for (var colonne = 0; colonne < jeu.Colonne; colonne++)
                {
                    var score = PositionScore(ligne, colonne, 1, 0);
                    if (score == jeu.Score)
                    {
                        return jeu.Score;
                    }
                    if (score == -jeu.Score)
                    {
                        return -jeu.Score;
                    }
                    pointsVertical += score;
                }
            }

            // Horizontal points
            // Check each row's score
            // 
            // Possible situations
            //  0  1  2  3  4  5  6
            // [x][x][x][x][ ][ ][ ] 0
            // [ ][x][x][x][x][ ][ ] 1
            // [ ][ ][x][x][x][x][ ] 2
            // [ ][ ][ ][x][x][x][x] 3
            // [ ][ ][ ][ ][ ][ ][ ] 4
            // [ ][ ][ ][ ][ ][ ][ ] 5
            for (var ligne = 0; ligne < jeu.Ligne; ligne++)
            {
                for (var colonne = 0; colonne < jeu.Colonne - 3; colonne++)
                {
                    var score = PositionScore(ligne, colonne, 0, 1);
                    if (score == jeu.Score)
                    {
                        return jeu.Score;
                    }
                    if (score == -jeu.Score) { 
                        return -jeu.Score; 
                    }
                    pointsHorizontal += score;
                }
            }

            // Diagonal points 1 (left-bottom)
            //
            // Possible situation
            //  0  1  2  3  4  5  6
            // [x][ ][ ][ ][ ][ ][ ] 0
            // [ ][x][ ][ ][ ][ ][ ] 1
            // [ ][ ][x][ ][ ][ ][ ] 2
            // [ ][ ][ ][x][ ][ ][ ] 3
            // [ ][ ][ ][ ][ ][ ][ ] 4
            // [ ][ ][ ][ ][ ][ ][ ] 5
            for (var ligne = 0; ligne < jeu.Ligne - 3; ligne++)
            {
                for (var colonne = 0; colonne < jeu.Colonne - 3; colonne++)
                {
                    var score = PositionScore(ligne, colonne, 1, 1);
                    if (score == jeu.Score)
                    {
                        return jeu.Score;
                    }
                    if (score == -jeu.Score)
                    {
                        return -jeu.Score;
                    }
                    pointsDiagonal += score;
                }
            }

            // Diagonal points 2 (right-bottom)
            //
            // Possible situation
            //  0  1  2  3  4  5  6
            // [ ][ ][ ][x][ ][ ][ ] 0
            // [ ][ ][x][ ][ ][ ][ ] 1
            // [ ][x][ ][ ][ ][ ][ ] 2
            // [x][ ][ ][ ][ ][ ][ ] 3
            // [ ][ ][ ][ ][ ][ ][ ] 4
            // [ ][ ][ ][ ][ ][ ][ ] 5
            for (var ligne = 3; ligne < jeu.Ligne; ligne++)
            {
                for (var colonne = 0; colonne <= jeu.Colonne - 4; colonne++)
                {
                    var score = PositionScore(ligne, colonne, -1, +1);
                    if (score == jeu.Score)
                    {
                        return jeu.Score;
                    }
                    if (score == -jeu.Score)
                    {
                        return -jeu.Score;
                    }
                    pointsDiagonal2 += score;
                }
            }
            // Retour du nombre de points
            points = pointsHorizontal + pointsVertical + pointsDiagonal + pointsDiagonal2;
            return points;
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
        public int DecisionIA()
        {
            // Changement de joueur
            joueur = Joueur.Machine;

            // Récupération des valeurs en retour de l'IA 
            var retourIA = Max(this, jeu.Profondeur);

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
            return round == Joueur.Machine ? Joueur.Humain : Joueur.Machine;
        }

        /// <summary>
        /// Méthode qui implémente le Max (minimax)
        /// </summary>
        /// <param name="puissance4"></param>
        /// <param name="profondeur"></param>
        /// <returns></returns>
        private List<long?> Max(Puissance4 puissance4, int profondeur)
        {
            var score = puissance4.score();
            // Si le jeu est terminée on retour une liste null
            if (puissance4.EstTermine(profondeur, score))
            {
                return new List<long?>() { null, score };
            }
            var maximum = new List<long?>() { null, -99999 };

            for (var colonne = 0; colonne < puissance4.jeu.Colonne; colonne++)
            {
                // Création d'une copie du jeu (on doit clôner la matrice sinon il y a une référence) 
                var copyPuissance4 = new Puissance4(puissance4.jeu, (int?[,])puissance4.matrice.Clone(), puissance4.joueur);
                if (copyPuissance4.Placer(colonne).Item1)
                {
                    jeu.Iteration++;
                    var prochainMouvement = Min(copyPuissance4, profondeur - 1);
                    if (maximum[0] == null || prochainMouvement[1] > maximum[1])
                    {
                        maximum[0] = colonne;
                        maximum[1] = prochainMouvement[1];
                    }
                }
            }
            return maximum;
        }

       /// <summary>
       /// Méthode qui implémente le min (minimax)
       /// </summary>
       /// <param name="puissance4"></param>
       /// <param name="profondeur"></param>
       /// <returns></returns>
        private List<long?> Min(Puissance4 puissance4, int profondeur)
        {
            var score = puissance4.score();
            // Si le jeu est terminée on retour une liste null
            if (puissance4.EstTermine(profondeur, score))
            {
                return new List<long?>() { null, score };
            }
            var minimum = new List<long?>() { null, 99999 };
            for (var colonne = 0; colonne < puissance4.jeu.Colonne; colonne++)
            {
                // Création d'une copie du jeu (on doit clôner la matrice sinon il y a une référence) 
                var copyPuissance4 = new Puissance4(puissance4.jeu, (int?[,])puissance4.matrice.Clone(), puissance4.joueur);

                if (copyPuissance4.Placer(colonne).Item1)
                {
                    jeu.Iteration++;
                    var prochainMouvement = Max(copyPuissance4, profondeur - 1);
                    if (minimum[0] == null || prochainMouvement[1] < minimum[1])
                    {
                        minimum[0] = colonne;
                        minimum[1] = prochainMouvement[1];
                    }
                }
            }
            return minimum;
        }
    }
}

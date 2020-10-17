using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace WpfApp2
{
    public class Board
    {
        #region Propriétés 
        private Jeu jeu;
        private int?[,] matrice;
        public Joueur joueur { get; set; }
        public int lastRowPosition { get; set; }
        #endregion


        #region Constructeur
        public Board(Jeu jeu, int?[,] matrice, Joueur joueur)
        {
            this.jeu = jeu;
            this.matrice = matrice;
            this.joueur = joueur;
        }
        #endregion



        private bool EstTermine(int profondeur, long score)
        {
            if (profondeur == 0 || score == jeu.Score || score == -jeu.Score || EstComplet())
            {
                return true;
            }
            return false;
        }

        public Tuple<bool, int?> Placer(int colonne)
        {
            int? valeurRetourRow = null;
            if (matrice[0, colonne] == null && colonne >= 0 && colonne < jeu.Colonne)
            {
                for (var row = jeu.Ligne - 1; row >= 0; row--)
                {
                    if (matrice[row, colonne] == null)
                    {
                        matrice[row, colonne] = (int)joueur;
                        valeurRetourRow = row;
                        break;
                    }
                }
                joueur = ChangementJoueur(joueur);
                return new Tuple<bool, int?>(true, valeurRetourRow);
            }
            else
            {
                return new Tuple<bool, int?>(false, valeurRetourRow);
            }
        }

        private long scorePosition(int ligne, int colonne, int deltaY, int deltaX)
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

            if (pointsHumain == 4)
            {
                //this.game.winning = winningHuman;
                return -this.jeu.Score;
            }
            else if (pointsMachine == 4)
            {
                //this.game.winning = winningCPU;
                return this.jeu.Score;
            }

            return pointsMachine;
        }

        private long score()
        {
            long points = 0;
            long vertical_points = 0;
            long horizontal_points = 0;
            long diagonal_points1 = 0;
            long diagonal_points2 = 0;

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
            for (var row = 0; row < this.jeu.Ligne - 3; row++)
            {
                // Für jede Column überprüfen
                for (var column = 0; column < this.jeu.Colonne; column++)
                {
                    // Die Column bewerten und zu den Punkten hinzufügen
                    var score = this.scorePosition(row, column, 1, 0);
                    if (score == this.jeu.Score) return this.jeu.Score;
                    if (score == -this.jeu.Score) return -this.jeu.Score;
                    vertical_points += score;
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
            for (var row = 0; row < this.jeu.Ligne; row++)
            {
                for (var column = 0; column < this.jeu.Colonne - 3; column++)
                {
                    var score = this.scorePosition(row, column, 0, 1);
                    if (score == this.jeu.Score) return this.jeu.Score;
                    if (score == -this.jeu.Score) return -this.jeu.Score;
                    horizontal_points += score;
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
            for (var row = 0; row < this.jeu.Ligne - 3; row++)
            {
                for (var column = 0; column < this.jeu.Colonne - 3; column++)
                {
                    var score = this.scorePosition(row, column, 1, 1);
                    if (score == this.jeu.Score) return this.jeu.Score;
                    if (score == -this.jeu.Score) return -this.jeu.Score;
                    diagonal_points1 += score;
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
            for (var row = 3; row < this.jeu.Ligne; row++)
            {
                for (var column = 0; column <= this.jeu.Colonne - 4; column++)
                {
                    var score = this.scorePosition(row, column, -1, +1);
                    if (score == this.jeu.Score) return this.jeu.Score;
                    if (score == -this.jeu.Score) return -this.jeu.Score;
                    diagonal_points2 += score;
                }

            }

            points = horizontal_points + vertical_points + diagonal_points1 + diagonal_points2;
            return points;
        }


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


        public int DecisionIA()
        {
            this.joueur = Joueur.computer;
            var retourIA = maximizePlay(this, this.jeu.Profondeur);
            return (int)retourIA[0].Value;
        }

        public Joueur ChangementJoueur(Joueur round)
        {
            return round == Joueur.computer ? Joueur.human : Joueur.computer;
        }

        private List<long?> maximizePlay(Board board, int depth)
        {
            var score = board.score();
            if (board.EstTermine(depth, score))
            {
                return new List<long?>() { null, score };
            }
            var max = new List<long?>() { null, -99999 };

            for (var column = 0; column < board.jeu.Colonne; column++)
            {
                var newBoard = new Board(board.jeu, (int?[,])board.matrice.Clone(), board.joueur);
                if (newBoard.Placer(column).Item1)
                {
                    this.jeu.Iteration++;

                    var nextmove = this.minimizePlay(newBoard, depth - 1);
                    if (max[0] == null || nextmove[1] > max[1])
                    {
                        max[0] = column;
                        max[1] = nextmove[1];
                    }

                }
            }
            
            return max;
        }

       
        private List<long?> minimizePlay(Board board, int depth)
        {
            var score = board.score();
            if (board.EstTermine(depth, score))
            {
                return new List<long?>() { null, score };
            }
            var min = new List<long?>() { null, 99999 };
            for (var column = 0; column < board.jeu.Colonne; column++)
            {
                var newBoard = new Board(board.jeu, (int?[,])board.matrice.Clone(), board.joueur);
                if (newBoard.Placer(column).Item1)
                {
                    this.jeu.Iteration++;

                    var nextmove = this.maximizePlay(newBoard, depth - 1);
                    if (min[0] == null || nextmove[1] < min[1])
                    {
                        min[0] = column;
                        min[1] = nextmove[1];
                    }

                }
            }
            return min;
        }
    }
}

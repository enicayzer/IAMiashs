using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App4
{
    public class Parametres
    {
        public String NomJoueur1 { get; set; }
        public String NomJoueur2 { get; set; }
        public Joueur TypeJoueur1 { get; set; }
        public Joueur TypeJoueur2 { get; set; }
        public int NiveauJoueur1 { get; set; }
        public int NiveauJoueur2 { get; set; }
        public Boolean Joueur1isIA { get; set; }
        public Boolean Joueur2isIA { get; set; }
        public Boolean Joueur1AlphaBeta { get; set; }
        public Boolean Joueur2AlphaBeta { get; set; }
        public List<long> ListeCoupsJ1 { get; set; }
        public List<long> ListeCoupsJ2 { get; set; }
        public Statuts Statut { get; set; }
        public int NbColonne { get; set; }
        public int NbLigne { get; set; }
        public int NbPointsGagnants { get; set; }
        public int Score { get; set; }
        
        public Parametres()
        {
        }

        public void calculDonnees()
        {
            if (ListeCoupsJ1.Count != 0)
            {
                Console.WriteLine("******* Joueur1 [Niveau: " + NiveauJoueur1 + " - AB: " + Joueur1AlphaBeta + "] __ NbCoups: " + ListeCoupsJ1.Count + " __ Moyenne TR: " + Math.Truncate(100 * ListeCoupsJ1.Average()) / 100);
            }
            if (ListeCoupsJ2.Count != 0)
            {
                Console.WriteLine("******* Joueur2 [Niveau: " + NiveauJoueur2 + " - AB: " + Joueur2AlphaBeta + "] __ NbCoups: " + ListeCoupsJ2.Count + " __ Moyenne TR: " + Math.Truncate(100 * ListeCoupsJ2.Average()) / 100);
            }
        }

        public int GetIALevelByLabel(String level_label)
        {
            int level_return = 0;
            switch (level_label)
            {
                case "Facile":
                    level_return = 2;
                    break;
                case "Moyen":
                    level_return = 4;
                    break;
                case "Difficile":
                    level_return = 6;
                    break;
            }
            return level_return;
        }

        public Parametres resetParametresNouvellePartie()
        {
            this.ListeCoupsJ1 = new List<long>();
            this.ListeCoupsJ2 = new List<long>();
            this.Statut = Statuts.EnCours;
            return this;
        }
    }
}

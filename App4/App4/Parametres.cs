using System;
using System.Collections.Generic;
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
        public Statuts Statut { get; set; }
        public int NbColonne { get; set; }
        public int NbLigne { get; set; }
        public int NbPointsGagnants { get; set; }
        public int Score { get; set; }
        
        public Parametres()
        {
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
    }
}

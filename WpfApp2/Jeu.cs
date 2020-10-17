using System;
using System.Collections.Generic;
using System.Text;

namespace WpfApp2
{
    public class Jeu
    {
        public int Ligne { get; set; }
        public int Colonne { get; set; }
        public Statuts Statut { get; set; }
        public int Profondeur { get; set; }
        public long Score { get; set; }
        public int Round { get; set; }
        public object Gagnant { get; set; }
        public int Iteration { get; set; }
    }

}

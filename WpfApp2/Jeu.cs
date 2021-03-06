﻿using System;
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
        public int Score { get; set; }
        // Nombre de points gagnant par défaut 4
        public int NbrPointsGagnant { get; set; }

        public bool EstAlphaBeta { get; set; }
    }
}

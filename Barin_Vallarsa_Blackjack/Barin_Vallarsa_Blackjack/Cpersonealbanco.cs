using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;

namespace Barin_Vallarsa_Blackjack
{
    public struct Personealbanco
    {
        public List<Ccarta> Mano;
        public int valoreMano ;
        public int spostamento;
        public bool sballa;
        public bool blackjack;
        public Personealbanco(int hand = 0, int move = 0)
        {
            Mano = new List<Ccarta>();
            valoreMano = hand;
            spostamento = move;
            sballa = false;
            blackjack = false;
        }

    }
}


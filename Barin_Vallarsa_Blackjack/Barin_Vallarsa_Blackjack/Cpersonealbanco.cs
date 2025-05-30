using System.Collections.Generic;

namespace Barin_Vallarsa_Blackjack
{
    class CPersonealbanco
    {
        public List<Ccarta> Mano;
        public int ValoreMano;
        public int Spostamento;
        public bool Sballa;
        public bool Blackjack;

        public CPersonealbanco()
        {
            Mano = new List<Ccarta>();
            ValoreMano = 0;
            Spostamento = 0;
            Sballa = false;
            Blackjack = false;
        }
    }
}


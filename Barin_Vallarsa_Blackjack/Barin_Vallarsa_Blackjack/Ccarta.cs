using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Barin_Vallarsa_Blackjack
{
    internal class Ccarta
    {
        public int value { get; private set; }
        public string seed { get; private set; }

        public string special { get; private set; } //serve ad identificare tra di loro le figure

        public Ccarta(int valore, string seme, string speciale)
        {
            value = valore;
            seed = seme;
            special = speciale;
        }

        public void abbassaAsso()
        {
            if(special== "asso" && value == 11)
            {
                value = 1;
            }
        }

        public string infocarta()
        {
            return $"{value}-{seed}-{special}\n";
        }
    }
}

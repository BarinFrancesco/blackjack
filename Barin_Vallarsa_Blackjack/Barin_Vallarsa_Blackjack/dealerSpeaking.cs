using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Barin_Vallarsa_Blackjack
{
    public partial class dealerSpeaking: Form
    {
        public dealerSpeaking(string parole)
        {
            InitializeComponent();
            Vocedealer(parole,25);
        }

        private void lbl_dealerVoice_Click(object sender, EventArgs e)
        {

        }

        public async void Vocedealer(string text, int delay)
        {
            lbl_dealerVoice.Text = "";
            foreach (char c in text)
            {
                lbl_dealerVoice.Text += c;
                await Task.Delay(delay); // attesa tra una lettera e l'altra
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }
    }
}

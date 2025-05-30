using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Barin_Vallarsa_Blackjack
{
    public partial class dealerSpeaking: Form
    {
        SoundPlayer suono;
        public dealerSpeaking(string parole, int scelta)
        {
            InitializeComponent();
            // crupier che parla
            string voce ="";
            switch (scelta)
            {
                case 1:
                    voce = "PuntaperGiocare.wav";
                    break;
                case 2:
                    voce = "boombaby.wav";
                    break;
                case 3:
                    voce = "SconfittaSound.wav";
                    break;
                case 4:
                    voce = "NoSoldiRitirare.wav";
                    break;
                case 5:
                    voce = "FinitoSoldi.wav";
                    break;
                case 6:
                    voce = "NonabbastanzaSoldi.wav";
                    break;
                case 7:
                    voce = "NonSoldiXraddoppiare.wav";
                    break;
                case 8:
                    voce = "Dopo2carte.wav";
                    break;
                case 9:
                    voce = "NosoldiDaPuntare.wav";
                    break;
                case 10:
                    voce = "pareggiato.wav";
                    break;
                case 11:
                    voce = "CiDispiace.wav";
                    break;
                default:
                    voce = "default.wav"; 
                    break;
            }
           suono = new SoundPlayer(voce);
           suono.Play();
           Vocedealer(parole, 25);
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
            suono.Stop();
            DialogResult = DialogResult.OK;
        }
    }
}

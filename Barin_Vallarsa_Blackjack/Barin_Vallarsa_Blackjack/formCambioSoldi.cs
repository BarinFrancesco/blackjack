using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace Barin_Vallarsa_Blackjack
{
    public partial class formCambioSoldi : Form
    {
        public formCambioSoldi()
        {
            InitializeComponent();
            lbl_dealerVoice.Text = "Buonasera, vuole cambiare dei soldi in fiches?";
        }
        public int money { get; set; }
        private void btn_ok_Click(object sender, EventArgs e)
        {
            // una volta inserito l'input andiamo a verificare che non sia null e che sia minore di 1000000000

            if (txt_input_soldi.Text.Length >= 10)
            {
                Vocedealer("Mi dispiace non sono capace di cambiare una quantità così grande di denaro!", 25);
                txt_input_soldi.Text = null;
                return;
            }

            if (txt_input_soldi.Text == "" || int.Parse(txt_input_soldi.Text) == 0)
            {
                Vocedealer("Deve darmi dei soldi da cambiare, non mi ha ancora dato niente.", 25);
                txt_input_soldi.Text = null;
                return;
            }

            int soldi = int.Parse(txt_input_soldi.Text);//se supera i controlli lo mandiamo al form principale
            money = soldi;
            DialogResult = DialogResult.OK;

        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            //impediamo all'utente di inserire caratteri che non siano numeri
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void btn_annulla_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.No; //non facciamo niente se l'utente vuole annullare l'operazione
        }

        //funzione aggiunta per creare l'effetto di una persona che parla
        private async void Vocedealer(string text, int delay)
        {
            lbl_dealerVoice.Text = "";
            foreach (char c in text)
            {
                lbl_dealerVoice.Text += c;
                await Task.Delay(delay); // attesa tra una lettera e l'altra
            }
        }

        private void lbl_dealerVoice_Click(object sender, EventArgs e)
        {

        }
    }
}
